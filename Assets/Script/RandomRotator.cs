using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public float tumble;
    public Rigidbody asteroid;

   void Start()
    {
        asteroid.angularVelocity = Random.insideUnitSphere * tumble;
    }
}
