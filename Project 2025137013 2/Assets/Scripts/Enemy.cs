using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("기본 설정")]
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
        health -= damage; // 받은 데미지만큼 체력 감소
        Debug.Log("Enemy Hit! Current Health: " + health);

        // 체력이 0 이하가 되면 Die 함수 호출
        if (health <= 0)
        {
            Die();
        }
    }

    // <<-- 3. 죽었을 때 실행될 함수 추가
    void Die()
    {
        Debug.Log("Enemy has been defeated!");
        // 이 Enemy 게임 오브젝트를 파괴
        Destroy(gameObject);
    }
}
