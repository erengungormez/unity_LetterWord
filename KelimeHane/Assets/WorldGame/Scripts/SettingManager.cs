using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    // G�rsel elementleri temsil eden de�i�kenler
    [Header("Elements")]
    [SerializeField] private Image soundsImage;
    [SerializeField] private Image hapticsImage;

    // Ayarlar� temsil eden durum de�i�kenleri
    [Header("Settings")]
    private bool soundState;
    private bool hapticsState;


    void Start()
    {
        LoadStates();    // Kaydedilmi� ses ve titre�im durumlar�n� y�kle

    }
    void Update()
    {
        
    }

    public void SoundsButtonCallback()  // Ses d��mesine t�kland���nda �a�r�lan metod
    {
        soundState = !soundState; // Ses durumunu tersine �evir (toggle)
        UpdateSoundsState();      // Ses durumunu g�ncelle
        SaveStates();             // G�ncellenen durumu kaydet
    }

    private void UpdateSoundsState()   // Ses durumunu g�ncelleyen i� metod
    {
        // Ses a��ksa sesi etkinle�tir, kapal�ysa devre d��� b�rak
        if (soundState)
        {
            EnableSounds();
        }
        else
            DisableSounds();
    }

    private void EnableSounds()  // Sesin etkinle�tirildi�i metod
    {
        SoundsManager.instance.EnableSound(); // Sesin etkinle�tirilmesi
        soundsImage.color = Color.white; // Ses g�rsel eleman�n�n rengini beyaz yap
    }
    private void DisableSounds() // Sesin devre d��� b�rak�ld��� metod
    {
        SoundsManager.instance.DisableSound(); // Sesi aktifle�tir.
        soundsImage.color = Color.gray; // Ses g�rsel eleman�n�n rengini gri yap
    }
    public void HapticsButtonCallback() // Titre�im d��mesine t�kland���nda �a�r�lan metod
    {
        hapticsState = !hapticsState; // Titre�im durumunu tersine �evir (toggle)
        UpdateHapticsState();  // Titre�im durumunu g�ncelle
        SaveStates();  // G�ncellenen durumu kaydet
    }
    private void UpdateHapticsState() // Titre�im durumunu g�ncelleyen i� metod
    {
        // Titre�im a��ksa titre�imi etkinle�tir, kapal�ysa devre d��� b�rak
        if (hapticsState)
        {
            EnableSounds(); 
        } 
        else
        {
            DisableSounds();

        }
    }
    private void EnableHaptics() // Titre�imin etkinle�tirildi�i metod
    {
        
        hapticsImage.color = Color.white; // Titre�im g�rsel eleman�n�n rengini beyaz yap
    }

    private void OnDisable() // Komponent devre d��� b�rak�ld���nda �a�r�lan metod
    {
        hapticsImage.color = Color.gray; // Titre�im g�rsel eleman�n�n rengini gri yap
    }

    private void LoadStates()  // Kaydedilmi� ses ve titre�im durumlar�n� y�kleyen metod
    {
        // PlayerPrefs kullanarak ses ve titre�im durumlar�n� y�kle
        soundState = PlayerPrefs.GetInt("Sounds", 1) == 1;
        hapticsState= PlayerPrefs.GetInt("Haptic", 1) == 1;

        // Ses ve titre�im durumlar�n� g�ncelle
        UpdateSoundsState();
        UpdateHapticsState();

    }
   private void SaveStates() // Ses ve titre�im durumlar�n� kaydeden metod
    {
        PlayerPrefs.SetInt("Sounds", soundState? 1 : 0);
        PlayerPrefs.SetInt("Haptics", hapticsState? 1 : 0);
    }
}