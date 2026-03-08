using UnityEngine;

public class TriggerZombie : MonoBehaviour
{
    public EnemyAI[] zombies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var z in zombies)
                z.StartChasing(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var z in zombies)
                z.StopChasing();
        }
    }
}