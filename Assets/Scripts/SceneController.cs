using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour        //handles the logic for micellenious functions
{
    public static SceneController instance;

    public PlayerHealth playerHealth;

    public ScoreManager scoreManager;

    [SerializeField] private float victoryVolume = 0.5f;
    [SerializeField] private float deathVolume = 0.5f;

    private void Awake()
    {   
        // to not destroy the object when swapping scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // play death sound if DeathScene is loaded
        if (SceneManager.GetActiveScene().name == "DeathScene")
        {
            SoundManager.instance.PlaySoundFXClip("PlayerDeath", transform, deathVolume);
        }
    }

    public void NextLevel() // Load all variables needed when loading a new scene
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>(); // re-find in current scene
        StaticData.playerHealth = playerHealth.health;  //save player health to static data before loading next scene

        scoreManager = FindFirstObjectByType<ScoreManager>(); // re-find in current scene
        StaticData.score = scoreManager.score; //save score to static data before loading next scene

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName) // load specific scene
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>(); // re-find in current scene
        StaticData.playerHealth =  playerHealth.health;

        scoreManager = FindFirstObjectByType<ScoreManager>(); // re-find in current scene
        StaticData.score = scoreManager.score; //save score to static data before loading next scene

        SceneManager.LoadSceneAsync(sceneName);
    }

    public void Victory()
    {
        StartCoroutine(VictoryCoroutine());
    }

    private IEnumerator VictoryCoroutine()  //Handles victory sound and scece swapping
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>(); // re-find in current scene
        scoreManager = FindFirstObjectByType<ScoreManager>(); // re-find in current scene

        yield return new WaitForSeconds(1f); //wait for victory sound to play
        SoundManager.instance.PlaySoundFXClip("Victory", transform, victoryVolume);


        yield return new WaitForSeconds(2f);
        StaticData.score = scoreManager.score; //save score to static data before loading next scene

        if (playerHealth.health > 0)
        {
            
            SceneManager.LoadScene("VictoryMenu");
        }
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DeathScene")
        {
            SoundManager.instance.PlaySoundFXClip("PlayerDeath", transform, deathVolume);
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
