using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VaderScript : Character {

	public float speed;

	public int attackRange = 10;

	private PlayerCharacter player;

	private Animator animator;

	private bool isFoundPlayer;
	private bool isAttacking;
	private float attackTimer;
	public float attackInterval = 3f;

	public GameObject laserPrefab;
	public AudioSource laserSound;
	public AudioSource DarthWilhelmScream;


	public override void Start( ) {
		animator = GetComponent<Animator>( );
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
		player.allEnemies.Add(this.gameObject);
		ScoreManager.Instance.allEnemies.Add(this.gameObject);
		attackTimer = attackInterval;
		startingHealth = 20;
	}

	public void Update( ) {
		if(isDead) return;
		if ( attackTimer > 0 )
			attackTimer -= Time.deltaTime;

		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		isAttacking = false;
		if( InRange(attackRange) ) {
			Vector3 dirToPlayer = new Vector3(player.transform.position.x - gameObject.transform.position.x, 0.0f, player.transform.position.z - gameObject.transform.position.z);
			gameObject.transform.rotation = Quaternion.LookRotation(dirToPlayer);
		} else {
			animator.SetBool("Aiming", false);
		}

		if ( InRange(attackRange) && attackTimer < 0 ) {
			attackTimer = attackInterval;
			animator.SetBool("Aiming", true);
			animator.SetTrigger("Pickup");
			laserSound.Play();
			Vector3 dirToPlayer = new Vector3(player.transform.position.x - gameObject.transform.position.x, 0.0f, player.transform.position.z - gameObject.transform.position.z);
			Instantiate(laserPrefab, transform.position + transform.up * 1.0f, transform.rotation);
			switch (Random.Range(0, 5))
			{
			case 4:
				animator.Play ("TwoHandSpellCasting", -1, 0f);
				break;
			case 3:
				animator.Play ("LookAround", -1, 0f);
				break;
			case 2:
				animator.Play ("Jump", -1, 0f);
				break;
			case 1:
				animator.Play ("GreatSwordKick", -1, 0f);
				break;
			default:
				animator.Play ("Idle", -1, 0f);
				break;
			}
		}



	}

	bool InRange(float range) {
		var distance = Vector3.Distance(this.transform.position, player.transform.position);
		if ( distance < range )
			return true;
		return false;
	}

	public void BeAttacked() {

		if ( isDead )
			return;

		int damage = (int)Random.Range(100, 200);

	}

	public override void TakeDamage(int amount){
		if (isDead ) return;
		base.TakeDamage(amount);
		animator.Play ("FlyingBack", -1, 0f);
		if(currentHealth <= 0)
		{
			isDead = true;
			Death();
		}
	}

	public void Attack( ) {

		// float distance = Vector3.Distance(player.transform.position, navAgent.nextPosition);

		// Vector3 dir = ( player.transform.position - transform.position ).normalized;

		// float direction = Vector3.Dot(transform.forward, dir);

		// if ( direction > 0 && distance < attribute.attackDistance ) {
		// 	player.BeAttacked( );
		// } else {

		// }

	}

	private void Death( ) {
		animator.SetBool("Dead", true);
		DarthWilhelmScream.Play();
		Destroy(this.gameObject, 5);
		ScoreManager.Instance.AddScore();
		ScoreManager.Instance.allEnemies.Remove(this.gameObject);
	}

}
