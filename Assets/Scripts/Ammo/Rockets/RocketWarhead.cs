/// <summary>
/// Warheads for any rocket.
/// </summary>
public class RocketWarhead
{
    public float HP = 0f;
    public float mass = 0f;

    public float kineticDamage = 0f;
    public float explosionDamage = 0f;
    public float flameDamage = 0f;
    public float fragmentDamage = 0f;

    public float flameTime = 0f;
    public int fragmentsAmount = 0;

    public void ShockKinetic()
    {
        HP = 250f;
        mass = 400f;

        kineticDamage = 300f;
    }

    public void Incendiary()
    {
        HP = 230f;
        mass = 350f;

        kineticDamage = 50f;
        explosionDamage = 100f;
        flameDamage = 20f;

        flameTime = 5f;
    }

    public void HighExplosiveFragmentation()
    {
        HP = 260f;
        mass = 450f;

        kineticDamage = 50f;
        explosionDamage = 100f;
        fragmentDamage = 15f;

        fragmentsAmount = 10;
    }

    public void Cumulative()
    {
        HP = 270f;
        mass = 400f;

        kineticDamage = 250f;
        explosionDamage = 100f;
    }

    public void Nuclear()
    {
        HP = 300f;
        mass = 600f;

        kineticDamage = 200f;
        explosionDamage = 500f;
    }
}
