using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : Interactable
{
    /// <summary>
    /// Interaction behaviour with cloth.
    /// </summary>
    /// 
    void Awake()
    {
        stats = FindObjectOfType<Stats>();
    }

    Stats stats;

    public override void Interact()
    {

        // Add cloth to inventory

        //increment pages in inventory
        if (stats.getPagesLeft() < 4)
        {
            stats.pagesUsed(false);
            Destroy(gameObject);
        }

    }



}
