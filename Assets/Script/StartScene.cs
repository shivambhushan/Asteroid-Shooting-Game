using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{

    public Text scorecard, highscorecard;
    int score, highscore;
    public Button button1;
    
    void Start()
    {
        display();
    }

    public void display()
    {
        score = Asteroid.score;
        highscore = Asteroid.highscore;
        scorecard.text = "Score: " + score.ToString();
        highscorecard.text = "Highscore: " + highscore.ToString();
    }

    public void onClick()
    {
        if (button1.interactable == true)
        {
            Time.timeScale = 1;
           
            SceneManager.LoadScene("Solar System");
        }
    }
}

