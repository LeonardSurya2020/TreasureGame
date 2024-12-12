using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private GameObject player;
    private Health playerHealth;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerHealth = player.gameObject.GetComponent<Health>();

            playerHealth.health -= 1;
            audioManager.PlaySFX(audioManager.fallToLava);
            if (playerHealth.health != 0)
            {
                playerHealth.PlayerInvincible();
                playerHealth.Respawn();
            }
            
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
