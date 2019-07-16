using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public float speed;
    public Rigidbody player;
    Vector3 pos;
    float width;

    private void Start()
    {
        pos = Camera.main.ViewportToWorldPoint(new Vector3(-6f, -8f, Camera.main.nearClipPlane));
        width = pos.z;
    }

    private void Update()
    {
        transform.Translate(Input.acceleration.x, 0, 0);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(player.position.x, -6.5f, 6.5f), 0.0f, Mathf.Clamp(player.position.z, -width, width));
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
    }

    //void FixedUpdate()
    //{
    //    Plane playerPlane = new Plane(Vector3.up, transform.position);
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    float hitdist = 0.0f;
    //    if (playerPlane.Raycast(ray, out hitdist))
    //    {
    //        Vector3 targetPoint = ray.GetPoint(hitdist);
    //        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
    //        //transform.position = targetPoint - transform.position;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    //    }
    //}
}
