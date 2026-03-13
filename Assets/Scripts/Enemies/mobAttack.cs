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
                    //Debug.Log("Melee attack logic");  
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
                    if (!canAttack) break; // mid-attack, do nothing

                    if (!canRanged) // ranged timer finished, takes priority
                    {
                        canAttack = false;
                        canRanged = true; // reset until next CD
                        anim.SetTrigger("RangedAttack");
                        StartCoroutine(RockBossRangedCDTimer());
                    }
                    else if (currentDistance.magnitude <= attackRange)
                    {
                        Debug.Log("Trying melee, canAttack: " + canAttack + " canRanged: " + canRanged);
                        TryAttack(); // melee when in range
                    }
                }
                break;
        }
    }

    void TryAttack()
    {
        Debug.Log("Trying to attack");

        anim.SetTrigger("Attack");
        StartCoroutine(AttackCD());
    }

    private IEnumerator AttackCD()
    {
        //Debug.Log("Attack CD started");
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    private void MeeleAttack()
    {

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
            playerHealth.TakeDamage(meleeDamage, direction);
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
