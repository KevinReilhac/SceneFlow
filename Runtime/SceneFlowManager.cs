using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kebab.SceneFlow.Settings;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.IO;
using Kebab.SceneFlow.Utils;
using System.Runtime.CompilerServices;

namespace Kebab.SceneFlow
{
    public static class SceneFlowManager
    {
        private const int FAKE_LOADING_DELTA = 50;
        private static SceneFlowSettings _settings;
        private static ALoadingScreen _loadingScreen;
        private static AsyncOperation loadAsyncOperation;
        private static ExitActionAwaiter exitActionAwaiter;

        public static bool IsLoadingScene { get; private set; } = false;
        public static event Action OnLoadScene;
        public static event Action OnSceneLoaded;

        private static SceneFlowSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Resources.Load<SceneFlowSettings>(Path.Join(SceneFlowSettings.SETTINGS_DIRECTORY, SceneFlowSettings.SETTINGS_FILENAME));
                    if (_settings == null)
                    {
                        Debug.LogError("SceneFlowSettings Asset not found in Ressources folder.");
                        return null;
                    }
                }
                return _settings;
            }
        }

        private static ALoadingScreen LoadingScreen
        {
            get
            {
                if (_loadingScreen == null)
                {
                    if (Settings == null)
                        return null;
                    _loadingScreen = GameObject.Instantiate(Settings.LoadingScreenPrefab);
                    _loadingScreen.gameObject.SetActive(false);
                }

                return _loadingScreen;
            }
        }

        /// <summary>
        /// Load a scene asyncronously
        /// </summary>
        /// <param name="sceneName"> Scene name </param>
        /// <param name="showLoadingScreen"> Display loading scene on True</param>
        public static void Load(string sceneName, bool showLoadingScreen = true)
        {
            if (Settings == null) return;
            if (!sceneName.EndsWith(".unity"))
                sceneName += ".unity";

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)

            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                if (scenePath.EndsWith(sceneName, StringComparison.InvariantCultureIgnoreCase))
                {
                    Load(i, showLoadingScreen);
                    return;
                }
            }

            Debug.LogError($"Scene {sceneName} not found in build settings.");
        }


        /// <summary>
        /// Load a scene asyncronously
        /// </summary>
        /// <param name="build index"> Scene name </param>
        /// <param name="showLoadingScreen"> Display loading scene on True</param>
        public static async void Load(int buildIndex, bool showLoadingScreen = true)
        {
            if (Settings == null) return;

            if (IsLoadingScene)
            {
                Debug.LogError("You already loading a scene.");
                return;
            }

            if (buildIndex <= -1)
            {
                Debug.LogError("Invalid build index.");
                return;
            }

            OnLoadScene?.Invoke();
            if (showLoadingScreen)
            {
                LoadingScreen.Show(async () =>
                {
                    await Load(SceneManager.LoadSceneAsync(buildIndex));
                });
            }
            else
            {
                await Load(SceneManager.LoadSceneAsync(buildIndex));
            }

        }


        private static async Task Load(AsyncOperation loadSceneOperation)
        {
            IsLoadingScene = true;
            SceneFlowManager.loadAsyncOperation = loadSceneOperation;
            loadSceneOperation.allowSceneActivation = false;

            LoadingScreen.UpdateProgress(0);
            while (loadSceneOperation.progress < 0.9f)
            {
                await Task.Yield();
                LoadingScreen.UpdateProgress(MathHelpers.Remap(loadSceneOperation.progress, 0f, 0.9f, 0f, 1f - Settings.FakeLoadingPercent));
            }

            await ProcessFakeLoadingTime();
            LoadingScreen.UpdateProgress(1f);

            if (!Settings.ActionToExitLoadingScreen)
                ExitLoadingScreen();
            else
                CreateExitActionAwaiter(OnPressExitAction);
        }

        private static void OnPressExitAction()
        {
            RemoveExitActionAwaiter();
            ExitLoadingScreen();
        }

        public static void ExitLoadingScreen()
        {
            if (loadAsyncOperation.progress < 0.9f)
            {
                Debug.Log("Loading is not finished.");
                return;
            }

            IsLoadingScene = false;
            loadAsyncOperation.allowSceneActivation = true;
            LoadingScreen.Hide();
            OnSceneLoaded?.Invoke();
        }

        private static async Task ProcessFakeLoadingTime()
        {
            for (float t = 0; t < Settings.FakeLoadingTime * 1000; t++)
            {
                await Task.Delay(FAKE_LOADING_DELTA);
                t += FAKE_LOADING_DELTA;
                float progress = MathHelpers.Remap(t / (Settings.FakeLoadingTime * 1000f), 0f, 1f, 1f - Settings.FakeLoadingPercent, 1f);
                LoadingScreen.UpdateProgress(progress);
            }
        }

        public static void LoadNextScene(bool showLoadingScreen = true)
        {
            int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentBuildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError("You already is at the last scene.");
                return;
            }

            Load(currentBuildIndex + 1, showLoadingScreen);
        }

        private static void CreateExitActionAwaiter(Action action)
        {
            if (exitActionAwaiter != null)
                return;

            GameObject exitActionAwaiterGameObject = new GameObject("ExitActionAwaiter", typeof(ExitActionAwaiter));
            exitActionAwaiter = exitActionAwaiterGameObject.GetComponent<ExitActionAwaiter>();
            exitActionAwaiter.Setup(action, Settings.ExitLoadingKeyboardAction, Settings.ExitLoadingGamepadAction, Settings.ExitLoadingMouseAction);
        }

        private static void RemoveExitActionAwaiter()
        {
            if (exitActionAwaiter != null)
            {
                GameObject.Destroy(exitActionAwaiter.gameObject);
                exitActionAwaiter = null;
            }

        }
    }
}