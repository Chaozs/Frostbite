using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds logic for enemy behaviour
/// </summary>
public class EnemyAI : MonoBehaviour {

    private GameObject player;   //player
    private Transform playerTransform;  //player transform
    private Stats playerStats;                //player stats
    private Torch torch;        //player's torch
    private float distance;
    private int moveSpeed = 4;
    public AudioSource monsterRoar;    //monster roar
    private bool soundPlaying;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        playerStats = player.GetComponent<Stats>();
        torch = GameObject.FindGameObjectWithTag("Torch").GetComponent<Torch>();
        monsterRoar.Stop();
    }
	
	void Update () {
        chasePlayer();
        monsterRoar.loop = soundPlaying;
    }

    //chase player function
    void chasePlayer()
    {
        //only chase if torch is lit
        if (torch.IsLit())
        {
            distance = calculateDistance(playerTransform);

            //play roar sound if not currently playing
            if (!soundPlaying)
            {
                monsterRoar.Play();
                soundPlaying = true;
                
            }
            transform.LookAt(playerTransform);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            
            //if player in range to take damage
            if(distance < 7)
            {
                playerStats.inMonsterRange = true;
            }
            else
            {
                playerStats.inMonsterRange = false;
            }
        }
        else
        {
            monsterRoar.Stop();
            soundPlaying = false;
        }
    }

    private float calculateDistance(Transform target)
    {
        Vector3 monPosition = transform.position;
        Vector3 targetPosition = playerTransform.position;
        float X = Math.Abs(monPosition.x - targetPosition.x);
        float Z = Math.Abs(monPosition.z - targetPosition.z);
        float D = Mathf.Sqrt(Mathf.Pow(X, 2) + Mathf.Pow(Z, 2));

        return D;
    }


}
