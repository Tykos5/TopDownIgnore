using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class enemyHealth : MonoBehaviour  // Handles health for all mobs except zombie.
{
    public float health, maxHealth;

    public float damageCooldown = 0.5f;
    public bool canTakeDamage = true;

    [SerializeField] private float reaperDeathVolume = 0.5f;
    [SerializeField] private float rockDeathVolume = 0.5f;


    public GameObject Owner;

    private Knockback kb;
    private Rigidbody2D rb;

    public UnityEvent onEnemyDeath;

    public ScoreManager scoreManager;

    public SceneController sceneController;

    void Start()
    {
        kb = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();

        sceneController = FindFirstObjectByType<SceneController>();

        // get max health from static data based on enemy type
        if (enemyType == EnemyType.MeleeReaper)
        {
            maxHealth = StaticData.meleeReaperHealth;
        }
        else if (enemyType == EnemyType.RangedReaper)
        {
            maxHealth = StaticData.rangedReaperHealth;
        }
        else if (enemyType == EnemyType.RockBoss)
        {
            maxHealth = StaticData.rockBossHealth;
        }
        health = maxHealth; // set health of owner to the correct int
    }

    public enum EnemyType  // enum to handle logic for different mobs.
    {
        MeleeReaper,
        RangedReaper,
        RockBoss
    }
    public EnemyType enemyType;


    public void TakeDamage(float damage, Vector2 hitSource)  // Called from player weapon script
    {
        if (canTakeDamage) // if not in iFrame
        {
            health -= damage;

            Debug.Log("Enemy took damage: " + health);

            if (health <= 0)
            {
                Die();

                onEnemyDeath.Invoke();
            }
            else // find hit source and apply knockback
            {
                Vector2 knockDir = (transform.position - (Vector3)hitSource).normalized; 
                kb.ApplyKnockback(knockDir, rb);
            }
            StartCoroutine(DamageIFrame()); // immunityFrames
        }
    }
    private IEnumerator DamageIFrame()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    void Die()
    {
        Debug.Log("Enemy Died");

        scoreManager = FindFirstObjectByType<ScoreManager>();

        //Add score to score scoremanager based on enemyType
        if (scoreManager != null)
        {
            switch (enemyType)
            {
                case EnemyType.MeleeReaper:
                    scoreManager.score += 20;
                    break;
                case EnemyType.RangedReaper:
                    scoreManager.score += 15;
                    break;
                case EnemyType.RockBoss:
                    scoreManager.score += 100;
                    break;
            }
        }

        if (enemyType == EnemyType.MeleeReaper || enemyType == EnemyType.RangedReaper) // play reaperDeath sound if a reaper dies
        {
            Debug.Log("Playing Reaper Death Sound");
            SoundManager.instance.PlaySoundFXClip("ReaperDeath", transform, reaperDeathVolume);
        }


        if (enemyType == EnemyType.RockBoss) 
        {
            // play rockBoss death sound
            SoundManager.instance.PlaySoundFXClip("RockDeath", transform, rockDeathVolume);

            // After 3 seconds, load VictoryMenu scene
            sceneController.Victory();

        }
        Destroy(gameObject);
    }
}