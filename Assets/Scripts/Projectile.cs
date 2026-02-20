using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1;

    public Rigidbody2D attackOrigin;

    void Start()
    {
        attackOrigin = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI Health = collision.GetComponent<EnemyAI>();

        enemyHealth hp = collision.GetComponent<enemyHealth>();

        if (collision.CompareTag("Enemy"))
        {
            //Damage the enemy
            if (Health != null)
            {
                Health.TakeDamage(damage, attackOrigin.transform.position);
            }

            //Destroy the projectile
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Reaper"))
        {
            //Damage the reaper
            if (hp != null)
            {
                hp.TakeDamage(damage, attackOrigin.transform.position);
            }
            //Destroy the projectile
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Wall"))
        {
            //Destroy the projectile
            Destroy(gameObject);
        }

    }
}
