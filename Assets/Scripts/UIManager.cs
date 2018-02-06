using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Main scene UI manage rthat handles the text effect as well as main menu functionality.
public class UIManager : MonoBehaviour {

	public Canvas gameCanvas;
	public Button startButton;

    public void StartGame(){
		StartCoroutine(StartCountDown());
    }

	IEnumerator StartCountDown() {
		yield return new WaitForSeconds(5.0f);
		gameCanvas.gameObject.SetActive(true);
		yield return new WaitForSeconds(30.0f);
		startButton.gameObject.SetActive(true);
	}
	
	public void StartScene() {
		Application.LoadLevel("testing");
	}

	void Start () {
	}
	
	void Update () {
		
	}
}
