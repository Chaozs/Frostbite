using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character controller script to allow character to interact with items.
/// </summary>
public class Interaction : MonoBehaviour
{
    private const float maxDistance = 2.5f;     // max distance the player can be from an item in order to interact with it
    Stats stats;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Player presses key F, attempting to interact with items
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Generate raycast to detect objects in front of player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                // Check if object is within interactable distance
                if (hit.distance < maxDistance)
                {
                    // Perform item interaction
                    Interactable item = hit.collider.gameObject.GetComponent<Interactable>();
                    //GameObject item = hit.transform.gameObject;
                    if(item != null)
                    {
                        item.Interact();
                    }
                }
            }
        }
    }
}
