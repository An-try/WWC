using UnityEngine;

public class PlasmaTurret : Turret
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _rightShootPlace;
    [SerializeField] private GameObject _leftShootPlace;

    private float _bulletForce;
    private float _turretScatter;

    private protected override void SetTurretParameters()
    {
        base.SetTurretParameters();

        _turnRate = 30f;
        _turretRange = 50000f;
        _defaultCooldown = 1f;
        _currentCooldown = _defaultCooldown;

        _rightTraverse = 180f;
        _leftTraverse = 180f;
        _elevation = 80f;
        _depression = 10f;

        _bulletForce = 50000f;
        _turretScatter = 0.001f;

        //_shootPlace = _rightShootPlace;
    }

    private protected override void Shoot()
    {



        ///
        //// Change shoot place(right or left in turn)
        //if (_shootPlace == _rightShootPlace)
        //{
        //    _shootPlace = _leftShootPlace;
        //}
        //else
        //{
        //    _shootPlace = _rightShootPlace;
        //}

        //// Scatter while firing
        //Vector3 scatter = new Vector3(Random.Range(-_turretScatter, _turretScatter),
        //                              Random.Range(-_turretScatter, _turretScatter),
        //                              Random.Range(-_turretScatter, _turretScatter));
        ///



        // Play shooting animation
        if (_shootAnimation.GetComponent<ParticleSystem>())
        {
            _shootAnimation.GetComponent<ParticleSystem>().Play();
        }
        foreach (ParticleSystem particleSystem in _shootAnimation.GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Play();
        }

        // Creating bullet with position and rotation of the shoot place
        GameObject bullet = Instantiate(_projectilePrefab, _shootPlace.transform.position, _shootPlace.transform.rotation);
        // Add force to the bullet so it will fly directly
        bullet.GetComponent<Rigidbody>().AddForce((_shootPlace.transform.forward/* + scatter*/) * _bulletForce);

        _currentCooldown = _defaultCooldown; // Add a cooldown to this turret

        _shootingSound.Play(); // Play an shoot sound
    }
}
