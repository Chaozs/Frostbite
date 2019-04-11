using System;
using UnityEngine;

/// <summary>
/// This class holds logic for enemy behaviour.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    private GameObject player;          // player
    private Stats playerStats;          // player stats
    private Torch torch;                // player's torch
    private Lighter lighter;            // player's lighter
    private AudioSource monsterSounds;  // monster roar

    private float distance;
    private readonly int moveSpeed = 6;
    private bool soundPlaying;

    /// <summary>
    /// Initialize.
    /// </summary>
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<Stats>();
        torch = GameObject.FindGameObjectWithTag("Torch").GetComponent<Torch>();
        lighter = GameObject.FindGameObjectWithTag("Lighter").GetComponent<Lighter>();
        monsterSounds = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Setup.
    /// </summary>
    void Start()
    {
        monsterSounds.Stop();
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        ChasePlayer();
        monsterSounds.loop = soundPlaying;
    }

    /// <summary>
    /// Ignores collisions with trees.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tree")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider>());
        }
    }

    /// <summary>
    /// Chases the player.
    /// </summary>
    private void ChasePlayer()
    {
        //only chase if torch/lighter is lit
        if (torch.IsLit() || lighter.IsLit())
        {
            distance = CalculateDistance(player.transform);

            //play roar sound if not currently playing
            if (!soundPlaying)
            {
                monsterSounds.Play();
                soundPlaying = true;

            }
            transform.LookAt(player.transform);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            //if player in range of enemy, kill player
            if (distance < 6)
            {
                playerStats.killPlayer();
            }
        }
        else
        {
            monsterSounds.Stop();
            soundPlaying = false;
        }
    }

    private float CalculateDistance(Transform target)
    {
        Vector3 monPosition = transform.position;
        Vector3 targetPosition = player.transform.position;
        float X = Math.Abs(monPosition.x - targetPosition.x);
        float Z = Math.Abs(monPosition.z - targetPosition.z);
        float D = Mathf.Sqrt(Mathf.Pow(X, 2) + Mathf.Pow(Z, 2));

        return D;
    }
}
