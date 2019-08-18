using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal bool IsAlive = true;

    internal GameObject CurrentAxis;
    internal GameObject CurrentCabin;
    internal GameObject CurrentCargo;

    private bool _autoFire = false;
}
