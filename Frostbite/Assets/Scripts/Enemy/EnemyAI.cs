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
	private AudioSource monsterWhispers; //monster whispering
	private Renderer render;
	private Stats characterStats;
	bool frostAura = false;
	bool whispering = false;

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
		monsterWhispers = transform.Find ("creepywhispers").GetComponent<AudioSource> ();

		render = GetComponentInChildren<Renderer> ();

    }

    /// <summary>
    /// Setup.
    /// </summary>
    void Start()
    {
        monsterSounds.Stop();

		characterStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
		monsterWhispers.loop = whispering;
		if (render.isVisible) {
			if (!whispering) {
				monsterWhispers.Play ();
				whispering = true;
			}
			if (!frostAura) {
				InvokeRepeating ("DecreaseTemperature", 0, 1);
				frostAura = true;
			} else {
				//do nothing
			}
		} else {
			if (frostAura) {
				CancelInvoke ();
				frostAura = false;
			}
			if (whispering) {
				monsterWhispers.Stop ();
				whispering = false;
			}
		}

        ChasePlayer();
        monsterSounds.loop = soundPlaying;
    }

	void DecreaseTemperature() {
		characterStats.SetTemperature (characterStats.GetTemperature() - 1);
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
