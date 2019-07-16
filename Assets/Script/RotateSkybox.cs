using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour {

    public float RotateSpeed;

    void Update ()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
	}
}
