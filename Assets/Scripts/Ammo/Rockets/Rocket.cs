using System.Collections.Generic;
using UnityEngine;

public abstract class Rocket : Ammo
{
    public float HP = 0f; // Health of the rocket

    private GameObject _opportuneTargetWithParameter;
    private Methods.SearchParameter _targetSearchParameter = Methods.SearchParameter.Distance;
    private List<string> _targetTags = new List<string>();

    private Rigidbody RocketRigidbody;

    public RocketWarhead rocketWarhead; // Rocket warhead component
    public RocketEngine rocketEngine; // Rocket engine component
    
    public GameObject ExplosionPrefab; // Explosion effect when rocket hit something
    public GameObject ExplosionSmokePrefab; // Smoke that takes off from a rocket

    public virtual void Awake() // Awake is called when the script instance is being loaded
    {
        RocketRigidbody = GetComponent<Rigidbody>(); // Get the rigidbody attached to this game object

        rocketWarhead = new RocketWarhead();
        rocketEngine = new RocketEngine();
    }

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        SetRocketParameters();
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    {
        if (HP <= 0)
        {
            DestroyRocket();
        }

        // Add missile velocity to the limit
        if (rocketEngine.missileVelocity < rocketEngine.maxMissileVelocity)
        {
            rocketEngine.missileVelocity++;
        }

        RocketRigidbody.velocity = transform.forward * rocketEngine.missileVelocity; // Apply velocity to the missile

        ObstaclesAvoidance(); // Avoid obstacles and move to the target
    }

    private void SetRocketParameters()
    {
        _targetTags.AddRange(Manager.Instance.AllGameTags); // Set enemies as targets for this turret

        // Check this turret tag
        switch (transform.tag)
        {
            case "Player":  // If this turret is on a player ship
                _targetTags.Remove("Player");
                //TargetsList = Manager.Enemies; // Set enemies as targets for this turret
                break;
            case "Ally": // If this turret is on an ally ship
                _targetTags.Remove("Player");
                _targetTags.Remove("Ally");
                break;
            case "Enemy": // If this turret is on an enemy ship
                _targetTags.Remove("Enemy");
                break;
            default:
                _targetTags.Clear();
                Debug.LogError("Rocket tag <b>\"" + gameObject.tag + "\"</b> is not valid. Rocket name: <b>" + gameObject.name +
                    ".</b> Rocket world position: <b>" + transform.position + ".</b> <i>This rocket will not be aimed at any target</i>.");
                break;
        }

        InvokeRepeating("SearchTheOpportuneTargetByParameter", 0, 1); // Search the nearest target each period of time

        Destroy(gameObject, 60f); // Destroy the rocket after some time
    }

    private void SearchTheOpportuneTargetByParameter()
    {
        _opportuneTargetWithParameter = Methods.SearchOpportuneTargetByParameter(transform, _targetTags, _targetSearchParameter);
    }

