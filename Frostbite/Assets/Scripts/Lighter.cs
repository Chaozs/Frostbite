using UnityEngine;

public class Lighter : MonoBehaviour
{
    /// <returns>
    /// Returns whether or not the lighter is currently on or not.
    /// </returns>
    public bool IsLit()
    {
        if (gameObject.activeSelf)
        {
            return true;
        }

        return false;
    }
}
