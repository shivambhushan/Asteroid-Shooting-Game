using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover1 : MonoBehaviour
{
    public Rigidbody RigidObject;
    public float speed;

	void Start ()
    {
        //RigidObject = GetComponent<Rigidbody>();
        RigidObject.velocity = transform.forward * speed;
        
        
	}
    void Update()
    {

    }
}
