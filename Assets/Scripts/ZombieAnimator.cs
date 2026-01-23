using System.IO;
using UnityEngine;

public class ZombieAnimator : MonoBehaviour
{
     Animator animator;
    EnemyAI enemyAI;

    public bool isMoving = false;
    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    void Update()
    {
        Vector2 dir = enemyAI.MoveDirection;
        float idleDis = enemyAI.idleDistance;
        
        float distanceToTarget = Vector2.Distance(enemyAI.transform.position, enemyAI.canChase ? (Vector3)enemyAI.target.position : (Vector3)enemyAI.startPos);

        isMoving = distanceToTarget > idleDis;

        animator.SetBool("isMoving", isMoving);

        animator.SetBool("isMoving", isMoving);

        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);
        
    }
}
