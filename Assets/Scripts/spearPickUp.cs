using UnityEngine;
using UnityEngine.Events;

public class spearPickUp : MonoBehaviour
{
    private PlayerAttack playerAttack;

    public UnityEvent pickedUp; // Event to invoke when the spear is picked up

    private void Start()
    {
        playerAttack = FindFirstObjectByType<PlayerAttack>(); // Find the PlayerAttack script in the scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StaticData.canSpear = true; // Update static data to indicate the player can use the spear
            Destroy(gameObject); // Remove the spear from the scene
            playerAttack.canSpear = true; // Allow the player to use the spear

            pickedUp.Invoke();
        }
    }
}
