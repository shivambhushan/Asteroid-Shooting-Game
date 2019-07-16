using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SSC;

public class LifeDec : MonoBehaviour
{
    public Image life, life1, life2, life3;
    public Text scoreText, scorecard, highscorecard, gameover;
    int score, highscore;
    public Button button1;
    public Text hText, sText;
    public Image Panel;
    public Animator anim;
    public static int count = 0;
    bool flag = false;
    public static bool flag1 = false;
    public Heart heart;
    public GameObject planet;

    private void Awake()
    {
        Time.timeScale = 1;
        Panel.enabled = false;
    }
    public void Start ()
    {
        Time.timeScale = 1;
        Panel.enabled = false;
        hText.enabled = true;
        sText.enabled = true;
        flag = false;
        flag1 = false;
        button1.GetComponent<Button>();
        button1.GetComponentInChildren<Image>().enabled = false;
        button1.GetComponentInChildren<Text>().enabled = false;
        scorecard.enabled = false;
        gameover.enabled = false;
        highscorecard.enabled = false;
        button1.interactable = false;
        button1.enabled = false;
        planet.SetActive(true);

        if (count == 0)
        {
            flag1 = false;
            life3.enabled = true;
            life2.enabled = true;
            life1.enabled = true;
            life.enabled = true;
        }

        
    }

    void Update ()
    {
        if (Panel.enabled==false)
        {
            Time.timeScale = 1;
            StopCoroutine(Game());
        }
        lifeDec();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        flag1 = false;
        if (other.tag == "Bolt")
        {
            
            count++;


            if (count < 1)
            {
                count = 0;
                flag1 = false;
                //gameObject.SetActive(false);
            }
            else if (count == 0)
            {
                flag1 = false;


            }
            else if (count >= 1)
            {
                flag1 = true;
                //gameObject.SetActive(true);

            }
            
            print("Bolt" + count);
        }

        
        

        if (other.tag=="Heart")
        {
            count--;
            
            LifeInc();
            if (count == -1)
            {
                flag1 = false;
                count = 0;
            }
            else if (count < 1)
            {
                count = 0;
                flag1 = false;
            }
            else if (count == 0)
            {
                flag1 = false;
                life3.enabled = true;
                life2.enabled = true;
                life1.enabled = true;
                life.enabled = true;
            }
            else if (life3.enabled == true && life2.enabled == true && life1.enabled == true && life.enabled == true)
            {
                flag1 = false;
            }
            print(count);
        }
        SaveLife();
    }

   
    void LifeInc()
    {
        if(count < 0)
        {
            flag1 = false;
        }
        if(count==0)
        {
            life3.enabled = true;
            flag1 = false;
        }
        if (count == 1)
        {
            life2.enabled = true;
        }
        if (count == 2)
        {
            life1.enabled = true;
        }
        if (count == 3)
        {
            print("hii");
        }
    }

    public void display()
    {
        scorecard.enabled = true;
        highscorecard.enabled = true;
        gameover.enabled = true;
        button1.enabled = true;
        button1.interactable = true;
        button1.GetComponentInChildren<Image>().enabled = true;
        button1.GetComponentInChildren<Text>().enabled = true;
        score = Asteroid.score;
        highscore = Asteroid.highscore;
        scorecard.text = "Score: " + score.ToString();
        highscorecard.text = "Highscore: " + highscore.ToString();
        Panel.enabled = true;
        flag = true;
        anim.Play("Zoom");
        
    }

    public void onClick()
    {
        if (button1.interactable == true)
        {
            
            StopCoroutine(Game());
            Time.timeScale = 1;
            SceneChangeManager.Instance.loadNextScene("Restart");

        }
    }

   
    void lifeDec()
    {
        if(count==1)
        {
            
            life3.enabled = false;
        }
        else if(count==2)
        {
            life2.enabled = false;
        }
        else if (count == 3)
        {
            life1.enabled = false;
        }
        else if (count == 4)
        {
            
            life.enabled = false;
            
            hText.enabled = false;
            sText.enabled = false;
            display();
            
            if(Panel.enabled==true)
            {
                count = 0;
                //Time.timeScale = 0;
                StartCoroutine(Game());
            }
        }

        
       
        //else if(count==5)
        //{
        //    gameObject.SetActive(false);
        //    hText.enabled = false;
        //    sText.enabled = false;
        //    display();
        //}
    }

    IEnumerator Game()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        planet.SetActive(false);
        Time.timeScale = 0;
    }

    public void SaveLife()
    {
        if (flag1 == true && count > 0) 
        {
            heart.LifeSave();
            
            //heart.enabled = true;
        }
        else if (flag1==false && count == 0)
        {
            //print("HEART");
            heart.StopAllCoroutines();
            
            //heart.enabled = false;
        }
    }
}
