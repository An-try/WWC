using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : Vehicle
{
    private enum PoliceLightsMode { Identic, Mirror, Snake }

    [SerializeField] private Light[] _lights;

    private PoliceLightsMode _policeLightsMode = PoliceLightsMode.Identic;

    private float _lightsChangeSpeed = 0.05f;

    private protected override void Start()
    {
        base.Start();

        foreach (Light light in _lights)
        {
            light.enabled = false;
        }

        StartCoroutine(Light());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _policeLightsMode = PoliceLightsMode.Identic;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _policeLightsMode = PoliceLightsMode.Mirror;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _policeLightsMode = PoliceLightsMode.Snake;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _lightsChangeSpeed += 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            _lightsChangeSpeed -= 0.01f;
        }
        _lightsChangeSpeed = Mathf.Clamp(_lightsChangeSpeed, 0.01f, 0.09f);
    }

    private IEnumerator Light()
    {
        while (true)
        {
            int lightIndex = 0;
            int lightMultiplier = 1;
            while (_policeLightsMode == PoliceLightsMode.Identic)
            {
                int secondIndex = _lights.Length / 2;
                _lights[lightIndex].enabled = false;
                _lights[lightIndex + secondIndex].enabled = false;

                lightIndex += lightMultiplier;
                if (lightMultiplier > 0)
                {
                    if (lightIndex + secondIndex >= _lights.Length)
                    {
                        lightMultiplier *= -1;
                        lightIndex += lightMultiplier * 2;
                    }
                }
                else if (lightMultiplier < 0)
                {
                    if (lightIndex < 0)
                    {
                        lightMultiplier *= -1;
                        lightIndex += lightMultiplier * 2;
                    }
                }

                _lights[lightIndex].enabled = true;
                _lights[lightIndex + secondIndex].enabled = true;

                yield return new WaitForSeconds(0.1f - _lightsChangeSpeed);
            }

            while (_policeLightsMode == PoliceLightsMode.Mirror)
            {
                int secondIndex = 0;
                _lights[lightIndex].enabled = false;
                switch (lightIndex)
                {
                    case 0:
                        secondIndex = 7;
                        break;
                    case 1:
                        secondIndex = 6;
                        break;
                    case 2:
                        secondIndex = 5;
                        break;
                    case 3:
                        secondIndex = 4;
                        break;
                    default:
                        Debug.Log("Error in police lighs");
                        break;
                }
                _lights[secondIndex].enabled = false;

                lightIndex += lightMultiplier;
                if (lightMultiplier > 0)
                {
                    if (lightIndex + _lights.Length / 2 >= _lights.Length)
                    {
                        lightMultiplier *= -1;
                        lightIndex += lightMultiplier * 2;
                    }
                }
                else if (lightMultiplier < 0)
                {
                    if (lightIndex < 0)
                    {
                        lightMultiplier *= -1;
                        lightIndex += lightMultiplier * 2;
                    }
                }

                _lights[lightIndex].enabled = true;
                switch (lightIndex)
                {
                    case 0:
                        secondIndex = 7;
                        break;
                    case 1:
                        secondIndex = 6;
                        break;
                    case 2:
                        secondIndex = 5;
                        break;
                    case 3:
                        secondIndex = 4;
                        break;
                    default:
                        Debug.Log("Error in police lighs");
                        break;
                }
                _lights[secondIndex].enabled = true;

                yield return new WaitForSeconds(0.1f - _lightsChangeSpeed);
            }

            while (_policeLightsMode == PoliceLightsMode.Snake)
            {
                yield return new WaitForSeconds(0.1f - _lightsChangeSpeed);
            }
        }
    }
}
