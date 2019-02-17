using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles logic regarding win condition of the game.
/// </summary>
public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;     // prefab for game over screen
    [SerializeField] private GameObject winScreen;          // prefab for "you win" screen
    private Stats characterStats;                           // Stats of the player
    private bool gameEnded;                                 // whether or not the game has ended

    void Start()
    {
        // Initialize variables
        characterStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        gameEnded = false;
    }
    
    void Update()
    {
        // Pop up the game over screen if character dies
        if (characterStats.IsDead() && GameObject.FindGameObjectWithTag("Finish") == null)
        {
            InitiateGameOver();
        }
    }

    private void InitiateGameOver()
    {
        GameObject.Instantiate(gameOverScreen);
        gameEnded = true;
    }

    public void InitiateWinScreen()
    {
        GameObject.Instantiate(winScreen);
        gameEnded = true;
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }
}
