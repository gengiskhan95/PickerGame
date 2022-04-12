using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Level States

    [Header("Level States")]

    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    #endregion

    #region Movement Settings

    [Header("Movement Settings")]

    float mouseFirstPosX;
    float sideSpeedDivider;
    Vector3 firstPosition;
    Vector3 targetPosition;
    public bool pauseMovement;

    [SerializeField] Rigidbody RB;
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float sideSpeed;
    [SerializeField] float screenDivideCount;
    [SerializeField] float SideMinZ;
    [SerializeField] float SideMaxZ;

    #endregion

    #region Tags

    [Header("Tags")]
    
    string TagCounterTrigger;
    string TagCollectableObjects;
    string TagLevelFinishTrigger;

    #endregion

    [SerializeField] [Space(15)] public List<GameObject> collectedObjects = new List<GameObject>();

    GameController GC;
    UIController UI;

    public static PlayerController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartMethods();
    }

    #region Start Methods

    void StartMethods()
    {
        GetGC();
        GetUI();
        GetTags();
        SetSideSpeed();
    }

    void GetGC()
    {
        GC = GameController.instance;
    }

    void GetUI()
    {
        UI = UIController.instance;
    }

    void GetTags()
    {
        TagCounterTrigger = GC.TagCounterTrigger;
        TagLevelFinishTrigger = GC.TagLevelFinishTrigger;
        TagCollectableObjects = GC.TagCollectableObjects;
    }

    void SetSideSpeed()
    {
        sideSpeedDivider = Screen.width / screenDivideCount;
    }

    #endregion

    void Update()
    {
        if (isLevelStart && !isLevelFail && !isLevelDone)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseFirstPosX = Input.mousePosition.x;
                firstPosition = RB.position;
            }

            else if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x != mouseFirstPosX)
                {
                    targetPosition.z = firstPosition.z + (Input.mousePosition.x - mouseFirstPosX) / sideSpeedDivider * (-1);
                    targetPosition.z = targetPosition.z > SideMaxZ ? SideMaxZ : targetPosition.z;
                    targetPosition.z = targetPosition.z < SideMinZ ? SideMinZ : targetPosition.z;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isLevelStart && !isLevelFail && !isLevelDone)
        {
            targetPosition.y = RB.position.y;
            targetPosition.x = RB.position.x;
            targetPosition.x = !pauseMovement ? targetPosition.x + speed * Time.fixedDeltaTime : targetPosition.x;
            RB.MovePosition(targetPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagCounterTrigger))
        {
            pauseMovement = true;
            other.gameObject.transform.parent.GetComponent<Collector>().DelayedCheck();
            other.gameObject.SetActive(false);
        }

        else if (other.CompareTag(TagLevelFinishTrigger))
        {
            if(speed == maxSpeed)
            {
                other.transform.tag = "Untagged";
                GC.FinalStageActions();
                GC.StageDone();
                GC.LevelDone();
                other.transform.parent.GetComponent<Stage>().OpenTheDoors();
            }
            else
            {
                speed += 0.1f;
                other.transform.tag = "Untagged";
                GC.FinalStageActions();
                GC.StageDone();
                GC.LevelDone();
                other.transform.parent.GetComponent<Stage>().OpenTheDoors();
            }
        }

        else if (other.CompareTag(TagCollectableObjects))
        {
            if (!collectedObjects.Contains(other.gameObject))
            {
                collectedObjects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagCollectableObjects))
        {
            if (collectedObjects.Contains(other.gameObject))
            {
                collectedObjects.Remove(other.gameObject);
            }
        }
    }
}
