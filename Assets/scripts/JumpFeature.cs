using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS COMMENT IS OUT OF DATE!!
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
[RequireComponent(typeof(LineRenderer))]
public class JumpFeature : MonoBehaviour
{
    /* Configurable Variables (Configure via editor) */
    [Header("Assets")]
    [SerializeField] private GameObject JumpReticleAsset;
    
    [Header("Trajectory Configuration")]
    [SerializeField] [Range(10, 100)] 
    private int LinePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] 
    private float TimeBetweenPoints = 0.1f; 
    
    [Header("Jump Controls")]
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;
    
    [Header("Impact Zone Controls")]
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;

    /* Instance Variables */
    private Rigidbody rigidbody;
    private LineRenderer lineRenderer;
    private GameObject JumpReticle;
    private LayerMask JumpFeatureCollisionMask;
    private int jumpFeatureLayer;
    private float jumpGrowthRate;
    private float durationTime;
    private int maxPoints;
    private float jumpDistance;
    private float impactRadius;
    private object subject;

    // Start is called before the first frame.
    private void Start()
    {
        durationTime = 0;
        rigidbody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        maxPoints = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        JumpReticle = Instantiate(JumpReticleAsset, 
                                  new Vector3(0, 0, 0), 
                                  Quaternion.identity);
        JumpReticle.SetActive(false);
        jumpFeatureLayer = gameObject.layer;
        CreateCollisionMask();
    }

    // Call by the script you wish to make jumpable. 
    // (i.e. the PlayerController)
    public void InitializeJumpFeature<T>(T subject) 
    where T : IJumpable, IStateful
    {
        subject.onPlanning += OnPlanningObserver;
        subject.onJumping += OnJumpingObserver;
        subject.onGround += OnGroundObserver;
        this.subject = subject;
        CalculateGrowthRate(subject.GetSpeed());
    }

    // Growth Rate should be the same speed as the entity to prevent jumping
    // from being faster than regular moving.
    private void CalculateGrowthRate(float speed)
    {
        int FRAMERATE = 50;  // Assuming a 50 Fps.
        jumpGrowthRate = speed / ((this.maxDist - this.minDist) * FRAMERATE);
    }

    // A collision mask is a 32-bit string where each bit represents a layer.
    // If the ith bit is 1, then that layer does not collide with the subject.
    // Otherwise, if the ith bit is 0, then that layer is collidable.
    private void CreateCollisionMask()
    {
        // 32 possible layers because an 'int' type has 32 bits. 
        for (int i = 0; i < 32; i++) 
        {
            if (!Physics.GetIgnoreLayerCollision(jumpFeatureLayer, i))
            {
                // Note: These are bitwise operators.
                int currentLayer = 1 << i;
                JumpFeatureCollisionMask |= currentLayer;
            }
        }
    }

    // Important: Subject should Invoke OnPlanning Action in FixedUpdate() 
    // so that the framerate is constant 50 Fps.
    private void OnPlanningObserver()
    {
        // Leave early if subject is not on the ground.
        if (!((subject as IStateful).CheckState(State.OnGround)))
            return;

        // Grow the jumpDistance and impactRadius w.r.t. durationTime.
        jumpDistance = Lerp(minDist, maxDist, durationTime);
        impactRadius = Lerp(minRadius, maxRadius, durationTime);
        Vector3 newSize = new Vector3(impactRadius, 0, impactRadius);
        JumpReticle.transform.localScale = newSize;

        // Render the trajectory and JumpReticle.
        lineRenderer.positionCount = maxPoints;  // Reset # of points in line.
        lineRenderer.enabled = true;
        JumpReticle.SetActive(true);
        DrawProjection(jumpDistance);
        incrementDuration();
    }
    
    // Subject should invoke the action OnJumping to initate this method.
    private void OnJumpingObserver()
    {
        Jump(jumpDistance); 
        (subject as IStateful)?.SetState(State.OnGround, false);
    }

    // Stop rendering the trajectory and JumpReticle and reset durationTime.
    private void OnGroundObserver()
    {
        durationTime = 0;
        lineRenderer.enabled = false;
        JumpReticle.SetActive(false); 
    }

    // Increment timeDuration until it reaches a max of 1.
    private void incrementDuration()
    {
        durationTime = Math.Min(durationTime + jumpGrowthRate, 1f);
    }

    // Standard Linear Interpolation Algorithm.
    private float Lerp(float minValue, float maxValue, float factor)
    {
        return (1 - factor) * minValue + factor * maxValue;
    }

    private void DrawProjection(float speed) 
    {
        Vector3 direction = transform.rotation * new Vector3(-1, 0, 0);
        Vector3 startPosition = transform.position;
        Vector3 startVelocity = new Vector3(direction.x * speed, 10.0f, direction.z * speed);
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            lineRenderer.SetPosition(i, point);
            
            // RayCasting Stuff (Messy Code for now while I'm learning.)
            Vector3 lastPosition = lineRenderer.GetPosition(i-1);
            Vector3 origin = lastPosition;
            Vector3 rayDirection = (point-lastPosition).normalized;
            RaycastHit hit;
            float maxDistance = (point - lastPosition).magnitude;
            // Check if the Raycast hit something (i.e. an gameObject in a collisionLayer).
            if (Physics.Raycast(origin,
                            rayDirection,
                            out hit,    // Out keyword means pass by reference rather than value.
                            maxDistance,
                            JumpFeatureCollisionMask))
            {
                // hit is a struct that contains information about the Raycast.
                lineRenderer.SetPosition(i, hit.point); // Move the last point to exactly where the raycast hit.
                JumpReticle.transform.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
                lineRenderer.positionCount = i + 1; // Stop rendering more points by setting the limit of PositionCount.
                return;
            }
        }
    }

    // Apply an instance velocity change to the rigidbody attached to this script. 
    private void Jump(float speed)
    {
        // Reset angular velocity and (x, z) rotation to keep this subject
        // parallel to the ground.
        rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        
        // Now that the rotation of the tank is corrected, carry on.
        Vector3 direction = transform.rotation * new Vector3(-1, 0, 0);
        rigidbody.velocity = new Vector3(direction.x * speed, 10f, direction.z * speed);
    }
}
    
