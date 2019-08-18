using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTypeOfVehiclePart : MonoBehaviour
{
    [SerializeField] private List<GameObject> _partWindows;
    [SerializeField] private GameObject _vehiclePartUiPrefab;

    private List<GameObject> _vehicleParts = new List<GameObject>();

    private VehiclePartsManager _vehiclePartsManager;

    private void Start()
    {
        _vehiclePartsManager = VehiclePartsManager.Instance;
    }

    public void OpenWindowOfParts(Transform buttonThatActivateWindow)
    {
        for (int i = 0; i < _partWindows.Count; i++)
        {
            if (i == buttonThatActivateWindow.GetSiblingIndex())
            {
                _partWindows[i].SetActive(!_partWindows[i].activeSelf);
            }
            else
            {
                _partWindows[i].SetActive(false);
            }
        }
        UpdateVehicleParts(_partWindows[buttonThatActivateWindow.GetSiblingIndex()].transform, buttonThatActivateWindow.GetSiblingIndex());
    }

    private void UpdateVehicleParts(Transform windowToOpen, int indexOfParts)
    {
        Transform vehiclePartsListUI = windowToOpen.GetComponent<VehiclePartsScrollViewUI>().VehiclePartsListUI;
        DestroyAllPartsUI(vehiclePartsListUI);
        InstantiateAllPartsUI(vehiclePartsListUI, indexOfParts);
    }

    private void DestroyAllPartsUI(Transform vehiclePartsListUI)
    {
        for (int i = vehiclePartsListUI.childCount - 1; i >= 0; i--)
        {
            Destroy(vehiclePartsListUI.GetChild(i).gameObject);
        }
    }

    private void InstantiateAllPartsUI(Transform vehiclePartsListUI, int indexOfParts)
    {
        _vehicleParts = _vehiclePartsManager.VehicleParts(indexOfParts);

        float newContentSizeY = _vehicleParts.Count * (_vehiclePartUiPrefab.GetComponent<RectTransform>().rect.height + vehiclePartsListUI.GetComponent<VerticalLayoutGroup>().spacing);
        _partWindows[indexOfParts].GetComponent<ScrollRect>().content.sizeDelta = new Vector2(0, newContentSizeY);

        for (int i = 0; i < _vehicleParts.Count; i++)
        {
            GameObject vehiclePartUi = Instantiate(_vehiclePartUiPrefab, vehiclePartsListUI);

            _vehicleParts[i].GetComponent<VehiclePart>().Awake();
            vehiclePartUi.GetComponent<VehiclePartUI>().PartImage.sprite = _vehicleParts[i].GetComponent<VehiclePart>().VehiclePartImage;
            vehiclePartUi.GetComponent<VehiclePartUI>().PartInfo.text = _vehicleParts[i].GetComponent<VehiclePart>().Info;
        }
        //_vehicleParts.Clear();
    }
}
