using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public PlayerHealth playerHealth;

    public ScoreManager scoreManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>(); // re-find in current scene

        StaticData.playerHealth = playerHealth.health;  //save player health to static data before loading next scene
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        scoreManager = FindFirstObjectByType<ScoreManager>(); // re-find in current scene
        StaticData.score = scoreManager.score; //save score to static data before loading next scene
    }

    public void LoadScene(string sceneName)
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>(); // re-find in current scene

        StaticData.playerHealth =  playerHealth.health;
        SceneManager.LoadSceneAsync(sceneName);

        scoreManager = FindFirstObjectByType<ScoreManager>(); // re-find in current scene
        StaticData.score = scoreManager.score; //save score to static data before loading next scene
    }

    public void Victory()
    {
        StartCoroutine(VictoryCoroutine());
    }

    private IEnumerator VictoryCoroutine()
    {

        yield return new WaitForSeconds(3f);
        StaticData.score = scoreManager.score; //save score to static data before loading next scene

        if (playerHealth.health > 0)
        {
            SceneManager.LoadScene("VictoryMenu");
        }
    }
}
