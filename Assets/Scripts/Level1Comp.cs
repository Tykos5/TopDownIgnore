using UnityEngine;

public class Level1Comp : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Level 1 Complete!");
            //Go to level 2
            SceneController.instance.NextLevel();

        }
    }
}
