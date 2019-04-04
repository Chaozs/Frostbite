using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : Interactable
{
    /// <summary>
    /// Interaction behaviour with cloth.
    /// </summary>

    Stats stats;

    void Awake()
    {
        stats = FindObjectOfType<Stats>();
    }

    public override void Interact()
    {
        if (!stats.getHasLootBody())
        {
            return;
        }

        // Add cloth to inventory

        //increment pages in inventory
        if (stats.getPagesLeft() < 4)
        {
            stats.pagesUsed(false);
            Destroy(gameObject);
        }

    }



}
