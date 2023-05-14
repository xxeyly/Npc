#region 引入

#endregion 引入

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XFramework;

public class ShortcutBar : BaseWindow
{
    #region 变量声明
    private List<ItemSlot> _shortcutBarItermContent;

    #endregion 变量声明
    public override void Init()
    {
    }

    protected override void InitView()
    {
       #region 变量查找
        BindUi(ref _shortcutBarItermContent, "ShortcutBarItermContent");
        for (int i = 0; i < _shortcutBarItermContent.Count; i++)
        {
            _shortcutBarItermContent[i].ViewStartInit();
            _shortcutBarItermContent[i].InitData(i);
        }
        #endregion 变量查找
    }

    protected override void InitListener()
    {
        #region 变量绑定

        #endregion 变量绑定
    }

    #region 变量方法

    #endregion 变量方法

    #region 自定义属性

    #endregion 自定义属性
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ItemTrigger(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))

        {
            ItemTrigger(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ItemTrigger(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ItemTrigger(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ItemTrigger(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ItemTrigger(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ItemTrigger(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ItemTrigger(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ItemTrigger(9);
        }
    }

    private void ItemTrigger(int index)
    {
        //按键是从1开始
        ItemSlot itemSlot = _shortcutBarItermContent[index - 1];
        if (itemSlot.item != null)
        {
            Debug.Log(itemSlot.item.ItemName);
        }
    }
}