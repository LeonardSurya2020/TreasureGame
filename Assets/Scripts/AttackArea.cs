using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public float damage = 1;
    private Health health;
    private PlayerMovement playerMovement;
    private EnemyAI enemyAI;
    public float throwForce = 1000f;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("FlyingEnemy"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            health.Damage(damage);
            EnemyAI enemyMov = collision.gameObject.GetComponent<EnemyAI>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb)
            {
                
                rb.velocity = Vector2.zero;
                Vector2 thrownDirec = Vector2.zero;
                rb.simulated = true;
                if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x)
                {
                    thrownDirec = new Vector2(-1, 1);
                }
                else if (collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
                {
                    thrownDirec = new Vector2(1, 1);
                }
                else if (collision.gameObject.transform.position.y > this.gameObject.transform.position.y)
                {
                    thrownDirec = new Vector2(1, 1);
                }

                StartCoroutine(ApplyKnockBackEnemy(rb, thrownDirec, enemyMov));
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //Debug.Log("kenak player");
            collision.gameObject.GetComponent<Health>().Damage(damage);

            PlayerMovement playermov = collision.gameObject.GetComponent<PlayerMovement>();

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                playermov.knockBacked = true;
                //Debug.Log("tidak null");
                Vector2 thrownDirec = Vector2.zero;
                rb.simulated = true;
                if (collision.gameObject.transform.position.x < this.gameObject.transform.position.x) 
                {
                    thrownDirec = new Vector2(-1, 1);
                }
                else if(collision.gameObject.transform.position.x > this.gameObject.transform.position.x)
                {
                    thrownDirec = new Vector2(1,1);
                }
                else if (collision.gameObject.transform.position.y > this.gameObject.transform.position.y)
                {
                    thrownDirec = new Vector2(1, 1);
                }

                    StartCoroutine(ApplyKnockback(rb, thrownDirec, playermov));
                collision.gameObject.GetComponent<Health>().PlayerInvincible();
                


            }
        }

    }

    private IEnumerator ApplyKnockBackEnemy(Rigidbody2D rb, Vector2 direction, EnemyAI enemyMov)
    {
        enemyMov.followEnabled = false;
        rb.velocity = Vector2.zero; // Atur kecepatan ke nol
                                    //rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        audioManager.PlaySFX(audioManager.enemyHit);
        rb.velocity = direction * throwForce;


        yield return new WaitForSeconds(0.5f); // Tunggu sejenak agar knockback terasa
        enemyMov.isKnocked = false;
        enemyMov.followEnabled = true;
    }

    private IEnumerator ApplyKnockback(Rigidbody2D rb, Vector2 direction, PlayerMovement playermov)
    {
        rb.velocity = Vector2.zero; // Atur kecepatan ke nol
                                    //rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        audioManager.PlaySFX(audioManager.hitClip);
        rb.velocity = direction * throwForce;


        yield return new WaitForSeconds(0.1f); // Tunggu sejenak agar knockback terasa
        playermov.knockBacked = false;
    }
}
