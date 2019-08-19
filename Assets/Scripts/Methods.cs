using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class Methods : MonoBehaviour
{
    //var watch = System.Diagnostics.Stopwatch.StartNew();
    //// The code that you want to measure comes here
    //watch.Stop();
    //var elapsedMs = watch.ElapsedMilliseconds;

    public enum SearchParameter { Distance, Durability, Armor, Speed }

    private static List<GameObject> _gameObjectsList = new List<GameObject>();

    /// <summary>
    /// Search nearest root game object by spesial parameter to originalTransform in the current scene,
    /// tagged with any tag in incoming list of targetTags. You can search for the object by a special parameter,
    /// if you want, or search only by distance.
    /// </summary>
    /// <param name="originalTransform"></param>
    /// <param name="targetTags"></param>
    /// <returns></returns>
    public static GameObject SearchOpportuneTargetByParameter(Transform originalTransform, List<string> targetTags, SearchParameter searchParameter)
    {
        _gameObjectsList.Clear();
        _gameObjectsList = new List<GameObject>();

        float parameterAmountInTarget = Mathf.Infinity; // Set distance to nearest target as infinity
        GameObject opportuneTarget = null; // Set nearest target as null game object

        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (string tag in targetTags)
            {
                if (root.CompareTag(tag))
                {
                    _gameObjectsList.Add(root);
                    break;
                }
            }
        }

        switch (searchParameter)
        {
            case SearchParameter.Distance:
                for (int i = 0; i < _gameObjectsList.Count; i++) // Pass all targets in targets list
                {
                    SetOpportuneGameObjectByParameter(i, Vector3.Distance(originalTransform.position, _gameObjectsList[i].transform.position), parameterAmountInTarget, ref opportuneTarget);
                }
                break;
            case SearchParameter.Durability:
                for (int i = 0; i < _gameObjectsList.Count; i++) // Pass all targets in targets list
                {
                    SetOpportuneGameObjectByParameter(i, _gameObjectsList[i].GetComponent<Vehicle>().Durability, parameterAmountInTarget, ref opportuneTarget);
                }
                break;
            case SearchParameter.Armor:
                for (int i = 0; i < _gameObjectsList.Count; i++) // Pass all targets in targets list
                {
                    SetOpportuneGameObjectByParameter(i, _gameObjectsList[i].GetComponent<Vehicle>().Armor, parameterAmountInTarget, ref opportuneTarget);
                }
                break;
            case SearchParameter.Speed:
                for (int i = 0; i < _gameObjectsList.Count; i++) // Pass all targets in targets list
                {
                    SetOpportuneGameObjectByParameter(i, _gameObjectsList[i].GetComponent<CarController>().MaxSpeed, parameterAmountInTarget, ref opportuneTarget);
                }
                break;
            default:
                Debug.LogError("Wrong search parameter. Target will be null.");
                break;
        }
        // Set the nearest target if it was found. Otherwise, the nearest target will be null
        return opportuneTarget;
    }

    private static void SetOpportuneGameObjectByParameter(int i, float parameterToCompare, float parameterInTarget, ref GameObject opportuneTarget)
    {
        if (parameterToCompare < parameterInTarget) // If this target is closer than previous nearest target
        {
            parameterInTarget = parameterToCompare; // Set new distance to the nearest target
            opportuneTarget = _gameObjectsList[i].gameObject; // Set new nearest target
        }
    }
}
