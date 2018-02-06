using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class EnemyLaser : MonoBehaviour
{
    public void Awake() {
        Destroy(gameObject, 5);
    }
    public void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 5.0f;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player")){
            var player = other.gameObject.GetComponent<PlayerCharacter>();
            player.TakeDamage(10);
            Destroy(gameObject);
        }
    }    

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            var player = other.gameObject.GetComponent<PlayerCharacter>();
            player.TakeDamage(10);
            Destroy(gameObject);
        }        
    }    
}