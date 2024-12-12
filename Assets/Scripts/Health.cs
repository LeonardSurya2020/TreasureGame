using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 3f;
    private float MAX_HEALTH = 3f;

    public Animator[] heartAnimator;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public bool takeDamage;
    public bool isPlayer;
    public SimpleFlash flashEffect;

    private Vector2 startPosition;

    public MainMenu menu;


    public float invincibleDuration = 2f;   // Durasi invincibility
    public float blinkInterval = 0.1f;      // Interval berkedip
    private SpriteRenderer spriteRenderer;

    public int playerLayer;
    public int enemyLayer;


    private void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    public void Damage(float amount)
    {
        this.health -= amount;
        flashEffect.Flash();
        if(!isPlayer && health <=0)
        {
            Die();
        }
        //Debug.Log("ouch");

    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if(!isPlayer)
        {
            return;
        }
        foreach (Animator anim in heartAnimator)
        {
            //img.sprite = emptyHeart;
            anim.SetBool("Heart", false);
            //anim.enabled = false;
            
        }
        for (int i = 0; i < health; i++)
        {
            heartAnimator[i].enabled = true;
            heartAnimator[i].SetBool("Heart", true);
            //hearts[i].sprite = fullHeart;
        }


        if (health <= 0)
        {
            Die();
            Debug.Log("HOEEEEEEEEEEEEEEEEEEEE");
            menu.GameOverScreen();
        }
    }

    public void Respawn()
    {
        transform.position = startPosition;
    }

    public void PlayerInvincible()
    {
       
        StartCoroutine(TemporaryInvincible());
    }

    public IEnumerator TemporaryInvincible()
    {
        float elapsedTime = 0;

        while(elapsedTime < invincibleDuration)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        spriteRenderer.enabled = true;
        //Physics2D.IgnoreLayerCollision(layer1, layer2, ignoreLayer);
        //yield return new WaitForSeconds(1f);
        //ignoreLayer = false;
        //Physics2D.IgnoreLayerCollision(layer1, layer2, ignoreLayer);
    }
}
