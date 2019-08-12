using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTypeOfVehiclePart : MonoBehaviour
{
    [SerializeField] private List<GameObject> _partWindows;

    public void OpenWindowOfParts(Transform index)
    {
        for (int i = 0; i < _partWindows.Count; i++)
        {
            if (i == index.GetSiblingIndex())
            {
                _partWindows[i].SetActive(true);
            }
            else
            {
                _partWindows[i].SetActive(false);
            }
        }
    }
}
