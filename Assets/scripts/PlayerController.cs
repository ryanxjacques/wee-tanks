using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/**
 * @brief The player controller.
 * 
 * @data 14th May 2024
 *
 * This class defines the player's behavior as well as game audio. The player
 * controller's main responsibility is to listen to the game environment and
 * interact with different component API (i.e. JumpFeature, InputController,
 * TankParent, Entity).
 */
[RequireComponent(typeof(JumpFeature))]
[RequireComponent(typeof(InputController))]
public class PlayerController : TankParent, IJumpable
{
    private JumpFeature _jumpFeature;
    private InputController _inputController;
    private AudioSource _audioSource;
    public AudioClip jump;
    public AudioClip move;
    public AudioClip ost;
    private Squished _squished;
    private Vector3 velocity;
    private Direction direction = new Direction();  //< Def. of Direction is in Entity.cs
    public event Action onPlanning;
    public event Action onJumping;
    public event Action onGround;

    [Header("Enemies Remaining")] [SerializeField]
    public int targets_remaining;    //< Represents the total number of enemies in scene.

    // Attach Observers
    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _inputController.onButton += OnButtonObserver;
        _jumpFeature = GetComponent<JumpFeature>();
        _jumpFeature.InitializeJumpFeature(this);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(ost,0.7f);
        targets_remaining = 0;
    }

    private void OnButtonObserver(Button button)
    {
        if (button.name == "Drive")
        {
            SetState(State.IsDriving, button.isDown);
            direction.drive = button.value;
            _audioSource.PlayOneShot(move,0.7f);
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
            _audioSource.PlayOneShot(jump,0.7f);
            onJumping?.Invoke();
        }
    }

    // Increment the global enemy counter.
    public void TargetFound()
    {
        targets_remaining +=1;
    }
    
    // Decrement the enemy counter.
    public void TargetDown()
    {
        targets_remaining -=1;
    }

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
    public void Death()
    {
        // Player is invulnerable in the air
        if (CheckState(State.OnGround))
            Destroy(gameObject);
    }
}

