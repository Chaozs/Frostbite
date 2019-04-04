using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : Interactable
{
    /// <summary>
    /// Interaction behaviour with dead body.
    /// </summary>
    /// 
    [SerializeField]
    private bool hasInteracted = false;
    [SerializeField]
    Stats stats;

    void Awake()
    {
        stats = FindObjectOfType<Stats>();
    }

    public override void Interact()
    {
        if (!hasInteracted)
        {
            stats.setIsReading(true);
            hasInteracted = true;
        }
    }
}
