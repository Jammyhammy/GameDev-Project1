using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
//Handles the AI for enemy characters, Vader, special abilities, etc.
public class EnemyCharacter : Character {

	public bool isVader = false;

	public int attackRange = 10;

	private PlayerCharacter player;

	private Animator animator;

	public bool isInBattle;
	private bool isAttacking;
	private float attackTimer;
	public float attackInterval = 3f;
	public float specialTimer;
	public float specialInterval = 15f;
	public ParticleSystem particleHit;

	public GameObject specialPrefab;
	public GameObject laserPrefab;
	public AudioSource specialSound;
	public AudioSource laserSound;
	public AudioSource DeathScream;

	public NavMeshAgent nav;

	public override void Start( ) {
		animator = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
		nav = GetComponent<NavMeshAgent>();
		player.allEnemies.Add(this.gameObject);
		ScoreManager.Instance.allEnemies.Add(this.gameObject);
		attackTimer = attackInterval;
		startingHealth = 20;
		if(isVader) {
			specialTimer = 6f;
			startingHealth = 1000;
			currentHealth = startingHealth;
		}
	}

	public void Update( ) {
		if(isDead) return;
		if ( attackTimer > 0 )
			attackTimer -= Time.deltaTime;
		if ( isVader && specialTimer > 0 )
			specialTimer -= Time.deltaTime;

		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		isAttacking = false;
		if( InRange(attackRange) ) {
			//Vector3 dirToPlayer = new Vector3(player.transform.position.x - gameObject.transform.position.x, 0.0f, player.transform.position.z - gameObject.transform.position.z);
			//gameObject.transform.rotation = Quaternion.LookRotation(dirToPlayer);

			Vector3 dir = (player.transform.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);			

			float distance = Vector3.Distance(this.transform.position, player.transform.position);
			if( distance > nav.stoppingDistance) {
				nav.SetDestination(player.transform.position);
				animator.SetFloat("Speed", 1.0f);
				animator.SetFloat("X", 1.0f);
				animator.SetFloat("Y", 1.0f);
			} else {
				nav.isStopped = true;
				nav.ResetPath();
				animator.SetFloat("Speed", 0.0f);
				animator.SetFloat("X", 0.0f);
				animator.SetFloat("Y", 0.0f);				
				//animator.SetFloat("Speed", 0.0f);
			}
			isInBattle = true;
			ScoreManager.Instance.InBattle();
		} else {
			if(isVader) specialTimer = 15f;
			//animator.SetBool("Aiming", false);
			isInBattle = false;
			ScoreManager.Instance.NotInBattle();
		}

		if ( InRange(attackRange) && attackTimer < 0 ) {
			attackTimer = attackInterval;
			animator.SetBool("Aiming", true);
			//animator.SetTrigger("Pickup");
			laserSound.Play();

			Vector3 dirToPlayer = new Vector3(player.transform.position.x - gameObject.transform.position.x, 0.0f, player.transform.position.z - gameObject.transform.position.z);
			Instantiate(laserPrefab, transform.position + transform.up * 1.0f, transform.rotation);

		}

		if ( isVader && InRange(attackRange) && specialTimer < 0 ) {
			specialTimer = specialInterval;
			animator.SetBool("Aiming", true);
			//animator.SetTrigger("Pickup");
			specialSound.Play();

			Vector3 dirToPlayer = new Vector3(player.transform.position.x - gameObject.transform.position.x, 0.0f, player.transform.position.z - gameObject.transform.position.z);
			Instantiate(specialPrefab, transform.position + transform.up * 1.0f + transform.forward * 2.0f, transform.rotation);

		}		
	}

	public bool InRange(float range) {
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
		particleHit.Play();
		base.TakeDamage(amount);
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
		Destroy(this.gameObject, 5);
		DeathScream.Play();
		ScoreManager.Instance.NotInBattle();
		ScoreManager.Instance.AddScore();
		ScoreManager.Instance.allEnemies.Remove(this.gameObject);
	}

}
