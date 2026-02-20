using System.IO;
using UnityEngine;

public class ZombieAnimator : MonoBehaviour
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
    void Update()
    {
        if (enemyAI != null)
        {
            Vector2 dir = enemyAI.MoveDirection;
            float idleDis = enemyAI.idleDistance;

            float distanceToTarget = Vector2.Distance(enemyAI.transform.position, enemyAI.canChase ? (Vector3)enemyAI.target.position : (Vector3)enemyAI.startPos);
        }
        else if (huntPlayer != null)
        {
            Vector2 dir = huntPlayer.transform.position - transform.position;
            float idleDis = huntPlayer.targetDistance;
            float distanceToTarget = Vector2.Distance(huntPlayer.transform.position, transform.position);
        }
        else
        {
            return;
        }

        isMoving = distanceToTarget > idleDis;

        animator.SetBool("isMoving", isMoving);

        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);

    }

    public void Attack()
    {
        //Debug.Log("Zombie Attack Animation Triggered");
        animator.SetTrigger("Attack");
    }
}
