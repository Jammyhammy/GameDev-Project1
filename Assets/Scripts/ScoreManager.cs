using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
//Handles all of the score and UI related functionality for the game. Also some helper functions for managing invincibility state.
public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance;

    public Text ScoreNumber;
    public Text HealthNumber;
    public Text SpareNumber;
    public int score;

    public float invincibilityTime = 10f;
    public float timeLeft;

    public Image timer1;
    public Image timer2;
    public bool hasInvincible = true;
    public bool isInvincible = false;
    public GameObject GameOver;
    public GameObject Win;

    public bool inBattle;
    public AudioSource normalBGM;
    public AudioSource attackBGM;
    public AudioSource invincibilityBGM;

    public List<GameObject> allEnemies;

    void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        score = 0;
        ScoreNumber.text = score.ToString();
        timeLeft = invincibilityTime;
        GameOver.SetActive(false);
        Win.SetActive(false);
    }
    void Update() {
        if(isInvincible) {
            timeLeft -= Time.deltaTime;
            var ratio = timeLeft / invincibilityTime;
            timer1.fillAmount = ratio;
            timer2.fillAmount = ratio;
            if(timeLeft <= 0) {
                isInvincible = false;
                normalBGM.mute = false;
                attackBGM.mute = false;
                invincibilityBGM.Stop();
                timer1.fillAmount = 1;
                timer2.fillAmount = 1;
                timer1.color = Color.red;
                timer2.color = Color.red;                    
            }
        }
        //Change when Vader can die to 0.
        if(allEnemies.Count <= 0) {
            YouWin();
        }
    }

    public void InBattle(){
        if(!inBattle) {
            inBattle = true;
            normalBGM.Stop();
            attackBGM.Play();
        }
    }

    public void NotInBattle() {
        if(inBattle) {
            if (!allEnemies.Any(x=>x.GetComponent<EnemyCharacter>().isInBattle))
            {
                inBattle = false;
                normalBGM.Play();
                attackBGM.Stop();                
            }
        }
    }

    public void AddScore() {
        score++;
        ScoreNumber.text = score.ToString();
    }
    public void YouInvincible() {
        hasInvincible = false;
        isInvincible = true;
        timer1.color = Color.cyan;
        timer2.color = Color.cyan;
        invincibilityBGM.Play();
        normalBGM.mute = true;
        attackBGM.mute = true;      
    }    

    public void YouDead() {
        GameOver.SetActive(true);
    }
    public void YouWin() {
        Win.SetActive(true);
    }

    public void NextLevel() {
        Application.LoadLevel("Base 1");
    }

    public void RestartGame() {
        Application.LoadLevel("lobby");
    }
}