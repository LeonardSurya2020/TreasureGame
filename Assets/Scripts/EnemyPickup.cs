using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPickup : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;
    public float throwForce = 5f;
    //public Vector3 direction {  get; set; }

    public enemyPatrol enemy;
    public EnemyAI enemyAI;

    private GameObject itemHolding;
    private PlayerMovement playerMov;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            Collider2D pickUpItem = Physics2D.OverlapCircle(transform.position, .4f, pickUpMask);
            playerMov = collision.gameObject.GetComponent<PlayerMovement>();
            if (pickUpItem)
            {
                audioManager.PlaySFX(audioManager.pickUpClip);
               playerMov.knockBacked = true;
               enemyAI.isKnocked = true;
               enemy.isChasing = true;
               itemHolding = pickUpItem.gameObject;
               itemHolding.transform.position = holdSpot.position;
               itemHolding.transform.parent = transform;
               if (itemHolding.GetComponent<Rigidbody2D>())
               {
                   itemHolding.GetComponent<Rigidbody2D>().simulated = false;
               }
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (itemHolding)
        {
            StartCoroutine(ThrowTimer(itemHolding));

        }
    }

    public IEnumerator ThrowTimer(GameObject item)
    {
        yield return new WaitForSeconds(1f);
        ThrowItem(item);
        yield return new WaitForSeconds(0.5f);
        playerMov.knockBacked = false;
        enemyAI.isKnocked = false;
        enemy.isChasing = false;
    }

    private void ThrowItem(GameObject item)
    {
        item.transform.parent = null;
        audioManager.PlaySFX(audioManager.throwPickup);
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 thrownDirec = Vector2.zero;
            rb.simulated = true;
            rb.gravityScale = 40;
            float direction = enemyAI.transform.localScale.x;

            if (direction < 0)
            {
                thrownDirec = Vector2.left;
            }
            else if (direction > 0)
            {
                thrownDirec = Vector2.right;
            }


            rb.velocity = thrownDirec * throwForce;
        }

        itemHolding = null;
    }


}
