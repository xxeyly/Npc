/// <summary>
/// 该脚本自动生成
/// </summary>

using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    public partial class ListenerFrameComponent
    {
        //监听生成开始
        [HideInInspector] public AtlasSceneComponent atlasSceneComponent = new AtlasSceneComponent();
        [HideInInspector] public AttributeCompositionSceneComponent attributeCompositionSceneComponent = new AttributeCompositionSceneComponent();
        [HideInInspector] public ItemSlotDataSaveSceneComponent itemSlotDataSaveSceneComponent = new ItemSlotDataSaveSceneComponent();
        [HideInInspector] public ItemAtlasDisplay itemAtlasDisplay = new ItemAtlasDisplay();
        [HideInInspector] public ItemAttributeShow itemAttributeShow = new ItemAttributeShow();
        [HideInInspector] public PersonalBelongings personalBelongings = new PersonalBelongings();
        [HideInInspector] public PurchaseItems purchaseItems = new PurchaseItems();
        [HideInInspector] public SmallPharmaceuticalApparatus smallPharmaceuticalApparatus = new SmallPharmaceuticalApparatus();
        [HideInInspector] public TempDragItemSlot tempDragItemSlot = new TempDragItemSlot();
        public class AtlasSceneComponent
        {
            public Item CreateItemByItemId(int arg0)
            {
                return Instance.ExecuteReturnEvent<int,Item>("AtlasSceneComponent","CreateItemByItemId",arg0);
            }
            public bool GetItemUnlocking(Item arg0)
            {
                return Instance.ExecuteReturnEvent<Item,bool>("AtlasSceneComponent","GetItemUnlocking",arg0);
            }
            public ItemAtlas GetItemAtlas()
            {
                return Instance.ExecuteReturnEvent<ItemAtlas>("AtlasSceneComponent","GetItemAtlas");
            }
            public List<Item> GetUnlockingItem()
            {
                return Instance.ExecuteReturnEvent<List<Item>>("AtlasSceneComponent","GetUnlockingItem");
            }
        }
        public class AttributeCompositionSceneComponent
        {
            public List<AttributeComposition> GetQualifiedAttributeComposition(List<BaseAttribute> arg0)
            {
                return Instance.ExecuteReturnEvent<List<BaseAttribute>,List<AttributeComposition>>("AttributeCompositionSceneComponent","GetQualifiedAttributeComposition",arg0);
            }
        }
        public class ItemSlotDataSaveSceneComponent
        {
            public void Load()
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent","Load");
            }
            public void Save()
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent","Save");
            }
            public void SetItemSlotDataSaveEvent(ItemSlotDataSave arg0)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent","SetItemSlotDataSaveEvent",arg0);
            }
            public void SetItemSlotDataLoadEvent(ItemSlotDataLoad arg0)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent","SetItemSlotDataLoadEvent",arg0);
            }
            public void SaveItemSlotSaveDataGroup(int arg0,List<ItemSlot> arg1)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent","SaveItemSlotSaveDataGroup",arg0,arg1);
            }
            public ItemSlotSaveDataGroup GetItemSlotSaveDataGroup(int arg0)
            {
                return Instance.ExecuteReturnEvent<int,ItemSlotSaveDataGroup>("ItemSlotDataSaveSceneComponent","GetItemSlotSaveDataGroup",arg0);
            }
            public void SaveSmallPharmaceuticalApparatusSaveDataGroup(int arg0,SmallPharmaceuticalApparatusSaveDataGroup arg1)
            {
                Instance.ExecuteEvent("ItemSlotDataSaveSceneComponent","SaveSmallPharmaceuticalApparatusSaveDataGroup",arg0,arg1);
            }
            public SmallPharmaceuticalApparatusSaveDataGroup GetSmallPharmaceuticalApparatusSaveDataGroup(int arg0)
            {
                return Instance.ExecuteReturnEvent<int,SmallPharmaceuticalApparatusSaveDataGroup>("ItemSlotDataSaveSceneComponent","GetSmallPharmaceuticalApparatusSaveDataGroup",arg0);
            }
        }
        public class ItemAtlasDisplay
        {
            public void SetSelectItemState(bool arg0)
            {
                Instance.ExecuteEvent("ItemAtlasDisplay","SetSelectItemState",arg0);
            }
            public void OnSelect(int arg0)
            {
                Instance.ExecuteEvent("ItemAtlasDisplay","OnSelect",arg0);
            }
            public void OnEnter(int arg0)
            {
                Instance.ExecuteEvent("ItemAtlasDisplay","OnEnter",arg0);
            }
            public void InitAtlas()
            {
                Instance.ExecuteEvent("ItemAtlasDisplay","InitAtlas");
            }
            public void AddSelectItemDelegate(SelectItemDelegate arg0)
            {
                Instance.ExecuteEvent("ItemAtlasDisplay","AddSelectItemDelegate",arg0);
            }
            public void RemoveSelectItemDelegate(SelectItemDelegate arg0)
            {
                Instance.ExecuteEvent("ItemAtlasDisplay","RemoveSelectItemDelegate",arg0);
            }
        }
        public class ItemAttributeShow
        {
            public void SetWindowDrag(bool arg0)
            {
                Instance.ExecuteEvent("ItemAttributeShow","SetWindowDrag",arg0);
            }
            public void ShowItemAttribute(Item arg0,Vector3 arg1)
            {
                Instance.ExecuteEvent("ItemAttributeShow","ShowItemAttribute",arg0,arg1);
            }
            public void HideItemAttribute()
            {
                Instance.ExecuteEvent("ItemAttributeShow","HideItemAttribute");
            }
        }
        public class PersonalBelongings
        {
            public void InitStorageItem()
            {
                Instance.ExecuteEvent("PersonalBelongings","InitStorageItem");
            }
            public bool AddItem(Item arg0)
            {
                return Instance.ExecuteReturnEvent<Item,bool>("PersonalBelongings","AddItem",arg0);
            }
            public void SetDragItemSlot(ItemSlot arg0)
            {
                Instance.ExecuteEvent("PersonalBelongings","SetDragItemSlot",arg0);
            }
            public void RemoveDragItemSlot()
            {
                Instance.ExecuteEvent("PersonalBelongings","RemoveDragItemSlot");
            }
        }
        public class PurchaseItems
        {
            public void AddNewItem(int arg0)
            {
                Instance.ExecuteEvent("PurchaseItems","AddNewItem",arg0);
            }
            public void OnSelect(int arg0)
            {
                Instance.ExecuteEvent("PurchaseItems","OnSelect",arg0);
            }
            public void OnEditorAttributeValue(ItemDemandItem arg0,AttributeValue arg1,Vector2 arg2)
            {
                Instance.ExecuteEvent("PurchaseItems","OnEditorAttributeValue",arg0,arg1,arg2);
            }
        }
        public class SmallPharmaceuticalApparatus
        {
            public void InitStorageItem()
            {
                Instance.ExecuteEvent("SmallPharmaceuticalApparatus","InitStorageItem");
            }
        }
        public class TempDragItemSlot
        {
            public bool GetDragSate()
            {
                return Instance.ExecuteReturnEvent<bool>("TempDragItemSlot","GetDragSate");
            }
            public void SetDragItemSlot(ItemSlot arg0)
            {
                Instance.ExecuteEvent("TempDragItemSlot","SetDragItemSlot",arg0);
            }
            public void SetEnterItemSlot(ItemSlot arg0)
            {
                Instance.ExecuteEvent("TempDragItemSlot","SetEnterItemSlot",arg0);
            }
            public void SetEnterItemSlotNull()
            {
                Instance.ExecuteEvent("TempDragItemSlot","SetEnterItemSlotNull");
            }
            public void RemoveDragItemSlot()
            {
                Instance.ExecuteEvent("TempDragItemSlot","RemoveDragItemSlot");
            }
        }

        //监听生成结束
    }
}