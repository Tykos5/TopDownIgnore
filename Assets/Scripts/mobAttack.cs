using UnityEngine;

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



    void Start()
    {
        
    }

   
    void FixedUpdate()
    {
     
        
    }
}
