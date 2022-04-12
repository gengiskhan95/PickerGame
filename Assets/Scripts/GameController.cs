using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int level;
    public int stage;
    int leveldifference;

    #region Level States

    [Header("Level States")]

    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    #endregion

    #region Level Prefabs

    [Header("Level Prefabs")]

    GameObject tempLevel;
    public Transform LevelContainer;

    [SerializeField] float TargetLevelCount = 3;
    [SerializeField] float DefaultLevelLength;
    [SerializeField] List<GameObject> DefaultLevelList = new List<GameObject>();
    [SerializeField] Vector3 tempPosition = Vector3.zero;

    #endregion

    #region Player Prefs

    [Header("Player Prefs")] public string currentLevel;

    #endregion

    #region Tags

    [Header("Tags")]
    
    public string TagPlayer;
    public string TagGround;
    public string TagCounterTrigger;
    public string TagCollectableObjects;
    public string TagLevelFinishTrigger;

    #endregion

    List<Stage> stageList = new List<Stage>();

    CameraController CC;
    PlayerController PC;
    UIController UI;

    public static int levelFailCount;
    public static int levelWinCount;

    public static GameController instance;

    private void Awake()
    {
        AwakeMethods();
    }

    #region Awake Methods

    void AwakeMethods()
    {
        SetInstance();
        SetLevel();
        GenerateLevelStart();
    }

    void SetInstance()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void SetLevel()
    {
        level = PlayerPrefs.GetInt(currentLevel);

        if (level == 0)
        {
            level = 1;
            PlayerPrefs.SetInt(currentLevel, 1);
        }
    }

    void GenerateLevelStart()
    {
        stageList.Clear();

        if (!LevelContainer)
        {
            Debug.LogWarning("Level Container Transform is Empty");
        }

        if (DefaultLevelList.Count < 1)
        {
            Debug.LogWarning("Level Container is Empty");
        }

        else
        {
            for (int i = 0; i < TargetLevelCount; i++)
            {
                tempLevel = Instantiate(DefaultLevelList[Random.Range(0, DefaultLevelList.Count)], tempPosition, Quaternion.identity, LevelContainer);
                tempLevel.name = "Level " + level.ToString();
                tempPosition.x += DefaultLevelLength;
                stageList.Add(tempLevel.GetComponent<Stage>());
                level++;
                leveldifference++;
            }
            level -= leveldifference;
        }
    }

    void GenerateLevel()
    {
        stageList.Clear();

        if (!LevelContainer)
        {
            Debug.LogWarning("Level Container Transform is Empty");
        }

        if (DefaultLevelList.Count < 1)
        {
            Debug.LogWarning("Level Container is Empty");
        }

        else
        {
            tempLevel = Instantiate(DefaultLevelList[Random.Range(0, DefaultLevelList.Count)], tempPosition, Quaternion.identity, LevelContainer);
            tempLevel.name = "Level " + (level+leveldifference-1).ToString();
            tempPosition.x += DefaultLevelLength;
            stageList.Add(tempLevel.GetComponent<Stage>());
        }

    }
    
    #endregion

    void Start()
    {
        StartMethods();
    }

    #region Start Methods

    void StartMethods()
    {
        GetPlayer();
        GetUI();
        GetCC();
        SendLevel();
    }

    void GetPlayer()
    {
        PC = PlayerController.instance;
    }

    void GetUI()
    {
        UI = UIController.instance;
    }

    void GetCC()
    {
        CC = CameraController.instance;
    }

    void SendLevel()
    {
        UI.level = level;
    }

    #endregion

    #region Tap To Start Actions

    public void TapToStartAction()
    {
        SendGameStarted();
    }

    void SendGameStarted()
    {
        isLevelStart = true;
        UI.isLevelStart = true;
        PC.isLevelStart = true;
        CC.isLevelStart = true;
    }

    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.LogWarning("<V> Key to Win Level");
            LevelDone();
        }

        else if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.LogWarning("<F> Key to Fail Level");
            LevelFail();
        }

        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.LogWarning("<L> Key to Reset Player Prefs");
            PlayerPrefs.DeleteAll();
        }
    }

    #region End Game Actions

    public void LevelFail()
    {
        SendLevelFail();
        UI.ShowEndGamePanel();    
    }

    void SendLevelFail()
    {
        isLevelFail = true;
        PC.isLevelFail = true;
        UI.isLevelFail = true;
    }

    public void StageDone()
    {
        stage++;
    }
    public void LevelDone()
    {
        isLevelDone = true;
        stage = 0;
        SendLevelDone();
        UI.ShowEndGamePanel();
    }

    void SendLevelDone()
    {
        isLevelStart = false;
        PC.isLevelStart = false;
        UI.isLevelStart = false;
        UI.isLevelDone = true;
    }

    public void FinalStageActions()
    {
        if (LevelContainer.childCount > TargetLevelCount)
        {
            Destroy(LevelContainer.GetChild(0).gameObject);
            GenerateNextLevel();
        }
        else
        {
            GenerateNextLevel();
        }
    }

    void GenerateNextLevel()
    {
        PlayerPrefs.SetInt(currentLevel, PlayerPrefs.GetInt(currentLevel) + 1);
        level = PlayerPrefs.GetInt(currentLevel);
        GenerateLevel();
    }

    public void EndGame()
    {
        if (isLevelDone)
        {
            isLevelDone = false;
            UI.CloseEndGamePanel();
            UI.ShowInGamePanel();
            TapToStartAction();
        }

        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    #endregion

}
