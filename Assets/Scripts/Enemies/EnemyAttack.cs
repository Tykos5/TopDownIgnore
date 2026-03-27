using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour    //handles attack logic for ZOMBIE   
{
    GameObject player;
    PlayerHealth playerHealth;
    private Animator anim;

    public float attackRange = 1.5f;
    public float attackDamage = 1f;
    public float attackCooldown = 2f;

    private bool canAttack = true;
    private Transform target;
    private Vector2 direction;

    [SerializeField] private float meleeVolume = 0.5f;

    //private Animator anim;

    private void Start()
    {
        attackDamage = StaticData.zombieDamage; // Get the zombie damage from static data

        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>(); // Find playerHealth script
        }
    }
    public void SetTarget(Transform newTarget) // set the target
    {
        target = newTarget;
    }

    public void TryAttack() // when in attack range to to attack
    {
        if (target == null)     
        {
            Debug.Log("No target");
            return;
        }
        else if (!canAttack)  // if attack on cooldown cancel attack
        {
            return;
        }

        StartCoroutine(AttackCoroutine()); // start attacking
        SoundManager.instance.PlaySoundFXClip("ZombieMelee", transform, meleeVolume); // play attack sound

    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;  //handles attackCooldown

        anim.SetTrigger("Attack"); // set animation

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void Damage() // function to damage the player, called from animation event.
    {
        float distance = Vector2.Distance(transform.position, target.position);
        direction = (target.position - transform.position).normalized;

        if (distance <= attackRange)        //if still within attackrange
        {

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, direction);  // damage player if all reqs are met
            }
            else
                Debug.LogWarning("PlayerHealth component not found on player.");
        }
    }
}