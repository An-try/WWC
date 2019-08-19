using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls all damage in the game.
/// </summary>
public class DamageManager : MonoBehaviour
{
    public static DamageManager instance; // Singleton for this script

    private void Awake() // Awake is called when the script instance is being loaded
    {
        if (instance == null) // If instance not exist
        {
            instance = this; // Set up instance as this script
        }
        else //If instance already exists
        {
            Destroy(this); // Destroy this script
        }
    }

    // Each damage is calculated by this formula: DAMAGE - DAMAGE * DEFENCE / 100
    // For example: DAMAGE is 500 and DEFENCE is 10(in percents). Thus, TOTAL_DAMAGE = 500 - 500 * 10 / 100 = 500 - 50 = 450

    public void DealProjectileDamage(float kineticDamage, Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Vehicle>() == null) // If the game object that was hit does not have a Ship script
        {
            return; // Return from this method and do not deal any damage because this object is not a ship
        }

        Vehicle carScript = collision.gameObject.GetComponentInParent<Vehicle>(); // Get Ship script of hitted ship
        float totalDamage = kineticDamage - kineticDamage * carScript.kineticDefence / 100f; // Set total damage
        carScript.Durability -= totalDamage; // Apply damage to ship
    }

    public void DealLaserDamage(float laserDamage, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Vehicle>() == null) // If the game object that was hit does not have a Ship script
        {
            return; // Return from this method and do not deal any damage because this object is not a ship
        }

        Vehicle carScript = hit.transform.GetComponentInParent<Vehicle>(); // Get Ship script of hitted ship
        float totalDamage = laserDamage - laserDamage * carScript.laserDefence / 100f; // Set total damage
        carScript.Durability -= totalDamage; // Apply damage to ship
    }

    public void DealRocketDamage(Rocket rocket, Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Vehicle>() == null) // If the game object that was hit does not have a Ship script
        {
            return; // Return from this method and do not deal any damage because this object is not a ship
        }

        Vehicle carScript = collision.gameObject.GetComponentInParent<Vehicle>(); // Get Ship script of hitted ship

        // Set the damage
        float kineticDamage = rocket.rocketWarhead.kineticDamage - rocket.rocketWarhead.kineticDamage * carScript.kineticDefence / 100f;
        float explosionDamage = rocket.rocketWarhead.explosionDamage - rocket.rocketWarhead.explosionDamage * carScript.explosionDefence / 100f;
        float fragmentDamage = (rocket.rocketWarhead.fragmentDamage - rocket.rocketWarhead.fragmentDamage * carScript.fragmentDefence / 100f) * rocket.rocketWarhead.fragmentsAmount;

        float totalDamage = kineticDamage + explosionDamage + fragmentDamage; // Set total damage

        carScript.Durability -= totalDamage; // Apply damage to ship

        // Start ship burning if the weapon has flame effect
        if (rocket.rocketWarhead.flameTime > 0) // If the weapon has flame time
        {
            // Start a coroutine of burning that deals damage over time
            StartCoroutine(Burning(carScript, rocket.rocketWarhead.flameDamage, rocket.rocketWarhead.flameTime));
            carScript.vehicleFiresAmount++; // Add an ship fire to ship fires amount of this ship. Thus, we can check if ship is on fire
        }
    }
    
    public IEnumerator Burning(Vehicle vehicle, float flameForce, float flameTime)
    {
        float flameTimeStep = 1f;

        while (flameTime > 0) // While there is a flame time
        {
            yield return new WaitForSeconds(flameTimeStep); // Wait for flame time
            float damage = (flameForce - flameForce * vehicle.flameDefence / 100f) * flameTimeStep;
            vehicle.Durability -= damage; // Set damage including time step
            flameTime -= flameTimeStep; // Decrease the flame time
        }

        vehicle.vehicleFiresAmount--; // Subtract the number of fires when the fire goes out
    }
}
