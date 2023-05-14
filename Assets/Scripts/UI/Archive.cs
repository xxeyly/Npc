#region 引入

using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion 引入
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class Archive : BaseWindow
{
    #region 变量声明
    private Button _save;

    private Button _load;

    #endregion 变量声明
    public override void Init()
    {
    }

    protected override void InitView()
    {
       #region 变量查找
        BindUi(ref _save, "Save");
        BindUi(ref _load, "Load");
        #endregion 变量查找
    }

    protected override void InitListener()
    {
        #region 变量绑定
        BindListener(_save, EventTriggerType.PointerClick, OnSaveClick);
        BindListener(_load, EventTriggerType.PointerClick, OnLoadClick);
        #endregion 变量绑定
    }

    #region 变量方法
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
    #endregion 变量方法

    #region 自定义属性

    #endregion 自定义属性
}