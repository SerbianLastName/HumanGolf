using UnityEngine;

public class ExplodingGameObject : MonoBehaviour
{
    private ParticleSystem explodingParticles;
    [SerializeField] private float explosionForce = 750f;
    [SerializeField] private int pointsAwarded;
    private MeshCollider meshCollider;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        explodingParticles = GetComponentInChildren<ParticleSystem>();
    }
    
    private void OnCollisionEnter(Collision _collider)
    {
        if (!_collider.gameObject.CompareTag("Player")) return;
        Destroy(meshCollider);
        Destroy(_rigidbody);
        explodingParticles.Play();
        float newExplosionForce =
            Random.Range(explosionForce - (explosionForce / 3), explosionForce + (explosionForce / 3));
        Vector3 newVelocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(-1.0f, 1.0f)) * newExplosionForce;
        PlayerPhysicsController.ApplyForceToPlayer(newVelocity);
        if (pointsAwarded != 0)
        {
            PlayerObject playerObject = _collider.gameObject.GetComponentInParent<PlayerObject>();
            playerObject._currentScore += pointsAwarded;
        }
        Destroy(gameObject, 2);
    }
}
