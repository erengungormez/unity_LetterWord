using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    public static WorldManager instance;     // Singleton deseni ile bu s�n�f�n tek �rne�i

    [Header(" Elements ")]
    [SerializeField] private string secretWord; // Gizli kelimenin depoland��� de�i�ken
    [SerializeField] private TextAsset wordsText; // Kelime listesini i�eren metin dosyas�
    private string words;

    [Header("Settings")]
    private bool shouldReset; // Oyun durumlar�na g�re gizli kelimenin s�f�rlanmas� gerekip gerekmedi�ini belirten flag

    private void Awake()
    {
        if (instance == null) // E�er instance hen�z atanmam��sa, bu instance'� ata; aksi takdirde bu nesneyi yok et
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        words = wordsText.text; // Metin dosyas�ndaki kelimeleri string'e �evir ve 'words' de�i�kenine ata
    }

    void Start()
    {
        SetNewSecretWord(); // �lk gizli kelimeyi ayarla

        GameManager.onGameStateChanged += GameStateChangedCallback; // GameManager'daki oyun durumu de�i�iklikleri takip eden bir delegenin metodu olarak GameStateChangedCallback metodu ekleniyor
    }

    private void OnDestroy() // Bu script yok edildi�inde delegenin metodu olarak GameStateChangedCallback metodu ��kar�l�yor
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState) // Oyun durumu de�i�ti�inde �a�r�lan metot
    {
        switch (gameState)
        {
            case GameState.Menu:
                // Oyun men� durumundayken yap�lacak i�lemler

                break;
            case GameState.Game: // Oyun durumundayken yap�lacak i�lemler

                if (shouldReset) // E�er shouldReset flag'i true ise yeni gizli kelimeyi ayarla
                {
                    SetNewSecretWord();
                }

                break;
            case GameState.LevelComplate: // Seviye tamamland���nda yap�lacak i�lemler
                shouldReset = true;
                break;

            case GameState.Gameover: // Oyun bitti�inde yap�lacak i�lemler
                shouldReset = true; // shouldReset flag'ini true yap, yeni seviyede gizli kelimeyi s�f�rla

                break;
           
        }
    }

    void Update()
    {
        
    }
    public string GetSecretWord() // D��ar�dan gizli kelimeye eri�im i�in get metodunu sa�layan metot
    {
        return secretWord.ToUpper(); // Gizli kelimeyi b�y�k harflerle d�nd�r
    }

    private void SetNewSecretWord() // Yeni gizli kelimeyi ayarlayan metot
    {
        Debug.Log("String length : " + words.Length);

        int wordCount = (words.Length + 2) / 7; // Kelime say�s�n� hesapla

        int wordIndex = Random.Range(0, wordCount); // Rastgele bir kelime se�

        int wordStartIndex = wordIndex * 7; // Se�ilen kelimenin ba�lang�� indeksini hesapla

        secretWord = words.Substring(wordStartIndex,5).ToUpper(); // Kelimeyi al ve gizli kelime olarak ayarla

        shouldReset = false; // shouldReset flag'ini false yap, ��nk� yeni gizli kelime ayarland�
    }
}
