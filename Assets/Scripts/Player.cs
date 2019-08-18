using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public delegate void AutoFireChangeEventHandler(bool autoFire);
    public AutoFireChangeEventHandler autoFireChangeEventHandler;

    public bool IsAlive = true;

    public GameObject CurrentAxis;
    public GameObject CurrentCabin;
    public GameObject CurrentCargo;

    public bool AutoFire = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
