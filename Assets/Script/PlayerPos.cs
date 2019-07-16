using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate, nextFire;
    public AudioSource audio;
	
	void Update ()
    {
        MoveShot();
	}

    //void playerPos()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        Vector3 pos = Input.mousePosition;
    //        RaycastHit hit;
    //        Ray ray = Camera.main.ScreenPointToRay(pos);
    //        if (Physics.Raycast(ray, out hit, 100f))
    //        {
    //            transform.LookAt(hit.point);
    //        }
    //    }
    //}

    void MoveShot()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audio.Play();
        }
    }
}
