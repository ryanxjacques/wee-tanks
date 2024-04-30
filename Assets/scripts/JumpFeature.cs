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
    public Action<string> onJump;

    public void DrawProjection(Vector3 startPosition, Vector3 direction, float speed) 
    {
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
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

    public void Jump(Rigidbody entity, Vector3 direction, float speed)
    {
        onJump.Invoke("Started");
        entity.velocity = new Vector3(direction.x * speed, 10f, direction.z * speed);
        LineRenderer.enabled = false;

    }
}
