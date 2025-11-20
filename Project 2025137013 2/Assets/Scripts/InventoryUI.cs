using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region
    public Sprite dirtSprite;
    public Sprite diamondSprite;
    public Sprite grassSprite;
    public Sprite waterSprite;
    public Sprite cloudSprite;
    #endregion
    public List<Transform> Slot = new List<Transform>();
    public GameObject SlotItem;
    List<GameObject> items = new List<GameObject>();
    // 인벤토리 호출 시 업데이트

    public int selectedIndex = -1;
    public void UpdateInventory(Inventory myInven)
    {
        //기존 슬롯 초기화
        foreach(var slotItems in items)
        {
            Destroy(slotItems);     //시작할 때 슬롯 아이템의 GameObject 삭제
        }
        items.Clear();  //시작할 때 아이템 리스트 클리어
        //내 인벤토리 데이터를 전체 탐색
        int idx = 0; //접근할 슬롯의 인덱스
        foreach (var item in myInven.items)
        {
#region
            var go = Instantiate(SlotItem, Slot[idx].transform);
            go.transform.localPosition = Vector3.zero;
            SlotItemPrefab sItem = go.GetComponent<SlotItemPrefab>();
            items.Add(go); //아이템 리스트에 하나 추가
#endregion
            switch (item.Key) //각 케이스별로 아이템 추가
            {
                case BlockType.Dirt:
                    sItem.ItemSetting(dirtSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case BlockType.Grass:
                    sItem.ItemSetting(grassSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case BlockType.Water:
                    sItem.ItemSetting(waterSprite, "x" + item.Value.ToString(), item.Key);
                    break;
            }
            idx++; //인덱스 한 칸 추가
        }
    }

    private void Update()
    {
        for (int i = 0; i < Mathf.Min(9, Slot.Count); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSelectedIndex(i);
            }
        }
    }

    public void SetSelectedIndex(int idx)
    {
        ResetSelection();
        if (selectedIndex == idx)
        {
            selectedIndex = -1;
        }
        else
        {
            if (idx >= items.Count)
            {
                selectedIndex = -1; 
            }
            else
            {
                SetSelection(idx);
                selectedIndex = idx;
            }
        }
    }

    public void ResetSelection()
    {
        foreach (var slot in Slot)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }

    void SetSelection(int _idx)
    {
        Slot[_idx].GetComponent<Image>().color = Color.yellow;
    }

    public BlockType GetInventorySlot()
    {
        return items[selectedIndex].GetComponent<SlotItemPrefab>().blockType;
    }
}