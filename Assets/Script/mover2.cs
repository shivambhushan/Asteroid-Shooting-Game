using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover2 : MonoBehaviour {
    public float speed;
    //public LifeDec lifeline;

    void Update()
    {
        transform.position += new Vector3(1, 0, 0) * -speed * Time.deltaTime;
        //LifeDec.count = 0;
        if (LifeDec.count < 1)
        {
            LifeDec.flag1 = false;
            LifeDec.count = 0;

            //print("hello");
        }


    }
}
