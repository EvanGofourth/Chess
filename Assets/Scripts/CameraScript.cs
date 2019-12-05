using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 1.5f;
    private float X;
    private float Y;

    public float panSpeed = 15f;

    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

    public bool white_player;

    private float ClampAngle(float angle, float min, float max){
 
     if (angle<90 || angle>270){       // if angle in the critic region...
         if (angle>180) angle -= 360;  // convert all angles to -180..+180
         if (max>180) max -= 360;
         if (min>180) min -= 360;
     }
     angle = Mathf.Clamp(angle, min, max);
     if (angle<0) angle += 360;  // if angle negative, convert to 0..360
     return angle;
    }

    private void Start()
    {

    }

    void Update()
    {
        //TODO: Check if this works...
        float fov = this.GetComponentInChildren<Camera>().fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;

        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = ClampAngle(transform.rotation.eulerAngles.x,-5f, 65f);
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }

        // Code to allow WASD movement of camera.
        Vector3 pos = transform.position;
        Vector3 rot = transform.localEulerAngles;

        if (Input.GetKey("w"))
        {

            Vector3 moveDirection = Vector3.Scale(this.GetComponentInChildren<Camera>().transform.forward, new Vector3(1, 0, 1));
            pos += (moveDirection * panSpeed * Time.deltaTime);

            
        }
        if (Input.GetKey("s"))
        {

            Vector3 moveDirection = Vector3.Scale(this.GetComponentInChildren<Camera>().transform.forward, new Vector3(1, 0, 1));
            pos -= (moveDirection * panSpeed * Time.deltaTime);

            
        }
        if (Input.GetKey("d"))
        {
            Vector3 moveDirection = Vector3.Scale(this.GetComponentInChildren<Camera>().transform.right, new Vector3(1, 0, 1));
            pos += (moveDirection * panSpeed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            
            Vector3 moveDirection = Vector3.Scale(this.GetComponentInChildren<Camera>().transform.right, new Vector3(1, 0, 1));
            pos -= (moveDirection * panSpeed * Time.deltaTime);
        }
        transform.position = pos;
    }
}
