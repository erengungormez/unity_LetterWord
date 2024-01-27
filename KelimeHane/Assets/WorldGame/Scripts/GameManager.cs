using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Idle, Game, LevelComplate, Gameover, Menu } // Oyun durumlar�n� temsil eden enum


public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton deseni i�in GameManager �rne�i

    [Header(" Settings ")]     // Oyun durumunu tutan de�i�ken
    public GameState gameState;

    [Header(" Events ")]    // Oyun durumu de�i�ti�inde tetiklenecek olay
    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        // Singleton deseni kontrol�
        if (instance == null)
        {
            instance = this; // E�er ba�ka bir GameManager �rne�i yoksa bu �rne�i atar
        }
        else
            Destroy(gameObject);
    }

    public void SetGameState ( GameState gameState) // Oyun durumunu de�i�tiren metod
    {
        this.gameState = gameState; // Oyun durumunu g�nceller
        onGameStateChanged?.Invoke(gameState); // Oyun durumu de�i�ti�inde olay� tetikler
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextButtonCallback() // "Next" butonuna t�klan�nca �a�r�lacak metod
    {
        SetGameState(GameState.Game); // Oyun durumunu "Game" olarak ayarlar 
    }

    public void PlayButtonCallback()     // "Play" butonuna t�klan�nca �a�r�lacak metod
    {
        SetGameState(GameState.Game);
    }

    public void BackButtonCallback()
    {                                   // "Back" butonuna t�klan�nca �a�r�lacak metod
        Debug.Log("Back button clicked!");
        SetGameState(GameState.Menu);
    }

    public bool IsGameState() // Oyun durumu "Game" mi?   
    {
        return gameState == GameState.Game; // E�er oyun durumu "Game" ise true, de�ilse false d�nd�r�r
    }
}
