using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    // private float gravity = -9.81f;
    public Action<string> onJump;

    public void DrawProjection(Vector3 startPosition, Vector3 direction, float speed) 
    {
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startVelocity = new Vector3(direction.x * speed, 10.0f, direction.y * speed);

        // Vector3 startVelocity = new Vector3(velocityState.x * 2, 10.0f, velocityState.z * 2);
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            // Explaination for equation below at timestamp 4:52.
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            LineRenderer.SetPosition(i, point);
        }   
    }
    public void Jump(Rigidbody entity, Vector3 direction, float speed)
    {
        onJump.Invoke("Started");
        entity.velocity = new Vector3(direction.x * speed, 10f, direction.y * speed);
        LineRenderer.enabled = false;
    }
}
