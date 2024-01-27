using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Idle, Game, LevelComplate, Gameover, Menu } // Oyun durumlarýný temsil eden enum


public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton deseni için GameManager örneði

    [Header(" Settings ")]     // Oyun durumunu tutan deðiþken
    public GameState gameState;

    [Header(" Events ")]    // Oyun durumu deðiþtiðinde tetiklenecek olay
    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        // Singleton deseni kontrolü
        if (instance == null)
        {
            instance = this; // Eðer baþka bir GameManager örneði yoksa bu örneði atar
        }
        else
            Destroy(gameObject);
    }

    public void SetGameState ( GameState gameState) // Oyun durumunu deðiþtiren metod
    {
        this.gameState = gameState; // Oyun durumunu günceller
        onGameStateChanged?.Invoke(gameState); // Oyun durumu deðiþtiðinde olayý tetikler
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextButtonCallback() // "Next" butonuna týklanýnca çaðrýlacak metod
    {
        SetGameState(GameState.Game); // Oyun durumunu "Game" olarak ayarlar 
    }

    public void PlayButtonCallback()     // "Play" butonuna týklanýnca çaðrýlacak metod
    {
        SetGameState(GameState.Game);
    }

    public void BackButtonCallback()
    {                                   // "Back" butonuna týklanýnca çaðrýlacak metod
        Debug.Log("Back button clicked!");
        SetGameState(GameState.Menu);
    }

    public bool IsGameState() // Oyun durumu "Game" mi?   
    {
        return gameState == GameState.Game; // Eðer oyun durumu "Game" ise true, deðilse false döndürür
    }
}
