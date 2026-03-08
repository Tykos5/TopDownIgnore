using System.Collections;
using System.Dynamic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mobAttack : MonoBehaviour
{

    GameObject player;
    PlayerHealth playerHealth;
    private Animator anim;


    public float attackRange = 1f;
    public float attackDamage = 1f;
    public float attackCD = 2f;
    public float hitCD = 0.5f;
    public bool canHit = true;

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
        Ranged
    }

    public attackType attacktype;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        }
        
    }

    void TryAttack()
    {
        canAttack = false;
        //Debug.Log("Trying to attack");
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
            playerHealth.TakeDamage(attackDamage, direction);
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
        if (canAttack)
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
}
