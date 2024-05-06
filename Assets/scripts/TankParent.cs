using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Movement1D
{
    public bool isTrue;
    public float direction;
    public Movement1D(bool isTrue, float direction)
    {
        this.isTrue = isTrue;
        this.direction = direction;
    }
}

/* Comment's Date: 30th April 2024
 * The TankParent Component defines how a tank moves. A tank should be able to
 * drive fowards and backwards, and rotate left and right.
 */
public class TankParent : MonoBehaviour
{
    protected Vector3 forward = new Vector3 (1,0,0);
    [Header("Tank Movement")]
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float rotation_speed = 10;

    protected Movement1D rotate = new Movement1D(false, 0f);
    protected Movement1D drive = new Movement1D(false, 0f);

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
        forward = Quaternion.AngleAxis(-rotation_speed, Vector3.up) * forward;
    }

    protected void RotateRight()
    {
        transform.rotation = Quaternion.AngleAxis(rotation_speed, Vector3.up) * transform.rotation;
        forward = Quaternion.AngleAxis(rotation_speed, Vector3.up) * forward; 
    }
}
