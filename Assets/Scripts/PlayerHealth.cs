using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    private Rigidbody2D rb;

    private Knockback kb;

    [SerializeField] float health = 10f;
    private bool canTakeDamage;
    private float damageCooldown = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        kb = GetComponent<Knockback>();
        canTakeDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        health -= damage;
        Debug.Log("Player took damage: " + health);

        kb.ApplyKnockback(direction, rb);

        if (health <= 0)
        {
            Debug.Log("Player died");
        }
        StartCoroutine(DamageIFrame());
    }

    private IEnumerator DamageIFrame()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

}
