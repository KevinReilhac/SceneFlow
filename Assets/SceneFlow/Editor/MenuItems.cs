using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Kebab.SceneFlow.Settings;
using System.Threading.Tasks;
using System.IO;

namespace Kebab.SceneFlow.Editors
{
    internal static class CreateSettingsOnLoad
    {
        private static string FullFilePath => Path.Join(
            SceneFlowSettings.SETTINGS_DIRECTORY,
            SceneFlowSettings.SETTINGS_FILENAME
        ).ToString();

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            CreateSettingsAsset();
        }

        private static async Task<bool> IsSettingsExist()
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<SceneFlowSettings>(FullFilePath);

            while (!resourceRequest.isDone)
                await Task.Yield();
            return resourceRequest.asset != null;
        }

        [MenuItem("SceneFlow/CreateSettingsAsset...", priority = 100)]
        private static async void CreateSettingsAsset()
        {
            if (await IsSettingsExist())
            {
                return;
            }

            SceneFlowSettings settings = ScriptableObject.CreateInstance<SceneFlowSettings>();
            string path = Path.Join("Assets\\Resources", SceneFlowSettings.SETTINGS_DIRECTORY);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(settings, Path.Join(path, SceneFlowSettings.SETTINGS_FILENAME + ".asset"));
            AssetDatabase.Refresh();
        }

        [MenuItem("SceneFlow/CreateSettingsAsset...", isValidateFunction: true)]
        private static bool CreateSettingsAssetValidate()
        {
            return Resources.Load<SceneFlowSettings>(FullFilePath) == null;
        }


        [MenuItem("SceneFlow/OpenSettings", priority = 0)]
        private static void OpenSettings()

        {
            string path = Path.Join("Assets\\Resources", FullFilePath + ".asset");
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
            EditorUtility.FocusProjectWindow();
        }

        [MenuItem("SceneFlow/OpenSettings", isValidateFunction: true)]
        private static bool OpenSettingsValidation()
        {
            return Resources.Load<SceneFlowSettings>(FullFilePath) != null;
        }

    }
}