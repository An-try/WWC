using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    internal float Hp; // Ship health points

    // Defence in percentage
    public float kineticDefence = 10f;
    public float explosionDefence = 10f;
    public float laserDefence = 10f;
    public float flameDefence = 10f;
    public float fragmentDefence = 10f;

    public float maxSpeed = 100f;
    public int transmission = 0; // Transmission can be positive and negative. This defines the ship moving direction - forward or backward

    public int vehicleFiresAmount = 0;

    //public GameObject MainHull;
    //public MeshRenderer MainHullTexture;

    public GameObject TargetCameraRotatesAround; // Player's ship game object around which the camera rotates

    public GameObject ShipBurningParticles;
    
    // Turret places on this ship
    public List<GameObject> LaserTurretPlaces;
    public List<GameObject> PlasmaTurretPlaces;
    public List<GameObject> RocketTurretPlaces;

    private protected virtual void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        // Spawn laser beam turrets on the ship
        foreach (GameObject TurretPlace in LaserTurretPlaces)
        {
            GameObject laserPulseTurret = Instantiate(Manager.Instance.LaserPulseTurretPrefab, TurretPlace.transform.position, TurretPlace.transform.rotation, TurretPlace.transform);
            laserPulseTurret.tag = gameObject.tag; // Set this ship tag to the new turret
        }

        // Spawn plasma turrets on the ship
        foreach (GameObject TurretPlace in PlasmaTurretPlaces)
        {
            GameObject plasmaTurret = Instantiate(Manager.Instance.PlasmaTurretPrefab, TurretPlace.transform.position, TurretPlace.transform.rotation, TurretPlace.transform);
            plasmaTurret.tag = gameObject.tag; // Set this ship tag to the new turret
        }

        // Spawn rocket turrets on the ship
        foreach (GameObject TurretPlace in RocketTurretPlaces)
        {
            GameObject rocketTurret = Instantiate(Manager.Instance.RocketTurretPrefab, TurretPlace.transform.position, TurretPlace.transform.rotation, TurretPlace.transform);
            rocketTurret.tag = gameObject.tag; // Set this ship tag to the new turret
        }

        // Set this ship tag to all game objects with colliders on the ship
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.gameObject.tag = gameObject.tag;
        }

        //InvokeRepeating("SetShipBurningSimulationIfShipIsOnFire", 0f, 0.1f); // Start or stop ship burning simulation
    }

    //private void FixedUpdate()
    //{
    //    if (Hp <= 0) // If health points are zero
    //    {
    //        DestroyCar(); // Destroy this ship
    //        return;
    //    }
    //}

    //private void SetShipBurningSimulationIfShipIsOnFire()
    //{
    //    for (int i = 0; i < ShipBurningParticles.transform.childCount; i++) // Pass all game objects that contains burning particles
    //    {
    //        // Get particle system on game object that contains particles
    //        ParticleSystem particleSystem = ShipBurningParticles.transform.GetChild(i).GetComponent<ParticleSystem>();
    //        var mainParticlesSettings = particleSystem.main; // Get main particle system settings
    //        Light burningLight = particleSystem.GetComponentInChildren<Light>(); // Get light that lights when ship is burning

    //        if (shipFiresAmount > 0) // If there is any fire on the ship
    //        {
    //            mainParticlesSettings.loop = true; // Set looping to burning particles
    //            particleSystem.Play(); // Start a burning particle system
    //            burningLight.enabled = true; // Turn on the burning light
    //            burningLight.intensity = Random.Range(250, 351); // Change the intensity of burning light to create a simple burning sensation
    //        }
    //        else
    //        {
    //            mainParticlesSettings.loop = false; // Set no looping to burning particles
    //            burningLight.enabled = false; // Turn off the burning light
    //        }
    //    }
    //}

    private void DestroyCar()
    {
        Destroy(gameObject); // Destroy this game object
    }
}
