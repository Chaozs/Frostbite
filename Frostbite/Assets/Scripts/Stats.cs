﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class contains the stat values of the character and its behaviour.
/// </summary>
public class Stats : MonoBehaviour
{
    private Torch torch;            // script attached to the torch

    [SerializeField] private int health;        // hp of character, ranging from 0 to 100
    [SerializeField] private int temperature;   // temperature of character, ranging from 0 to 35; determines walking speed
    [SerializeField]
    private GameObject openBook;                // book on dead body giving player hints on what to do
    [SerializeField]
    private GameObject[] torchStuff;
    private bool isLosingHealthAndTemp;         // indicates if coroutine LoseHealthAndTempOverTime() is running
    private bool isGainingHealthAndTemp;        // indicates if coroutine GainHealthAndTempOverTime() is running
    public bool inMonsterRange;
    private bool isTakingDamageFromMonster;     // indicates if coroutine loseHealthEnemy() is running
    private int pagesLeft;                      // how many pages left to burn
    private PlayerController playerController;
    private bool isReading;
    private bool hasLootBody;

    // Use this for initialization
    void Awake()
    {
        // Set stats to starting values
        health = 100;
        temperature = 35;
        isLosingHealthAndTemp = false;
        isGainingHealthAndTemp = false;
        isTakingDamageFromMonster = false;
        pagesLeft = 3;
        playerController = gameObject.GetComponent<PlayerController>();
        hasLootBody = false;
    }

    void Update()
    {
        // Find the script attached to torch
        if (torch == null)
        {
            torch = GameObject.FindGameObjectWithTag("Torch").GetComponent<Torch>();
        }
        
        if (!torch.IsLit())
        {
            // If torch is not lit, lose HP and temperature over time
            if (!isLosingHealthAndTemp)
            {
                StartCoroutine(LoseHealthAndTempOverTime());
            }

            if (isGainingHealthAndTemp)
            {
                isGainingHealthAndTemp = false;
            }
        }
        else if (torch.IsLit())
        {
            // If torch is lit, gain HP and temperature over time
            if (!isGainingHealthAndTemp)
            {
                StartCoroutine(GainHealthAndTempOverTime());
            }

            if (isLosingHealthAndTemp)
            {
                isLosingHealthAndTemp = false;
            }
        }
    }

    //getter for pages left
    public int getPagesLeft()
    {
        return pagesLeft;
    }

    //function for adding or consuming pages
    public void pagesUsed(bool use)
    {
        if (use)
        {
            pagesLeft = pagesLeft - 1;
        }
        else
        {
            pagesLeft = pagesLeft + 1;
        }
        //update display for books
        playerController.updateBooks();
    }

    /// <summary>
    ///  Returns the speed that the character should be moving at based on their current temperature.
    /// </summary>
    /// <returns>Speed of the character as an integer.</returns>
    public int GetSpeed()
    {
        // Cannot move if player is dead
        if (IsDead())
        {
            return 0;
        }

        // Calculate character speed based on temperature
        // speed ranges from 1 to 5
        // -------------------------------
        // |   temperature   |   speed   |
        // -------------------------------
        // |      > 25       |     5     |
        // |      21-25      |     4     |
        // |      16-20      |     3     |
        // |      10-15      |     2     |
        // |      < 10       |     1     |
        // -------------------------------
        if (temperature > 25)
        {
            return 5;
        }
        else if (temperature > 20)
        {
            return 4;
        }
        else if (temperature > 15)
        {
            return 3;
        }
        else if (temperature > 9)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// Coroutine which loses health and temperature over time.
    /// If nothing is lit, health is lost at a rate of 3 health per 2 seconds (subject to change), or per 4 seconds if lighter is on.
    /// If nothing is lit, temperature is lost at a rate of 1 degree per 2 seconds (subject to change), or per 4 seconds if lighter is on.
    /// Called when the torch is not lit, corresponding to the character freezing.
    /// </summary>
    private IEnumerator LoseHealthAndTempOverTime()
    {
        isLosingHealthAndTemp = true;
        while (isLosingHealthAndTemp)
        {
            // Lose health
            if (health >= 3)
            {
                health -= 3;
            }
            else if (health > 0)
            {
                health = 0;
            }

            // Lose temperature
            if (temperature > 0)
            {
                temperature--;
            }

            // If lighter is equipped, lose HP/temp slower
            if (GameObject.FindGameObjectWithTag("Lighter") != null)
            {
                yield return new WaitForSeconds(4);
            }
            else
            {
                yield return new WaitForSeconds(2);
            }
        }
    }

    /// <summary>
    /// Coroutine which gains health and temperature over time.
    /// Health is gained at a rate of 2 health per 1 seconds (subject to change).
    /// Temperature is gained at a rate of 2 degrees per 1 seconds (subject to change).
    /// Called when the torch is lit, corresponding to the character warming up.
    /// </summary>
    private IEnumerator GainHealthAndTempOverTime()
    {
        isGainingHealthAndTemp = true;
        while (isGainingHealthAndTemp)
        {
            // Gain hp
            if (health <= 97)
            {
                health += 2;
            }
            else if (health < 100)
            {
                health = 100;
            }

            // Gain temperature
            if (temperature < 35)
            {
                temperature += 2;
            }
            yield return new WaitForSeconds(1);
        }
        isGainingHealthAndTemp = false;
    }

    /// <summary>
    /// Lose health from monster
    /// </summary>
    private IEnumerator loseHealthEnemy()
    {
        isTakingDamageFromMonster = true;

        while (isTakingDamageFromMonster)
        {
            // lose HP
            if (health > 0)
            {
                health = health - 30;
            }
            else if (health < 0)
            {
                health = 0;
            }
            print(health);
            yield return new WaitForSeconds(3);
        }

        isTakingDamageFromMonster = false;
    }

    public void killPlayer()
    {
        health = 0;
    }

    /// <summary>
    /// Current alive/death status of character, based on health.
    /// If health is 0, character is deemed dead. If health > 0, character is alive.
    /// </summary>
    /// <returns>true is player is dead, false if player is alive.</returns>
    public bool IsDead()
    {
        return (health <= 0);
    }

    /// ----------------------------------------------------
    /// Getters and setters
    /// ----------------------------------------------------
    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public int GetTemperature()
    {
        return temperature;
    }

    public void SetTemperature(int temperature)
    {
        this.temperature = temperature;
    }

    public void setHasLootBody(bool loot)
    {
        hasLootBody = loot;
    }

    public bool getHasLootBody()
    {
        return hasLootBody;
    }
    public void setIsReading(bool reading)
    {
        //player is now reading the book
        if(reading)
        {
            openBook.SetActive(true);
            isReading = true;
        }
        else
        //player is now no longer reading book, and loots 3 books + torch
        {
            openBook.SetActive(false);
            isReading = false;
            for (int i = 0; i < 3; i++)
            {
                pagesUsed(false);
            }
            foreach(GameObject torchItem in torchStuff)
            {
                torchItem.SetActive(true);
            }
            hasLootBody = true;
        }
    }

    public bool getIsReading()
    {
        return isReading;
    }
}
