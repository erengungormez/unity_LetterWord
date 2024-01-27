using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class KeyboardColorizer : MonoBehaviour
{
    // Klavye harf tuþlarýný temsil eden dizi
    [Header(" Elements ")]
    private KeyboardKey[] keys;
    // Klavye durumunu sýfýrlamak için kullanýlan kontrol deðiþkeni
    [Header(" Settings ")]
    private bool shouldReset;


    private void Awake()
    {
        keys = GetComponentsInChildren<KeyboardKey>();
    }



    void Start() // Oyun durumu deðiþikliklerini dinleyen metot
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy() // Bu script yok edildiðinde oyun durumu deðiþikliklerini dinleyen metotu kaldýr
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)  // Oyun durumu deðiþtiðinde çaðrýlan metot
    {
        switch (gameState)
        {
            // Eðer sýfýrlama yapýlmasý gerekiyorsa, klavye tuþlarýný baþlat
            case GameState.Game:
                if (shouldReset)
                {
                    Initialize();
                }
                break;

            case GameState.LevelComplate:
                // Seviye tamamlandýðýnda sýfýrlama yapýlmasý gerekiyor
                shouldReset = true;
                break;

            case GameState.Gameover:
                // Oyun bittiðinde sýfýrlama yapýlmasý gerekiyor
                shouldReset = true;
                break;
            
        }
    }

    private void Initialize() // Klavye tuþlarýný baþlatan metot
    {
        for (int i = 0; i < keys.Length; i++) // Her bir klavye tuþunu baþlat
        {
            keys[i].Initialize();
        }
        shouldReset = false; // Sýfýrlama iþlemi tamamlandý
    }

    void Update()
    {
        
    }



    public void Colorize(string secretWord, string wordToCheck)  // Klavye tuþlarýný renklendiren metot
    {
        for (int i = 0; i < keys.Length; i++)  // Klavye tuþlarýný döngüyle kontrol et
        {
            char keyLetter = keys[i].GetLetter();

            for (int j = 0; j< wordToCheck.Length; j++) // Kelimeyi kontrol etmek için ikinci bir döngü
            {
                // Harf eþleþmediyse diðer harfi kontrol et
                if (keyLetter != wordToCheck[j])
                {
                    continue;
                }

                if (keyLetter == secretWord[j])  // Harf eþleþtiyse duruma göre tuþu renklendir
                {
                    // Geçerli 
                    keys[i].SetValid();
                }
                else if (secretWord.Contains(keyLetter))
                {
                    // Potantiel
                    keys[i].SetPotantiel();
                }
                else
                {
                    // Geçersiz 
                    keys[i].SetInvalid();
                }
            }

        }
    }
}
