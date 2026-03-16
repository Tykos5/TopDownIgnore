using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    private Rigidbody2D rb;

    private Knockback kb;

    public HealthBar healthBar;

    public float health;
    private bool canTakeDamage;
    private float damageCooldown = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = StaticData.playerHealth;
        rb = GetComponent<Rigidbody2D>();
        kb = GetComponent<Knockback>();
        canTakeDamage = true;

        if (healthBar == null)
            healthBar = FindFirstObjectByType<HealthBar>();

        healthBar.SetHealth((int)health); // syncs UI to actual health
    }

    public void TakeDamage(float damage, Vector2 direction)
    {
        if (!canTakeDamage)
            return;

        health -= damage;
        Debug.Log("Player took damage: " + health);

        healthBar.SetHealth((int)health);

        kb.ApplyKnockback(direction, rb);

        if (health <= 0)
        {
            Debug.Log("Player died");

            Died();
        }
        StartCoroutine(DamageIFrame());
    }

    private IEnumerator DamageIFrame()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        healthBar = FindFirstObjectByType<HealthBar>();
        if (healthBar != null)
            healthBar.SetHealth((int)health);
    }

    private void Died()
    {
        // save score to static data before loading death scene
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();

        // Switch to DeathScene
        SceneManager.LoadScene("DeathScene");
    }
}
