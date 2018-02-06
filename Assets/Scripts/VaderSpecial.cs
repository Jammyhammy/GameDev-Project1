using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
// The controller for when vader does his special move. A giant orb of hurt.

public class VaderSpecial : MonoBehaviour
{
    public void Awake() {
        Destroy(gameObject, 20);
    }
    public void Update()
    {
        transform.position += transform.forward * Time.deltaTime * Random.Range(1.0f, 10f);
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
            player.TakeDamage(20);
            Destroy(gameObject);
        }        
    }    
}