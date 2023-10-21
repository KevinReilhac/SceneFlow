using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ForceRefreshScripts
{
    [MenuItem("Tools/ReloadScripts")]
    public static void RefreshScripts()
    {
        EditorUtility.RequestScriptReload();
    }
}
