using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Comment's Date: 30th April 2024
 * The JumpFeature component defines the player's jump.
 * Usuage: Call DrawProjection to display the entity's jump trajectory.
 *         Call Jump to add a force to the entity and to stop drawing the
 *         projection. The entity will travel according to its jump 
 *         trajectory.
 * Credit: LlamAcademy "Projectile Trajectory with Simple Math & Line Renderer"
 */

public interface IJumpable
{
    event Action onJumping;
    event Action onPlanning;
    event Action onGround;
    float GetSpeed();
}

[RequireComponent(typeof(Rigidbody))]
public class JumpFeature : MonoBehaviour
{
    [SerializeField]
    private LineRenderer LineRenderer;
    [Header("Display Controls Test")]
    [SerializeField] 
    [Range(10, 100)] 
    private int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;
    [SerializeField]
    private float minDist = 0f;
    [SerializeField]
    private float maxDist = 20.0f;
    private float jumpGrowthRate;
    private float durationTime = 0;
    private float _velocity_;
    private Rigidbody _rigidbody;
    private object _subject;


    public void InitializeJumpFeature<T>(T subject) where T : IJumpable, IStateful
    {
        subject.onPlanning += OnPlanningObserver;
        subject.onJumping += OnJumpingObserver;
        subject.onGround += OnGroundObserver;
        this._subject = subject;
        // jumpGrowthRate = Speed over Distance * FrameRate.
        this.jumpGrowthRate = subject.GetSpeed() / ((maxDist - minDist) * 50);
        Debug.Log(jumpGrowthRate);
        Debug.Log(Time.frameCount);
    }

    private void OnPlanningObserver()
    {
        if ((_subject as IStateful).CheckState(State.OnGround))
        {
            LineRenderer.enabled = true;
            LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
            _velocity_ = LerpJumpDistance();
            incrementDuration();
            DrawProjection(_velocity_);
        }
    }

    private void OnJumpingObserver()
    {
        Jump(_velocity_);
        (_subject as IStateful)?.SetState(State.OnGround, false);
    }

    private void OnGroundObserver()
    {
        durationTime = 0;
        LineRenderer.enabled = false;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void incrementDuration()
    {
        if (durationTime == 1f)
            return;
        if (durationTime > 1f) {
            durationTime = 1f;
            return;
        }
        durationTime += jumpGrowthRate;
    }

    private float LerpJumpDistance()
    {
        return (1 - durationTime) * minDist + durationTime * maxDist;
    }

    private void DrawProjection(float speed) 
    {
        Vector3 direction = transform.rotation * new Vector3(-1, 0, 0);
        Vector3 startPosition = transform.position;
        Vector3 startVelocity = new Vector3(direction.x * speed, 10.0f, direction.z * speed);
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            LineRenderer.SetPosition(i, point);
        }   
    }

    private void Jump(float speed)
    {
        // BUG FIX: Tank flips after jumping.
        // Reset angular velocity and (x, z) rotation. This keeps the tank parallel to the ground.
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        
        // Now that the rotation of the tank is corrected, carry on.
        Vector3 direction = transform.rotation * new Vector3(-1, 0, 0);
        _rigidbody.velocity = new Vector3(direction.x * speed, 10f, direction.z * speed);
    }
}
