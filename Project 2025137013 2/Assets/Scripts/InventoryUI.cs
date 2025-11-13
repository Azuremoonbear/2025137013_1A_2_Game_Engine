using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> Slot = new List<Transform>();
    public GameObject SlotItem;
    List<GameObject> items = new List<GameObject>();
    // 인벤토리 호출 시 업데이트

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
            var go = Instantiate(SlotItem, Slot[idx].transform);
            go.transform.localPosition = Vector3.zero;
            SlotItemPrefab sItem = go.GetComponent<SlotItemPrefab>();
            items.Add(go); //아이템 리스트에 하나 추가

            //switch (item.Key) //각 케이스별로 아이템 추가
            {
                //case BlockType.Dirt:
            }
            idx++; //인덱스 한 칸 추가
        }
    }
}