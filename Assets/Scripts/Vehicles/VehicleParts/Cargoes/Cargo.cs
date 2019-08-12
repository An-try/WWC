using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : VehiclePart
{
    private protected int _cargoSize;

    [SerializeField] private Material[] _defaultMaterials;
    [SerializeField] private MeshRenderer[] _cargoRenderers;

    internal Material CargoMaterial
    {
        set
        {
            foreach (MeshRenderer meshRenderer in _cargoRenderers)
            {
                meshRenderer.material = value;
            }
        }
    }

    internal void SetDefaultMaterials()
    {
        for (int i = 0; i < _cargoRenderers.Length; i++)
        {
            _cargoRenderers[i].material = _defaultMaterials[i];
        }
    }
}
