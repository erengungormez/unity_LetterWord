using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    public static WorldManager instance;     // Singleton deseni ile bu sýnýfýn tek örneði

    [Header(" Elements ")]
    [SerializeField] private string secretWord; // Gizli kelimenin depolandýðý deðiþken
    [SerializeField] private TextAsset wordsText; // Kelime listesini içeren metin dosyasý
    private string words;

    [Header("Settings")]
    private bool shouldReset; // Oyun durumlarýna göre gizli kelimenin sýfýrlanmasý gerekip gerekmediðini belirten flag

    private void Awake()
    {
        if (instance == null) // Eðer instance henüz atanmamýþsa, bu instance'ý ata; aksi takdirde bu nesneyi yok et
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        words = wordsText.text; // Metin dosyasýndaki kelimeleri string'e çevir ve 'words' deðiþkenine ata
    }

    void Start()
    {
        SetNewSecretWord(); // Ýlk gizli kelimeyi ayarla

        GameManager.onGameStateChanged += GameStateChangedCallback; // GameManager'daki oyun durumu deðiþiklikleri takip eden bir delegenin metodu olarak GameStateChangedCallback metodu ekleniyor
    }

    private void OnDestroy() // Bu script yok edildiðinde delegenin metodu olarak GameStateChangedCallback metodu çýkarýlýyor
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState) // Oyun durumu deðiþtiðinde çaðrýlan metot
    {
        switch (gameState)
        {
            case GameState.Menu:
                // Oyun menü durumundayken yapýlacak iþlemler

                break;
            case GameState.Game: // Oyun durumundayken yapýlacak iþlemler

                if (shouldReset) // Eðer shouldReset flag'i true ise yeni gizli kelimeyi ayarla
                {
                    SetNewSecretWord();
                }

                break;
            case GameState.LevelComplate: // Seviye tamamlandýðýnda yapýlacak iþlemler
                shouldReset = true;
                break;

            case GameState.Gameover: // Oyun bittiðinde yapýlacak iþlemler
                shouldReset = true; // shouldReset flag'ini true yap, yeni seviyede gizli kelimeyi sýfýrla

                break;
           
        }
    }

    void Update()
    {
        
    }
    public string GetSecretWord() // Dýþarýdan gizli kelimeye eriþim için get metodunu saðlayan metot
    {
        return secretWord.ToUpper(); // Gizli kelimeyi büyük harflerle döndür
    }

    private void SetNewSecretWord() // Yeni gizli kelimeyi ayarlayan metot
    {
        Debug.Log("String length : " + words.Length);

        int wordCount = (words.Length + 2) / 7; // Kelime sayýsýný hesapla

        int wordIndex = Random.Range(0, wordCount); // Rastgele bir kelime seç

        int wordStartIndex = wordIndex * 7; // Seçilen kelimenin baþlangýç indeksini hesapla

        secretWord = words.Substring(wordStartIndex,5).ToUpper(); // Kelimeyi al ve gizli kelime olarak ayarla

        shouldReset = false; // shouldReset flag'ini false yap, çünkü yeni gizli kelime ayarlandý
    }
}
