using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class contains the stat values of the character.
/// </summary>
public class Stats : MonoBehaviour
{
    private int health;         // hp of character, from 0 to 100
    private int temperature;    // temperature of character, ranging from 0 to 35

    // Use this for initialization
    void Start()
    {
        // Set stats to starting values
        health = 100;
        temperature = 35;
    }

    /// <summary>
    ///  Returns the speed that the character should be moving at based on their current temperature.
    /// </summary>
    /// <returns>Speed of the character.</returns>
    public int GetSpeed()
    {
        // Calculate character speed based on temperature
        int speed = temperature / 2;
        return speed;
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
}
