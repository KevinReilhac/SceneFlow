using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kebab.SceneFlow.Settings
{
    public class SceneFlowSettings : ScriptableObject
    {
        [Flags]
        public enum EGamepadAction
        {
            A = 1 << 0,
            B = 1 << 1,
            X = 1 << 2,
            Y = 1 << 3,
            Start = 1 << 4,
        }


        [Flags]
        public enum EKeyboardMouseAction
        {
            Space = 1 << 0,
            Enter = 1 << 1,
            Escape = 1 << 2,
            MouseLeftClick = 1 << 3,
            MouseRightClick = 1 << 4,
        }

        public const string SETTINGS_DIRECTORY = "SceneFlow";

        public const string SETTINGS_FILENAME = "SceneFlowSettings";

        [Tooltip("The prefab to use for the loading screen.")]
        public ALoadingScreen LoadingScreenPrefab;
        [Tooltip("The fake loading time in seconds.")]
        [Min(0)] public float FakeLoadingTime = 0f;
        [Tooltip("The percentage where the fake loading starts.")]
        [Range(0f, 1f)] public float FakeLoadingPercent = 0.2f;
        [Tooltip("If false, the new scene will be displayed immediately after the loading (fake or not) is finished.")]
        public bool ActionToExitLoadingScreen = false;
        public EKeyboardMouseAction ExitLoadingKeyboardMouseAction = (EKeyboardMouseAction)(-1);
        public EGamepadAction ExitLoadingGamepadAction = (EGamepadAction)(-1);

    }
}



