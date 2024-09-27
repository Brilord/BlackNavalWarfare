using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float speed = 10f;             // Speed of the missile
    public float rotationSpeed = 200f;    // Rotation speed for homing behavior
    public Transform target;              // Target for the missile (assign in inspector or via script)
    public float lifetime = 5f;           // How long before the missile self-destructs
    public GameObject explosionPrefab;    // Explosion effect to instantiate on collision

    private float lifetimeTimer;

    // Start is called before the first frame update
    void Start()
    {
        lifetimeTimer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMissile();
        UpdateLifetime();
    }

    // Move the missile forward and rotate toward the target if assigned
    void MoveMissile()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(transform.forward, direction).y;
            transform.Rotate(0, rotateAmount * rotationSpeed * Time.deltaTime, 0);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Check if the missile has exceeded its lifetime and should self-destruct
    void UpdateLifetime()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0)
        {
            Explode();
        }
    }

    // Handle collision with objects
    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    // Explode the missile and destroy the game object
    void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
