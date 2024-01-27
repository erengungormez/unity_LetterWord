using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    [Header("Elements")] // Klavye tu�lar� i�in bir dizi
    [SerializeField] private GameObject keyboard;
    private KeyboardKey[] keys;

    // Klavye ve harf ipu�lar�n�n fiyatlar�n� g�steren metin ��eleri
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI keyboardPriceText;
    [SerializeField] private TextMeshProUGUI letterPriceText;

    // Oyun durumlar�na g�re s�f�rlama kontrol� ve ipu�lar�n�n fiyatlar�
    [Header("Settings")]
    private bool shouldReset;
    [SerializeField] private int keyboardHintPrice;
    [SerializeField] private int letterHintPrice;
    


    private void Awake()
    {
        keys = keyboard.GetComponentsInChildren<KeyboardKey>(); // Klavye tu�lar� dizisini al
    }
    void Start()
    {
        // Klavye ve harf ipucu fiyatlar�n� metin ��elerine atanmas�
        keyboardPriceText.text = keyboardHintPrice.ToString();
        letterPriceText.text = letterHintPrice.ToString();

        GameManager.onGameStateChanged += GameStateChangedCallback; // Oyun durumu de�i�ikliklerini takip eden delegenin metodu olarak GameStateChangedCallback metodu ekleniyor
    }

    private void OnDestroy() // Bu script yok edildi�inde delegenin metodu olarak GameStateChangedCallback metodu ��kar�l�yor
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;

    }

    
    private void GameStateChangedCallback(GameState gameState) // Oyun durumu de�i�ti�inde �a�r�lan metot
    {
        switch (gameState) 
        {
            case GameState.Menu: // Oyun men� durumundayken yap�lacak i�lemler
                break;

            case GameState.Game: // Oyun durumundayken yap�lacak i�lemler

                if (shouldReset) // E�er shouldReset flag'i true ise, harf ipu�lar�yla ilgili bilgileri s�f�rla
                {
                    letterHintGivenIndices.Clear();
                    shouldReset = false;
                }

                break;
            case GameState.LevelComplate:  // Seviye tamamland���nda yap�lacak i�lemler
                shouldReset = true;
                break;


            case GameState.Gameover:
                shouldReset = true;
                break;
            
        }
    }

    public void KeyboardHint() // Klavye ipucu metodunu �a��ran buton taraf�ndan kullan�l�r
    {
        // Sahip olunan coin miktar�, klavye ipucu fiyat�ndan d���kse, i�lemi ger�ekle�tirme
        if (DataManager.instance.GetCoins()< keyboardHintPrice)
        {
            return;
        }

        string secretWord = WorldManager.instance.GetSecretWord(); // Gizli kelimeyi al

        List<KeyboardKey> untouchedKeys = new List<KeyboardKey>();  // Dokunmam�� klavye tu�lar�n� i�eren bir liste

        for (int i = 0; i < keys.Length; i++) // T�m klavye tu�lar�n� kontrol et ve dokunmam�� olanlar� listeye ekle
        {
            if (keys[i].IsUntouched())
            {
                untouchedKeys.Add(keys[i]);
            }
        }

        List<KeyboardKey> t_untouchedKeys = new List<KeyboardKey>(untouchedKeys); // Bir kopya liste olu�tur

        for (int i = 0; i < untouchedKeys.Count; i++) // Gizli kelimenin i�inde bulunan harfleri ��kart
        {
            if (secretWord.Contains(untouchedKeys[i].GetLetter()))  
            {
                t_untouchedKeys.Remove(untouchedKeys[i]);
            }
        }

        if (t_untouchedKeys.Count <= 0) // E�er t�m tu�lar kullan�ld�ysa, i�lemi ger�ekle�tirme
        {
            return;
        }
        int randomKeyIndex = Random.Range(0,t_untouchedKeys.Count); // Rastgele bir tu� se� ve onu ge�ersiz k�l
        t_untouchedKeys[randomKeyIndex].SetInvalid();

        DataManager.instance.RemoveCoins(keyboardHintPrice);// Coinleri azalt
    }

    List <int> letterHintGivenIndices = new List<int>(); // Daha �nce verilen harf ipu�lar�n� takip eden indeksleri i�eren liste
    public void LetterHint()   // Harf ipucu metodunu �a��ran buton taraf�ndan kullan�l�r
    {
        // Sahip olunan coin miktar�, harf ipucu fiyat�ndan d���kse, i�lemi ger�ekle�tirme
        if (DataManager.instance.GetCoins() < letterHintPrice)
        {
            return;
        }

        if (letterHintGivenIndices.Count >=5) // E�er t�m harf ipu�lar� verildiyse, i�lemi ger�ekle�tirme
        {
            Debug.Log("All hints");
            return;
        }

        List<int> letterHintNotGivenIndices = new List<int>(); // Verilmemi� harf ipu�lar� indekslerini i�eren liste

        for (int i = 0; i < 5; i++)
        {
            // 5 harf i�in indeksleri kontrol et ve verilmemi� olanlar� listeye ekle
            if (!letterHintGivenIndices.Contains(i))
            {
                letterHintNotGivenIndices.Add(i);
            }
        }

        WordContainer currentWordContainer = InputManager.instance.GetCurrentWordContainer(); // �u anki kelime konteyn�r�n� al

        string secretWord = WorldManager.instance.GetSecretWord();  // Gizli kelimeyi al

        int randomIndex = letterHintNotGivenIndices[Random.Range(0,letterHintNotGivenIndices.Count)]; // Verilmemi� harf ipucu indekslerinden rastgele birini se�
        letterHintGivenIndices.Add(randomIndex); // Verilen indeksi listeye ekle

        currentWordContainer.AddAsHint(randomIndex, secretWord[randomIndex]); // Harf ipucunu �u anki kelime konteyn�r�na ekle

        DataManager.instance.RemoveCoins(letterHintPrice);      // Coinleri azalt
    }
}
