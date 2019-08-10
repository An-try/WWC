using UnityEngine;

/// <summary>
/// Class for any lasers in the game. This script is attached to the cannons of laser turrets. 
/// </summary>
public class Laser : Ammo
{
    private LineRenderer lineRenderer; // Component that draws a line

    private float laserDamagePerHit = 10f;
    public float laserHitDuration;
    private float currentHitDuration;
    public float laserLength;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        lineRenderer = GetComponentInChildren<LineRenderer>(); // Get line renderer in the children's game object of this game object
    }

    public void SetHitDuration(float hitDuration)
    {
        laserHitDuration = hitDuration;
        currentHitDuration = hitDuration;
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    {
        if (currentHitDuration > 0) // If there is a laser duration
        {
            // Turn on the line renderer and draw a laser
            lineRenderer.enabled = true;
            DrawLaserAndDealDamage();
            currentHitDuration -= Time.deltaTime; // Decrease laser hit duration
        }
        else // If the laser duration time is up
        {
            lineRenderer.enabled = false; // Turn off the line renderer
        }
    }

    private void DrawLaserAndDealDamage()
    {
        // In this line renderer there are two points: starter point, from where laser starts and second point where laser hit something
        lineRenderer.SetPosition(0, transform.position); // Set first laser point to this game object position

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit)) // If raycast hit something
        {
            lineRenderer.SetPosition(1, hit.point); // Set the second point of the line renderer to the hit point

            // Deal a laser damage. Laser has a duration and each cycle of duration has several parts
            // Amount of this parts is calculates like this: HIT_DURATION / TIME_BETWEEN_EACH_DURATION_PART
            // For example: 0.1 / 0.02(Fixed update time) = 5 parts
            // Thus, the damage formula looks like this: DAMAGE_PER_HIT / (HIT_DURATION / TIME_BETWEEN_EACH_DURATION_PART)
            DamageManager.instance.DealLaserDamage(laserDamagePerHit / (laserHitDuration / Time.deltaTime), hit);
        }
        else // If raycast hit nothing
        {
            // Set the imaginary point in front of the turret
            Vector3 laserTargetPoint = new Vector3(transform.position.x + transform.forward.x * laserLength,
                                                   transform.position.y + transform.forward.y * laserLength,
                                                   transform.position.z + transform.forward.z * laserLength);

            lineRenderer.SetPosition(1, laserTargetPoint); // Set the second point of the line renderer to the imaginary point
        }
    }
}
