using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XFramework;

[InfoBox("按键管理")]
public class KeyManagerSceneComponent : SceneComponent
{
    [LabelText("打开背包")] public KeyCode personalBelongings;

    public override void StartComponent()
    {
    }

    public override void InitComponent()
    {
    }

    public override void EndComponent()
    {
    }

    private void Update()
    {
        //背包
        if (Input.GetKeyDown(personalBelongings))
        {
            if (GetViewState(typeof(PersonalBelongings)))
            {
                HideView(typeof(PersonalBelongings));
            }
            else
            {
                ShowView(typeof(PersonalBelongings));
            }
        }
    }
}