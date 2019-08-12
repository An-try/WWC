using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabin : VehiclePart
{
    private protected float _maxSpeed;
    private protected float _power;

    [SerializeField] private Material[] _defaultMaterials;
    [SerializeField] private MeshRenderer[] _cabinRenderers;

    internal Material CabinMaterial
    {
        set
        {
            foreach (MeshRenderer meshRenderer in _cabinRenderers)
            {
                meshRenderer.material = value;
            }
        }
    }

    internal void SetDefaultMaterials()
    {
        for (int i = 0; i < _cabinRenderers.Length; i++)
        {
            _cabinRenderers[i].material = _defaultMaterials[i];
        }
    }
}
