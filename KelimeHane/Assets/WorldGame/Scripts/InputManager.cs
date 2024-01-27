using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager instance; // Singleton deseni ile bu sýnýfýn tek örneði

    // Kelime konteynerlerini temsil eden dizi
    [Header(" Elements ")]
    [SerializeField] private WordContainer[] wordContainers;
    [SerializeField] private Button tryButton;
    [SerializeField] private KeyboardColorizer keyboardColorizer;

    // Oyun içindeki durumlar için kullanýlan deðiþkenler
    [Header(" Settings ")]
    private int currentWordContainerIndex;
    private bool canAddLetter = true;
    private bool shouldReset;

    // Olaylarý temsil eden delegeler
    [Header(" Events ")]
    public static Action onLetterAdded;
    public static Action onLetterRemoved;


    private void Awake() // Singleton deseni için Awake metodu
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    void Start()
    {
        Initialize();

        KeyboardKey.onKeyPressed += KeyPressedCallback; // Klavye tuþlarýnýn basýlma olayýný dinleyen metotu ekle
        GameManager.onGameStateChanged += GameStateChangedCallback; // Oyun durumu deðiþikliklerini dinleyen metotu ekle

    }

    private void OnDestroy() // Bu script yok edildiðinde olay dinleyicilerini kaldý
    {
        KeyboardKey.onKeyPressed -= KeyPressedCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;

    }

    private void GameStateChangedCallback(GameState gameState) // Oyun durumu deðiþtiðinde çaðrýlan metot
    {
        switch (gameState)
        {
            
            case GameState.Game: // Eðer sýfýrlama yapýlmasý gerekiyorsa, baþlangýç ayarlarýný yap

                if (shouldReset)
                {
                    Initialize();
                }
                break;

            case GameState.LevelComplate: // Seviye tamamlandýðýnda sýfýrlama yapýlmasý gerekiyor
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

    public void Initialize() // Oyun baþladýðýnda çaðrýlan metot
    {
        currentWordContainerIndex = 0;
        canAddLetter = true;

        DisableTryButton(); // "Denemeye Baþla" butonunu etkisiz hale getir

        // Tüm kelime konteynerlarýný baþlat
        for (int i = 0; i < wordContainers.Length; i++)
            wordContainers[i].Initialize();

        shouldReset = false;
    }
    
    private void KeyPressedCallback(char letter) // Klavye tuþlarýndan biri basýldýðýnda çaðrýlan metot
    {

        if (!canAddLetter) // Harf eklemeye izin verilmiyorsa geri dön
        {
            return;     
        }
        // Aktif kelime konteynerine harf ekle
        wordContainers[currentWordContainerIndex].Add(letter);

        // Konteyner tamamlandýysa "Denemeye Baþla" butonunu etkinleþtir
        if (wordContainers[currentWordContainerIndex].IsComplete())
        {
            canAddLetter = false;
            EnableTryButton();
           
        }

        onLetterAdded?.Invoke(); // Harf eklendi olayýný tetikle

    }
    
    public void CheckWord() // Kelime kontrolü için çaðrýlan metot
    {
        string wordToCheck = wordContainers[currentWordContainerIndex].GetWord(); 
        string secretWord = WorldManager.instance.GetSecretWord();

        wordContainers[currentWordContainerIndex].Colorize(secretWord); // Kelime konteynerini renklendir
        keyboardColorizer.Colorize (secretWord, wordToCheck); // Klavye tuþlarýný renklendir


        if (wordToCheck == secretWord)  // Doðru kelime bulunduysa
        {

            SetLevelComplete(); // Level ý tamamla
            
        }
        else  // Yanlýþ kelime, bir sonraki konteynere geç ve "Denemeye Baþla" butonunu etkisiz hale getir
        {
            // Debug.Log("Level Failed");
            currentWordContainerIndex++;
            DisableTryButton();

            if (currentWordContainerIndex >= wordContainers.Length) // Eðer tüm konteynerler tamamlandýysa oyunu bitir
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

    private void SetLevelComplete() // Seviye tamamlandýðýnda çaðrýlan meto
    {
        UpdateData(); // Verileri güncelle ve oyun durumunu "Seviye Tamamlandý" olarak ayarla
        GameManager.instance.SetGameState(GameState.LevelComplate);
    }

    private void UpdateData()     // Verileri güncelleyen metot
    {
        // Skor ve coin ekleyerek verileri güncelle
        int scoreToAdd = 6 - currentWordContainerIndex;

        DataManager.instance.IncreseScore(scoreToAdd);
        DataManager.instance.AddCoins(scoreToAdd*3);
    }

    public void BackspacePressedCallback()     // Geri alma tuþuna basýldýðýnda çaðrýlan metot
    {
        bool removedLetter = wordContainers[currentWordContainerIndex].RemoveLetter(); // Harf silindiðinde "Denemeye Baþla" butonunu etkisiz hale getir

        if (removedLetter)
        {
            DisableTryButton();
        }
        canAddLetter = true;

        onLetterRemoved?.Invoke();
    }
    
    public void EnableTryButton() // "Denemeye Baþla" butonunu etkinleþtiren metot
    {
        tryButton.interactable = true;
    } 
    public void DisableTryButton()     // "Denemeye Baþla" butonunu etkisiz hale getiren metot
    {
        tryButton.interactable = false;
    }
    
    public WordContainer GetCurrentWordContainer()  // Þu anki kelime konteynerini döndüren metot
    {  
        return wordContainers[currentWordContainerIndex];
    }

}
