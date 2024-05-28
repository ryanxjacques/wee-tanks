using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smashing : MonoBehaviour
{
    public int move_speed = 10;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward*move_speed*Time.deltaTime);
    }   
}
