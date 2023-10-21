using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kebab.SceneFlow;

public class SceneController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneFlowManager.Load(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneFlowManager.Load("Scene2");
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneFlowManager.Load(2);
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            SceneFlowManager.LoadNextScene();

        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //if (Input.GetKeyDown(KeyCode.Alpha0))
    }
}
