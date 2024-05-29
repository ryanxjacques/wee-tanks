using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Simple script to move the bullet forward.
public class BulletController : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField]
    private GameObject player;
    public float bulletSpeed;

    private void Start()
    {
        player = GameObject.Find("Player");
        _playerController = player.GetComponent<PlayerController>();
    }

    // Handles if the bullet hit a wall.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playerController.Death();
            
            // Spagetti code that makes it so the bullet is only destroyed by
            // the player when the player is on the ground.
            if (_playerController.CheckState(State.OnGround))
                Destroy(gameObject);
            return;
        }
        Destroy(gameObject); // Destroy the bullet
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += (bulletSpeed * transform.forward * Time.deltaTime);
    }
}
