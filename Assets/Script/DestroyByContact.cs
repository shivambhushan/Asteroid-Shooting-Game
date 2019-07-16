using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public int newScoreValue;
    private Asteroid scorecalc;
    //public Rigidbody player;
    //private Renderer rend;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            scorecalc = gameControllerObject.GetComponent<Asteroid>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Planet")
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
        }
        if (other.tag == "Bolt")
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            scorecalc.AddScore(newScoreValue);
        }


        Destroy(gameObject);
        
    }
}
