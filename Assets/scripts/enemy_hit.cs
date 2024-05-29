using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_hit : MonoBehaviour
{
    private GameObject player;
    private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        _playerController = player.GetComponent<PlayerController>();
        _playerController.TargetFound();
    }

    // Update is called once per frame
    public void Death()
    {
        _playerController.TargetDown();
        Destroy(gameObject);   
    }
}
