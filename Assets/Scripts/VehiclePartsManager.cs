using System.Collections.Generic;
using UnityEngine;

public class VehiclePartsManager : MonoBehaviour
{
    internal List<GameObject> AllAxes;
    internal List<GameObject> AllCabins;
    internal List<GameObject> AllCargoes;

    [SerializeField] private List<GameObject> _militarySmallAxes;
    [SerializeField] private List<GameObject> _militaryMediumAxes;
    [SerializeField] private List<GameObject> _militaryBigAxes;

    [SerializeField] private List<GameObject> _militarySmallCabins;
    [SerializeField] private List<GameObject> _militaryMediumCabins;
    [SerializeField] private List<GameObject> _militaryBigCabins;

    [SerializeField] private List<GameObject> _militarySmallCargoes;
    [SerializeField] private List<GameObject> _militaryMediumCargoes;
    [SerializeField] private List<GameObject> _militaryBigCargoes;

    private void Start()
    {
        AllAxes.AddRange(_militarySmallAxes);
        AllAxes.AddRange(_militaryMediumAxes);
        AllAxes.AddRange(_militaryBigAxes);

        AllCabins.AddRange(_militarySmallCabins);
        AllCabins.AddRange(_militaryMediumCabins);
        AllCabins.AddRange(_militaryBigCabins);

        AllCargoes.AddRange(_militarySmallCargoes);
        AllCargoes.AddRange(_militaryMediumCargoes);
        AllCargoes.AddRange(_militaryBigCargoes);
    }
}
