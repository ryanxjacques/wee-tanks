using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Comment's Date: 30th April 2024
 * The PlayerController class defines the player game object. It is a
 * derivative of the TankParent component. The PlayerController
 * communicates with the InputController and JumpFeature.
 */
[RequireComponent(typeof(JumpFeature))]
[RequireComponent(typeof(InputController))]
public class Player_Controller : TankParent
{
    private JumpFeature _jumpFeature;
    private InputController _inputController;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private Vector3 velocity;
    private bool driveEnabled = true;
    private bool rotateEnabled = true;

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
        if (button.name == "Jump" && button.isDown == true)
        {
            driveEnabled = false;
            _jumpFeature.DrawProjection(transform.position, forward, speed);
        }
        if (button.name == "Jump" && button.isDown == false)
        {
            _audioSource.Play();
            rotateEnabled = false;
            _jumpFeature.Jump(_rigidbody, forward, speed);
        }
    }

    // Renable Movement after hitting the ground.
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            driveEnabled = true;
            rotateEnabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (rotateEnabled && rotate.isTrue)
        {
            Rotate(rotate.direction);
            // Rotate(keyData.x);
        }

        if (driveEnabled && drive.isTrue)
        {
            Drive(drive.direction);
        }
    }
}

