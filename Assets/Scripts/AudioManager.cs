using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("BackGround Audio Clip")]
    public AudioClip backgroundClip;

    [Header("Player Audio Clip")]
    public AudioClip jumpClip;
    public AudioClip attackClip;
    public AudioClip hitClip;
    public AudioClip pickUpClip;
    public AudioClip hitScoreClip;
    public AudioClip fallToLava;
    public AudioClip throwPickup;

    [Header("Enemy Audio Clip")]
    public AudioClip enemyHit;

    [Header("UI Audio Clip")]
    public AudioClip confirm;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
