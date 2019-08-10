using UnityEngine;

/// <summary>
/// Class for any projectile.
/// </summary>
public abstract class Projectile : Ammo
{
    private float kineticDamage = 300f;

    private float lifeTime = 60f;

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        Destroy(gameObject, lifeTime); // Destroy plasma projectile after a while
    }

    private void OnCollisionEnter(Collision collision) // Called when this collider/rigidbody has begun touching another rigidbody/collider
    {
        DamageManager.instance.DealProjectileDamage(kineticDamage, collision); // Call a method of dealing damage by this rocket to the hitted ship

        DestroyProjectile(); // Destroy this projectile
    }

    private void DestroyProjectile()
    {
        GameObject spark = Instantiate(SparksPrefab); // Instantiate sparks
        spark.transform.position = transform.position; // Set sparks position
        Destroy(spark, 0.3f); // Destroy sparks after some time
        Destroy(gameObject); // Destroy this projectile
    }
}
