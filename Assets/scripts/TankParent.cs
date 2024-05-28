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
    protected Vector3 forward = new Vector3 (1,0,0); // Do not change! If you do, change all lines that calculate forward.
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
    
    // Used by AI
    protected void Rotate(float direction, Vector3 bound)
    {
        if (direction == -1) {  // A is pressed
            RotateLeft(bound);
        } else if (direction == 1) { // D is pressed
            RotateRight(bound);
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

    // Used by AI
    protected void RotateLeft(Vector3 bound)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(-rotation_speed, Vector3.up) * transform.rotation;
        Vector3 targetForward = targetRotation * new Vector3(-1, 0, 0);
        if (targetForward.x < bound.x) {
            transform.rotation = targetRotation;
            forward = bound;
        } else if (targetForward.x > bound.x) {
            transform.rotation = targetRotation;
            forward = transform.rotation * new Vector3(-1, 0, 0);
        }
        Debug.Log($"LEFT {forward} {bound}");
    }

    // Used by AI
    protected void RotateRight(Vector3 bound)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(rotation_speed, Vector3.up) * transform.rotation;
        Vector3 targetForward = targetRotation * new Vector3(-1, 0, 0);
        if (targetForward.x > bound.x) {
            transform.rotation = targetRotation;
            forward = bound;
        } else if (targetForward.x < bound.x) {
            transform.rotation = targetRotation;
            forward = transform.rotation * new Vector3(-1, 0, 0);
        }
        Debug.Log($"RIGHT {forward} {bound}");
    }
}
