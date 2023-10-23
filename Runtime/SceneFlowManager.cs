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
                        Debug.LogError("Create A SceneFlowSettings in Ressources folder.");
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
                }

                return _loadingScreen;
            }
        }

        /// <summary>
        /// Load a scene asyncronously
        /// </summary>
        /// <param name="scene"> Scene name </param>
        /// <param name="showLoadingScreen"> Display loading scene on True</param>
        public static async void Load(string scene, bool showLoadingScreen = true)
        {
            if (Settings == null) return;

            if (IsLoadingScene)
            {
                Debug.LogError("You already loading a scene.");
                return;
            }

            OnLoadScene?.Invoke();
            if (showLoadingScreen)
            {
                LoadingScreen.Show(async () =>
                {
                    await Load(SceneManager.LoadSceneAsync(scene), showLoadingScreen);
                });
            }
            else
            {
                await Load(SceneManager.LoadSceneAsync(scene), showLoadingScreen);
            }
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

            OnLoadScene?.Invoke();
            if (showLoadingScreen)
            {
                LoadingScreen.Show(async () =>
                {
                    await Load(SceneManager.LoadSceneAsync(buildIndex), showLoadingScreen);
                });
            }
            else
            {
                await Load(SceneManager.LoadSceneAsync(buildIndex), showLoadingScreen);
            }
        }


        private static async Task Load(AsyncOperation loadSceneOperation, bool showLoadingScreen = true)
        {
            SceneFlowManager.loadAsyncOperation = loadSceneOperation;
            loadSceneOperation.allowSceneActivation = false;

            LoadingScreen.UpdateProgress(0);
            while (loadSceneOperation.progress < 0.9f)
            {
                LoadingScreen.UpdateProgress(MathHelpers.Remap(loadSceneOperation.progress, 0f, 1f, 0f, 1f - Settings.FakeLoadingPercent));
                await Task.Yield();
            }

            await ProcessFakeLoadingTime();
            LoadingScreen.UpdateProgress(1f);
            if (!Settings.ActionToExitLoadingScreen)
                ExitLoadingScreen();
        }

        public static void ExitLoadingScreen()
        {
            if (loadAsyncOperation.progress < 0.9f)
            {
                Debug.Log("Loading is not finished.");
                return;
            }

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
    }
}