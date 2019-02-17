using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls all wind behaviour.
/// </summary>
public class Wind : MonoBehaviour
{
    GameObject house;       // GameObject of the house/destination

    void Start()
    {
        house = GameObject.FindGameObjectWithTag("House");
    }
    
    void Update()
    {
        UpdateWindDirection();
    }

    /// <summary>
    /// Update the wind direction to point towards the house.
    /// </summary>
    private void UpdateWindDirection()
    {
        transform.LookAt(house.transform, Vector3.zero);
    }
}
