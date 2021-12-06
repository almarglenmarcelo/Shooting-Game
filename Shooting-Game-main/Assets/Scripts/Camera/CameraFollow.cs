using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f; //gano kabilis masundan ng cam ung player

    Vector3 offset; //distance between the target and the camera

    void Start()
    {
        offset = transform.position - target.position;

    }

    void FixedUpdate()
    {
        Vector3 targetCamPosition = target.position + offset; //store the next position of the camera
        transform.position = Vector3.Lerp(transform.position, targetCamPosition, smoothing * Time.deltaTime);
    }

}
