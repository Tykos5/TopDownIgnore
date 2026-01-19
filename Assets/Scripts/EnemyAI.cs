using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;
    private Animator animator;

    public float speed = 5f;
    public float nextWayPointDistance = 3f;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private Vector2 movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        InvokeRepeating("UpdatePath", 0.2f, 0.5f);
    }

    void UpdatePath()
    {
        if (AstarPath.active == null) return;
        if (target == null) return;

        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        reachedEndOfPath = false;

        //Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;
        //rb.AddForce(force);

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }


        // Animation (use movement direction, not velocity)
        bool isMoving = direction.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
        }
    }
}
