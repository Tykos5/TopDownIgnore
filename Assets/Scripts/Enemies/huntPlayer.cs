using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class HuntPlayer : MonoBehaviour // ´logic for mobs chasing players
{
    public GameObject player;
    public Knockback kb;

    private Animator anim;

    public float moveSpeed = 2;
    public float targetDistance = 5f;


    public enum  MovementType
    {
        Chase,      // Move toward player
        KeepRange   // Stay at a certain distance
    }

    public MovementType movementType;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        kb = GetComponent<Knockback>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()  // Find distance to player and and calculate wanted target position
    {
        if (player == null)
            return;

        Vector2 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;



        Vector2 moveDirection = Vector2.zero;

        switch (movementType)
        {
            case MovementType.Chase:                // if chase move towards player constantly
                if (distanceToPlayer > targetDistance)
                {
                    moveDirection = directionToPlayer.normalized;
                    animate(moveDirection);
                }
                break;

            case MovementType.KeepRange:            // if keepRange move towards keeping  "targetdistance" away from player
                if (distanceToPlayer > targetDistance)
                    moveDirection = directionToPlayer.normalized;
                else if (distanceToPlayer < targetDistance)
                    moveDirection = -directionToPlayer.normalized;
                animate(directionToPlayer);
                break;
        }

        if (!kb.isBeingKnockedBack)
            transform.position += (Vector3)(moveDirection * moveSpeed * Time.fixedDeltaTime); // Move entity if not being knocked back

        
    }
    void animate(Vector2 vec)  // set the animation variables for correct animation direction.
    {
        // Update animation parameters
        anim.SetFloat("X", vec.x);
        anim.SetFloat("Y", vec.y);
    }
}