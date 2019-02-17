using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Transform torch;
    
    void Update()
    {
        /// Walking/idle torch animation
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            // Play running animation if character is moving
            torch.GetComponent<Animator>().Play("Running");
        }
        else
        {
            // Play idle animation if character is not moving
            torch.GetComponent<Animator>().Play("Idle");
        }
    }
}
