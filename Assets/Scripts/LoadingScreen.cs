using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kebab.SceneFlow;
using System;

public class LoadingScreen : ALoadingScreen
{
    [SerializeField] private Image fill;

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    internal override void UpdateProgress(float progress)
    {
        fill.fillAmount = progress;
    }

}
