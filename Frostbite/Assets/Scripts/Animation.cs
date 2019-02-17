using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls animation behaviour in the game.
/// </summary>
public class Animation : MonoBehaviour
{
    private Animator torchAnim;     // Animator for the torch
    private Stats characterStats;   // Stats of the character

    void Start()
    {
        // Initialize variables
        torchAnim = GameObject.FindGameObjectWithTag("Torch").GetComponent<Animator>();
        characterStats = GetComponent<Stats>();
    }

    void Update()
    {
        // Set the torch animation speed based on character walking speed
        torchAnim.speed = GetAnimationSpeed(characterStats.GetSpeed());

        // Walking/idle torch animation
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            // Play running animation if character is moving
            torchAnim.Play("Running");
        }
        else
        {
            // Play idle animation if character is not moving
            torchAnim.GetComponent<Animator>().Play("Idle");
        }
    }

    /// <summary>
    /// Returns the speed of the animation based on character walking speed.
    /// </summary>
    /// <param name="speed">Character walking speed from character's Stats.</param>
    /// <returns>Speed of the animation.</returns>
    private float GetAnimationSpeed(int speed)
    {
        return (speed / 5.0f);
    }
}
