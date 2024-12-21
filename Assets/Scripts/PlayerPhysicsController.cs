using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    public static PlayerPhysicsController instance;
    private static Rigidbody[] _characterRigidBodies;
    private static Rigidbody rb;
    private const float MID_AIR_FORCE = 10f;
    private const float MOTION_THRESHOLD = 0.2f;
    private const float MOTION_COOLDOWN = 1.5f;
    private const float SPEED_THRESHOLD = 0.75f;
    
    private void Awake()
    {
        instance = this;
    }

    public static void InitController(Rigidbody[] characterRigidBodies)
    {
        _characterRigidBodies = characterRigidBodies;
        rb = _characterRigidBodies[0];
    }
    private void FixedUpdate()
    {
        
        switch (GameManager.activePlayerState)
        {
            case ActivePlayerState.MidAir:
                CheckMotion();
                break;
            case ActivePlayerState.OnGround:
                CheckMotion();
                break;
        }
    }

    public static void ApplyForceToPlayer(Vector3 force)
    {
        foreach (Rigidbody _rb in _characterRigidBodies)
        {
            _rb.AddForce(force / _characterRigidBodies.Length, ForceMode.Impulse);
        }
    }
    private static void AddDragToPlayer(float drag)
    {
        foreach (Rigidbody _rb in _characterRigidBodies)
        {
            _rb.linearDamping = drag;
        }
    }
    public static void HandleMotion(Vector2 input)
        {
            Vector3 velocity = rb.linearVelocity.normalized;
            Vector3 velocityXZ = new Vector3(velocity.x, 0f, velocity.z).normalized;
            Vector3 perpendicularDirection = Vector3.Cross(velocityXZ, Vector3.up).normalized;
            Vector3 appliedForce = (perpendicularDirection / 2) * (-input.x * MID_AIR_FORCE);
            if (GameManager.activePlayerState == ActivePlayerState.OnGround)
            {
                appliedForce = (perpendicularDirection / 2) * (-input.x * (MID_AIR_FORCE));
            }
            if (velocity.magnitude < SPEED_THRESHOLD)
            {
                appliedForce = perpendicularDirection * 0;
            }
            float drag = Mathf.Abs(Mathf.Clamp(input.y, -1f, 0f));
            ApplyForceToPlayer(appliedForce);
            AddDragToPlayer(drag/5);
        }
    
    

    private static void CheckMotion()
    {
        float linVelocity = rb.linearVelocity.magnitude;
        float angVelocity = rb.angularVelocity.magnitude;
        if (linVelocity > MOTION_THRESHOLD
            || angVelocity > MOTION_THRESHOLD
            || ShotController.shotTakenAt + MOTION_COOLDOWN > Time.time
            || GameManager.activePlayerState == ActivePlayerState.AfterStroke
            || GameManager.activePlayerState == ActivePlayerState.InHole)
            return;
        {
            GameManager.UpdateActivePlayerState(ActivePlayerState.AfterStroke);
            GameManager.HandleStrokeOver();
        }
    }


}
