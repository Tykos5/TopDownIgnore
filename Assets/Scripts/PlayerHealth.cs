using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    private Rigidbody2D rb;

    private Knockback kb;

    public HealthBar healthBar;

    [SerializeField] float maxHealth = 10f;
    public float health;
    private bool canTakeDamage;
    private float damageCooldown = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        kb = GetComponent<Knockback>();
        canTakeDamage = true;

        if (healthBar == null)
            healthBar = FindFirstObjectByType<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
