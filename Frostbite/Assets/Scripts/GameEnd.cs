using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles logic regarding win condition of the game.
/// </summary>
public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;     // prefab for game over screen
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
        if (!gameEnded)
        {
            GameObject.Instantiate(gameOverScreen);
            gameEnded = true;
        }
    }

    public void InitiateWinScreen()
    {
        if (!gameEnded)
        {
            SceneManager.LoadScene("EndGameScene", LoadSceneMode.Single);
            gameEnded = true;
        }
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }
}
