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

    public Vector2 startPos;

    public float idleDistance { get; private set; } = 1f;
    public float attackRange { get; private set; } = 1.5f;

    public float attackDuration = 1f;
    public float attackCooldown = 2f;
    public float damageCooldown = 0.5f;
    private bool canTakeDamage = true;

    public float health, maxHealth = 5;

    private Knockback kb;

    public EnemyAttack enemyAttack;


    public Vector2 MoveDirection { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        kb = GetComponent<Knockback>();

        startPos = transform.position;

        health = maxHealth;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 destination = canChase && target != null ? (Vector2)target.position : startPos;
        float distanceToDestination = Vector2.Distance(rb.position, destination);

        Move(distanceToDestination, speed, rb);

        //start zombie attack function when in range
        if (canChase && target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (enemyAttack == null)
            {
                Debug.LogWarning("EnemyAttack component is missing on the enemy.");
                return;
            }
            if (canChase && distanceToTarget <= attackRange)
            {
                //Debug.Log("In attack range, trying to attack from EnemyAI script");
                enemyAttack.TryAttack();
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
    }

    private void Move(float distance, float speed, Rigidbody2D rb)
    {
        if (kb == null || !kb.isBeingKnockedBack)
        {
            if (distance > idleDistance)
            {
                rb.MovePosition(rb.position + MoveDirection * speed * Time.fixedDeltaTime);
            }
        }
    }

    public void StartChasing(Transform chaseTarget)
    {
        canChase = true;
        target = chaseTarget;

        if (enemyAttack != null)
            enemyAttack.SetTarget(chaseTarget);
    }

    public void StopChasing()
    {
        canChase = false;
        target = null;
    }

    public void TakeDamage(float damage, Vector2 hitSource)
    {
        if (canTakeDamage)
        {
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
            else
            {
                Vector2 knockDir = (transform.position - (Vector3)hitSource).normalized;
                kb.ApplyKnockback(knockDir, rb);
            }
            StartCoroutine(DamageIFrame());
        }
    }
    private IEnumerator DamageIFrame()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    void Die()
    {
        // Implement death logic here
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
