using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Comment's Date: 30th April 2024
 * The PlayerController class defines the player game object. It is a
 * derivative of the TankParent component. The PlayerController
 * communicates with the InputController and JumpFeature.
 */

// The PlayerState is highly flexible. States can be moved around or added. 
// The only caution is to make sure that each state has its own dedicated bit.
[Flags]
public enum PlayerState
{
    OnGround   = 1 << 0, // 00001
    IsDriving  = 1 << 1, // 00100
    IsRotating = 1 << 2, // 01000
    IsJumping  = 1 << 3, // 10000
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
    [Header("Player State")]
    [SerializeField] // You can see the player state within the inspector.
    private PlayerState state = PlayerState.OnGround;
    private Direction direction = new Direction(); //< Holds direction for drive and rotate.

    // Attach Observers
    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _inputController.onButton += OnButtonObserver;
        _jumpFeature = GetComponent<JumpFeature>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    /* For checking if the player is in state */
    private bool CheckState(PlayerState playerState)
    {
        return state.HasFlag(playerState);
    }

    /* For checking multiple states and whether those states are true or false. */
    private bool CheckStates(params (PlayerState playerState, bool value)[] playerStates)
    {
        bool result = true;
        foreach (var tuple in playerStates)
        {
            if (tuple.value == false) {
                result = result && !CheckState(tuple.playerState);
            } else {
                result = result && CheckState(tuple.playerState);
            }
        }
        return result;
    }

    /* For moving the player in or out of a state. */
    private void SetState(PlayerState playerState, bool value)
    {
        if (value) {
            state |= playerState;
        } else {
            state &= ~playerState;
        }
    }

    private void OnButtonObserver(Button button)
    {
        if (button.name == "Drive")
        {
            SetState(PlayerState.IsDriving, button.isDown);
            direction.drive = button.value;
        }
        if (button.name == "Rotate")
        {
            SetState(PlayerState.IsRotating, button.isDown);
            direction.rotate = button.value; 
        }
        if (button.name == "Jump")
        {
            SetState(PlayerState.IsJumping, button.isDown);
            Jump();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            SetState(PlayerState.OnGround, true);
        }
    }

    private void Jump()
    {
        if (!CheckState(PlayerState.OnGround))
            return;
        
        if (CheckStates((PlayerState.OnGround, true), (PlayerState.IsJumping, false)))
        {
            SetState(PlayerState.OnGround, false);
            _jumpFeature.Jump(_rigidbody, forward, speed);
            _audioSource.Play();
            return;
        }
        _jumpFeature.DrawProjection(transform.position, forward, speed);
    }

    private void Update()
    {
        if (CheckState(PlayerState.IsJumping))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (CheckState(PlayerState.IsRotating))
        {
            Rotate(direction.rotate);
        }
        if (CheckStates((PlayerState.IsDriving, true), (PlayerState.OnGround, true), (PlayerState.IsJumping, false)))
        {
            Drive(direction.drive);
        }
    }
}

