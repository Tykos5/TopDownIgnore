using UnityEngine;

public class TriggerZombie : MonoBehaviour // handles zombies active chase boundary
{
    public EnemyAI[] zombies;

    private void OnTriggerEnter2D(Collider2D collision) // start chasing when entering zone
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var z in zombies)
                z.StartChasing(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // stop chasing when exiting zone
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var z in zombies)
                z.StopChasing();
        }
    }
}