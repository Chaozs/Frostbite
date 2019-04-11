using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : Interactable
{
    /// <summary>
    /// Interaction behaviour with dead body.
    /// </summary>

    [SerializeField] Stats stats;
    private bool interacted;

    void Awake()
    {
        stats = FindObjectOfType<Stats>();
        interacted = false;
    }

    public override void Interact()
    {
        if (!interacted)
        {
            interacted = true;
            stats.SetIsReading(true);
        }
    }
}
