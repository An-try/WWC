using UnityEngine;

public class TwinLaserTurret : Turret
{
    [SerializeField] private GameObject RightLaserBeam;
    [SerializeField] private GameObject LeftLaserBeam;
    
    private float hitDuration;

    private protected override void SetTurretParameters()
    {
        base.SetTurretParameters();

        _turnRate = 30f;
        _turretRange = 50000f;
        _defaultCooldown = 0.1f;
        _currentCooldown = _defaultCooldown;
        
        _rightTraverse = 180f;
        _leftTraverse = 180f;
        _elevation = 40f;
        _depression = 10f;

        hitDuration = 0.1f;
    }

    private protected override void Shoot()
    {
        RightLaserBeam.GetComponent<Laser>().laserLength = _turretRange; // Set the laset lenght to the right laser beam
        RightLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration); // Set the hit duration to the right laser beam

        LeftLaserBeam.GetComponent<Laser>().laserLength = _turretRange; // Set the laset lenght to the left laser beam
        LeftLaserBeam.GetComponent<Laser>().SetHitDuration(hitDuration); // Set the hit duration to the left laser beam

        _currentCooldown = _defaultCooldown + hitDuration; // Add a cooldown to this turret

        _shootingSound.Play(); // Play an shoot sound
    }
}
