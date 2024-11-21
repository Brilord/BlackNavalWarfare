using System.Collections.Generic;
using UnityEngine;

public class EnemyLargeBulletScript : MonoBehaviour
{
    [SerializeField]
    private int bulletDamage = 10;

    [SerializeField]
    private float bulletSpeed = 10f;

    [SerializeField]
    private float maxRange = 20f;

    [SerializeField]
    private AudioClip spawnSound;

    private AudioSource audioSource;
    private Vector2 startPosition;

    // To track objects that have already been hit
    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();

    void Start()
    {
        startPosition = transform.position;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = spawnSound;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }

    void Update()
    {
        transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime);

        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitObjects.Contains(collision.gameObject)) return; // Skip if already hit

        hitObjects.Add(collision.gameObject); // Add to the hit tracker

        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        GunboatScript playerGunboat = collision.gameObject.GetComponent<GunboatScript>();
        if (playerGunboat != null)
        {
            playerGunboat.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        BaseScript playerBase = collision.gameObject.GetComponent<BaseScript>();
        if (playerBase != null)
        {
            playerBase.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        CruiserScript playerCruiser = collision.gameObject.GetComponent<CruiserScript>();
        if (playerCruiser != null)
        {
            playerCruiser.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        SmallAntiAirShip playerAntiAirShip = collision.gameObject.GetComponent<SmallAntiAirShip>();
        if (playerAntiAirShip != null)
        {
            playerAntiAirShip.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        BattleShipScript playerBattleShip = collision.gameObject.GetComponent<BattleShipScript>();
        if (playerBattleShip != null)
        {
            playerBattleShip.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public int GetBulletDamage()
    {
        return bulletDamage;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }
}