    private void ObstaclesAvoidance() // TODO: Improve the obstacles avoidance algorithm
    {
        float rayLength = 400f;
        float rocketSizeToAvoid = 3f;

        #region DEBUG RAYS
        Debug.DrawRay(transform.position + new Vector3(rocketSizeToAvoid, rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);
        Debug.DrawRay(transform.position + new Vector3(rocketSizeToAvoid, -rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);
        Debug.DrawRay(transform.position + new Vector3(-rocketSizeToAvoid, rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);
        Debug.DrawRay(transform.position + new Vector3(-rocketSizeToAvoid, -rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);

        Debug.DrawRay(transform.position, (transform.forward + Vector3.up) * rayLength);
        Debug.DrawRay(transform.position, (transform.forward + Vector3.up + Vector3.right) * rayLength);
        Debug.DrawRay(transform.position, (transform.forward + Vector3.right) * rayLength);
        Debug.DrawRay(transform.position, (transform.forward + Vector3.right + Vector3.down) * rayLength);
        Debug.DrawRay(transform.position, (transform.forward + Vector3.down) * rayLength);
        Debug.DrawRay(transform.position, (transform.forward + Vector3.down + Vector3.left) * rayLength);
        Debug.DrawRay(transform.position, (transform.forward + Vector3.left) * rayLength);
        Debug.DrawRay(transform.position, (transform.forward + Vector3.left + Vector3.up) * rayLength);
        #endregion

        // Rays that check whether the path is free in front of the rocket
        Ray rayForwardFirst  = new Ray(transform.position + new Vector3(rocketSizeToAvoid, rocketSizeToAvoid, 0f),   transform.forward);
        Ray rayForwardSecond = new Ray(transform.position + new Vector3(rocketSizeToAvoid, -rocketSizeToAvoid, 0f),  transform.forward);
        Ray rayForwardThird  = new Ray(transform.position + new Vector3(-rocketSizeToAvoid, rocketSizeToAvoid, 0f),  transform.forward);
        Ray rayForwardFourth = new Ray(transform.position + new Vector3(-rocketSizeToAvoid, -rocketSizeToAvoid, 0f), transform.forward);

        // Rays that check whether the path is clear in different directions in front of the rocket
        Ray rayUp        = new Ray(transform.position, transform.forward + Vector3.up);
        Ray rayUpRight   = new Ray(transform.position, transform.forward + Vector3.up + Vector3.right);
        Ray rayRight     = new Ray(transform.position, transform.forward + Vector3.right);
        Ray rayRightDown = new Ray(transform.position, transform.forward + Vector3.right + Vector3.down);
        Ray rayDown      = new Ray(transform.position, transform.forward + Vector3.down);
        Ray rayDownLeft  = new Ray(transform.position, transform.forward + Vector3.down + Vector3.left);
        Ray rayLeft      = new Ray(transform.position, transform.forward + Vector3.left);
        Ray rayLeftUp    = new Ray(transform.position, transform.forward + Vector3.left + Vector3.up);

        RaycastHit hit = new RaycastHit();

        // If there is any obstacle on the rocket way
        if (Physics.Raycast(rayForwardFirst, out hit, rayLength / 2) || Physics.Raycast(rayForwardSecond, out hit, rayLength / 2) ||
            Physics.Raycast(rayForwardThird, out hit, rayLength / 2) || Physics.Raycast(rayForwardFourth, out hit, rayLength / 2))
        {
            // If forward rays of this missile does not cross current target of this missile avoid this object
            if (hit.collider.transform.root.gameObject != _opportuneTargetWithParameter)
            {
                // Try to avoid by rotating to the new direction that have no obstacles
                if (!Physics.Raycast(rayUp, out hit, rayLength)) // If the upper ray hits nothing
                {
                    RotateToTarget(false, rayUp.GetPoint(rayLength)); // Move to the new point up
                    return;
                }
                if (!Physics.Raycast(rayUpRight, out hit, rayLength)) // If the upper right ray hits nothing
                {
                    RotateToTarget(false, rayUpRight.GetPoint(rayLength)); // Move to the new point up right
                    return;
                }
                if (!Physics.Raycast(rayRight, out hit, rayLength)) // If the right ray hits nothing
                {
                    RotateToTarget(false, rayRight.GetPoint(rayLength)); // Move to the new point right
                    return;
                }
                if (!Physics.Raycast(rayRightDown, out hit, rayLength)) // If the lower right ray hits nothing
                {
                    RotateToTarget(false, rayRightDown.GetPoint(rayLength)); // Move to the new point right down
                    return;
                }
                if (!Physics.Raycast(rayDown, out hit, rayLength)) // If the lower ray hits nothing
                {
                    RotateToTarget(false, rayDown.GetPoint(rayLength)); // Move to the new point down
                    return;
                }
                if (!Physics.Raycast(rayDownLeft, out hit, rayLength)) // If the lower left ray hits nothing
                {
                    RotateToTarget(false, rayDownLeft.GetPoint(rayLength)); // Move to the new point down left
                    return;
                }
                if (!Physics.Raycast(rayLeft, out hit, rayLength)) // If the left ray hits nothing
                {
                    RotateToTarget(false, rayLeft.GetPoint(rayLength)); // Move to the new point left
                    return;
                }
                if (!Physics.Raycast(rayLeftUp, out hit, rayLength)) // If the upper left ray hits nothing
                {
                    RotateToTarget(false, rayLeftUp.GetPoint(rayLength)); // Move to the new point left up
                    return;
                }
            }
        }
        else // If there isn't obstacle on the rocket way
        {
            RotateToTarget(true, transform.position + transform.forward); // Move rocket to target. If there is no target move forward
        }
    }

    private void RotateToTarget(bool moveToTarget, Vector3 newAvoidingPosition) // Move to the new position
    {
        if (_opportuneTargetWithParameter && moveToTarget) // If target exists and moving to target is allow
        {
            Quaternion targetRotation = Quaternion.LookRotation(_opportuneTargetWithParameter.transform.position - transform.position); // Set new look rotation based on target
            RocketRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rocketEngine.turnRate)); // Apply new rotation
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(newAvoidingPosition - transform.position); // Set new look rotation based on avoiding
            RocketRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rocketEngine.turnRate)); // Apply new rotation
        }
    }
    
    private void OnCollisionEnter(Collision collision) // Called when this collider/rigidbody has begun touching another rigidbody/collider
    {
        DamageManager.instance.DealRocketDamage(this, collision); // Call a method of dealing damage by this rocket to the hitted ship

        DestroyRocket(); // Call destroy rocket method
    }

    private void DestroyRocket()
    {
        // Instantiate an explosion effects
        GameObject spark = Instantiate(SparksPrefab, transform.position, Quaternion.identity);
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        GameObject smoke = Instantiate(ExplosionSmokePrefab, transform.position, Quaternion.identity);

        // Destroy this objects after some time
        Destroy(spark.gameObject, 2f);
        Destroy(explosion.gameObject, 2f);
        Destroy(smoke.gameObject, 2f);

        Destroy(gameObject); // Destroy rocket
    }
}
