using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite[] livesImage;
    public Image livesImageDisplay;
    public GameObject titleImage;

    public int score;
    public Text scoreText;

	// Use this for initialization
	void Start () {
        score = 0;
    }
	
	// Update is called once per frame
	void Update () {}

    public void UpDateLives(int currentLives)
    {
        Debug.Log("Player lives: " + currentLives);
        livesImageDisplay.sprite = livesImage[currentLives];
    }

    public void UpDateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    public void ShowTitleScreen() {
        titleImage.SetActive(true);
    }


    public void HideTitleScreen() {
        titleImage.SetActive(false);
        score = 0;
        scoreText.text = "Score: 0 ";
    }
}
