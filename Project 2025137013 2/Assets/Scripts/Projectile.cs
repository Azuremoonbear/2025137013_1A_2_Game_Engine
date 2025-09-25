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
        // 1. �ε��� ������Ʈ���� Enemy ��ũ��Ʈ�� �ִ��� ã�ƺ���.
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        // 2. ���� Enemy ��ũ��Ʈ�� ã�Ҵٸ� (��, ���� �ε����ٸ�)
        if (enemy != null)
        {
            // ã�� ���� TakeDamage �Լ��� ȣ���ؼ� �������� �ش�.
            enemy.TakeDamage(damage);
        }

        // 3. ���̵� ���̵� ��򰡿� �ε������� �߻�ü�� ������ �ı��ȴ�.
        Destroy(gameObject);
    }
}