using System;
using UnityEngine;

namespace Kebab.SceneFlow.Settings
{
    /// <summary>
    /// The settings for the SceneFlow system.
    /// </summary>
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
        public enum EKeyboardAction
        {
            Space = 1 << 0,
            Enter = 1 << 1,
            Escape = 1 << 2,
        }
        
        [Flags]
        public enum EMouseAction
        {
            LeftClick = 1 << 0,
            RightClick = 1 << 1,
        }

        public const string SETTINGS_DIRECTORY = "SceneFlow";
        public const string SETTINGS_FILENAME = "SceneFlowSettings";


        /// <summary>
        /// The prefab to use for the loading screen.
        /// </summary>
        [Tooltip("The prefab to use for the loading screen.")]
        public ALoadingScreen LoadingScreenPrefab;

        /// <summary>
        /// The fake loading time in seconds.
        /// </summary>
        [Tooltip("The fake loading time in seconds.")]
        [Min(0)] public float FakeLoadingTime = 0f;

        /// <summary>
        /// The percentage where the fake loading starts.
        /// </summary>
        [Tooltip("The percentage where the fake loading starts.")]
        [Range(0f, 1f)] public float FakeLoadingPercent = 0.2f;

        /// <summary>
        /// If false, the new scene will be displayed immediately after the loading (fake or not) is finished.
        /// </summary>
        [Tooltip("If false, the new scene will be displayed immediately after the loading (fake or not) is finished.")]
        public bool ActionToExitLoadingScreen = false;

        [Tooltip("The action to exit the loading screen using the keyboard.")]
        public EKeyboardAction ExitLoadingKeyboardAction = (EKeyboardAction)(-1);

        [Tooltip("The action to exit the loading screen using the gamepad.")]
        public EGamepadAction ExitLoadingGamepadAction = (EGamepadAction)(-1);

        [Tooltip("The action to exit the loading screen using the mouse.")]
        public EMouseAction ExitLoadingMouseAction = (EMouseAction)(-1);
    }
}



