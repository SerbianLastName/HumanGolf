using System;
using UnityEngine;

public class BloodDetection : MonoBehaviour
{
    [SerializeField] ParticleSystem bloodParticles;
    private float hitTime;
    [SerializeField] private float playbackTime = 1.5f;
    [SerializeField] private float impactLimit = 10;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") ||
            collision.relativeVelocity.magnitude < impactLimit ||
            GameManager.shotTaken + 1 > Time.time)
        {
            return;
        }
        
        bloodParticles.Play();
        hitTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - (hitTime + playbackTime) < 0f) return;
        bloodParticles.Stop();
    }
}
