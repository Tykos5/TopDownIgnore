using UnityEngine;
using Pathfinding;

public class ZombieLogic : MonoBehaviour
{ 
    public AIPath aiPath;
    private Animator animator;

    private Vector2 movement;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement direction from AI
        movement = aiPath.desiredVelocity; // This is a Vector3

        // Send values to Animator
        animator.SetFloat("X", movement.x);
        animator.SetFloat("Y", movement.y);

        //Only start tracking if player is inside trigger collider

    }
}
