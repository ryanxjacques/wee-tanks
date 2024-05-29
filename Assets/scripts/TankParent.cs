using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/**
 * @brief Abstract class for tank behaving entities.
 * 
 * @data 14th May 2024
 *
 * This class defines how a tank should move.
 */
public class TankParent : Entity
{
    [SerializeField]
    protected Vector3 forward = new Vector3 (1,0,0); // Do not change! If you do, change all lines that calculate forward.
    [SerializeField]
    protected Quaternion rotation;
    [Header("Tank Movement")]
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float rotation_speed = 10f;

    public float GetSpeed()
    {
        return speed;
    }

    protected void Drive(float direction)
    {
        if (direction == -1) { // S is pressed
            DriveBackward();
        } else if (direction == 1) { // W is pressed
            DriveForward();
        }
    }

    protected void Rotate(float direction)
    {
        if (direction == -1) {  // A is pressed
            RotateLeft();
        } else if (direction == 1) { // D is pressed
            RotateRight();
        }
    }

    protected void DriveForward()
    {
        transform.position = transform.position + (speed * forward * Time.deltaTime);
    }

    protected void DriveBackward()
    {
        transform.position = transform.position - (speed * forward * Time.deltaTime);
    }

    protected void RotateLeft()
    {
        transform.rotation = Quaternion.AngleAxis(-rotation_speed, Vector3.up) * transform.rotation;
        forward = transform.rotation * new Vector3(-1, 0, 0);
    }

    protected void RotateRight()
    {
        transform.rotation = Quaternion.AngleAxis(rotation_speed, Vector3.up) * transform.rotation;
        forward = transform.rotation * new Vector3(-1, 0, 0);
    }
}
