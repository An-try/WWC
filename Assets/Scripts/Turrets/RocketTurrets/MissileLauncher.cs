using UnityEngine;

public class MissileLauncher : Turret
{
    public GameObject MissilePrefab;

    public override void SetTurretParameters()
    {
        base.SetTurretParameters();

        _turnRate = 30f;
        _turretRange = 50000f;
        _cooldown = 5f;
        _currentCooldown = _cooldown;

        _rightTraverse = 180f;
        _leftTraverse = 180f;
        _elevation = 40f;
        _depression = 10f;
    }

    public override bool AimedAtEnemy()
    {
        // Missile launcher always "aimed on enemy" if there is the nearest target
        if (NearestTargetWithParameter != null)
        {
            return true;
        }
        return false;
    }

    private protected override void Shoot()
    {
        GameObject missile = Instantiate(MissilePrefab); // Create a new missile
        
        missile.transform.position = TurretCannons.transform.position; // Set missile position to this turret cannons position
        missile.transform.rotation = TurretCannons.transform.rotation; // Set missile rotation to this turret cannons rotation
        missile.tag = gameObject.tag; // Set missile tag equal to this turret

        _currentCooldown = _cooldown; // Add a cooldown to this turret

        _shootingSound.Play(); // Play an shoot sound
    }
}
