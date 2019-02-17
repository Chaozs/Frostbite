using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Interactable
{
    GameEnd gameEnd;

    /// <summary>
    /// Interaction behaviour with the house/destination.
    /// Initiates the win screen.
    /// </summary>
    public override void Interact()
    {
        gameEnd = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEnd>();
        gameEnd.InitiateWinScreen();
    }
}
