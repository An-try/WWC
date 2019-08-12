using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePart : MonoBehaviour
{
    private protected float _hp;
    private protected float _armor;
    private protected float _weight;
    private protected float _penetratingProtection;
    private protected float _explosiveProtection;

    [SerializeField] private Sprite _vehiclePartImage;
}
