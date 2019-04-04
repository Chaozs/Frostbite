using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character controller script to allow character to interact with items.
/// </summary>
public class Interaction : MonoBehaviour
{
    private const float maxDistance = 4f;     // max distance the player can be from an item in order to interact with it
    Stats stats;
    private PlayerController playerController;

    // Use this for initialization
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        stats = gameObject.GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player presses key F, attempting to interact with items
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (stats.getIsReading())
            {
                stats.setIsReading(false);
            }
            else
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
                        if (item != null)
                        {
                            item.Interact();
                        }
                    }
                }
            }
        }
    }
}
