using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance { get => _instance; }

    [Header("DEBUG")]
    public AudioClip sfxDebug;
    public AudioClip musicDebug;
    [Header("SFX")]
    public AudioClip sfxUIButton;
    public AudioClip sfxWin;
    public AudioClip sfxLose;
    public AudioClip sfxAttackMelee;
    public AudioClip sfxAttackRange;
    public AudioClip sfxKnockback;
    public AudioClip sfxHeal;
    public AudioClip sfxHurt;
    public AudioClip sfxDying;
    public AudioClip sfxPotion;
    [Header("Music")]
    public AudioClip musicMainMenu;
    public AudioClip musicLevelSelection;
    public AudioClip musicBattle;
    public AudioClip musicBossBattle;

    private AudioSource _source;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _source = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void PlayAudio(AudioClip audio)
    {
        _source.clip = sfxDebug; //TODO change DEBUG
        _source.Play();
        print("AUDIO");
    }

    public void PlayUIButtonSFX()
    {
        PlayAudio(sfxUIButton);
    }

    public void PlayWinSFX()
    {
        PlayAudio(sfxWin);
    }

    public void PlayLoseSFX()
    {
        PlayAudio(sfxLose);
    }

    public void PlaySpellSFX(Spell spell)
    {
        if (spell.utilityType == UtilityType.Knockback) PlayAudio(sfxKnockback);
        else if (spell.utilityType == UtilityType.Healing) PlayAudio(sfxHeal);
        else if (spell.spellAreaType == SpellAreaType.Range) PlayAudio(sfxAttackRange);
        else PlayAudio(sfxAttackMelee);
    }

    public void PlayHurtSFX()
    {
        PlayAudio(sfxHurt);
    }

    public void PlayDyingSFX()
    {
        PlayAudio(sfxDying);
    }

    public void PlayPotionSFX()
    {
        PlayAudio(sfxPotion);
    }

    public void PlayAttackRangeSFX()
    {
        PlayAudio(sfxAttackRange);
    }

    public void PlayMainMenuMusic()
    {
        PlayAudio(musicMainMenu);
    }

    public void PlayLevelSelectionMusic()
    {
        PlayAudio(musicLevelSelection);
    }

    public void PlayBattleMusic()
    {
        PlayAudio(musicBattle);
    }

    public void PlayBossBattleMusic()
    {
        PlayAudio(musicBossBattle);
    }

    public void StopAudio()
    {
        _source.Stop();
    }
}
