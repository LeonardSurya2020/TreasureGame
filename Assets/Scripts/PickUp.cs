using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdSpot;
    public LayerMask pickUpMask;
    public float throwForce = 5f;
    //public Vector3 direction {  get; set; }

    public PlayerMovement player;

    private GameObject itemHolding;
    public Rigidbody2D playerRB;

    private bool isDiamond;
    private float temporaryWeight;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (itemHolding)
            {
                audioManager.PlaySFX(audioManager.throwPickup);
                ThrowItem(itemHolding);
            }
            else
            {
               Collider2D pickUpItem = Physics2D.OverlapCircle(transform.position, .4f, pickUpMask);
                if(pickUpItem)
                {
                    itemHolding = pickUpItem.gameObject;
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;
                    audioManager.PlaySFX(audioManager.pickUpClip);
                    if(itemHolding.GetComponent<Rigidbody2D>())
                    {
                        if (itemHolding.CompareTag("Diamond"))
                        {
                            temporaryWeight = itemHolding.GetComponent<Rigidbody2D>().gravityScale;
                            Debug.Log("ini diamond");
                            player.isCarry = true;
                            playerRB.gravityScale += temporaryWeight;
                            isDiamond = true;

                        }

                        itemHolding.GetComponent <Rigidbody2D>().simulated = false;
                    }
                }
            }
        }
    }

    private void ThrowItem(GameObject item)
    {
        item.transform.parent = null;

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            rb.simulated = true;
            Vector2 thrownDirec = Vector2.zero;
            
            float direction = player.scale;

            if(direction < 0)
            {
                thrownDirec = Vector2.left;
            }
            else if(direction > 0)
            {
                thrownDirec  = Vector2.right;
            }

            //rb.AddForce(thrownDirec * throwForce, ForceMode2D.Impulse);
            DecreaseWeight();
            rb.velocity = thrownDirec * throwForce;
            
        }
        else
        {
            Debug.Log("hihiha");
        }

        itemHolding = null;
    }

    private void DecreaseWeight()
    {
        if(isDiamond)
        {
            playerRB.gravityScale -= temporaryWeight;
            isDiamond = false;
        }
        
    }

}
