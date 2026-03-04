using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1;

    public Rigidbody2D attackOrigin;

    private Rigidbody2D rb;





    public enum projectileType
    {
        Spear,
        reaperOrb
    }

    public projectileType projectiletype;

    private Vector2 lastVelocity;


    void Start()
    {
        attackOrigin = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // capture velocity BEFORE collision resolution
    private void FixedUpdate()
    {
        if (projectiletype == projectileType.reaperOrb)
        {
           
            if (rb.linearVelocity.sqrMagnitude > 0.01f)
                lastVelocity = rb.linearVelocity;

            //slowly rotate the orb
            rb.rotation += 5f;
        }


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
            {
                Debug.LogWarning("PlayerHealth component not found on collided object.");
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

        if (collision.collider.CompareTag("Wall"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;

            // Use lastVelocity instead of rb.velocity — it's clean pre-collision data
            float speed = lastVelocity.magnitude;
            Vector2 newDirection = Vector2.Reflect(lastVelocity.normalized, normal);

            // Push out along normal to escape the wall geometry
            transform.position += (Vector3)(normal * 0.05f);

            rb.linearVelocity = newDirection * speed;
        }
    }
}
