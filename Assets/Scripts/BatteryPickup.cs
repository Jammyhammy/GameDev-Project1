using UnityEngine;
using System.Collections;
public class BatteryPickup : MonoBehaviour
{
    public enum BatteryType {
        BAD,
        LOW,
        MED,
        HIGH
    }
    public BatteryType batteryType;
    public AudioSource goodaudio;
    public AudioSource badaudio;
    public bool isPicked = false;

    void Awake () {
        batteryType = (BatteryType)Random.Range(0, 4);
        switch (batteryType)
        {
            case BatteryType.BAD:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case BatteryType.LOW:
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case BatteryType.MED:
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case BatteryType.HIGH:
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                break;                                            
        }        
    }
    void Update () 
    {
        transform.Rotate (new Vector3(0, 30, 0) * Time.deltaTime);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !isPicked){
            isPicked = true;
            gameObject.GetComponent<Renderer>().enabled = false;
            ActivateBattery(other.gameObject);
            //gameObject.SetActive(false);
        }
    }

    void ActivateBattery(GameObject player) {
        var playerStats = player.GetComponent<PlayerCharacter>();
        switch (batteryType)
        {
            case BatteryType.BAD:
                badaudio.Play();
                playerStats.TakeDamage(10);
                break;
            case BatteryType.LOW:
                goodaudio.Play();
                playerStats.TakeDamage(-5);
                break;
            case BatteryType.MED:
                goodaudio.Play();
                playerStats.TakeDamage(-10);
                break;
            case BatteryType.HIGH:
                goodaudio.Play();
                playerStats.TakeDamage(-20);
                break;                                            
        }
    }
}