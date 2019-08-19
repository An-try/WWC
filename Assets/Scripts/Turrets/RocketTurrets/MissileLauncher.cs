using UnityEngine;

public class MissileLauncher : Turret
{
    [SerializeField] private GameObject _missilePrefab;

    private protected override void SetTurretParameters()
    {
        base.SetTurretParameters();

        _turnRate = 30f;
        _turretRange = 50000f;
        _defaultCooldown = 5f;
        _currentCooldown = _defaultCooldown;

        _rightTraverse = 180f;
        _leftTraverse = 180f;
        _elevation = 40f;
        _depression = 10f;
    }

    private protected override bool AimedAtEnemy()
    {
        // Missile launcher always "aimed on enemy" if there is the nearest target
        if (_opportuneTargetWithParameter != null)
        {
            return true;
        }
        return false;
    }

    private protected override void Shoot()
    {
        GameObject missile = Instantiate(_missilePrefab); // Create a new missile
        
        missile.transform.position = _shootPlace.transform.position; // Set missile position to this turret cannons position
        missile.transform.rotation = _shootPlace.transform.rotation; // Set missile rotation to this turret cannons rotation
        missile.tag = gameObject.tag; // Set missile tag equal to this turret

        _currentCooldown = _defaultCooldown; // Add a cooldown to this turret

        _shootingSound.Play(); // Play an shoot sound
    }
}
