using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;

    [Header("Sounds")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource letterAddedSound;
    [SerializeField] private AudioSource letterRemovedSound;
    [SerializeField] private AudioSource levelComplatedSound;
    [SerializeField] private AudioSource gameoverSound;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        InputManager.onLetterAdded += PlayLetterAddedSound;
        InputManager.onLetterRemoved += PlayLetterAddedSound;

        GameManager.onGameStateChanged += GameStateChangedCallback; 
    }

    private void OnDestroy()
    {
        InputManager.onLetterAdded -= PlayLetterAddedSound;
        InputManager.onLetterRemoved -= PlayLetterAddedSound;

        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }


    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            
            case GameState.LevelComplate:
                levelComplatedSound.Play();
                break;
            case GameState.Gameover:
                gameoverSound.Play();
                break;
            
        }
    }



    void Update()
    {
        
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }

    private void PlayLetterAddedSound() 
    {
        letterAddedSound.Play();    
    }

    private void PlayLetterRemovedSound()
    {
        letterRemovedSound.Play();
    }
    public void EnableSound()
    {
        buttonSound.volume = 1.0f;
        letterAddedSound.volume = 1.0f;
        letterRemovedSound.volume = 1.0f;
        levelComplatedSound.volume = 1.0f;
        gameoverSound.volume = 1.0f;
    }

    public void DisableSound()
    {
        buttonSound.volume = 0.0f;
        letterAddedSound.volume = 0.0f;
        letterRemovedSound.volume = 0.0f;
        levelComplatedSound.volume = 0.0f;
        gameoverSound.volume = 0.0f;
    }











}
