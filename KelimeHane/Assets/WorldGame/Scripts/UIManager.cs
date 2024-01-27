using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{   
    public static UIManager instance;     // Singleton deseni ile bu sýnýfýn tek örneði

    // CanvasGroup elemanlarý, farklý ekranlarý temsil eder
    [Header(" Elements ")]
    [SerializeField] private CanvasGroup menuCG;
    [SerializeField] private CanvasGroup gameCG;
    [SerializeField] private CanvasGroup levelCompletedCG;
    [SerializeField] private CanvasGroup gameoverCG;
    [SerializeField] private CanvasGroup settingsCG;

    // Menü elemanlarý
    [Header(" Menu Elements ")]
    [SerializeField] private TextMeshProUGUI menuCoins;
    [SerializeField] private TextMeshProUGUI menuBestScore;


    // Seviye tamamlandý elemanlarý
    [Header(" Level Complate Elements ")]
    [SerializeField] private TextMeshProUGUI levelCompleteCoins;
    [SerializeField] private TextMeshProUGUI levelCompleteSecretWord;
    [SerializeField] private TextMeshProUGUI levelCompleteScore;
    [SerializeField] private TextMeshProUGUI levelCompleteBestScore;

    // Oyun bitti elemanlarý
    [Header(" Game Over Elements ")]
    [SerializeField] private TextMeshProUGUI gameoverCoins;
    [SerializeField] private TextMeshProUGUI gameoverSecretWord;
    [SerializeField] private TextMeshProUGUI gameoverBestScore;

    // Oyun elemanlarý
    [Header("Game Elements")]
    [SerializeField] private TextMeshProUGUI gameScore;
    [SerializeField] private TextMeshProUGUI gameCoins;
   

    public void Awake()
    {
       if (instance == null) // Eðer instance henüz atanmadýysa, bu örneði ata
        {
            instance = this;
        }
       else
            // Zaten bir instance varsa, bu örneði yok et
            Destroy(gameObject);
    }
    
    void Start()
    {
        // UI elemanlarýný gizle ve baþlangýç durumuna getir
        ShowMenu();
        HideGame();
        HideLevelComplete();
        HideGameover();

        GameManager.onGameStateChanged += GameStateChangedCallback; // Oyun durumu deðiþikliklerini takip eden delegenin metodu olarak GameStateChangedCallback metodu ekleniyor
        DataManager.onCoinsUpdate += UpdateCoinsTexts; // Coins güncellendiðinde tetiklenecek olan metotu belirten delegenin metodu olarak UpdateCoinsTexts metodu ekleniyor
    }

    private void OnDestroy() // Bu script yok edildiðinde delegenin metodu olarak GameStateChangedCallback ve UpdateCoinsTexts metotlarý çýkarýlýyor
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        DataManager.onCoinsUpdate -= UpdateCoinsTexts;

    }

    private void GameStateChangedCallback (GameState gameState)      // Oyun durumu deðiþtiðinde çaðrýlan metot
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

    public void UpdateCoinsTexts() // Coins güncellendiðinde çaðrýlan metot
    {
        menuCoins.text = DataManager.instance.GetCoins().ToString();
        gameCoins.text = menuCoins.text;
        levelCompleteCoins.text = menuCoins.text;
        gameoverCoins.text = menuCoins.text;
    }

    private void ShowMenu () // Menu elemanlarýný gösteren metot
    {
        // Menüdeki coin ve en iyi skor bilgilerini güncelle
        menuCoins.text = DataManager.instance.GetCoins().ToString();
        menuBestScore.text = DataManager.instance.GetBestScore().ToString();

        ShowCG(menuCG);  // Menüyü göster
    }

    private void HideMenu() // Menu elemanlarýný gizleyen metot
    {
        HideCG(menuCG); // Menüyü gizle
    }


    private void ShowGame()  // Oyun elemanlarýný gösteren metot
    {
        // Oyundaki coin ve skor bilgilerini güncelle
        gameCoins.text = DataManager.instance.GetCoins().ToString();
        gameScore.text = DataManager.instance.GetScore().ToString();


        ShowCG(gameCG); // Oyunu göster
    }

    private void HideGame() // Oyun elemanlarýný gizleyen metot
    {
        HideCG(gameCG);
    }

    private void ShowLevelComplete() // Seviye tamamlandý elemanlarýný gösteren metot
    {
        // Seviye tamamlandý ekranýndaki coin, gizli kelime, skor ve en iyi skor bilgilerini güncelle
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

    private void ShowGameover() // Oyun bitti elemanlarýný gösteren metot
    {
        // Oyun bitti ekranýndaki coin, gizli kelime, skor ve en iyi skor bilgilerini güncelle
        gameoverCoins.text = DataManager.instance.GetCoins().ToString();
        gameoverSecretWord.text= WorldManager.instance.GetSecretWord();
        gameoverBestScore.text = DataManager.instance.GetBestScore().ToString();

        ShowCG(gameoverCG);
    }

    private void ShowCG(CanvasGroup cg)  // CanvasGroup gösterme metodu
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
