using Unity.VisualScripting;
using UnityEngine;

public class SpinMesh : MonoBehaviour
{
    [SerializeField] private float spinMin = 20f;
    [SerializeField] private float spinMax = 35f;
    [SerializeField] private float forceAmount = 100f;
    [SerializeField] private Transform meshTransform;
    private float _spinSpeed;

    private void Start()
    {
        _spinSpeed = Random.Range(spinMin, spinMax);
        if (Random.Range(0, 10) > 5)
            _spinSpeed = -_spinSpeed;

        float rotationAmount = Random.Range(0f, 360f);
        meshTransform.Rotate(0f, 0f, rotationAmount, Space.Self);
    }

    private void Update()
    {
        float rotationAmount = _spinSpeed * Time.deltaTime;
        meshTransform.Rotate(0f, 0f, rotationAmount, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Rigidbody otherRb = collision.rigidbody;
        //
        // if (!otherRb.CompareTag("Player")) return;
        // {
        //     Vector3 bladeTipVelocity = meshTransform.right * (_spinSpeed * forceAmount * Mathf.Deg2Rad * (meshTransform.localScale.x / 2f));
        //     PlayerPhysicsController.ApplyForceToPlayer(bladeTipVelocity);
        // }
        Rigidbody otherRb = collision.rigidbody;

        // Ensure the colliding object has a Rigidbody and the correct tag
        if (otherRb == null || !otherRb.CompareTag("Player")) return;

        // Ensure there is at least one contact point
        if (collision.contactCount == 0) return;

        // Get the first contact point
        ContactPoint contact = collision.GetContact(0);
        Vector3 collisionPoint = contact.point;

        // Vector from the blade's center to the collision point
        Vector3 bladeCenter = meshTransform.position;
        Vector3 radiusVector = collisionPoint - bladeCenter;

        // Prevent issues with zero radius vector
        if (radiusVector.sqrMagnitude < Mathf.Epsilon) return;

        // Calculate tangential velocity at the point of collision
        Vector3 tangentialDirection = Vector3.Cross(meshTransform.forward, radiusVector.normalized);
        float bladeSpeed = _spinSpeed * Mathf.Deg2Rad * radiusVector.magnitude; // Speed proportional to radius

        // Apply force proportional to speed and scale
        Vector3 bladeTipVelocity = tangentialDirection * bladeSpeed * forceAmount;

        // Clamp the force to prevent excessively large impulses
        bladeTipVelocity = Vector3.ClampMagnitude(bladeTipVelocity, 250f); // Adjust max force as needed

        // Apply impulse to the colliding Rigidbody
        // otherRb.AddForce(bladeTipVelocity, ForceMode.Impulse);
        PlayerPhysicsController.ApplyForceToPlayer(bladeTipVelocity);
    }
    
}