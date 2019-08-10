/// <summary>
/// Engines for any rocket.
/// </summary>
public class RocketEngine
{
    public float HP = 0f;
    public float mass = 0f;

    public float missileVelocity = 0f;
    public float maxMissileVelocity = 0f;
    public float turnRate = 0f;

    public void SolidFuel()
    {
        HP = 100f;
        mass = 400f;

        missileVelocity = 100.0f;
        maxMissileVelocity = 500.0f;
        turnRate = 2.0f;
    }

    public void LiquidPropellant()
    {
        HP = 110f;
        mass = 400f;

        missileVelocity = 100.0f;
        maxMissileVelocity = 500.0f;
        turnRate = 2.0f;
    }

    public void Hybrid()
    {
        HP = 125f;
        mass = 400f;

        missileVelocity = 100.0f;
        maxMissileVelocity = 500.0f;
        turnRate = 2.0f;
    }

    public void HypersonicStraightThroughAirJet()
    {
        HP = 150f;
        mass = 400f;

        missileVelocity = 100.0f;
        maxMissileVelocity = 500.0f;
        turnRate = 2.0f;
    }
}
