using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1f;

    void Update() {
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X")*sensitivity,0), Space.World);
        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y")*sensitivity, 0, 0));
    }
}