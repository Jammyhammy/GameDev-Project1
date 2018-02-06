using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PlayerLaser : MonoBehaviour
{
    public void Awake() {
        Destroy(gameObject, 5);
    }
    public void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 100.0f;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyCharacter>();
            if(enemy.isVader)
            {
                enemy.TakeDamage(10);
            }
            else
            {
                enemy.TakeDamage(100);
                Destroy(gameObject);
            }
        }
    }
}
