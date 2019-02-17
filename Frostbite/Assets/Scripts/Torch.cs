using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all information regarding the torch
/// </summary>
public class Torch : MonoBehaviour
{
    private GameObject flames;              // actual flames GameObject effect on the torch

    [SerializeField] private bool isLit;    // indicates whether the torch is currently lit or not

    // Use this for initialization
    void Start()
    {
        flames = GameObject.FindGameObjectWithTag("Flames");    // find the flames object and set it

        // Set variables to starting values
        isLit = false;
    }

    void Update()
    {
        if (isLit && !flames.activeSelf)
        {
            // Set flames gameobject to active if torch is lit
            flames.SetActive(true);
        }
        else if (!isLit && flames.activeSelf)
        {
            // Set flames gameobject to inactive if torch is not lit
            flames.SetActive(false);
        }
    }

    /// ----------------------------------------------------
    /// Getters and setters
    /// ----------------------------------------------------
    public void SetIsLit(bool lit)
    {
        isLit = lit;
    }
    public bool IsLit()
    {
        return isLit;
    }
}
