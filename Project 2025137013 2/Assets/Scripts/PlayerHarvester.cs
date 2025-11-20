using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public float rayDistance = 5f;       // 채집 가능 거리
    public LayerMask hitMask = ~0;       // 가능한 레이어 전부 다 (일단)
    public int toolDamage = 1;            // 타격 데미지
    public float hitCooldown = 0.15f;     // 연타 간격

    private float _nextHitTime;
    private Camera _cam;
    public Inventory inventory;         // 플레이어 인벤(없으면 자동 부착)
    InventoryUI invenUI;
    public GameObject selectedBlock;

    void Awake()
    {
        _cam = Camera.main;
        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();
        invenUI = FindObjectOfType<InventoryUI>();
    }

    void Update()
    {
        if (invenUI.selectedIndex < 0)
        {
            selectedBlock.transform.localScale = Vector3.zero;
            // 선택된 idx가 -1이면 수확모드
            if (Input.GetMouseButton(0) && Time.time >= _nextHitTime)
            {
                _nextHitTime = Time.time + hitCooldown;
            }
        }
        else
        {
            Ray rayDebug = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //화면 중앙
            if (Physics.Raycast(rayDebug, out var hitDebug, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawRay (hitDebug.point, hitDebug.normal, Color.red, 2f);
                Vector3Int placePos = AdjacentCellOnHitFace(hitDebug);
                selectedBlock.transform.localScale = Vector3.one;
                selectedBlock.transform.position = placePos;
                selectedBlock.transform.rotation = Quaternion.identity;
            }
            else
            {
                selectedBlock.transform.localScale = Vector3.zero;
            }

            if (Input.GetMouseButtonDown(0))
            {
                // 선택된 idx가 0 이상이면 설치 모드
                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //화면 중앙
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3Int placePos = AdjacentCellOnHitFace(hit);
                }
            }

        }
    }

    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position;
        Vector3 adjCenter = baseCenter + hit.normal; // 맞춘 블록의 줌심 (정수 좌표(x, y, z))
        return Vector3Int.RoundToInt(adjCenter); // 그 면의 바깥쪽으로 정확히 한 칸 이동
    }
}