using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Controller : MonoBehaviour
{
    public Vector3 forward = new Vector3 (1,0,0);
    public int speed = 1;
    public int rotation_speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        forward = Vector3.Normalize(forward);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     if(Input.GetKey("w"))
     {
        transform.position = transform.position + (speed * forward* Time.deltaTime);
     }   
    if(Input.GetKey("s"))
     {
        transform.position = transform.position - (speed * forward* Time.deltaTime);
     }   
    if(Input.GetKey("a"))
     {
        transform.rotation = Quaternion.AngleAxis(rotation_speed, Vector3.up) * transform.rotation;
        forward = Quaternion.AngleAxis(rotation_speed, Vector3.up) * forward; 
     }   
    if(Input.GetKey("d"))
     {
        transform.rotation = Quaternion.AngleAxis(-rotation_speed, Vector3.up) * transform.rotation;
        forward = Quaternion.AngleAxis(-rotation_speed, Vector3.up) * forward; 
     }   
    }
}
