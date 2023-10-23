using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kebab.SceneFlow.Settings
{
    public class SceneFlowSettings : ScriptableObject
    {
        public const string SETTINGS_DIRECTORY = "SceneFlow";
        public const string SETTINGS_FILENAME = "SceneFlowSettings";

        public ALoadingScreen LoadingScreenPrefab;
        [Min(0)] public float FakeLoadingTime = 0f;
        [Range(0f, 1f)] public float FakeLoadingPercent = 0.2f;
        public bool ActionToExitLoadingScreen = false;
    }
}

