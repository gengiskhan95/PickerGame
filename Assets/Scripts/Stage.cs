using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    #region Drone Settings

    [Header("Drone Settings")]

    [SerializeField] GameObject Drone;
    public Transform droneWayPointContainer;
    [SerializeField] public bool isHaveDrone;

    #endregion

    #region Collectable Objects & Settings

    [Header("Collectible Object & Settings")]

    [SerializeField] public Transform CollactableContainer;
    [SerializeField] public Transform spawnPointsContainer;
    [SerializeField] List<GameObject> CollectableObjects = new List<GameObject>();

    GameObject tempCollactableObj;

    #endregion

    #region Fractuable Settings

    [Header("Fractuable Collectables")]

    public bool isHaveFractuableCollactables;

    [SerializeField] List<GameObject> FractuableCollactableList = new List<GameObject>();

    #endregion

    #region Ground & Doors

    [Header("Doors")]

    [SerializeField] List<GameObject> Doors;

    #endregion

    public GameObject targetCollactableObject;

    private void Awake()
    {
        AwakeMethods();
    }

    #region Awake Methods

    void AwakeMethods()
    {
        PickRandomCollectableType();
        SpawnCollectables();
    }

    void PickRandomCollectableType()
    {
        if (isHaveFractuableCollactables)
        {
            if (FractuableCollactableList.Count > 0)
            {
                targetCollactableObject = FractuableCollactableList[Random.Range(0, FractuableCollactableList.Count)];
            }

            else
            {
                Debug.LogWarning(name + " 's FractuableCollactableObjects is Empty");
            }
        }

        else
        {
            if (CollectableObjects.Count > 0)
            {
                targetCollactableObject = CollectableObjects[Random.Range(0, CollectableObjects.Count)];
            }

            else
            {
                Debug.LogWarning(name + " 's CollactableObjectsVariants is Empty");
            }
        }
    }

    void SpawnCollectables()
    {
        if (!isHaveDrone)
        {
            if (spawnPointsContainer.childCount > 0)
            {
                for (int i = 0; i < spawnPointsContainer.childCount; i++)
                {
                    tempCollactableObj = Instantiate(targetCollactableObject, spawnPointsContainer.GetChild(i).position, Quaternion.identity, CollactableContainer);
                    tempCollactableObj.name = "Collactable Object" + i.ToString();
                }
            }

            else
            {
                Debug.LogWarning(name + " Spawn Point Container Empty");
            }
        }

        else
        {
            Drone.gameObject.SetActive(true);
        }

    }

    #endregion

    public void OpenTheDoors()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Doors[i].gameObject.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        }
    }
}
