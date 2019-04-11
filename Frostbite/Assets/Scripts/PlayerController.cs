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
    private Stats stats;

    [SerializeField] private GameObject[] books = new GameObject[4];
    [SerializeField] private GameObject inventory;

    private bool showInventory = false;
    private int currentDisplayedBooks = 0;

    // Use this for initialization
    void Start()
    {
        // Initialize variables
        lighter = GameObject.FindGameObjectWithTag("Lighter");
        torch = GameObject.FindGameObjectWithTag("Torch");
        torchScript = torch.GetComponent<Torch>();

        // Set torch as active initally
        if (lighter != null)
        {
            lighter.SetActive(false);
        }

        //book inventory hidden by default
        for (int i = 0; i < books.Length; i++)
        {
            books[i].SetActive(false);
        }
        currentDisplayedBooks = stats.GetPagesLeft();
        for (int i = 0; i < stats.GetPagesLeft(); i++)
        {
            books[i].SetActive(true);
        }
    }

    void Awake()
    {
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stats.GetHasLootBody())
        {
            return;
        }

        if (!showInventory)
        {
            // Handle Q key input
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleEquipLighterTorch();
            }

            // Handle E key input
            if (Input.GetKeyDown(KeyCode.E) && !stats.IsLookingAtSnowman())
            {
                ToggleLitTorch();
            }
        }

        // Handle I key input
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
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
            //torch can be put out whenever
            if (torchScript.IsLit())
            {
                torchScript.SetIsLit(!torchScript.IsLit());
            }
            else
            {
                // only light torch is there is paper left
                if (stats.GetPagesLeft() > 0)
                {
                    stats.PagesUsed(true);
                    //use up a page if there are pages left
                    torchScript.SetIsLit(!torchScript.IsLit());
                }
            }

        }
    }

    private void ToggleInventory()
    {
        //if inventory open, hide all books shown
        if (showInventory)
        {
            inventory.transform.Translate(0f, -0.6f, 0);
            showInventory = !showInventory;
        }
        //show all books owned
        else
        {
            inventory.transform.Translate(0f, 0.6f, 0);
            showInventory = !showInventory;
        }
    }

    //update display for books
    public void UpdateBooks()
    {
        Debug.Log(currentDisplayedBooks + ", " + stats.GetPagesLeft());
        if (currentDisplayedBooks < stats.GetPagesLeft())
        {
            books[stats.GetPagesLeft() - 1].SetActive(true);
            currentDisplayedBooks++;
        }
        else if (currentDisplayedBooks > stats.GetPagesLeft())
        {
            books[currentDisplayedBooks - 1].SetActive(false);
            currentDisplayedBooks--;
        }
    }

    //getter for whether inventory is open
    public bool IsInventoryOpen()
    {
        return showInventory;
    }
}
