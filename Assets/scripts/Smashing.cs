using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the AI script for Crasher. Attached to prefab 'Crasher'.
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
        if(player)
        {
            Vector3 playerpos = player.transform.position;
            playerpos.y = transform.position.y;
            transform.LookAt(playerpos);
            transform.Translate(Vector3.forward*move_speed*Time.deltaTime);
            transform.Rotate(0, 90, 0);
        }
    }   
}
