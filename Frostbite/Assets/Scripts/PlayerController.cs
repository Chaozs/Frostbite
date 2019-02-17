using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds logic for behaviour that the player may perform, ie. handles actions of the player.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private GameObject lighter;
    private GameObject torch;
    private Torch torchScript;

    // Use this for initialization
    void Start()
    {
        // Initialize variables
        lighter = GameObject.FindGameObjectWithTag("Lighter");
        torch = GameObject.FindGameObjectWithTag("Torch");
        torchScript = torch.GetComponent<Torch>();

        // Set torch as active initally
        lighter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Q key input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleEquipLighterTorch();
        }
        
        // Handle E key input
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleLitTorch();
        }
    }

    /// <summary>
    /// Toggles between equipping the lighter or the torch.
    /// This can only be done if the torch is not lit.
    /// </summary>
    private void ToggleEquipLighterTorch()
    {
        if (!torchScript.IsLit())
        {
            lighter.SetActive(!lighter.activeSelf); // Toggle holding lighter
            torch.SetActive(!lighter.activeSelf);   // enable/disable torch depending on if lighter is being held
        }
    }

    /// <summary>
    /// Toggles between lighting/putting out the torch.
    /// Lighting the torch consumes 1 cloth resource.
    /// Putting out the torch will play an animation of player dipping torch into snow.
    /// </summary>
    private void ToggleLitTorch()
    {
        // Only light the torch if torch is currently equipped
        if (torch.activeSelf)
        {
            // !!! TODO: CONSUME 1 CLOTH RESOURCE !!!
            torchScript.SetIsLit(!torchScript.IsLit());
        }
    }
}
