using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Level States

    [Header("Level States")]

    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    #endregion

    #region Tap to Start Panel

    [Header("Tap To Start Panel")]

    [SerializeField] GameObject PanelTapToStart;
    [SerializeField] Button ButtonTapToStart;

    #endregion

    #region In Game Panel

    [Header ("In Game Panel")]

    [SerializeField] GameObject PanelInGame;
    [SerializeField] TextMeshProUGUI TextCurrentLevel;
    [SerializeField] TextMeshProUGUI TextNextLevel;

    #endregion

    #region End Game Panel

    [Header("End Game Panel")]

    [SerializeField] GameObject PanelEndGame;
    [SerializeField] TextMeshProUGUI TextPanelEndGameMessage;
    [SerializeField] Button ButtonEndGame;
    [SerializeField] TextMeshProUGUI TextPanelEndGameButton;

    #endregion

    #region Strings

    [Header("Strings")]

    [SerializeField] List<string> EndGameButtonWinStrings;
    [SerializeField] List<string> EndGameMessageWinStrings;
    [SerializeField] List<string> EndGameButtonFailStrings;
    [SerializeField] List<string> EndGameMessageFailStrings;

    #endregion

    public int level;

    GameController GC;

    public static UIController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartMethods();
    }

    #region Start Methods

    void StartMethods()
    {
        GetGC();
        ShowTapToStartPanel();
    }

    void GetGC()
    {
        GC = GameController.instance;
    }

    #endregion

    #region Tap To Start Panel

    public void ShowTapToStartPanel()
    {
        PanelTapToStart.SetActive(true);
        ButtonTapToStart.gameObject.SetActive(true);
    }

    public void CloseTapToStartPanel()
    {
        PanelTapToStart.SetActive(false);
    }

    public void ButtonActionTapToStart()
    {
        ButtonTapToStart.gameObject.SetActive(false);
        CloseTapToStartPanel();
        ShowInGamePanel();
        GC.TapToStartAction();
    }

    #endregion

    #region Panel In Game

    public void ShowInGamePanel()
    {
        PanelInGame.SetActive(true);
        UpdateInGamePanelTexts();
    }

    public void UpdateInGamePanelTexts()
    {
        TextCurrentLevel.text = GC.level.ToString();
        TextNextLevel.text = (GC.level + 1).ToString();
    }

    void CloseInGamePanel()
    {
        PanelInGame.SetActive(false);
    }

    #endregion

    #region Panel End Game

    public void ShowEndGamePanel()
    {
        CloseInGamePanel();
        FillEndGameTexts();
        ButtonEndGame.gameObject.SetActive(true);
        PanelEndGame.SetActive(true);
    }

    void FillEndGameTexts()
    {
        if (isLevelDone)
        {
            if (EndGameButtonWinStrings.Count > 0)
            {
                TextPanelEndGameButton.text = EndGameButtonWinStrings[Random.Range(0, EndGameButtonWinStrings.Count)];
            }

            else
            {
                Debug.LogWarning("End Game Button Win Strings Empty");
            }

            if (EndGameMessageWinStrings.Count > 0)
            {
                TextPanelEndGameMessage.text = EndGameMessageWinStrings[Random.Range(0, EndGameMessageWinStrings.Count)];
            }

            else
            {
                Debug.LogWarning("End Game Message Win Strings Empty");
            }
        }
        
        else
        {
            if (EndGameButtonFailStrings.Count > 0)
            {
                TextPanelEndGameButton.text = EndGameButtonFailStrings[Random.Range(0, EndGameButtonFailStrings.Count)];
            }

            else
            {
                Debug.LogWarning("End Game Button Fail Strings Empty");
            }

            if (EndGameMessageFailStrings.Count > 0)
            {
                TextPanelEndGameMessage.text = EndGameMessageFailStrings[Random.Range(0, EndGameMessageFailStrings.Count)];
            }

            else
            {
                Debug.LogWarning("End Game Message Fail Strings Empty");
            }
        }
    }

    public void ButtonActionEndGame()
    {
        ButtonEndGame.gameObject.SetActive(false);
        GC.EndGame();
    }

    public void CloseEndGamePanel()
    {
        isLevelDone = false;
        PanelEndGame.SetActive(false);
    }

    #endregion

}