using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : Interactable
{
    /// <summary>
    /// Interaction behaviour with dead body.
    /// </summary>

    [SerializeField]
    Stats stats;

    void Awake()
    {
        stats = FindObjectOfType<Stats>();
    }

    public override void Interact()
    {
        stats.setIsReading(true);
    }
}
