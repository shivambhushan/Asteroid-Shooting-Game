using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{

    public float Rspeed;
    //public float Rtime=50f;
    public float OrbitDegree;
    public Transform Target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0, Rspeed * Time.deltaTime, 0);
        //transform.Rotate(0, (360 / (Rtime * 60 * 60)) * Time.deltaTime , 0, Space.Self);
        transform.RotateAround(Target.position, Vector3.up, OrbitDegree);
	}
}
