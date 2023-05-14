//引入开始

using UnityEngine.UI;
using UnityEngine.EventSystems;
//引入结束
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class Archive : BaseWindow
{
    //变量声明开始
    private Button _save;

    private Button _load;

    //变量声明结束
    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _save, "Save");
        BindUi(ref _load, "Load");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_save, EventTriggerType.PointerClick, OnSaveClick);
        BindListener(_load, EventTriggerType.PointerClick, OnLoadClick);
        //变量绑定结束
    }

    //变量方法开始
    private void OnSaveClick(BaseEventData targetObj)
    {
        Debug.Log("保存数据");
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.Save();
    }

    private void OnLoadClick(BaseEventData targetObj)
    {
        Debug.Log("读取数据");
        ListenerFrameComponent.Instance.itemSlotDataSaveSceneComponent.Load();
    }
    //变量方法结束

    //自定义属性开始

    //自定义属性结束
}