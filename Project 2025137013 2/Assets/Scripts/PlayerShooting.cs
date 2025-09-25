using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefabs;
    public GameObject projectilePrefabs2;
    public Transform firePoint;
    Camera cam;

    private GameObject currentProjectile; // ���� ��� ���� ���⸦ ������ ����


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        // ������ ���۵Ǹ� ù ��° ���⸦ �⺻���� ����
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

        // �ϵ��ڵ��� projectilePrefabs ���, ���� ���õ� currentProjectile�� �߻�
        GameObject proj = Instantiate(currentProjectile, firePoint.position, Quaternion.LookRotation(direction));
    }

    void ChangeWeapon()
    {
        // ���� ���Ⱑ ù ��° ������
        if (currentProjectile == projectilePrefabs)
        {
            // �� ��° ����� ��ü
            currentProjectile = projectilePrefabs2;
            Debug.Log("���� ��ü: �� ��° ����");
        }
        // �ƴ϶�� (��, �� ��° ������)
        else
        {
            // �ٽ� ù ��° ����� ��ü
            currentProjectile = projectilePrefabs;
            Debug.Log("���� ��ü: ù ��° ����");
        }
    }
}
