using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefabs;
    public GameObject projectilePrefabs2;
    public Transform firePoint;
    Camera cam;

    private GameObject currentProjectile; // 현재 사용 중인 무기를 저장할 변수


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        // 게임이 시작되면 첫 번째 무기를 기본으로 설정
        currentProjectile = projectilePrefabs;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeWeapon();
        }

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        // 하드코딩된 projectilePrefabs 대신, 현재 선택된 currentProjectile을 발사
        GameObject proj = Instantiate(currentProjectile, firePoint.position, Quaternion.LookRotation(direction));
    }

    void ChangeWeapon()
    {
        // 현재 무기가 첫 번째 무기라면
        if (currentProjectile == projectilePrefabs)
        {
            // 두 번째 무기로 교체
            currentProjectile = projectilePrefabs2;
            Debug.Log("무기 교체: 두 번째 무기");
        }
        // 아니라면 (즉, 두 번째 무기라면)
        else
        {
            // 다시 첫 번째 무기로 교체
            currentProjectile = projectilePrefabs;
            Debug.Log("무기 교체: 첫 번째 무기");
        }
    }
}
