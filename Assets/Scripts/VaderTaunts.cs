using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class VaderTaunts : MonoBehaviour
{
	private float tauntTimer;
	public float tauntInterval = 10f;

    public List<AudioSource> taunts;
    public EnemyCharacter myself;
    public void Start() {
        myself = GetComponent<EnemyCharacter>();
    }
    public void Update()
    {
        if(myself.isDead) return;
        if(myself.InRange(myself.attackRange)){
            if ( tauntTimer > 0 )
                tauntTimer -= Time.deltaTime;        

            if (tauntTimer <= 0 && taunts.Count > 0) {
                tauntTimer = tauntInterval;
                var random = Random.Range(0, taunts.Count - 1);
                taunts[random].Play();
            }
        }
    }
}