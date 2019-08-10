using UnityEngine;

public class Missile : Rocket
{
    public override void Awake() // Awake is called when the script instance is being loaded
    {
        base.Awake(); // Calling the base class of this method


        /// Attaching components to the new missile:

        /// Attaching Warhead:
        //warheadMissile.ShockKinetic();
        rocketWarhead.Incendiary();
        //WarheadMissile.HighExplosiveFragmentation();
        //WarheadMissile.Cumulative();
        //WarheadMissile.Nuclear();

        /// Attaching Engine:
        rocketEngine.SolidFuel();
        //EngineMissile.LiquidPropellant();
        //EngineMissile.Hybrid();
        //EngineMissile.HypersonicStraightThroughAirJet();


        HP = rocketWarhead.HP + rocketEngine.HP; // Set missile health points based on components HP
    }
}
