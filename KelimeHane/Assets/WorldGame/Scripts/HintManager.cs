using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    [Header("Elements")] // Klavye tuþlarý için bir dizi
    [SerializeField] private GameObject keyboard;
    private KeyboardKey[] keys;

    // Klavye ve harf ipuçlarýnýn fiyatlarýný gösteren metin öðeleri
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI keyboardPriceText;
    [SerializeField] private TextMeshProUGUI letterPriceText;

    // Oyun durumlarýna göre sýfýrlama kontrolü ve ipuçlarýnýn fiyatlarý
    [Header("Settings")]
    private bool shouldReset;
    [SerializeField] private int keyboardHintPrice;
    [SerializeField] private int letterHintPrice;
    


    private void Awake()
    {
        keys = keyboard.GetComponentsInChildren<KeyboardKey>(); // Klavye tuþlarý dizisini al
    }
    void Start()
    {
        // Klavye ve harf ipucu fiyatlarýný metin öðelerine atanmasý
        keyboardPriceText.text = keyboardHintPrice.ToString();
        letterPriceText.text = letterHintPrice.ToString();

        GameManager.onGameStateChanged += GameStateChangedCallback; // Oyun durumu deðiþikliklerini takip eden delegenin metodu olarak GameStateChangedCallback metodu ekleniyor
    }

    private void OnDestroy() // Bu script yok edildiðinde delegenin metodu olarak GameStateChangedCallback metodu çýkarýlýyor
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;

    }

    
    private void GameStateChangedCallback(GameState gameState) // Oyun durumu deðiþtiðinde çaðrýlan metot
    {
        switch (gameState) 
        {
            case GameState.Menu: // Oyun menü durumundayken yapýlacak iþlemler
                break;

            case GameState.Game: // Oyun durumundayken yapýlacak iþlemler

                if (shouldReset) // Eðer shouldReset flag'i true ise, harf ipuçlarýyla ilgili bilgileri sýfýrla
                {
                    letterHintGivenIndices.Clear();
                    shouldReset = false;
                }

                break;
            case GameState.LevelComplate:  // Seviye tamamlandýðýnda yapýlacak iþlemler
                shouldReset = true;
                break;


            case GameState.Gameover:
                shouldReset = true;
                break;
            
        }
    }

    public void KeyboardHint() // Klavye ipucu metodunu çaðýran buton tarafýndan kullanýlýr
    {
        // Sahip olunan coin miktarý, klavye ipucu fiyatýndan düþükse, iþlemi gerçekleþtirme
        if (DataManager.instance.GetCoins()< keyboardHintPrice)
        {
            return;
        }

        string secretWord = WorldManager.instance.GetSecretWord(); // Gizli kelimeyi al

        List<KeyboardKey> untouchedKeys = new List<KeyboardKey>();  // Dokunmamýþ klavye tuþlarýný içeren bir liste

        for (int i = 0; i < keys.Length; i++) // Tüm klavye tuþlarýný kontrol et ve dokunmamýþ olanlarý listeye ekle
        {
            if (keys[i].IsUntouched())
            {
                untouchedKeys.Add(keys[i]);
            }
        }

        List<KeyboardKey> t_untouchedKeys = new List<KeyboardKey>(untouchedKeys); // Bir kopya liste oluþtur

        for (int i = 0; i < untouchedKeys.Count; i++) // Gizli kelimenin içinde bulunan harfleri çýkart
        {
            if (secretWord.Contains(untouchedKeys[i].GetLetter()))  
            {
                t_untouchedKeys.Remove(untouchedKeys[i]);
            }
        }

        if (t_untouchedKeys.Count <= 0) // Eðer tüm tuþlar kullanýldýysa, iþlemi gerçekleþtirme
        {
            return;
        }
        int randomKeyIndex = Random.Range(0,t_untouchedKeys.Count); // Rastgele bir tuþ seç ve onu geçersiz kýl
        t_untouchedKeys[randomKeyIndex].SetInvalid();

        DataManager.instance.RemoveCoins(keyboardHintPrice);// Coinleri azalt
    }

    List <int> letterHintGivenIndices = new List<int>(); // Daha önce verilen harf ipuçlarýný takip eden indeksleri içeren liste
    public void LetterHint()   // Harf ipucu metodunu çaðýran buton tarafýndan kullanýlýr
    {
        // Sahip olunan coin miktarý, harf ipucu fiyatýndan düþükse, iþlemi gerçekleþtirme
        if (DataManager.instance.GetCoins() < letterHintPrice)
        {
            return;
        }

        if (letterHintGivenIndices.Count >=5) // Eðer tüm harf ipuçlarý verildiyse, iþlemi gerçekleþtirme
        {
            Debug.Log("All hints");
            return;
        }

        List<int> letterHintNotGivenIndices = new List<int>(); // Verilmemiþ harf ipuçlarý indekslerini içeren liste

        for (int i = 0; i < 5; i++)
        {
            // 5 harf için indeksleri kontrol et ve verilmemiþ olanlarý listeye ekle
            if (!letterHintGivenIndices.Contains(i))
            {
                letterHintNotGivenIndices.Add(i);
            }
        }

        WordContainer currentWordContainer = InputManager.instance.GetCurrentWordContainer(); // Þu anki kelime konteynýrýný al

        string secretWord = WorldManager.instance.GetSecretWord();  // Gizli kelimeyi al

        int randomIndex = letterHintNotGivenIndices[Random.Range(0,letterHintNotGivenIndices.Count)]; // Verilmemiþ harf ipucu indekslerinden rastgele birini seç
        letterHintGivenIndices.Add(randomIndex); // Verilen indeksi listeye ekle

        currentWordContainer.AddAsHint(randomIndex, secretWord[randomIndex]); // Harf ipucunu þu anki kelime konteynýrýna ekle

        DataManager.instance.RemoveCoins(letterHintPrice);      // Coinleri azalt
    }
}
