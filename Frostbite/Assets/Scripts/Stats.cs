using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class contains the stat values of the character and its behaviour.
/// </summary>
public class Stats : MonoBehaviour
{
    private Torch torch;    // script attached to the torch
    
    [SerializeField] private int temperature;           // temperature of character, ranging from 0 to 35; determines walking speed
    [SerializeField] private GameObject openBook;       // book on dead body giving player hints on what to do
    [SerializeField] private GameObject[] torchStuff;
    private bool isLosingWarmth;
    private bool isGainingWarmth;
    public bool inMonsterRange;
    private int pagesLeft;                              // how many pages left to burn
    private PlayerController playerController;
    private bool isReading;
    private bool hasLootBody = false;
    private AudioSource source;
    private AudioClip whispers;
    private bool whisperIsPlaying;

    // Use this for initialization
    void Awake()
    {
        // Set stats to starting values
        temperature = 35;
        isLosingWarmth = false;
        isGainingWarmth = false;
        pagesLeft = 3;
        playerController = gameObject.GetComponent<PlayerController>();
        foreach (GameObject playerItem in torchStuff)
        {
            playerItem.transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
        }
        source = gameObject.transform.Find("FirstPersonCharacter").gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!hasLootBody)
        {
            return;
        }

        // Find the script attached to torch
        if (torch == null)
        {
            torch = GameObject.FindGameObjectWithTag("Torch").GetComponent<Torch>();
        }

        // Disable torch if player is looking at snowman
        if (IsLookingAtSnowman())
        {
            if(!whisperIsPlaying)
            {
                whisperIsPlaying = true;
                source.Play();
            }
            torch.SetIsLit(false);
        }
        else
        {
            if(whisperIsPlaying)
            {
                source.Stop();
                whisperIsPlaying = false;
            }
        }

        if (!torch.IsLit())
        {
            // If torch is not lit, lose HP and temperature over time
            if (!isLosingWarmth)
            {
                StartCoroutine(LoseTemperatureOverTime());
            }

            if (isGainingWarmth)
            {
                isGainingWarmth = false;
            }
        }
        else if (torch.IsLit())
        {
            // If torch is lit, gain HP and temperature over time
            if (!isGainingWarmth)
            {
                StartCoroutine(GainTemperatureOverTime());
            }

            if (isLosingWarmth)
            {
                isLosingWarmth = false;
            }
        }
    }

    public bool IsLookingAtSnowman()
    {
        Camera cam = transform.Find("FirstPersonCharacter").GetComponent<Camera>();
        GameObject snowman = GameObject.Find("Snowman_02");
        Vector3 viewPos = cam.WorldToViewportPoint(snowman.transform.position);
        if (viewPos.x > 0 && viewPos.x < 1 &&
            viewPos.y > 0 && viewPos.y < 1 &&
            viewPos.z > 0)
        {
            return true;
        }

        return false;
    }


    //getter for pages left
    public int GetPagesLeft()
    {
        return pagesLeft;
    }

    //function for adding or consuming pages
    public void PagesUsed(bool use)
    {
        if (use)
        {
            pagesLeft = pagesLeft - 1;
        }
        else
        {
            pagesLeft = pagesLeft + 1;
        }
        //update display for books
        playerController.UpdateBooks();
    }

    /// <summary>
    ///  Returns the speed that the character should be moving at based on their current temperature.
    /// </summary>
    /// <returns>Speed of the character as an integer.</returns>
    public int GetSpeed()
    {
        // Cannot move if player is dead
        if (IsDead())
        {
            return 0;
        }

        // Calculate character speed based on temperature
        // speed ranges from 1 to 5
        // -------------------------------
        // |   temperature   |   speed   |
        // -------------------------------
        // |      > 25       |     5     |
        // |      21-25      |     4     |
        // |      16-20      |     3     |
        // |      10-15      |     2     |
        // |      < 10       |     1     |
        // -------------------------------
        if (temperature > 25)
        {
            return 5;
        }
        else if (temperature > 20)
        {
            return 4;
        }
        else if (temperature > 15)
        {
            return 3;
        }
        else if (temperature > 9)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// Coroutine which loses temperature over time.
    /// If nothing is lit, temperature is lost at a rate of 1 degree per 2 seconds (subject to change), or per 4 seconds if lighter is on.
    /// Called when the torch is not lit, corresponding to the character freezing.
    /// </summary>
    private IEnumerator LoseTemperatureOverTime()
    {
        isLosingWarmth = true;
        while (isLosingWarmth)
        {
            // Lose temperature
            if (temperature > 0)
            {
                temperature--;
            }
            
            if (GameObject.FindGameObjectWithTag("Lighter") != null)
            {
                // If lighter is equipped, lose HP/temp slower
                yield return new WaitForSeconds(2);
            }
            else if (IsLookingAtSnowman())
            {
                // Lose HP/temp faster if looking at snowman
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                // Default speed to lose HP/temp
                yield return new WaitForSeconds(1);
            }
        }
    }

    /// <summary>
    /// Coroutine which gains temperature over time.
    /// Temperature is gained at a rate of 2 degrees per 1 seconds (subject to change).
    /// Called when the torch is lit, corresponding to the character warming up.
    /// </summary>
    private IEnumerator GainTemperatureOverTime()
    {
        isGainingWarmth = true;
        while (isGainingWarmth)
        {
            // Gain temperature
            if (temperature < 35)
            {
                temperature += 2;
            }
            yield return new WaitForSeconds(1);
        }
        isGainingWarmth = false;
    }

    public void KillPlayer()
    {
        temperature = 0;
    }

    /// <summary>
    /// Current alive/death status of character, based on temperature.
    /// If temperature is 0, character is deemed dead. If temperature > 0, character is alive.
    /// </summary>
    /// <returns>True if player is dead, false if player is alive.</returns>
    public bool IsDead()
    {
        return (temperature <= 0);
    }

    /// ----------------------------------------------------
    /// Getters and setters
    /// ----------------------------------------------------

    public int GetTemperature()
    {
        return temperature;
    }

    public void SetTemperature(int temperature)
    {
        this.temperature = temperature;
    }

    public bool GetHasLootBody()
    {
        return hasLootBody;
    }

    public void SetIsReading(bool reading)
    {
        //player is now reading the book
        if (reading)
        {
            openBook.SetActive(true);
            isReading = true;
        }
        else
        //player is now no longer reading book, and loots 3 books + torch
        {
            openBook.SetActive(false);
            isReading = false;
            foreach (GameObject playerItem in torchStuff)
            {
                playerItem.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
            }
            hasLootBody = true;
        }
    }

    public bool getIsReading()
    {
        return isReading;
    }
}
