using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.VisualScripting;
using System.Dynamic;

public class mobAttack : MonoBehaviour
{

    GameObject player;
    PlayerHealth playerHealth;
    private Animator anim;

    public float attackRange = 1f;
    public float attackDamage = 1f;
    public float attackCD = 2f;

    private bool canAttack = true;
    private Transform target;
    private Vector2 direction;

    private Vector2 currentDistance;


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

   
    void FixedUpdate()
    {
        currentDistance = player.transform.position - transform.position;

        if (currentDistance.magnitude <= attackRange && canAttack)
        {
           TryAttack();
        }
    }

    void TryAttack()
    {
        canAttack = false;
        Debug.Log("Trying to attack");
        anim.SetTrigger("Attack");
        StartCoroutine(AttackCD());
    }

    private IEnumerator AttackCD()
    {
        Debug.Log("Attack CD started");
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    private void Attack()
    {

        playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.Log("playerhealth not found");
            return;
        }

        if (currentDistance.magnitude <= attackRange)
        {
            Debug.Log("Attacking player");
            playerHealth.TakeDamage(attackDamage, transform.position);
        }
        
    }
}
