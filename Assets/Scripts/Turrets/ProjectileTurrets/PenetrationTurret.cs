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

    public override void SetTurretParameters()
    {
        base.SetTurretParameters();

        _turnRate = 30f;
        _turretRange = 50000f;
        _cooldown = 1f;
        _currentCooldown = _cooldown;

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

        // Creating shoot animation with position and rotation of the shoot place. Also parent shoot animation to the shoot place
        GameObject shootAnimation = Instantiate(ShootAnimationPrefab, ShootPlace.transform.position, ShootPlace.transform.rotation, ShootPlace.transform);
        shootAnimation.GetComponentInChildren<ParticleSystem>().Play();
        Destroy(shootAnimation.gameObject, shootAnimation.GetComponentInChildren<ParticleSystem>().main.duration);

        // Creating bullet with position and rotation of the shoot place
        GameObject bullet = Instantiate(_projectilePrefab, ShootPlace.transform.position, ShootPlace.transform.rotation);
        // Add force to the bullet so it will fly directly
        bullet.GetComponent<Rigidbody>().AddForce((ShootPlace.transform.forward + scatter) * bulletForce);

        _currentCooldown = _cooldown; // Add a cooldown to this turret

        _shootingSound.Play(); // Play an shoot sound
        _shootingSound.pitch = Random.Range(0.9f, 1.1f);
        _animator.SetTrigger("Shoot");
    }
}
