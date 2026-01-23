using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Windows.WebCam;

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public bool canChase = false;

    public float speed = 5f;
    
    Rigidbody2D rb;

    private Vector2 movement;

    public Vector2 startPos;


    public float idleDistance { get; private set; } = 1f;
    public float attackDistance { get; private set; } = 1.5f;


    public Vector2 MoveDirection { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 destination = canChase && target != null ? (Vector2)target.position : startPos;
        float distanceToDestination = Vector2.Distance(rb.position, destination);

        // Only move if outside idleDistance
        if (distanceToDestination > idleDistance)
        {
            rb.MovePosition(rb.position + MoveDirection * speed * Time.fixedDeltaTime);
        }


        //start zombie attack function when in range
        if (canChase && target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (distanceToTarget <= attackDistance)
            {
                // Attack the player
                 Debug.Log("Zombie Attacking");

            }
        }
    }

    private void Update()
    {
        //get zombie position and direction to player
        if (canChase && target != null)
        {
            MoveDirection = (target.position - transform.position).normalized;
        }
        else
        {
            MoveDirection = (startPos - (Vector2)transform.position).normalized;
        }

        //move zombie towards player
        movement = MoveDirection;
        
    }
    public void StartChasing(Transform chaseTarget)
    {
        canChase = true;
        target = chaseTarget;
    }

    public void StopChasing()
    {
        canChase = false;
        target = null;
    }
}
