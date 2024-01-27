using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{   
    public static UIManager instance;     // Singleton deseni ile bu s�n�f�n tek �rne�i

    // CanvasGroup elemanlar�, farkl� ekranlar� temsil eder
    [Header(" Elements ")]
    [SerializeField] private CanvasGroup menuCG;
    [SerializeField] private CanvasGroup gameCG;
    [SerializeField] private CanvasGroup levelCompletedCG;
    [SerializeField] private CanvasGroup gameoverCG;
    [SerializeField] private CanvasGroup settingsCG;

    // Men� elemanlar�
    [Header(" Menu Elements ")]
    [SerializeField] private TextMeshProUGUI menuCoins;
    [SerializeField] private TextMeshProUGUI menuBestScore;


    // Seviye tamamland� elemanlar�
    [Header(" Level Complate Elements ")]
    [SerializeField] private TextMeshProUGUI levelCompleteCoins;
    [SerializeField] private TextMeshProUGUI levelCompleteSecretWord;
    [SerializeField] private TextMeshProUGUI levelCompleteScore;
    [SerializeField] private TextMeshProUGUI levelCompleteBestScore;

    // Oyun bitti elemanlar�
    [Header(" Game Over Elements ")]
    [SerializeField] private TextMeshProUGUI gameoverCoins;
    [SerializeField] private TextMeshProUGUI gameoverSecretWord;
    [SerializeField] private TextMeshProUGUI gameoverBestScore;

    // Oyun elemanlar�
    [Header("Game Elements")]
    [SerializeField] private TextMeshProUGUI gameScore;
    [SerializeField] private TextMeshProUGUI gameCoins;
   

    public void Awake()
    {
       if (instance == null) // E�er instance hen�z atanmad�ysa, bu �rne�i ata
        {
            instance = this;
        }
       else
            // Zaten bir instance varsa, bu �rne�i yok et
            Destroy(gameObject);
    }
    
    void Start()
    {
        // UI elemanlar�n� gizle ve ba�lang�� durumuna getir
        ShowMenu();
        HideGame();
        HideLevelComplete();
        HideGameover();

        GameManager.onGameStateChanged += GameStateChangedCallback; // Oyun durumu de�i�ikliklerini takip eden delegenin metodu olarak GameStateChangedCallback metodu ekleniyor
        DataManager.onCoinsUpdate += UpdateCoinsTexts; // Coins g�ncellendi�inde tetiklenecek olan metotu belirten delegenin metodu olarak UpdateCoinsTexts metodu ekleniyor
    }

    private void OnDestroy() // Bu script yok edildi�inde delegenin metodu olarak GameStateChangedCallback ve UpdateCoinsTexts metotlar� ��kar�l�yor
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        DataManager.onCoinsUpdate -= UpdateCoinsTexts;

    }

    private void GameStateChangedCallback (GameState gameState)      // Oyun durumu de�i�ti�inde �a�r�lan metot
    {
        switch (gameState)
        {
            case GameState.Menu:
                ShowMenu();
                HideGame();
                break;

            case GameState.Game:

                ShowGame();
                HideMenu();
                HideLevelComplete();
                HideGameover();
                break;


            case GameState.LevelComplate:
                ShowLevelComplete();
                HideGame();
                break;


            case GameState.Gameover:
                ShowGameover();    
                HideGame();
                break;

              
        }
    }

    public void UpdateCoinsTexts() // Coins g�ncellendi�inde �a�r�lan metot
    {
        menuCoins.text = DataManager.instance.GetCoins().ToString();
        gameCoins.text = menuCoins.text;
        levelCompleteCoins.text = menuCoins.text;
        gameoverCoins.text = menuCoins.text;
    }

    private void ShowMenu () // Menu elemanlar�n� g�steren metot
    {
        // Men�deki coin ve en iyi skor bilgilerini g�ncelle
        menuCoins.text = DataManager.instance.GetCoins().ToString();
        menuBestScore.text = DataManager.instance.GetBestScore().ToString();

        ShowCG(menuCG);  // Men�y� g�ster
    }

    private void HideMenu() // Menu elemanlar�n� gizleyen metot
    {
        HideCG(menuCG); // Men�y� gizle
    }


    private void ShowGame()  // Oyun elemanlar�n� g�steren metot
    {
        // Oyundaki coin ve skor bilgilerini g�ncelle
        gameCoins.text = DataManager.instance.GetCoins().ToString();
        gameScore.text = DataManager.instance.GetScore().ToString();


        ShowCG(gameCG); // Oyunu g�ster
    }

    private void HideGame() // Oyun elemanlar�n� gizleyen metot
    {
        HideCG(gameCG);
    }

    private void ShowLevelComplete() // Seviye tamamland� elemanlar�n� g�steren metot
    {
        // Seviye tamamland� ekran�ndaki coin, gizli kelime, skor ve en iyi skor bilgilerini g�ncelle
        levelCompleteCoins.text = DataManager.instance.GetCoins().ToString();
        levelCompleteSecretWord.text = WorldManager.instance.GetSecretWord();
        levelCompleteScore.text = DataManager.instance.GetScore().ToString();   
        levelCompleteBestScore.text = DataManager.instance.GetBestScore().ToString();
       


        ShowCG(levelCompletedCG);
    }
    private void HideGameover()
    {
        HideCG(gameoverCG);
    }

    public void ShowSettings()
    {
        ShowCG(settingsCG);
    }

    public void HideSettings()
    {
        HideCG(settingsCG);
    }

    private void HideLevelComplete()
    {
        HideCG(levelCompletedCG);
    }

    private void ShowGameover() // Oyun bitti elemanlar�n� g�steren metot
    {
        // Oyun bitti ekran�ndaki coin, gizli kelime, skor ve en iyi skor bilgilerini g�ncelle
        gameoverCoins.text = DataManager.instance.GetCoins().ToString();
        gameoverSecretWord.text= WorldManager.instance.GetSecretWord();
        gameoverBestScore.text = DataManager.instance.GetBestScore().ToString();

        ShowCG(gameoverCG);
    }

    private void ShowCG(CanvasGroup cg)  // CanvasGroup g�sterme metodu
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    private void HideCG(CanvasGroup cg) // CanvasGroup gizleme metodu
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
