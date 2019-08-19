using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Turret : MonoBehaviour
{
    private protected AudioSource _shootingSound;

    //public Item item; // Item for this turret

    private protected GameObject _opportuneTargetWithParameter;
    private Methods.SearchParameter _targetSearchParameter = Methods.SearchParameter.Distance;
    private List<string> _targetTags = new List<string>();

    [SerializeField] private GameObject _turretBase; // Base platform of the turret that rotates horizontally
    [SerializeField] private GameObject _turretCannons; // Cannons of the turret that totates vertically

    [SerializeField] private protected GameObject _shootPlace;
    [SerializeField] private protected GameObject _shootAnimation;
    //public GameObject HitHole;

    private protected Vector3 _aimPoint; // The point that the turret should look at

    private protected float _turnRate; // Turret turning speed
    private protected float _turretRange;
    private protected float _defaultCooldown;
    private protected float _currentCooldown;

    [Range(0.0f, 180.0f)] private protected float _rightTraverse; // Maximum right turn in degrees
    [Range(0.0f, 180.0f)] private protected float _leftTraverse; // Maximum left turn in degrees
    [Range(0.0f, 90.0f)] private protected float _elevation; // Maximum turn up in degrees
    [Range(0.0f, 90.0f)] private protected float _depression; // Maximum turn down in degrees

    private protected bool _turretAI; // If the turret is controlled by AI

    private protected virtual void Awake()
    {
        _shootingSound = GetComponent<AudioSource>();
    }

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        SetTurretParameters();
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    {
        if (_turretAI) // If the turret is controlled by AI
        {
            AutomaticTurretControl();
        }
        else // If the turret is not controlled by AI
        {
            ManualTurretControl();
        }

        CooldownDecrease();
    }

    private protected abstract void Shoot();

    private protected virtual void SetTurretParameters()
    {
        _targetTags.AddRange(Manager.Instance.AllGameTags); // Set enemies as targets for this turret

        // Check this turret tag
        switch (transform.tag)
        {
            case "Player":  // If this turret is on a player ship
                _turretAI = Player.Instance.AutoFire; // Set if the turret is controlled by AI
                Player.Instance.autoFireChangeEventHandler += ChangeTurretAutoFire;
                _targetTags.Remove("Player");
                //TargetsList = Manager.Enemies; // Set enemies as targets for this turret
                break;
            case "Ally": // If this turret is on an ally ship
                _turretAI = true; // Set the turret under AI control
                _targetTags.Remove("Player");
                _targetTags.Remove("Ally");
                break;
            case "Enemy": // If this turret is on an enemy ship
                _turretAI = true; // Set the turret under AI control
                _targetTags.Remove("Enemy");
                break;
            default:
                _turretAI = true;
                _targetTags.Clear();
                Debug.LogError("Cannon tag <b>\"" + gameObject.tag + "\"</b> is not valid. Cannon name: <b>\"" + gameObject.name +
                    "\"</b>. Cannon world position: <b>" + transform.position + "</b>. <i>This cannon will not shoot</i>.");
                break;
        }

        InvokeRepeating("SearchTheOpportuneTargetByParameter", 0, 1);
    }

    private void SearchTheOpportuneTargetByParameter()
    {
        _opportuneTargetWithParameter = Methods.SearchOpportuneTargetByParameter(transform, _targetTags, _targetSearchParameter);
    }

    // Set the turret AI. This method executes by delegate of PlayerMovement script
    private void ChangeTurretAutoFire(bool turretAutoFire)
    {
        _turretAI = turretAutoFire;
    }

    // Decrease turret cooldown each fixed update
    private void CooldownDecrease()
    {
        if (_currentCooldown >= 0)
        {
            _currentCooldown -= Time.fixedDeltaTime;
        }
    }

    private bool CooldownIsZero()
    {
        if (_currentCooldown <= 0)
        {
            return true;
        }
        return false;
    }

    // Method executes when turret AI is enabled
    private void AutomaticTurretControl()
    {
        if (_opportuneTargetWithParameter) // If there is any target
        {
            _aimPoint = _opportuneTargetWithParameter.transform.position; // Set a target position as an aim point

            RotateBase(); // Totate base of the turret to the aim point
            RotateCannons(); // Totate cannons of the turret to the aim point
        }
        else // If there is no target
        {
            RotateToDefault(); // Rotate turret to default
        }

        // If the turret is aimed at the enemy, its cooldown is zero and it is not aimed at the owner
        if (AimedAtEnemy() && CooldownIsZero() && !AimedAtOwner())
        {
            Shoot();
        }
    }

    // Method executes when turret AI is disabled
    private void ManualTurretControl()
    {
        _aimPoint = PlayerCameraController.instance.cameraLookingPoint; // Set the point on which camera is looking as an aim point

        RotateBase(); // Totate base of the turret to the aim point
        RotateCannons(); // Totate cannons of the turret to the aim point

        // If a player press left mouse button, turret cooldown is zero and it is not aimed at the owner
        if (Input.GetKey(KeyCode.Mouse0) && CooldownIsZero() && !AimedAtOwner())
        {
            Shoot();
        }
    }

    private void RotateBase()
    {
        // Get local position of aim point in relative to this turret
        Vector3 localTargetPos = transform.InverseTransformPoint(_aimPoint);
        localTargetPos.y = 0f; // Put the aiming point at the same height with this tower

        Vector3 clampedLocalVector2Target = localTargetPos; // New point to rotate with clamped rotate traverses

        if (localTargetPos.x >= 0f) // If the aim point is located to the right of the turret
        {
            // Set new position to rotate with max right traverse
            clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _rightTraverse, float.MaxValue);
        }
        else // If the aim point is located to the left of the turret
        {
            // Set new position to rotate with max left traverse
            clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _leftTraverse, float.MaxValue);
        }

        Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVector2Target); // Create a new rotation that looking at new point
        // Rotates current turret to the new quaternion
        Quaternion newRotation = Quaternion.RotateTowards(_turretBase.transform.localRotation, rotationGoal, _turnRate * Time.fixedDeltaTime);

        _turretBase.transform.localRotation = newRotation; // Apply intermediate rotation to the turret
    }

    private void RotateCannons()
    {
        // Get local position of aim point in relative to this turret
        Vector3 localTargetPos = _turretBase.transform.InverseTransformPoint(_aimPoint);
        localTargetPos.x = 0f; // Put the aiming point at the same vertical with this tower

        Vector3 clampedLocalVec2Target = localTargetPos; // New point to rotate with clamped rotate traverses

        if (localTargetPos.y >= 0f) // If the aim point is located above the turret
        {
            // Set new position to rotate with max up traverse
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _elevation, float.MaxValue);
        }
        else // If the aim point is located below the turret
        {
            // Set new position to rotate with max down traverse
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _depression, float.MaxValue);
        }

        Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target); // Create a new rotation that looking at new point
        // Rotates current turret to the new quaternion
        Quaternion newRotation = Quaternion.RotateTowards(_turretCannons.transform.localRotation, rotationGoal, _turnRate * Time.fixedDeltaTime);

        _turretCannons.transform.localRotation = newRotation; // Apply intermediate rotation to the turret
    }

    private void RotateToDefault()
    {
        // Set new intermediate rotation of base and cannons to default rotation
        Quaternion newBaseRotation =
            Quaternion.RotateTowards(_turretBase.transform.localRotation, Quaternion.identity, _turnRate * Time.fixedDeltaTime);
        Quaternion newCannonRotation =
            Quaternion.RotateTowards(_turretCannons.transform.localRotation, Quaternion.identity, 2.0f * _turnRate * Time.fixedDeltaTime);

        // Apply intermediate rotation
        _turretBase.transform.localRotation = newBaseRotation;
        _turretCannons.transform.localRotation = newCannonRotation;
    }

    private protected virtual bool AimedAtEnemy()
    {
        // Select specific layers by shifting the bits. These layers will be ignored by the turret raycast
        // Layer 8 is a bullet and 9 is a missile
        int layerMask = (1 << 8) | (1 << 9);
        layerMask = ~layerMask; // Invert these layers. So raycast will ignore bullets and missiles

        // Create an outgoing ray from cannons with turret range lenght
        Ray aimingRay = new Ray(_turretCannons.transform.position, _turretCannons.transform.forward * _turretRange);

        // If the turret is targeting an object except bullets and rockets (determined by layerMask)
        if (Physics.Raycast(aimingRay, out RaycastHit hit, _turretRange, layerMask))
        {
            if (hit.collider.transform.root.gameObject == _opportuneTargetWithParameter) // If aiming on current nearest target
            {
                return true; // Aimed at the enemy
            }
            else // If not aiming on current nearest target
            {
                return false; // Not aimed at the enemy
            }
        }
        else // If turret is not aimed at anything
        {
            return false; // Not aimed at the enemy
        }
    }

    // If the turret aimed at the ship on which it is attached
    private bool AimedAtOwner()
    {
        // Create an outgoing ray from cannons with turret range lenght
        Ray aimingRay = new Ray(_turretCannons.transform.position, _turretCannons.transform.forward * _turretRange);

        if (Physics.Raycast(aimingRay, out RaycastHit hit, _turretRange)) // If turret aiming at some object
        {
            if (hit.collider.transform.root == gameObject.transform.root) // If this object is the current ship on which turret is attached
            {
                return true; // Aimed at the owner
            }
            else
            {
                return false; // Not aimed at the owner
            }
        }
        return false; // Not aimed at the owner
    }
}
