using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Comment's Date: 30th April 2024
 * The PlayerController class defines the player game object. It is a
 * derivative of the TankParent component. The PlayerController
 * communicates with the InputController and JumpFeature.
 */

public enum PlayerState
{
    ON_GROUND,
    IN_AIR,
}

[RequireComponent(typeof(JumpFeature))]
[RequireComponent(typeof(InputController))]
public class Player_Controller : TankParent
{
    private JumpFeature _jumpFeature;
    private InputController _inputController;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private Vector3 velocity;
    private PlayerState state = PlayerState.ON_GROUND;
    private bool isJumping = false;

    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _inputController.onButton += OnButtonObserver;
        _jumpFeature = GetComponent<JumpFeature>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnButtonObserver(Button button)
    {
        if (button.name == "Drive")
        {
            drive.isTrue = button.isDown;
            drive.direction = button.value;
        }
        if (button.name == "Rotate")
        {
            rotate.isTrue = button.isDown;
            rotate.direction = button.value;    
        }
        if (button.name == "Jump" && state == PlayerState.ON_GROUND)
        {
            isJumping = button.isDown;
            Jump();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            state = PlayerState.ON_GROUND;
        }
    }

    private void Jump()
    {
        if (state == PlayerState.IN_AIR)
            return;
        
        if (isJumping == false && state == PlayerState.ON_GROUND)
        {
            state = PlayerState.IN_AIR;
            _jumpFeature.Jump(_rigidbody, forward, speed);
            _audioSource.Play();
            return;
        }

        _jumpFeature.DrawProjection(transform.position, forward, speed);
    }

    private void Update()
    {
        if (isJumping)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (rotate.isTrue)
        {
            Rotate(rotate.direction);
        }
        if (drive.isTrue && !isJumping && state == PlayerState.ON_GROUND)
        {
            Drive(drive.direction);
        }
    }
}

