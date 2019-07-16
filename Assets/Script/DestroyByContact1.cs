using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact1 : MonoBehaviour
{
    public GameObject explosion;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Heart")
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
        }
        if (other.tag == "Planet")
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
