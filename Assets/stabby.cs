using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stabby : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        // Activate collision only when player is not on ground.
        if (other.gameObject.tag == "player")
        {
            other.gameObject.GetComponent<PlayerController>().Death();
        }
    }
}
