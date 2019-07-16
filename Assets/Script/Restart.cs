using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SSC;

public class Restart : MonoBehaviour {

    public Button button1;
    ///public Image Loading;
   // public Text LoadText;

    private void Start()
    {
       // Loading.enabled = false;
        //LoadText.enabled = false;
        Time.timeScale = 1;
    }

    public void onClick()
    {
        if (button1.interactable == true)
        {
            //Loading.enabled = true;
            ///LoadText.enabled = true;
            Time.timeScale = 1;
            SceneChangeManager.Instance.loadNextScene("Solar System");


        }
    }

    


}
