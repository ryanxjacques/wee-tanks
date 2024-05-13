using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Comment's Date: 30th April 2024
 * The PlayerController class defines the player game object. It is a
 * derivative of the TankParent component. The PlayerController
 * communicates with the InputController and JumpFeature.
 */



[RequireComponent(typeof(JumpFeature))]
[RequireComponent(typeof(InputController))]
public class PlayerController : TankParent, IJumpable
{
    private JumpFeature _jumpFeature;
    private InputController _inputController;
    private AudioSource _audioSource;
    private Squished _squished;
    private Vector3 velocity;
    private Direction direction = new Direction();  //< Def. of Direction is in Entity.cs
    public event Action onPlanning;
    public event Action onJumping;
    public event Action onGround;

    // Attach Observers
    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _inputController.onButton += OnButtonObserver;
        _jumpFeature = GetComponent<JumpFeature>();
        _jumpFeature.InitializeJumpFeature(this);
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnButtonObserver(Button button)
    {
        if (button.name == "Drive")
        {
            SetState(State.IsDriving, button.isDown);
            direction.drive = button.value;
        }
        if (button.name == "Rotate")
        {
            SetState(State.IsRotating, button.isDown);
            direction.rotate = button.value; 
        }
        if (button.name == "Jump")
        {
            SetState(State.IsPlanning, button.isDown);
        }
        if (button.name == "Jump" && button.isDown == false && CheckState(State.OnGround))
        {
            _audioSource.Play();
            onJumping?.Invoke();
        }
    }

    // TODO: make a CollisionManager Component. Have it be responsible for reseting 
    // the rigidbody's angular velocity and x,z rotation.
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            SetState(State.OnGround, true);
            forward = transform.rotation * new Vector3(-1, 0, 0); // Recalibrate rotation
            onGround.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (CheckState(State.IsPlanning))
        {
            onPlanning?.Invoke();
        }
        if (CheckState(State.IsRotating))
        {
            Rotate(direction.rotate);
        }
        if (CheckStates((State.IsDriving, true), (State.OnGround, true), (State.IsPlanning, false)))
        {
            Drive(direction.drive);
        }
    }
}

