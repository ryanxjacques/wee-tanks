using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * @brief Allow the player to squish enemies.
 * 
 * @data 14th May 2024
 *
 * This class allows a player to destroy enemies by jumping on them.
 */
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
        {
            other.gameObject.GetComponent<enemy_hit>().Death();
        }
    }
}
