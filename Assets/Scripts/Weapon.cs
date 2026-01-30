using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1;

    [SerializeField] public Rigidbody2D attackOrigin;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemyHealth = collision.GetComponent<EnemyAI>();

        if (collision.CompareTag("Enemy"))
        {
           
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, attackOrigin.transform.position);
            }
        }
    }
}
