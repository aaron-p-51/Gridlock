using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class PlayerUIController : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject m_IntroPage;
    [SerializeField] private GameObject m_InGamePage;
    [SerializeField] private GameObject m_GameOverPage;
    [SerializeField] private GameObject m_ConfirmMainMenuPage;
    [Header("Into Page")]
    [SerializeField] private Button m_PlayButton;
    [Header("In Game Page")]
    [SerializeField] private TMP_Text m_InGameScoreText;
    [Header("Game Over Page")]
    [SerializeField] private TMP_Text m_GameOverScoreText;
    [SerializeField] private TMP_Text m_GameOverHighScoreText;
    [SerializeField] private Button m_GameOverPlayAgainButton;
    [SerializeField] private Button m_GameOverMainMenuButton;
    [Header("Confirm Main Menu")]
    [SerializeField] private Button m_ConfirmMainMenuBackButton;
    [SerializeField] private Button m_ConfirmMainMenuMainMenuButton;


    public enum PlayerUIPage
    {
        IntroPage,
        InGamePage, 
        GameOverPage,
        ConfirmMainMenuPage
    };

    private Dictionary<PlayerUIPage, GameObject> m_UIPages = new Dictionary<PlayerUIPage, GameObject>();


    private void Awake()
    {
        BuildUIPagesDictionary();
        AddButtonClickListeners();
    }

    private void OnDestroy()
    {
        RemoveButtonClickListeners();
    }

    private void BuildUIPagesDictionary()
    {
        m_UIPages.Add(PlayerUIPage.IntroPage, m_IntroPage);
        m_UIPages.Add(PlayerUIPage.InGamePage, m_InGamePage);
        m_UIPages.Add(PlayerUIPage.GameOverPage, m_GameOverPage);
        m_UIPages.Add(PlayerUIPage.ConfirmMainMenuPage, m_ConfirmMainMenuPage);
    }

    private void AddButtonClickListeners()
    {
        m_PlayButton.onClick.AddListener(HandeOnPlayButtonClicked);
        m_GameOverPlayAgainButton.onClick.AddListener(HandleOnGameOverPlayAgainButtonClicked);
        m_GameOverMainMenuButton.onClick.AddListener(HandleOnGameOverMainMenuButtonClicked);
        m_ConfirmMainMenuBackButton.onClick.AddListener(HandleOnConfirmMainMenuBackButtonClicked);
        m_ConfirmMainMenuMainMenuButton.onClick.AddListener(HandleConfirmMainMenuMainMenuButtonClicked);
    }

    private void RemoveButtonClickListeners()
    {
        m_PlayButton.onClick.RemoveListener(HandeOnPlayButtonClicked);
        m_GameOverPlayAgainButton.onClick.RemoveListener(HandleOnGameOverPlayAgainButtonClicked);
        m_GameOverMainMenuButton.onClick.RemoveListener(HandleOnGameOverMainMenuButtonClicked);
        m_ConfirmMainMenuBackButton.onClick.RemoveListener(HandleOnConfirmMainMenuBackButtonClicked);
        m_ConfirmMainMenuMainMenuButton.onClick.RemoveListener(HandleConfirmMainMenuMainMenuButtonClicked);
    }

    public void UdpateInGameScoreText(int score)
    {
        m_InGameScoreText.text = score.ToString();   
    }

    public void SetUIPageActive(PlayerUIPage page)
    {
        foreach (KeyValuePair<PlayerUIPage, GameObject> kvp in m_UIPages)
        {
            kvp.Value.SetActive(page == kvp.Key);
        }
    }

    private void HandeOnPlayButtonClicked()
    {

    }

    private void HandleOnGameOverPlayAgainButtonClicked()
    {

    }

    private void HandleOnGameOverMainMenuButtonClicked()
    {
        SetUIPageActive(PlayerUIPage.ConfirmMainMenuPage);
    }

    private void HandleOnConfirmMainMenuBackButtonClicked()
    {
        SetUIPageActive(PlayerUIPage.GameOverPage);
    }

    private void HandleConfirmMainMenuMainMenuButtonClicked()
    {

    }


}
