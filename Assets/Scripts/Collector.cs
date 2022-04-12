using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collector : MonoBehaviour
{
    #region Delay Time

    [Header("Delay Time")]

    [SerializeField] float delayTime;

    #endregion

    #region Current Stage

    [Header("Current Stage")]

    [SerializeField] Stage currentStage;

    #endregion

    #region Count

    [Header("Count")]

    [SerializeField] TextMeshPro TextCount;

    #endregion

    #region Collider

    [Header("Vertical Collider")]

    [SerializeField] BoxCollider VerticalBoxCollider;

    #endregion

    #region Animator

    [Header("Animator")]

    [SerializeField] Animator Anim;

    #endregion

    #region Ball Count

    [Header("Ball Count")]

    [SerializeField] int ballCount;
    [SerializeField] int maxBallCount;
    [SerializeField] int targetBallCount;

    #endregion

    GameObject tempFractuable;
    Transform CollactableContainer;

    Vector3 forceVector;

    string tagCollactableObjects;

    GameController GC;

    void Start()
    {
        StartMethods();
    }

    #region StartMethods

    void StartMethods()
    {
        GetGC();
        GetTag();
        GetCollactableContainer();
        SetTargetBallCount();
        ChangeText();
    }

    void GetGC()
    {
        GC = GameController.instance;
    }

    void GetTag()
    {
        tagCollactableObjects = GC.TagCollectableObjects;
    }

    void GetCollactableContainer()
    {
        CollactableContainer = currentStage.CollactableContainer;
    }

    void SetTargetBallCount()
    {
            if (currentStage.isHaveDrone)
            {
                maxBallCount = currentStage.droneWayPointContainer.childCount - 1;
            }
            
            else
            {
                if (currentStage.isHaveFractuableCollactables)
                {
                    maxBallCount = currentStage.spawnPointsContainer.childCount * CollactableContainer.GetChild(0).childCount;
                }
                
                else
                {
                    maxBallCount = currentStage.spawnPointsContainer.childCount;
                }
            }

        targetBallCount = Mathf.RoundToInt(maxBallCount / 20) + Mathf.RoundToInt(GC.level / 20);
        targetBallCount = targetBallCount > maxBallCount ? maxBallCount - 2 : targetBallCount;
        targetBallCount = targetBallCount + GameController.levelWinCount * (maxBallCount / 20);
        targetBallCount = targetBallCount < 10 ? 10 : targetBallCount;
    }

    void ChangeText()
    {
        TextCount.text = ballCount.ToString() + " / " + targetBallCount.ToString();
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(tagCollactableObjects))
        {
            collision.collider.transform.tag = "Untagged";
            FractureObject(collision.collider.gameObject);
            ballCount++;

            if (ballCount > targetBallCount)
            {
                ChangeText();
            }

            else
            {
                ChangeText();
            }
        }
    }

    void FractureObject(GameObject fractureObject)
    {
        while(fractureObject.transform.childCount>0)
        {
            tempFractuable = fractureObject.transform.GetChild(0).gameObject;
            tempFractuable.transform.SetParent(CollactableContainer);
            tempFractuable.SetActive(true);
            forceVector = Vector3.up * Random.Range(0, 3f) + Vector3.right * Random.Range(-2, 2f) + Vector3.forward * Random.Range(-1, 1f);
            tempFractuable.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        }

        Destroy(fractureObject);

    }

    public void DelayedCheck()
    {
        Invoke("CheckCollactables", delayTime);
    }

    void CheckCollactables()
    {
        if (ballCount < targetBallCount)
        {
            GameController.levelWinCount = 0;
            GC.LevelFail();
        }
        else
        {
            GameController.levelWinCount += 1;
            TextCount.gameObject.SetActive(false);
            Anim.enabled = true;
            currentStage.OpenTheDoors();
            StartCoroutine(DelayedPass());
            Destroy(CollactableContainer.gameObject);
        }
    }

    IEnumerator DelayedPass()
    {
        yield return new WaitForSeconds(1);
        PlayerController.instance.pauseMovement = false;
        VerticalBoxCollider.enabled = false;
    }

}
