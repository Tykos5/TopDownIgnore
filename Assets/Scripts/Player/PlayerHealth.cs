using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour // handles players health and hp UI
{

    private Rigidbody2D rb;

    private Knockback kb;

    public HealthBar healthBar;

    public float health;
    private bool canTakeDamage;
    private float damageCooldown = 0.5f;

    [SerializeField] private float playerHitVolume = 0.5f;

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

    public void TakeDamage(float damage, Vector2 direction) // takedamage called from enemy scripts.
    {
        if (!canTakeDamage)
            return;

        SoundManager.instance.PlaySoundFXClip("PlayerHit", transform, playerHitVolume); // hit sfx

        health -= damage;
        Debug.Log("Player took damage: " + health);

        healthBar.SetHealth((int)health); // sync UI to current health

        kb.ApplyKnockback(direction, rb);

        if (health <= 0)
        {
            Debug.Log("Player died");

            Died();
        }
        StartCoroutine(DamageIFrame()); 
    }

    private IEnumerator DamageIFrame() // damage iFrames
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // find the health bar for every new scene
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
