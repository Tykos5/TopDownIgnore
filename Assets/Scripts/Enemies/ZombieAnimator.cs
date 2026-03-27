using System.IO;
using UnityEngine;

public class ZombieAnimator : MonoBehaviour  // handles zombies animation
{
    Animator animator;
    EnemyAI enemyAI;
    HuntPlayer huntPlayer;


    public bool isMoving = false;
    float distanceToTarget;
    float idleDis;
    Vector2 dir;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponentInParent<EnemyAI>();
        huntPlayer = GetComponentInParent<HuntPlayer>();
    }
    void FixedUpdate()
    {
        if (enemyAI != null)  // get data from enemyAI, zombies main script
        {
            dir = enemyAI.MoveDirection;
            idleDis = enemyAI.idleDistance;

            distanceToTarget = Vector2.Distance(enemyAI.transform.position, enemyAI.canChase ? (Vector3)enemyAI.target.position : (Vector3)enemyAI.startPos);
        }
        else if (huntPlayer != null) // get data from huntPlayer
        {
            Vector2 dir = huntPlayer.transform.position - transform.position;
            idleDis = huntPlayer.targetDistance;
            distanceToTarget = Vector2.Distance(huntPlayer.transform.position, transform.position);
        }
        else
        {
            return;
        }

        isMoving = distanceToTarget > idleDis;

        // set animation variables
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);

    }

    public void Attack()  // start attack animation
    {
        animator.SetTrigger("Attack");
    }
}
