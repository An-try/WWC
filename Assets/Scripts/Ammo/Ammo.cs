using UnityEngine;

/// <summary>
/// Base class for any ammo in the game.
/// </summary>
public abstract class Ammo : MonoBehaviour
{
    public GameObject SparksPrefab; // Sparks when hit something

    // TODO: Make an hit hole effect
    public GameObject HitHolePrefab; // Hit hole from the impact will be in the place of impact of the object
}
