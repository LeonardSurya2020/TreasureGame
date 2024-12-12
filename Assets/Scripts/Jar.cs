using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jar : MonoBehaviour
{
    private int coinValue = 100;
    private int jewelValue = 200;
    private int diamond = 500;
    public GameObject scorePointText;
    private Score score;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin") || collision.gameObject.CompareTag("Necklace"))
        {
            score = scorePointText.gameObject.GetComponent<Score>();
            if (score != null )
            {
                Debug.Log("masuk deh");
                score.AddScore(coinValue);
                audioManager.PlaySFX(audioManager.hitScoreClip);
                Destroy(collision.gameObject);
            } else
            {
                Debug.Log("gaada woi");
            }
            
        }
        else if(collision.gameObject.CompareTag("Ruby") || collision.gameObject.CompareTag("Emerald"))
        {
            score = scorePointText.gameObject.GetComponent<Score>();
            if (score != null)
            {
                Debug.Log("masuk deh jewel");
                score.AddScore(jewelValue);
                audioManager.PlaySFX(audioManager.hitScoreClip);
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("gaada woi");
            }
        }
        else if(collision.gameObject.CompareTag("Diamond"))
        {
            score = scorePointText.gameObject.GetComponent<Score>();
            if (score != null)
            {
                Debug.Log("masuk deh");
                score.AddScore(diamond);
                audioManager.PlaySFX(audioManager.hitScoreClip);
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("gaada woi");
            }
        }
    }
}
