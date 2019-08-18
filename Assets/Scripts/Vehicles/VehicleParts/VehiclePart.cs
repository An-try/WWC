using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VehiclePart : MonoBehaviour
{
    private protected string _name;
    private protected float _durability;
    private protected float _armor;
    private protected float _weight;
    private protected float _penetratingProtection;
    private protected float _explosiveProtection;

    [SerializeField] internal Sprite VehiclePartImage;

    [SerializeField] private Material[] _defaultMaterials;
    [SerializeField] private MeshRenderer[] _partRenderers;

    internal void Awake()
    {
        _name = "Vehicle part";
        _durability = 999999;
    }

    internal Material PartMaterial
    {
        set
        {
            foreach (MeshRenderer meshRenderer in _partRenderers)
            {
                meshRenderer.material = value;
            }
        }
    }

    internal void SetDefaultMaterials()
    {
        for (int i = 0; i < _partRenderers.Length; i++)
        {
            _partRenderers[i].material = _defaultMaterials[i];
        }
    }

    internal string Info
    {
        get
        {
            return "<b>" + _name + "</b>" +
                   "\nDurability: " + _durability;
        }
    }
}
