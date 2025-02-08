using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace Kebab.SceneFlow.Settings.Editors
{
    [CustomEditor(typeof(SceneFlowSettings))]
    public class SceneFlowSettingsCustomEditor : Editor
    {
        private const string TOOLTIP_FAKE_LOADING_TIME = "The fake loading time in seconds.";

        private SerializedProperty loadingScreenPrefab;
        private SerializedProperty fakeLoadingTime;
        private SerializedProperty fakeLoadingPercent;
        private SerializedProperty actionToExitLoadingScreen;
        private SerializedProperty exitLoadingKeyboardMouseAction;
        private SerializedProperty exitLoadingGamepadAction;
        private SerializedProperty exitLoadingMouseAction;

        private GUIContent fakeLoadingTimeContent;
        private GUIContent exitLoadingKeyboardMouseActionContent;
        private GUIContent exitLoadingGamepadActionContent;
        private GUIContent exitLoadingMouseActionContent;


        private void OnEnable()
        {
            fakeLoadingTimeContent = new GUIContent("Fake Loading Time", TOOLTIP_FAKE_LOADING_TIME);
            exitLoadingKeyboardMouseActionContent = new GUIContent("Exit Loading Keyboard Mouse Action", "The action to exit the loading screen using the keyboard or mouse.");
            exitLoadingGamepadActionContent = new GUIContent("Exit Loading Gamepad Action", "The action to exit the loading screen using the gamepad.");
            exitLoadingMouseActionContent = new GUIContent("Exit Loading Mouse Action", "The action to exit the loading screen using the mouse.");

            loadingScreenPrefab = serializedObject.FindProperty(nameof(SceneFlowSettings.LoadingScreenPrefab));
            fakeLoadingTime = serializedObject.FindProperty(nameof(SceneFlowSettings.FakeLoadingTime));
            fakeLoadingPercent = serializedObject.FindProperty(nameof(SceneFlowSettings.FakeLoadingPercent));
            exitLoadingMouseAction = serializedObject.FindProperty(nameof(SceneFlowSettings.ExitLoadingMouseAction));

            actionToExitLoadingScreen = serializedObject.FindProperty(nameof(SceneFlowSettings.ActionToExitLoadingScreen));
            exitLoadingKeyboardMouseAction = serializedObject.FindProperty(nameof(SceneFlowSettings.ExitLoadingKeyboardMouseAction));
            exitLoadingGamepadAction = serializedObject.FindProperty(nameof(SceneFlowSettings.ExitLoadingGamepadAction));
        }



        public override void OnInspectorGUI()
        {
            Header("Fake loading");
            float newFakeLoadingTime = EditorGUILayout.FloatField(fakeLoadingTimeContent, fakeLoadingTime.floatValue);
            if (newFakeLoadingTime < 0f)
                newFakeLoadingTime = 0f;
            fakeLoadingTime.floatValue = newFakeLoadingTime;
            if (fakeLoadingTime.floatValue > 0f)
                EditorGUILayout.PropertyField(fakeLoadingPercent);

            DrawExitLoadingScreen();

            Header("References");
            EditorGUILayout.PropertyField(loadingScreenPrefab);
            if (loadingScreenPrefab.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("No loading screen prefab selected.", MessageType.Error);
                EditorGUILayout.Space(10);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawExitLoadingScreen()
        {
            Header("Exit loading screen");
            EditorGUILayout.PropertyField(actionToExitLoadingScreen);
            if (actionToExitLoadingScreen.boolValue)
            {
                SceneFlowSettings.EKeyboardMouseAction exitLoadingKeyboardMouseActionValue = (SceneFlowSettings.EKeyboardMouseAction)exitLoadingKeyboardMouseAction.enumValueFlag;
                SceneFlowSettings.EGamepadAction exitLoadingGamepadActionValue = (SceneFlowSettings.EGamepadAction)exitLoadingGamepadAction.enumValueFlag;
                SceneFlowSettings.EMouseAction exitLoadingMouseActionValue = (SceneFlowSettings.EMouseAction)exitLoadingMouseAction.enumValueFlag;

                SceneFlowSettings.EKeyboardMouseAction newExitLoadingKeyboardMouseActionValue = (SceneFlowSettings.EKeyboardMouseAction)EditorGUILayout.EnumFlagsField(exitLoadingKeyboardMouseActionContent, exitLoadingKeyboardMouseActionValue);
                SceneFlowSettings.EGamepadAction newExitLoadingGamepadActionValue = (SceneFlowSettings.EGamepadAction)EditorGUILayout.EnumFlagsField(exitLoadingGamepadActionContent, exitLoadingGamepadActionValue);
                SceneFlowSettings.EMouseAction newExitLoadingMouseActionValue = (SceneFlowSettings.EMouseAction)EditorGUILayout.EnumFlagsField(exitLoadingMouseActionContent, exitLoadingMouseActionValue);

                exitLoadingKeyboardMouseAction.enumValueFlag = (int)newExitLoadingKeyboardMouseActionValue;
                exitLoadingGamepadAction.enumValueFlag = (int)newExitLoadingGamepadActionValue;
                exitLoadingMouseAction.enumValueFlag = (int)newExitLoadingMouseActionValue;
            }
        }

        private void Header(string title)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        }

    }
}

