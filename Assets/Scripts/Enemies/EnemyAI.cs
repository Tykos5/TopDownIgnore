using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Windows.WebCam;
using UnityEngine.Events;
using UnityEditor.PackageManager.Requests;

// This Class handles most logic for the ZOMBIE enemy except for the attack.

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public bool canChase = false;

    public float speed = 5f;
    
    Rigidbody2D rb;

    public Vector2 startPos;

    [SerializeField] private float deathVolume = 0.5f;
    [SerializeField] private float hitVolume = 0.5f;


    public float idleDistance { get; private set; } = 1f;
    public float attackRange { get; private set; } = 1.5f;

    public float attackDuration = 1f;
    public float attackCooldown = 2f;
    public float damageCooldown = 0.5f;
    private bool canTakeDamage = true;

    public float health, maxHealth = 5;

    private Knockback kb;

    public EnemyAttack enemyAttack;

    public UnityEvent onEnemyDeath;

    public ScoreManager scoreManager;

    public Vector2 MoveDirection { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        kb = GetComponent<Knockback>();

        startPos = transform.position; // save starting position

        maxHealth = StaticData.zombieHealth; // Get the zombie health from static data

        health = maxHealth; // set health
    }


    void FixedUpdate()
    {
        // Calculate distance to target
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
            if (canChase && distanceToTarget <= attackRange)  // try to attack if in range and within zombie boundary
            {
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
        else  // if player is out of boundary target becomes its starting position
        {
            MoveDirection = (startPos - (Vector2)transform.position).normalized;
        }
    }

    private void Move(float distance, float speed, Rigidbody2D rb)  // Move the zombie if its not being knocked back
    {
        if (kb == null || !kb.isBeingKnockedBack)
        {
            if (distance > idleDistance)
            {
                rb.MovePosition(rb.position + MoveDirection * speed * Time.fixedDeltaTime);
            }
        }
    }

    public void StartChasing(Transform chaseTarget) // Set taget to player if within boundary
    {
        canChase = true;
        target = chaseTarget;

        if (enemyAttack != null)
            enemyAttack.SetTarget(chaseTarget);
    }

    public void StopChasing() // when outside boundary no target, moves towards startPos
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
                onEnemyDeath.Invoke(); // Enemy death event to call that a zombie died

                SoundManager.instance.PlaySoundFXClip("ZombieDeath", transform, deathVolume); // zombie death sound

                Die();
            }
            else
            {
                SoundManager.instance.PlaySoundFXClip("ZombieHit", transform, hitVolume); // Zombie hit sound
                Vector2 knockDir = (transform.position - (Vector3)hitSource).normalized;    // Set knockback Direction
                kb.ApplyKnockback(knockDir, rb);
            }
            StartCoroutine(DamageIFrame()); // Immunity time after being hit
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
        scoreManager = FindFirstObjectByType<ScoreManager>();
        scoreManager.score += 10; // Add 10 points for killing zombie

        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
