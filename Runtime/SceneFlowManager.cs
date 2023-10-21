using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kebab.SceneFlow.Settings;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.IO;

namespace Kebab.SceneFlow
{
    public static class SceneFlowManager
    {
        private static SceneFlowSettings _settings;
        private static ALoadingScreen _loadingScreen;

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
        public static void Load(string scene, bool showLoadingScreen = true)
        {
            if (IsLoadingScene)
            {
                Debug.LogError("You already loading a scene.");
                return;
            }

            Load(SceneManager.LoadSceneAsync(scene), showLoadingScreen);
        }

        /// <summary>
        /// Load a scene asyncronously
        /// </summary>
        /// <param name="build index"> Scene name </param>
        /// <param name="showLoadingScreen"> Display loading scene on True</param>
        public static void Load(int buildIndex, bool showLoadingScreen = true)
        {
            if (IsLoadingScene)
            {
                Debug.LogError("You already loading a scene.");
                return;
            }

            Load(SceneManager.LoadSceneAsync(buildIndex), showLoadingScreen);
        }


        public static async void Load(AsyncOperation loadSceneOperation, bool showLoadingScreen = true)
        {
            if (Settings == null) return;

            OnLoadScene?.Invoke();

            if (showLoadingScreen) LoadingScreen?.Show();

            LoadingScreen.UpdateProgress(0);
            await Task.Delay(Mathf.RoundToInt(Settings.FakeLoadingTime * 1000f));
            while (!loadSceneOperation.isDone)
            {
                LoadingScreen.UpdateProgress(loadSceneOperation.progress);
                await Task.Yield();
            }

            LoadingScreen.Hide();
            OnSceneLoaded?.Invoke();
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