using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float damage = 1f;
    public float speed = 20f;
    public float lifetime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. 부딪힌 오브젝트에서 Enemy 스크립트가 있는지 찾아본다.
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        // 2. 만약 Enemy 스크립트를 찾았다면 (즉, 적과 부딪혔다면)
        if (enemy != null)
        {
            // 찾은 적의 TakeDamage 함수를 호출해서 데미지를 준다.
            enemy.TakeDamage(damage);
        }

        // 3. 적이든 벽이든 어딘가에 부딪혔으면 발사체는 스스로 파괴된다.
        Destroy(gameObject);
    }
}