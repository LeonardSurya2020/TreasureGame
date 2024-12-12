using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public GameObject playerObject;

    [Header("Physics")]
    public float speed = 200f, jumpForce = 100f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true, isJumping, isInAir;
    public bool directionLookEnabled = true;
    private enemyPatrol enemyPatrol;

    [SerializeField] Vector3 startOffset;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] public RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;
    private bool isOnCoolDown;

    public bool groundTypeEnemy;
    public bool isKnocked;
 

    public void Start()
    {
        // mencari GameObject dengan Tag Player
        playerObject = GameObject.FindGameObjectWithTag("Player");
        target = playerObject.transform; // set target sebagai player.transformation.position;

        // untuk mencari path
        seeker = GetComponent<Seeker>();

        rb = GetComponent<Rigidbody2D>();
        isJumping = false;
        isInAir = false;
        isOnCoolDown = false;
        enemyPatrol = GetComponent<enemyPatrol>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if(isKnocked)
        {
            return;
        }
        if (TargetInDistance() && followEnabled )
        {
            PathFollow();
        }
        else
        {
            enemyPatrol.isChasing = false;
            
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);

        }
    }

    private void PathFollow()
    {
        enemyPatrol.isChasing = true;
        activateDistance = 10f;

        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;


        // Jump
        if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                if (isInAir) return;
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                StartCoroutine(JumpCoolDown());

            }
        }
        if (isGrounded)
        {
            isJumping = false;
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }

        // Movement

        if(!isKnocked)
        {
            if(groundTypeEnemy)
            {
                rb.velocity = new Vector2(force.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(force.x, force.y);
            }
            
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                
            }
        }
    }

    private bool TargetInDistance()
    {
        if (target == null)
        {
            return false;
        }
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;

    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemyPatrol.isChasing)
        {
            if (collision.CompareTag("PointA") && collision.transform.position.x < transform.position.x)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                rb.velocity = new Vector2(rb.velocity.x * 5, jumpForce);
                followEnabled = false;
                StartCoroutine(FollowControl());
            }
            else if (collision.CompareTag("PointB") && collision.transform.position.x < transform.position.x)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                rb.velocity = new Vector2(rb.velocity.x * 5, jumpForce);
                followEnabled = false;
                StartCoroutine(FollowControl());
            }
        }


    }

    public IEnumerator FollowControl()
    {
        
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector2.zero;
        followEnabled = true;
    }

}
