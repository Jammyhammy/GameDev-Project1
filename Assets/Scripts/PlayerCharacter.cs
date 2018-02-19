using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//Handles all of the health and damage related functionality for player.

public class PlayerCharacter : Character
{
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public bool lightSaberDepleted = false;
    public List<GameObject> allEnemies;
    public AudioSource hurtSound;
    public AudioSource lowHealthSound;
    

    bool damaged;
    bool healed;
    bool hasPalette = true;

    public void Awake()
    {
        startingHealth = 100;
		currentHealth = startingHealth;
        isDead = false;
        healthSlider.value = currentHealth;
        lowHealthSound.enabled = false;
    }
    public void Update() {
        if(damaged) {
            damageImage.color = flashColour;
        } else if (healed) {
            damageImage.color = Color.blue;
        }
        else {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        healed = false;
		if (currentHealth <= 0 && isDead) {
			lowHealthSound.enabled = true;
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		else if (currentHealth <= 0 && !isDead) {
			lowHealthSound.enabled = true;
			damageImage.color = Color.Lerp (damageImage.color, Color.green, flashSpeed * Time.deltaTime);
		}
        else {
            lowHealthSound.enabled = false;
        }
    }

    public override void TakeDamage(int amount){ 
        if(!ScoreManager.Instance.isInvincible) {
            if(isDead)
                return;
            if(amount > 0) {
                hurtSound.Play();
                damaged = true;
            } else {
                healed = true;
            }
            currentHealth -= amount;
            if(currentHealth > 0) lightSaberDepleted = false;
            if(currentHealth <= 0 && ScoreManager.Instance.hasInvincible)
            {
                if(currentHealth < 0 ) currentHealth = 0;
                lightSaberDepleted = true;
                damageImage.color = new Color(0f, 100f, 190f, 30f);
                ScoreManager.Instance.YouInvincible();  
            }
            else if (currentHealth <= 0 && lightSaberDepleted) {
                ScoreManager.Instance.YouDead();
                isDead = true;
            }            
            else if (currentHealth <= 0) {
                lightSaberDepleted = true;
                if(currentHealth < 0 ) currentHealth = 0;
            }
        } else {
            if(amount < 0) currentHealth -= amount;
            if(currentHealth > 0) lightSaberDepleted = false;
        }
        UpdateHealth(currentHealth);
    } 
    public void UpdateHealth(int health)
    {
        if(health > 100) {
            healthSlider.value = 100;
            var spare = health - 100;
            ScoreManager.Instance.HealthNumber.text = "100";
            ScoreManager.Instance.SpareNumber.text = spare.ToString();
        } else {
            healthSlider.value = health;
            ScoreManager.Instance.SpareNumber.text = "0";
            ScoreManager.Instance.HealthNumber.text = health.ToString();
        }
    }
}