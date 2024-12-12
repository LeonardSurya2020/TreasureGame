using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class enemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    public float speed;
    public float scale;
    public bool firstStart;
    public bool secondStart;

    public bool isChasing;
    public bool patrolPause;
    // Start is called before the first frame update
    void Start()
    {
        //firstStart = true;
        //secondStart = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", true);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isChasing)
        {
            if(patrolPause)
            {
                return;
            }
            
            if (firstStart)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                Vector3 localScale = transform.localScale;
                localScale.x = 1f;
                scale = localScale.x;
                transform.localScale = localScale;

            }
            else if (secondStart)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                Vector3 localScale = transform.localScale;
                localScale.x = -1f;
                scale = localScale.x;
                transform.localScale = localScale;

            }
        } 
        else
        {
            patrolPause = false;
            firstStart = false;
            secondStart = false;
        }
        //Debug.Log("curr " + currentPoint);
        //else
        //{
        //    Vector2 point = currentPoint.position - transform.position;


        //    if (currentPoint == pointB.transform)
        //    {
        //        rb.velocity = new Vector2(speed, rb.velocity.y);
        //        Vector3 localScale = transform.localScale;
        //        localScale.x = 1f;
        //        scale = localScale.x;
        //        transform.localScale = localScale;
        //    }
        //    else if (currentPoint == pointA.transform)
        //    {
        //        rb.velocity = new Vector2(-speed, rb.velocity.y);
        //        Vector3 localScale = transform.localScale;
        //        localScale.x = -1f;
        //        scale = localScale.x;
        //        transform.localScale = localScale;
        //    }

        //    if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        //    {
        //        currentPoint = pointA.transform;
        //    }
        //    if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        //    {
        //        currentPoint = pointB.transform;
        //    }
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isChasing == false)
        {
            if (collision.CompareTag("PointC") || collision.CompareTag("PointB"))
            {
                pointA = collision.gameObject;
                currentPoint = pointA.transform;
                StartCoroutine(PatrolPause());
                secondStart = false;
                firstStart = true;
            }
            else if (collision.CompareTag("PointD") || collision.CompareTag("PointA"))
            {
                pointB = collision.gameObject;
                currentPoint = pointB.transform;
                StartCoroutine(PatrolPause());
                firstStart = false;
                secondStart = true;
            }
        }
    }

    private IEnumerator PatrolPause()
    {
        if (!isChasing)
        {
            patrolPause = true;
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(1f);
            patrolPause = false;
        }
    }
}
