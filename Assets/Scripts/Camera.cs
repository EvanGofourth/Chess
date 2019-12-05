using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public bool rotate_around_object = true;
    public GameObject focus_object ;
    public float rotation_speed = 5.0f;
    [Range(0.01f, 1.0f)]
    public float smooth_factor = 0.5f;
    private Vector3 _camera_offset;
    private void LateUpdate()
    {
        if(rotate_around_object)
        {
            Quaternion cam_turn_angle = 
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotation_speed, Vector3.up);

            _camera_offset = cam_turn_angle * _camera_offset;
        }
        Vector3 new_pos = transform.position + _camera_offset;

        transform.position = Vector3.Slerp(transform.position, new_pos, smooth_factor);

        if (rotate_around_object)
            transform.LookAt(focus_object.transform);
    }

    private void Start()
    {
        focus_object = GameObject.Find("GamePiece");
    }
}
