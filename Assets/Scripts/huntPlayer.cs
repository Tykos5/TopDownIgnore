using UnityEngine;

public class HuntPlayer : MonoBehaviour
{
    public GameObject player;
    public Knockback kb;

    private Animator anim;

    public int moveSpeed = 2;
    public float targetDistance = 5f;


    public enum MovementType
    {
        Chase,      // Move toward player
        Flee,       // Move away from player
        KeepRange   // Stay at a certain distance
    }

    public MovementType movementType;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        kb = GetComponent<Knockback>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        Vector2 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        Vector2 moveDirection = Vector2.zero;

        switch (movementType)
        {
            case MovementType.Chase:
                if (distanceToPlayer > targetDistance)
                    moveDirection = directionToPlayer.normalized;
                break;

            case MovementType.Flee:
                if (distanceToPlayer < targetDistance)
                    moveDirection = -directionToPlayer.normalized;
                break;

            case MovementType.KeepRange:
                if (distanceToPlayer > targetDistance)
                    moveDirection = directionToPlayer.normalized;
                else if (distanceToPlayer < targetDistance)
                    moveDirection = -directionToPlayer.normalized;
                break;
        }

        if (!kb.isBeingKnockedBack)
            transform.position += (Vector3)(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

}