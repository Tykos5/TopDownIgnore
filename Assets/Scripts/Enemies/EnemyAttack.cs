using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    GameObject player;

    //ZombieAnimator zombieAnim;
    //HuntPlayer huntPlayer;
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
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void TryAttack()
    {
        //Debug.Log("Trying to attack");
        if (target == null)
        {
            Debug.Log("No target");
            return;
        }
        else if (!canAttack)
        {
            //Debug.Log("Cannot attack yet, on cooldown");
            return;
        }

        //Debug.Log("Attack reqs met");
        StartCoroutine(AttackCoroutine());
        SoundManager.instance.PlaySoundFXClip("zombieMelee", transform, meleeVolume);

    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;

        anim.SetTrigger("Attack");

        //zombieAnim.Attack();

        //Debug.Log("Enemy attacks!");

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void Damage()
    {
        //Debug.Log("Damage function started");
        float distance = Vector2.Distance(transform.position, target.position);
        direction = (target.position - transform.position).normalized;

        if (distance <= attackRange)
        {
            //Debug.Log("Target in range to damage");

            if (playerHealth != null)
            {
                //Debug.Log("Dealing damage to player");
                playerHealth.TakeDamage(attackDamage, direction);
            }
            else
                Debug.LogWarning("PlayerHealth component not found on player.");
        }
    }
}