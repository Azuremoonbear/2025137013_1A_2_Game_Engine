using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�⺻ ����")]
    public float health = 5f;
    public float moveSpeed = 2f;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    public void TakeDamage(float damage)
    {
        health -= damage; // ���� ��������ŭ ü�� ����
        Debug.Log("Enemy Hit! Current Health: " + health);

        // ü���� 0 ���ϰ� �Ǹ� Die �Լ� ȣ��
        if (health <= 0)
        {
            Die();
        }
    }

    // <<-- 3. �׾��� �� ����� �Լ� �߰�
    void Die()
    {
        Debug.Log("Enemy has been defeated!");
        // �� Enemy ���� ������Ʈ�� �ı�
        Destroy(gameObject);
    }
}
