using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_hit : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerController>().TargetFound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Death()
    {
        player.GetComponent<PlayerController>().TargetDown();
        Destroy(gameObject);
        
    }
}
