using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the camera movement and player following
/// </summary>
public class Camera_Controller : MonoBehaviour
{
    public Transform target;//target to follow
    public float smoothness = 0.25f;// parameter for smooth movement

    public Vector3 offset = new Vector3(0f, 0f, -10f);//offset for the camera Z position

    private Vector3 speed = Vector3.zero;//camera speed
    
    void FixedUpdate()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, target.position + offset,ref speed, smoothness);//vector with a smooth interpolation for the camera movement
        transform.position = targetPosition;//sets the camera position
    }
}
