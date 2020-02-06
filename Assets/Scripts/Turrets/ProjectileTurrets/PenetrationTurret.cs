using UnityEngine;

public class PenetrationTurret : Turret
{
    private Animator _animator;
    [SerializeField] private GameObject _projectilePrefab;

    private float bulletForce;
    private float turretScatter;

    private protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    private protected override void SetTurretParameters()
    {
        base.SetTurretParameters();

        _turnRate = 30f;
        _turretRange = 50000f;
        _defaultCooldown = 1f;
        _currentCooldown = _defaultCooldown;

        _rightTraverse = 180f;
        _leftTraverse = 180f;
        _elevation = 40f;
        _depression = 10f;

        bulletForce = 50000f;
        turretScatter = 0.001f;
    }

    private protected override void Shoot()
    {
        // Scatter while firing
        Vector3 scatter = new Vector3(Random.Range(-turretScatter, turretScatter),
                                      Random.Range(-turretScatter, turretScatter),
                                      Random.Range(-turretScatter, turretScatter));

        // Play shooting animation
        if (_shootAnimation.GetComponent<ParticleSystem>())
        {
            _shootAnimation.GetComponent<ParticleSystem>().Play();
        }
        foreach (ParticleSystem particleSystem in _shootAnimation.GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Play();
        }

        // Do recoil force
        transform.root.GetComponent<Rigidbody>().AddForce(_turretCannons.transform.forward * -50000);

        // Creating bullet with position and rotation of the shoot place
        GameObject bullet = Instantiate(_projectilePrefab, _shootPlace.transform.position, _shootPlace.transform.rotation);
        // Add force to the bullet so it will fly directly
        bullet.GetComponent<Rigidbody>().AddForce((_shootPlace.transform.forward + scatter) * bulletForce);

        _currentCooldown = _defaultCooldown; // Add a cooldown to this turret

        _shootingSound.Play(); // Play an shoot sound
        _shootingSound.pitch = Random.Range(0.9f, 1.1f);
        _animator.SetTrigger("Shoot");
    }
}
