using UnityEngine;

public class spearPickUp : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = FindFirstObjectByType<PlayerAttack>(); // Find the PlayerAttack script in the scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Remove the spear from the scene
            playerAttack.canSpear = true; // Allow the player to use the spear
        }
    }
}
