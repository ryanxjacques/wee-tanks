using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// public interface ITank<State> where State : Enum
// {
//     State state {get; set;}
// }


/* Used in Child Class (i.e. Player Controller). I get Unity 
  'ExtensionOfNativeClass' Error if this struct isn't delcare here. */

// [Flags]
// public enum State { }

/* Comment's Date: 30th April 2024
 * The TankParent Component defines how a tank moves. A tank should be able to
 * drive fowards and backwards, and rotate left and right.
 */
public class TankParent : Entity
{
    protected Vector3 forward = new Vector3 (1,0,0); // Do not change! If you do, change all lines that calculate forward.
    [Header("Tank Movement")]
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float rotation_speed = 10;

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
