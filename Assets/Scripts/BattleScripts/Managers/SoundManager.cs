using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("DEBUG")]
    public AudioClip sfxDebug;
    public AudioSource musicDebug;
    [Header("Settings")]
    public float volume;
    [Header("SFX")]
    public AudioSource sfxUIButton;
    public AudioSource sfxWin;
    public AudioSource sfxLose;
    public AudioSource sfxAttackMelee;
    public AudioSource sfxAttackBow;
    public AudioSource sfxAttackMagic;
    public AudioSource sfxHurt;
    public AudioSource sfxDying;
    public AudioSource sfxPotion;
    [Header("Music")]
    public AudioSource musicMainMenu;
    public AudioSource musicLevelSelection;
    public AudioSource musicBattle;
    public AudioSource musicBoosBattle;

    private void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.volume = volume;
        source.clip = sfxDebug;
        //source.Play();
        //sfxDebug.GetComponent<AudioSource>().Play();
    }
}
