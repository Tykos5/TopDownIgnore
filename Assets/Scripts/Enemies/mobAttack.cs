using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class mobAttack : MonoBehaviour   // Handles attack logic for all mobs except zombie
{

    GameObject player;
    PlayerHealth playerHealth;
    private Animator anim;


    public float attackRange = 1f;
    public float meleeDamage = 1f;
    public float rangedDamage = 1f;
    public float attackCD = 2f;
    public float hitCD = 0.5f;
    public bool canHit = true;

    public float RockBossMeleeDMG = 2f;
    public float RockBossRangedCD = 5f;
    private bool rangedOnCooldown = false;

    public bool isAttacking = false;
    public bool isShooting = false;
    public bool canRange = true;

    public float projectileDuration = 5f;
    public float projectileSpeed = 3f;
    public float projectileSpawnDistance = 0.5f;
    public GameObject projectilePrefab;

    private bool canAttack = true;
    private Transform target;
    private Vector2 direction;

    private Vector2 currentDistance;

    [Header("Volume")]
    [SerializeField] private float reaperOrbVolume = 0.5f;
    [SerializeField] private float reaperMeleeVolume = 0.5f;
    [SerializeField] private float rockBossRangedVolume = 0.5f;
    [SerializeField] private float rockBossMeleeVolume = 0.5f;



    public enum attackType
    {
        Melee,  //(reaper)
        Ranged, //(reaper)
        RockBoss
    }

    public attackType attacktype;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        // set different variables to correct value based on enemytype
        if (attacktype == attackType.Melee)
        {
            meleeDamage = StaticData.meleeReaperDamage;
        }

        if (attacktype == attackType.Ranged)
        {
            rangedDamage = StaticData.rangedReaperDamage;
            projectileDuration = StaticData.orbDespawnTime;
        }

        if (attacktype == attackType.RockBoss)
        {
            meleeDamage = StaticData.RockBossMeleeDMG;
            rangedDamage = StaticData.RockBossRangedDMG;
            projectileDuration = StaticData.RockBossOrbDespawnTime;
            RockBossRangedCD = StaticData.RockBossRangedCD;
        }


    }

    void FixedUpdate()
    {
        currentDistance = player.transform.position - transform.position;

        switch (attacktype)
        {
            case attackType.Melee:      // if enemytype meleereaper, try to attack if within attackrange
                {
                    if (currentDistance.magnitude <= attackRange && canAttack)
                    {
                        TryAttack();
                    }
                }
                break;


            case attackType.Ranged: // if enemytype rangedReaper, try to attack
                {
                    if (canAttack)
                    {
                        RangedAttack();
                    }
                }
                break;

            case attackType.RockBoss:  // if rockBoss use ranged attack if cooldown is over
                {
                    // Ranged takes priority regardless of melee state
                    if (!rangedOnCooldown && canAttack)
                    {
                        canAttack = false;
                        rangedOnCooldown = true;
                        anim.SetTrigger("RangedAttack");  // animate
                        StartCoroutine(RockBossRangedCDTimer());  // ranged attack cooldown 
                    }
                    else if (currentDistance.magnitude <= attackRange && canAttack && rangedOnCooldown)  // if within meleerange and not in range attack animation
                    {
                        canAttack = false; 
                        TryAttack();            // try to attack
                    }
                }
                break;
        }
    }

    void TryAttack()    // set attack animation and start attack cooldown
    {
        canAttack = false;
        anim.SetTrigger("Attack");

        StartCoroutine(AttackCD());
    }

    private IEnumerator AttackCD() // attack cooldown, melee
    {
        //Debug.Log("Attack CD started");
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    private void MeleeAttack()  // called from animation event
    {
        // play attack sound based on enemytype

        if (attacktype == attackType.Melee)
        {
            SoundManager.instance.PlaySoundFXClip("ReaperMelee", transform, reaperMeleeVolume);
        }
        else if (attacktype == attackType.RockBoss)
        {
            SoundManager.instance.PlaySoundFXClip("RockMelee", transform, rockBossMeleeVolume);
        }

        playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.Log("playerhealth not found");
            return;
        }


        if (currentDistance.magnitude <= attackRange && canHit)   // deal damage and send direction for handling knockback if within attack range when attack goes of
        {
            direction = (player.transform.position - transform.position).normalized;

            //Debug.Log("Attacking player");
            if (attacktype == attackType.Melee)
                playerHealth.TakeDamage(meleeDamage, direction);
            else if (attacktype == attackType.RockBoss)
                playerHealth.TakeDamage(RockBossMeleeDMG, direction);
        }
        StartCoroutine(HitCD());
    }

    private IEnumerator HitCD() // iFrames for player
    {
        canHit = false;
        yield return new WaitForSeconds(hitCD);
        canHit = true;
    }

    public void RangedAttack()  // projectile attack
    {
        if (canAttack && attacktype == attackType.Ranged) // if ranged reaper, send one orb in the players direction. 
        {
            //sfx reaperOrb
            SoundManager.instance.PlaySoundFXClip("ReaperOrb", transform, reaperOrbVolume);

            direction = (player.transform.position - transform.position).normalized;

            canAttack = false;
            Debug.Log("projectile attack initiated");
            Vector2 spawnPosition = (Vector2)transform.position + direction * projectileSpawnDistance;
            GameObject intProjectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            //Shoot
            Rigidbody2D rb = intProjectile.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * projectileSpeed;

            Destroy(intProjectile, projectileDuration);
            StartCoroutine(AttackCD());
        }
    }

    public void RockBossRangedAttack()      //rockboss ranged attack, send one orb in each diagonal direction.
    {
        if (attacktype == attackType.RockBoss && canRange)
        {
            //sfx rockBossOrb
            SoundManager.instance.PlaySoundFXClip("RockOrb", transform, rockBossRangedVolume);

            canRange = false; // prevent multiple calls until CD resets

            Vector2[] diagonals = 
            {
                new Vector2(1, 1).normalized,
                new Vector2(1, -1).normalized,
                new Vector2(-1, 1).normalized,
                new Vector2(-1, -1).normalized
            };

            foreach (Vector2 dir in diagonals)
            {
                Vector2 spawnPosition = (Vector2)transform.position + dir * projectileSpawnDistance;
                GameObject intProjectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

                Rigidbody2D rb = intProjectile.GetComponent<Rigidbody2D>();
                rb.linearVelocity = dir * projectileSpeed;

                Destroy(intProjectile, projectileDuration);
            }
            StartCoroutine(canRangeCD());   
        } 
    }

    private IEnumerator canRangeCD()  // prevents multiple calls from one attack instance
    {
        yield return new WaitForSeconds(1);
        canRange = true;
    }

    private IEnumerator RockBossRangedCDTimer()  // handles time to fire again
    {
        yield return new WaitForSeconds(RockBossRangedCD);
        rangedOnCooldown = false; // signals ranged is ready to fire again
    }

    public void OnRangedAnimComplete() // called from animation event, prevents melee from going of while range animaion is ongoing
    {
        canAttack = true; // free to melee again while ranged is still on cooldown
    }
}
