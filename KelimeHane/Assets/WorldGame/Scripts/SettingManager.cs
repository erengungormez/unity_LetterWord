using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    // Görsel elementleri temsil eden deðiþkenler
    [Header("Elements")]
    [SerializeField] private Image soundsImage;
    [SerializeField] private Image hapticsImage;

    // Ayarlarý temsil eden durum deðiþkenleri
    [Header("Settings")]
    private bool soundState;
    private bool hapticsState;


    void Start()
    {
        LoadStates();    // Kaydedilmiþ ses ve titreþim durumlarýný yükle

    }
    void Update()
    {
        
    }

    public void SoundsButtonCallback()  // Ses düðmesine týklandýðýnda çaðrýlan metod
    {
        soundState = !soundState; // Ses durumunu tersine çevir (toggle)
        UpdateSoundsState();      // Ses durumunu güncelle
        SaveStates();             // Güncellenen durumu kaydet
    }

    private void UpdateSoundsState()   // Ses durumunu güncelleyen iç metod
    {
        // Ses açýksa sesi etkinleþtir, kapalýysa devre dýþý býrak
        if (soundState)
        {
            EnableSounds();
        }
        else
            DisableSounds();
    }

    private void EnableSounds()  // Sesin etkinleþtirildiði metod
    {
        SoundsManager.instance.EnableSound(); // Sesin etkinleþtirilmesi
        soundsImage.color = Color.white; // Ses görsel elemanýnýn rengini beyaz yap
    }
    private void DisableSounds() // Sesin devre dýþý býrakýldýðý metod
    {
        SoundsManager.instance.DisableSound(); // Sesi aktifleþtir.
        soundsImage.color = Color.gray; // Ses görsel elemanýnýn rengini gri yap
    }
    public void HapticsButtonCallback() // Titreþim düðmesine týklandýðýnda çaðrýlan metod
    {
        hapticsState = !hapticsState; // Titreþim durumunu tersine çevir (toggle)
        UpdateHapticsState();  // Titreþim durumunu güncelle
        SaveStates();  // Güncellenen durumu kaydet
    }
    private void UpdateHapticsState() // Titreþim durumunu güncelleyen iç metod
    {
        // Titreþim açýksa titreþimi etkinleþtir, kapalýysa devre dýþý býrak
        if (hapticsState)
        {
            EnableSounds(); 
        } 
        else
        {
            DisableSounds();

        }
    }
    private void EnableHaptics() // Titreþimin etkinleþtirildiði metod
    {
        
        hapticsImage.color = Color.white; // Titreþim görsel elemanýnýn rengini beyaz yap
    }

    private void OnDisable() // Komponent devre dýþý býrakýldýðýnda çaðrýlan metod
    {
        hapticsImage.color = Color.gray; // Titreþim görsel elemanýnýn rengini gri yap
    }

    private void LoadStates()  // Kaydedilmiþ ses ve titreþim durumlarýný yükleyen metod
    {
        // PlayerPrefs kullanarak ses ve titreþim durumlarýný yükle
        soundState = PlayerPrefs.GetInt("Sounds", 1) == 1;
        hapticsState= PlayerPrefs.GetInt("Haptic", 1) == 1;

        // Ses ve titreþim durumlarýný güncelle
        UpdateSoundsState();
        UpdateHapticsState();

    }
   private void SaveStates() // Ses ve titreþim durumlarýný kaydeden metod
    {
        PlayerPrefs.SetInt("Sounds", soundState? 1 : 0);
        PlayerPrefs.SetInt("Haptics", hapticsState? 1 : 0);
    }
}