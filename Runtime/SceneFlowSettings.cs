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
    }
}

