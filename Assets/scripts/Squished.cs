using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squished : MonoBehaviour
{
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Activate collision only when player is not on ground.
        if (other.gameObject.tag == "enemy" && !player.CheckState(State.OnGround))
            Destroy(other.gameObject);
    }
}
