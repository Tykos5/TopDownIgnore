using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class mobAttack : MonoBehaviour
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
    private bool canRanged = false;

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
        Melee,
        Ranged,
        RockBoss
    }

    public attackType attacktype;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

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
            case attackType.Melee:
                {
                    if (currentDistance.magnitude <= attackRange && canAttack)
                    {

                        TryAttack();
                    }
                }
                break;


            case attackType.Ranged:
                {
                    if (canAttack)
                    {
                        RangedAttack();
                    }
                }
                break;

            case attackType.RockBoss:
                {
                    // Ranged takes priority regardless of melee state
                    if (!canRanged && canAttack)
                    {
                        canAttack = false;
                        canRanged = true;
                        anim.SetTrigger("RangedAttack");
                        StartCoroutine(RockBossRangedCDTimer());
                    }
                    else if (currentDistance.magnitude <= attackRange && canAttack && canRanged)
                    {
                        canAttack = false; // add this back
                        TryAttack();
                    }
                }
                break;
        }
    }

    void TryAttack()
    {
        Debug.Log("Trying to attack");

        canAttack = false;
        anim.SetTrigger("Attack");

        StartCoroutine(AttackCD());
    }

    private IEnumerator AttackCD()
    {
        //Debug.Log("Attack CD started");
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    private void MeleeAttack()
    {
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


        if (currentDistance.magnitude <= attackRange && canHit)
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

    private IEnumerator HitCD()
    {
        canHit = false;
        yield return new WaitForSeconds(hitCD);
        canHit = true;
    }

    public void RangedAttack()
    {
        if (canAttack && attacktype == attackType.Ranged)
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

    public void RockBossRangedAttack()
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

    private IEnumerator canRangeCD()
    {
        yield return new WaitForSeconds(1);
        canRange = true;
    }

    private IEnumerator RockBossRangedCDTimer()
    {
        yield return new WaitForSeconds(RockBossRangedCD);
        canRanged = false; // signals ranged is ready to fire again
    }

    public void OnRangedAnimComplete()
    {
        canAttack = true; // free to melee again while ranged is still on cooldown
    }
}
