using System.Collections.Generic;
using UnityEngine;

public class VehiclePartsManager : MonoBehaviour
{
    internal static VehiclePartsManager Instance;
    
    internal List<GameObject> AllAxes = new List<GameObject>();
    internal List<GameObject> AllCabins = new List<GameObject>();
    internal List<GameObject> AllCargoes = new List<GameObject>();

    [SerializeField] private List<GameObject> _militarySmallAxes;
    [SerializeField] private List<GameObject> _militaryMediumAxes;
    [SerializeField] private List<GameObject> _militaryBigAxes;

    [SerializeField] private List<GameObject> _militarySmallCabins;
    [SerializeField] private List<GameObject> _militaryMediumCabins;
    [SerializeField] private List<GameObject> _militaryBigCabins;

    [SerializeField] private List<GameObject> _militarySmallCargoes;
    [SerializeField] private List<GameObject> _militaryMediumCargoes;
    [SerializeField] private List<GameObject> _militaryBigCargoes;

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

    private void Start()
    {
        AddVehiclePartsToLists();
    }

    private void AddVehiclePartsToLists()
    {
        AddNotNullItemsInFirstListToSecondList(_militarySmallAxes, ref AllAxes);
        AddNotNullItemsInFirstListToSecondList(_militaryMediumAxes, ref AllAxes);
        AddNotNullItemsInFirstListToSecondList(_militaryBigAxes, ref AllAxes);

        AddNotNullItemsInFirstListToSecondList(_militarySmallCabins, ref AllCabins);
        AddNotNullItemsInFirstListToSecondList(_militaryMediumCabins, ref AllCabins);
        AddNotNullItemsInFirstListToSecondList(_militaryBigCabins, ref AllCabins);

        //AddNotNullItemsInFirstListToSecondList(_militarySmallCargoes, ref AllCargoes);
        AddNotNullItemsInFirstListToSecondList(_militaryMediumCargoes, ref AllCargoes);
        AddNotNullItemsInFirstListToSecondList(_militaryBigCargoes, ref AllCargoes);
    }

    private void ClearAllVehiclePartsLists()
    {
        AllAxes.Clear();
        AllCabins.Clear();
        AllCargoes.Clear();

        AllAxes = new List<GameObject>();
        AllCabins = new List<GameObject>();
        AllCargoes = new List<GameObject>();
    }

    /// <summary>
    /// index(0) => Get AllAxis; index(1) => Get AllCabins; index(2) => Get AllCargoes.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    internal List<GameObject> VehicleParts(int index)
    {
        ClearAllVehiclePartsLists();
        AddVehiclePartsToLists();

        switch (index)
        {
            case 0:
                return AllAxes;
            case 1:
                return AllCabins;
            case 2:
                return AllCargoes;
            default:
                return null;
        }
    }

    private void AddNotNullItemsInFirstListToSecondList(List<GameObject> listToAdd, ref List<GameObject> originalList)
    {
        foreach (GameObject vehiclePart in listToAdd)
        {
            if (vehiclePart)
            {
                originalList.Add(vehiclePart);
            }
        }
    }
}
