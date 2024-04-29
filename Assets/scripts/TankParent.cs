using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankParent : MonoBehaviour
{
    protected Vector3 forward = new Vector3 (1,0,0);
    [Header("Tank Movement")]
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float rotation_speed = 10;

    protected void Drive(float direction)
    {
        if (direction == -1) {
            DriveBackward();
        } else if (direction == 1) {
            DriveForward();
        }
    }

    protected void Rotate(float direction)
    {
        if (direction == 1) {
            RotateLeft();
        } else if (direction == -1) {
            RotateRight();
        }
    }

    protected void DriveForward()
    {
        transform.position = transform.position + (speed * forward* Time.deltaTime);
    }

    protected void DriveBackward()
    {
        transform.position = transform.position - (speed * forward* Time.deltaTime);
    }

    protected void RotateLeft()
    {
        transform.rotation = Quaternion.AngleAxis(rotation_speed, Vector3.up) * transform.rotation;
        forward = Quaternion.AngleAxis(rotation_speed, Vector3.up) * forward;
    }

    protected void RotateRight()
    {
        transform.rotation = Quaternion.AngleAxis(-rotation_speed, Vector3.up) * transform.rotation;
        forward = Quaternion.AngleAxis(-rotation_speed, Vector3.up) * forward; 
    }
}
