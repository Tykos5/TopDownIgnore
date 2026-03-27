using UnityEngine;

public class Level1Comp : MonoBehaviour     // Swap to next level when triggered
{
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Level Complete!");
            //Go to next level
            SceneController.instance.NextLevel();
        }
    }
}
