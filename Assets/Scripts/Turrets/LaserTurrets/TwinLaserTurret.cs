using UnityEngine;

public class TwinLaserTurret : Turret
{
    public GameObject RightLaserBeam;
    public GameObject LeftLaserBeam;
    
    public float hitDuration;

    public override void SetTurretParameters()
    {
        base.SetTurretParameters();

        _turnRate = 30f;
        _turretRange = 50000f;
        _cooldown = 0.1f;
        _currentCooldown = _cooldown;
        
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

        _currentCooldown = _cooldown + hitDuration; // Add a cooldown to this turret

        GetComponent<AudioSource>().Play(); // Play an shoot sound
    }
}
