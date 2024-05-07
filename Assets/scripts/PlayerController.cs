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
public class Player_Controller : TankParent, IJumpable
{
    private JumpFeature _jumpFeature;
    private InputController _inputController;
    private AudioSource _audioSource;
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
        if (button.name == "Jump" && button.isDown == false)
        {
            _audioSource.Play();
            onJumping?.Invoke();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            SetState(State.OnGround, true);
            onGround.Invoke();
        }
    }

    private void Update()
    {
        if (CheckState(State.IsPlanning))
        {
            onPlanning?.Invoke();
        }
    }

    private void FixedUpdate()
    {
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

