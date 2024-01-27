using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager instance; // Singleton deseni ile bu s�n�f�n tek �rne�i

    // Kelime konteynerlerini temsil eden dizi
    [Header(" Elements ")]
    [SerializeField] private WordContainer[] wordContainers;
    [SerializeField] private Button tryButton;
    [SerializeField] private KeyboardColorizer keyboardColorizer;

    // Oyun i�indeki durumlar i�in kullan�lan de�i�kenler
    [Header(" Settings ")]
    private int currentWordContainerIndex;
    private bool canAddLetter = true;
    private bool shouldReset;

    // Olaylar� temsil eden delegeler
    [Header(" Events ")]
    public static Action onLetterAdded;
    public static Action onLetterRemoved;


    private void Awake() // Singleton deseni i�in Awake metodu
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    void Start()
    {
        Initialize();

        KeyboardKey.onKeyPressed += KeyPressedCallback; // Klavye tu�lar�n�n bas�lma olay�n� dinleyen metotu ekle
        GameManager.onGameStateChanged += GameStateChangedCallback; // Oyun durumu de�i�ikliklerini dinleyen metotu ekle

    }

    private void OnDestroy() // Bu script yok edildi�inde olay dinleyicilerini kald�
    {
        KeyboardKey.onKeyPressed -= KeyPressedCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;

    }

    private void GameStateChangedCallback(GameState gameState) // Oyun durumu de�i�ti�inde �a�r�lan metot
    {
        switch (gameState)
        {
            
            case GameState.Game: // E�er s�f�rlama yap�lmas� gerekiyorsa, ba�lang�� ayarlar�n� yap

                if (shouldReset)
                {
                    Initialize();
                }
                break;

            case GameState.LevelComplate: // Seviye tamamland���nda s�f�rlama yap�lmas� gerekiyor
                shouldReset = true;
                break;

            case GameState.Gameover:
                shouldReset = true;
                    break;
        }
    }


    void Update()
    {
        
    }

    public void Initialize() // Oyun ba�lad���nda �a�r�lan metot
    {
        currentWordContainerIndex = 0;
        canAddLetter = true;

        DisableTryButton(); // "Denemeye Ba�la" butonunu etkisiz hale getir

        // T�m kelime konteynerlar�n� ba�lat
        for (int i = 0; i < wordContainers.Length; i++)
            wordContainers[i].Initialize();

        shouldReset = false;
    }
    
    private void KeyPressedCallback(char letter) // Klavye tu�lar�ndan biri bas�ld���nda �a�r�lan metot
    {

        if (!canAddLetter) // Harf eklemeye izin verilmiyorsa geri d�n
        {
            return;     
        }
        // Aktif kelime konteynerine harf ekle
        wordContainers[currentWordContainerIndex].Add(letter);

        // Konteyner tamamland�ysa "Denemeye Ba�la" butonunu etkinle�tir
        if (wordContainers[currentWordContainerIndex].IsComplete())
        {
            canAddLetter = false;
            EnableTryButton();
           
        }

        onLetterAdded?.Invoke(); // Harf eklendi olay�n� tetikle

    }
    
    public void CheckWord() // Kelime kontrol� i�in �a�r�lan metot
    {
        string wordToCheck = wordContainers[currentWordContainerIndex].GetWord(); 
        string secretWord = WorldManager.instance.GetSecretWord();

        wordContainers[currentWordContainerIndex].Colorize(secretWord); // Kelime konteynerini renklendir
        keyboardColorizer.Colorize (secretWord, wordToCheck); // Klavye tu�lar�n� renklendir


        if (wordToCheck == secretWord)  // Do�ru kelime bulunduysa
        {

            SetLevelComplete(); // Level � tamamla
            
        }
        else  // Yanl�� kelime, bir sonraki konteynere ge� ve "Denemeye Ba�la" butonunu etkisiz hale getir
        {
            // Debug.Log("Level Failed");
            currentWordContainerIndex++;
            DisableTryButton();

            if (currentWordContainerIndex >= wordContainers.Length) // E�er t�m konteynerler tamamland�ysa oyunu bitir
            {
                //  Debug.Log("Gameover");
                GameManager.instance.SetGameState(GameState.Gameover);
            }
            else
            {
                canAddLetter = true;
            }

        }
    }

    private void SetLevelComplete() // Seviye tamamland���nda �a�r�lan meto
    {
        UpdateData(); // Verileri g�ncelle ve oyun durumunu "Seviye Tamamland�" olarak ayarla
        GameManager.instance.SetGameState(GameState.LevelComplate);
    }

    private void UpdateData()     // Verileri g�ncelleyen metot
    {
        // Skor ve coin ekleyerek verileri g�ncelle
        int scoreToAdd = 6 - currentWordContainerIndex;

        DataManager.instance.IncreseScore(scoreToAdd);
        DataManager.instance.AddCoins(scoreToAdd*3);
    }

    public void BackspacePressedCallback()     // Geri alma tu�una bas�ld���nda �a�r�lan metot
    {
        bool removedLetter = wordContainers[currentWordContainerIndex].RemoveLetter(); // Harf silindi�inde "Denemeye Ba�la" butonunu etkisiz hale getir

        if (removedLetter)
        {
            DisableTryButton();
        }
        canAddLetter = true;

        onLetterRemoved?.Invoke();
    }
    
    public void EnableTryButton() // "Denemeye Ba�la" butonunu etkinle�tiren metot
    {
        tryButton.interactable = true;
    } 
    public void DisableTryButton()     // "Denemeye Ba�la" butonunu etkisiz hale getiren metot
    {
        tryButton.interactable = false;
    }
    
    public WordContainer GetCurrentWordContainer()  // �u anki kelime konteynerini d�nd�ren metot
    {  
        return wordContainers[currentWordContainerIndex];
    }

}
