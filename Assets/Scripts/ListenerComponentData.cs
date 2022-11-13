/// <summary>
/// 该脚本自动生成
/// </summary>

using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    public partial class ListenerComponent
    {
        //监听生成开始
        public ItemSlotDataSaveSceneComponent itemSlotDataSaveSceneComponent = new ItemSlotDataSaveSceneComponent();
        public ItemAttributeShow itemAttributeShow = new ItemAttributeShow();
        public PersonalBelongings personalBelongings = new PersonalBelongings();
        public SmallPharmaceuticalApparatus smallPharmaceuticalApparatus = new SmallPharmaceuticalApparatus();
        public TempDragItemSlot tempDragItemSlot = new TempDragItemSlot();
        public AtlasSceneComponent atlasSceneComponent = new AtlasSceneComponent();
        public AttributeCompositionSceneComponent attributeCompositionSceneComponent = new AttributeCompositionSceneComponent();
        public class ItemSlotDataSaveSceneComponent
        {
            public void SaveItemSlotSaveDataGroup(int arg0, List<ItemSlot> arg1)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent_SaveItemSlotSaveDataGroup",arg0,arg1);
            }
            public void SaveSmallPharmaceuticalApparatusSaveDataGroup(int arg0, SmallPharmaceuticalApparatusSaveDataGroup arg1)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent_SaveSmallPharmaceuticalApparatusSaveDataGroup",arg0,arg1);
            }
            public void SetItemSlotDataSaveEvent(ItemSlotDataSave arg0)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent_SetItemSlotDataSaveEvent",arg0);
            }
            public void SetItemSlotDataLoadEvent(ItemSlotDataLoad arg0)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent_SetItemSlotDataLoadEvent",arg0);
            }
            public void Save()
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent_Save");
            }
            public void Load()
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent_Load");
            }
            public  ItemSlotSaveDataGroup GetItemSlotSaveDataGroup(int arg0)
            {
                return Instance.ExecuteReturnEvent<int, ItemSlotSaveDataGroup>("ItemSlotDataSaveSceneComponent_GetItemSlotSaveDataGroup",arg0);
            }
            public  SmallPharmaceuticalApparatusSaveDataGroup GetSmallPharmaceuticalApparatusSaveDataGroup(int arg0)
            {
                return Instance.ExecuteReturnEvent<int, SmallPharmaceuticalApparatusSaveDataGroup>("ItemSlotDataSaveSceneComponent_GetSmallPharmaceuticalApparatusSaveDataGroup",arg0);
            }
        }
        public class ItemAttributeShow
        {
            public void ShowItemAttribute(Item arg0, Vector3 arg1)
            {
                Instance.ExecuteEvent("ItemAttributeShow_ShowItemAttribute",arg0,arg1);
            }
            public void HideItemAttribute()
            {
                Instance.ExecuteEvent("ItemAttributeShow_HideItemAttribute");
            }
            public void SetWindowDrag(bool arg0)
            {
                Instance.ExecuteEvent("ItemAttributeShow_SetWindowDrag",arg0);
            }
        }
        public class PersonalBelongings
        {
            public void InitStorageItem()
            {
                Instance.ExecuteEvent("PersonalBelongings_InitStorageItem");
            }
            public void SetDragItemSlot(ItemSlot arg0)
            {
                Instance.ExecuteEvent("PersonalBelongings_SetDragItemSlot",arg0);
            }
            public void RemoveDragItemSlot()
            {
                Instance.ExecuteEvent("PersonalBelongings_RemoveDragItemSlot");
            }
            public  bool AddItem(Item arg0)
            {
                return Instance.ExecuteReturnEvent<Item, bool>("PersonalBelongings_AddItem",arg0);
            }
        }
        public class SmallPharmaceuticalApparatus
        {
            public void InitStorageItem()
            {
                Instance.ExecuteEvent("SmallPharmaceuticalApparatus_InitStorageItem");
            }
        }
        public class TempDragItemSlot
        {
            public void SetDragItemSlot(ItemSlot arg0)
            {
                Instance.ExecuteEvent("TempDragItemSlot_SetDragItemSlot",arg0);
            }
            public void RemoveDragItemSlot()
            {
                Instance.ExecuteEvent("TempDragItemSlot_RemoveDragItemSlot");
            }
            public void SetEnterItemSlot(ItemSlot arg0)
            {
                Instance.ExecuteEvent("TempDragItemSlot_SetEnterItemSlot",arg0);
            }
            public void SetEnterItemSlotNull()
            {
                Instance.ExecuteEvent("TempDragItemSlot_SetEnterItemSlotNull");
            }
            public bool GetDragSate()
            {
                return Instance.ExecuteReturnEvent<bool>("TempDragItemSlot_GetDragSate");
            }
        }
        public class AtlasSceneComponent
        {
            public  bool GetItemUnlocking(Item arg0)
            {
                return Instance.ExecuteReturnEvent<Item, bool>("AtlasSceneComponent_GetItemUnlocking",arg0);
            }
            public  Item CreateItemByItemId(int arg0)
            {
                return Instance.ExecuteReturnEvent<int, Item>("AtlasSceneComponent_CreateItemByItemId",arg0);
            }
        }
        public class AttributeCompositionSceneComponent
        {
            public  List<AttributeComposition> GetQualifiedAttributeComposition(List<Attribute> arg0)
            {
                return Instance.ExecuteReturnEvent<List<Attribute>, List<AttributeComposition>>("AttributeCompositionSceneComponent_GetQualifiedAttributeComposition",arg0);
            }
        }

        //监听生成结束
    }
}