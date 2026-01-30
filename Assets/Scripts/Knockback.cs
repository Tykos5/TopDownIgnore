using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool isBeingKnockedBack { get; private set; }

    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    public void ApplyKnockback(Vector2 direction, Rigidbody2D rb)
    {
        StartCoroutine(KnockbackCoroutine(direction, rb));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, Rigidbody2D rb)
    {
        isBeingKnockedBack = true;

        rb.linearVelocity = direction * knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        isBeingKnockedBack = false;
    }
}