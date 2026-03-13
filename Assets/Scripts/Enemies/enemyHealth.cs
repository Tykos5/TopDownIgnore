using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class enemyHealth : MonoBehaviour
{
    public float health, maxHealth;

    public float damageCooldown = 0.5f;
    public bool canTakeDamage = true;


    public GameObject Owner;

    private Knockback kb;
    private Rigidbody2D rb;

    public UnityEvent onEnemyDeath;

    void Start()
    {
        kb = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();

        // get max health from static data based on enemy type
        if (enemyType == EnemyType.Zombie)
        {
            maxHealth = StaticData.zombieHealth;
        }
        else if (enemyType == EnemyType.MeleeReaper)
        {
            maxHealth = StaticData.meleeReaperHealth; 
        }
        else if (enemyType == EnemyType.RangedReaper)
        {
            maxHealth = StaticData.rangedReaperHealth; 
        }
        else if (enemyType == EnemyType.RockBoss)
        {
            maxHealth = StaticData.rockBossHealth;
        }
        health = maxHealth;
    }

    public enum EnemyType
    {
        Zombie,
        MeleeReaper,
        RangedReaper,
        RockBoss
    }
    public EnemyType enemyType;


    public void TakeDamage(float damage, Vector2 hitSource) 
    {
        if (canTakeDamage)
        {
            Debug.Log("Enemy took damage: " + health);

            health -= damage;

            if (health <= 0)
            {
                // Call for dissapearing wall do dissapear and gate to open
                onEnemyDeath.Invoke();

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
