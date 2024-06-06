using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance { get => _instance; }

    [Header("SFX")]
    public AudioClip sfxUIButton;
    public AudioClip sfxWin;
    public AudioClip sfxLose;
    public AudioClip sfxkMelee;
    public AudioClip sfxArrow;
    public AudioClip sfxKnockback;
    public AudioClip sfxMagicAttack;
    public AudioClip sfxMagicHeal;
    public AudioClip sfxHurt;
    public AudioClip sfxPotion;
    public AudioClip sfxWalk;
    [Header("Music")]
    public AudioClip musicMainMenu;
    public AudioClip musicLevelSelection;
    public AudioClip musicFirstBattle;
    public AudioClip musicSecondBattle;
    public AudioClip musicBossBattle;
    [Header("AudioSources")]
    public AudioSource sfx;
    public AudioSource music;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }

    }

    // SFX -------------------------------------------

    public void PlaySFX(AudioClip audio, float volume = 0.3f, float pitch = 1)
    {
        sfx.clip = audio;
        sfx.volume = volume;
        sfx.pitch = pitch;
        sfx.Play();
    }

    public void PlayUIButtonSFX()
    {
        PlaySFX(sfxUIButton);
    }

    public void PlayWinSFX()
    {
        StopMusic();
        PlaySFX(sfxWin);
    }

    public void PlayLoseSFX()
    {
        StopMusic();
        PlaySFX(sfxLose);
    }

    public void PlaySpellSFX(Spell spell, float volume = 0.3f, float pitch = 1)
    {
        switch (spell.soundType)
        {
            case SoundType.Melee:
                PlaySFX(sfxkMelee, volume, pitch);
                break;
            case SoundType.Arrow:
                PlaySFX(sfxArrow, volume, pitch);
                break;
            case SoundType.Knockback:
                PlaySFX(sfxKnockback, volume, pitch);
                break;
            case SoundType.MagicAttack:
                PlaySFX(sfxMagicAttack, volume, pitch);
                break;
            case SoundType.MagicHeal:
                PlaySFX(sfxMagicHeal, volume, pitch);
                break;
        }
    }

    public void PlayCritsSFX(Spell spell)
    {
        PlaySpellSFX(spell, pitch: 0.25f);
    }

    public void PlayHurtSFX()
    {
        PlaySFX(sfxHurt);
    }

    public void PlayDyingSFX()
    {
        PlaySFX(sfxHurt, pitch: 0.5f);
    }

    public void PlayPotionSFX()
    {
        PlaySFX(sfxPotion);
    }

    public void PlayWalkSFX()
    {
        PlaySFX(sfxWalk, volume: 0.75f);
    }

    public void StopSFX()
    {
        sfx.Stop();
    }

    // Music -------------------------------------------

    public void PlayMusic(AudioClip audio, float pitch = 1)
    {
        music.clip = audio;
        music.pitch = pitch;
        music.Play();
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(musicMainMenu);
    }

    public void PlayLevelSelectionMusic()
    {
        PlayMusic(musicLevelSelection);
    }

    public void PlayBattleMusic(int level)
    {
        switch (level)
        {
            case 1:
                PlayMusic(musicFirstBattle);
                break;
            case 2:
                PlayMusic(musicSecondBattle);
                break;
            case 3:
                PlayMusic(musicBossBattle);
                break;
        }
    }

    public void StopMusic()
    {
        music.Stop();
    }
}
