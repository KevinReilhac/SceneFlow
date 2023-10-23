using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kebab.SceneFlow
{
    public abstract class ALoadingScreen : MonoBehaviour
    {
        protected void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public abstract void Show(Action onVisible);
        public abstract void Hide();
        public virtual void OnScenePreloaded() {}
        public virtual void UpdateProgress(float progress) {}

        private void OnApplicationQuit()
        {
            DestroyImmediate(gameObject);
        }
    }
}