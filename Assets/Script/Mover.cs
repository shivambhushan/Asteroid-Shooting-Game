using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public static float speed = 3f;
   
    void Update()
    {
        transform.position += new Vector3(1,0,0)  * -speed * Time.deltaTime;
        
    }
}
