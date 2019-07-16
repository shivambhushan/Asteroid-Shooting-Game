using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetText : MonoBehaviour
{
    public Text Ptext;
    //public string EnterText;
    
    // Use this for initialization
	void Start ()
    {
        
        Ptext.enabled = false;
        Ptext.GetComponentInParent<Image>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        
        Ptext.enabled = true;
        Ptext.GetComponentInParent<Image>().enabled = true;

        Ptext.text = this.gameObject.name;
        StartCoroutine(WaitText());
    }
   
    IEnumerator WaitText()
    {
        yield return new WaitForSeconds(5f);
        Ptext.enabled = false;
        Ptext.GetComponentInParent<Image>().enabled = false;
    }

    
}
