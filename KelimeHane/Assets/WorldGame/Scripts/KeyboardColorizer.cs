using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class KeyboardColorizer : MonoBehaviour
{
    // Klavye harf tu�lar�n� temsil eden dizi
    [Header(" Elements ")]
    private KeyboardKey[] keys;
    // Klavye durumunu s�f�rlamak i�in kullan�lan kontrol de�i�keni
    [Header(" Settings ")]
    private bool shouldReset;


    private void Awake()
    {
        keys = GetComponentsInChildren<KeyboardKey>();
    }



    void Start() // Oyun durumu de�i�ikliklerini dinleyen metot
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy() // Bu script yok edildi�inde oyun durumu de�i�ikliklerini dinleyen metotu kald�r
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)  // Oyun durumu de�i�ti�inde �a�r�lan metot
    {
        switch (gameState)
        {
            // E�er s�f�rlama yap�lmas� gerekiyorsa, klavye tu�lar�n� ba�lat
            case GameState.Game:
                if (shouldReset)
                {
                    Initialize();
                }
                break;

            case GameState.LevelComplate:
                // Seviye tamamland���nda s�f�rlama yap�lmas� gerekiyor
                shouldReset = true;
                break;

            case GameState.Gameover:
                // Oyun bitti�inde s�f�rlama yap�lmas� gerekiyor
                shouldReset = true;
                break;
            
        }
    }

    private void Initialize() // Klavye tu�lar�n� ba�latan metot
    {
        for (int i = 0; i < keys.Length; i++) // Her bir klavye tu�unu ba�lat
        {
            keys[i].Initialize();
        }
        shouldReset = false; // S�f�rlama i�lemi tamamland�
    }

    void Update()
    {
        
    }



    public void Colorize(string secretWord, string wordToCheck)  // Klavye tu�lar�n� renklendiren metot
    {
        for (int i = 0; i < keys.Length; i++)  // Klavye tu�lar�n� d�ng�yle kontrol et
        {
            char keyLetter = keys[i].GetLetter();

            for (int j = 0; j< wordToCheck.Length; j++) // Kelimeyi kontrol etmek i�in ikinci bir d�ng�
            {
                // Harf e�le�mediyse di�er harfi kontrol et
                if (keyLetter != wordToCheck[j])
                {
                    continue;
                }

                if (keyLetter == secretWord[j])  // Harf e�le�tiyse duruma g�re tu�u renklendir
                {
                    // Ge�erli 
                    keys[i].SetValid();
                }
                else if (secretWord.Contains(keyLetter))
                {
                    // Potantiel
                    keys[i].SetPotantiel();
                }
                else
                {
                    // Ge�ersiz 
                    keys[i].SetInvalid();
                }
            }

        }
    }
}
