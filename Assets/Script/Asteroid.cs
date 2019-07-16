using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour {

    public GameObject hazard;
    public Vector3 spawnValues;
    public float spawnWait, startWait, waveWait;
    public int hazardCount;
    public Text scoreText, highscoreText;
    static public int score, highscore;
    public Material material1, material2, material3;

    void Start ()
    {
        score = 0;
        UpdateScore();
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        highscoreText.text = "Highscore: " + highscore;
        StartCoroutine(SpawnWaves());
    }
	
	void Update ()
    {
        if (score > highscore)
        {
            highscore = score;
            highscoreText.text = "Highscore: " + highscore;
            PlayerPrefs.SetInt("highscore", highscore);
        }
        changeScreen();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.Save();
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(1, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "score: " + score;
    }

    void changeScreen()
    {
        if(score>30)
        {
            RenderSettings.skybox = material1;
            Mover.speed = 5f;
        }
        if(score>60)
        {
            RenderSettings.skybox = material2;
            Mover.speed = 7f;
        }
        if (score > 90)
        {
            RenderSettings.skybox = material3;
            Mover.speed = 9f;
        }
        if (score > 120)
        {
            RenderSettings.skybox = material1;
            Mover.speed = 11f;
        }
        if (score > 150)
        {
            RenderSettings.skybox = material2;
            Mover.speed = 13f;
        }
        if (score > 180)
        {
            RenderSettings.skybox = material3;
            Mover.speed = 15f;
        }
    }
}
