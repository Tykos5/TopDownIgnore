using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1;

    public Rigidbody2D attackOrigin;




    public enum projectileType
    {
        Spear,
        reaperOrb
    }

    public projectileType projectiletype;

    void Start()
    {
        attackOrigin = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI Health = collision.GetComponent<EnemyAI>();
        enemyHealth hp = collision.GetComponent<enemyHealth>();

        switch (projectiletype)
        {
            case projectileType.Spear:
                {
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
                break;
            //case projectileType.reaperOrb:
            //    {
            //        if (collision.CompareTag("Player"))
            //            {
            //            //Damage the player
            //            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            //            if (playerHealth != null)
            //            {
            //                playerHealth.TakeDamage(damage, attackOrigin.transform.position);
            //            }
            //            //Destroy the projectile
            //            Destroy(gameObject);
            //        }

            //        else if (collision.CompareTag("Wall"))
            //        {
            //            //Bounce the projectile off the wall
                        
            //        }
            //    }
            //    break;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (projectiletype != projectileType.reaperOrb)
            return;

        // Damage player and destroy
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {                 Debug.LogWarning("PlayerHealth component not found on collided object.");
                return;
            }

            if (playerHealth != null)
            {
                Vector2 knockDir =
                    (collision.transform.position - transform.position).normalized;

                playerHealth.TakeDamage(damage, knockDir);
            }

            Destroy(gameObject);
            return;
        }

        // Bounce off wall
        if (collision.collider.CompareTag("Wall"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            Vector2 normal = collision.contacts[0].normal;
            Vector2 newDirection = Vector2.Reflect(rb.linearVelocity.normalized, normal);

            rb.linearVelocity = newDirection * rb.linearVelocity.magnitude;
        }
    }
}
