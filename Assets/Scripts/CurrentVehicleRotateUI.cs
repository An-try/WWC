using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentVehicleRotateUI : MonoBehaviour
{
    private void Update()
    {
        transform.RotateAround(transform.position, transform.up, 20f * Time.deltaTime);
    }
}
