using UnityEngine;

public class Weapon : MonoBehaviour // handles player melee collision
{
    public float damage = 1;

    [SerializeField] public Rigidbody2D attackOrigin;

    void Start()
    {   
        attackOrigin = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) // on collision deal damage to correct mob and deal damage
    {
        enemyHealth health = collision.GetComponent<enemyHealth>();

        EnemyAI HP = collision.GetComponent<EnemyAI>();

        if (collision.CompareTag("Reaper"))
        {
           
            if (health != null)
            {
                health.TakeDamage(damage, attackOrigin.transform.position);
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (HP != null)
            {
                HP.TakeDamage(damage, attackOrigin.transform.position);
            }
        }
        else if (collision.CompareTag("RockBoss"))
        {
            if (health != null)
            {
                health.TakeDamage(damage, attackOrigin.transform.position);
            }
        }
    }
}
