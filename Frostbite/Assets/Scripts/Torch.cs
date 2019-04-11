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
    private bool timerRunning;              // whether or not the torch timer is currently running
	AudioSource torchSounds;

	private bool isSoundPlaying;

	private void Awake() {
		torchSounds = GetComponent<AudioSource>();
	}

    /// Use this for initialization
    void Start()
    {
        flames = GameObject.FindGameObjectWithTag("Flames");    // find the flames object and set it

		torchSounds.Stop ();
        // Set variables to starting values
        isLit = false;
        timerRunning = false;
    }
    
    /// Updates the status of the torch
    void Update()
    {
		torchSounds.loop = isSoundPlaying;
        if (isLit)
        {
			if (!isSoundPlaying) {
				torchSounds.Play ();
				isSoundPlaying = true;
			}
			
            // Set flames gameobject to active if torch is lit
            if (!flames.activeSelf)
            {
                flames.SetActive(true);
            }

            // Start timer if it is not already started
            if (!timerRunning)
            {
                StartCoroutine(TorchFireTimer());
            }
        }
        else
        {
			torchSounds.Stop ();
			isSoundPlaying = false;
            // Set flames gameobject to inactive if torch is not lit
            if (flames.activeSelf)
            {
                flames.SetActive(false);
            }

            // Stop the timer
            timerRunning = false;
        }
    }

    /// <summary>
    /// Timer for the time left on the torch when it is lit.
    /// When the timer hits 0, the fire on the torch will run out and torch will no longer be lit.
    /// </summary>
    private IEnumerator TorchFireTimer()
    {
        timerRunning = true;
        int timeLeft = 30;      // number of seconds the torch lasts

        // Timer countdown
        while (timeLeft > 0 && timerRunning)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        isLit = false;
        timerRunning = false;
    }

    /// -------------------------------------
    /// Getters and Setters
    /// -------------------------------------
    public void SetIsLit(bool lit)
    {
        isLit = lit;
    }
    public bool IsLit()
    {
        return isLit;
    }
}
