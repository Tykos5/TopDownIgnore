using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public enum EnemyType
    { 
        Zombie, 
        Reaper 
    }

    public enum level
    {
        Level1,
        Level2,
        Level3
    }

    public int difficulty;

    public level currentLevel;
    public EnemyType enemyType;

    [Header("Reaper Enemies")]
    public GameObject[] meleeReapers;
    public GameObject[] rangedReapers;

    [Header("Zombie Enemies")]
    public GameObject[] zombies;

    public int zombiesSpawned; // To track how many zombies spawned
    public int zombiesKilled; // To track how many zombies killed

    public GameObject[] doorSpawn; // Array to hold all game objects for easy management


    void Start()
    {
        zombiesKilled = 0; // Initialize zombies killed to 0

        difficulty = StaticData.difficulty; // Get the difficulty from the static data

        // Disable all enemies at the start of the level

        // Reaper disable
        if (enemyType == EnemyType.Reaper) 
        { 
            foreach (GameObject meleeReaper in meleeReapers)
            {
                if (meleeReaper != null)
                    meleeReaper.SetActive(false); // Disable all melee reapers at the start
            }
            foreach (GameObject rangedReaper in rangedReapers)
            {
                if (rangedReaper != null)
                    rangedReaper.SetActive(false); // Disable all ranged reapers at the start
            }
        }

        // Zombie disable
        else if (enemyType == EnemyType.Zombie)
        {
            foreach (GameObject zombie in zombies)
            {
                if (zombie != null)
                    zombie.SetActive(false); // Disable all zombies at the start
            }
        }


        if (currentLevel == level.Level2 && enemyType == EnemyType.Zombie)
        {
            switch (difficulty)
            {
                case 0: // Easy
                    for (int i = 0; i < 2; i++)
                    {
                        zombies[i].SetActive(true);
                    }
                    zombiesSpawned = 2;
                    break;

                case 1: // Medium
                    for (int i = 0; i < 4; i++)
                    {
                        zombies[i].SetActive(true);
                    }
                    zombiesSpawned = 4;
                    break;

                case 2: // Hard
                    for (int i = 0; i < 6; i++)
                    {
                        zombies[i].SetActive(true);
                    }
                    zombiesSpawned = 6;
                    break;

                case 3: // Insane
                    for (int i = 0; i < 8; i++)
                    {
                        zombies[i].SetActive(true);
                    }
                    zombiesSpawned = 8;
                    break;

                case 4:
                    for (int i = 0; i < 8; i++)
                    {
                        zombies[i].SetActive(true);
                    }
                    zombiesSpawned = 8;
                    break;
            }
        }

    }
 
    private bool activated = false; // To ensure enemies are spawned only once


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //spawn the reapers on trigger with the player
        if (collision.CompareTag("Player") && !activated)
        {
            activated = true;
            if (enemyType == EnemyType.Reaper)
            {
                foreach (GameObject meleeReaper in meleeReapers)
                {
                    if (meleeReaper != null)
                        meleeReaper.SetActive(true); // Enable all melee reapers
                }
                foreach (GameObject rangedReaper in rangedReapers)
                {
                    if (rangedReaper != null)
                        rangedReaper.SetActive(true); // Enable all ranged reapers
                }
            }

            else if (enemyType == EnemyType.Zombie)
            {
                if (currentLevel == level.Level1)
                {

                    switch (difficulty)
                    {
                        case 0: // Easy
                            for (int i = 0; i < 1; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            zombiesSpawned = 1;
                            break;

                        case 1: // Medium
                            for (int i = 0; i < 3; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            zombiesSpawned = 3;
                            break;

                        case 2: // Hard
                            for (int i = 0; i < 5; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            zombiesSpawned = 5;
                            break;

                        case 3: // Insane
                            for (int i = 0; i < 7; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            zombiesSpawned = 7;
                            break;

                        case 4:
                            for (int i = 0; i < 7; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            zombiesSpawned = 7;
                            break;
                    }
                }
            }
        }
    }

    public void zombieKilled()
    {
        zombiesKilled++;

        if (zombiesKilled >= zombiesSpawned)
        {
            // All zombies have been killed, you can perform any additional actions here
            Debug.Log("All zombies have been killed!");

            if (currentLevel == level.Level1)
            {
                doorSpawn[0].SetActive(true); // Enable the door
                doorSpawn[1].SetActive(false); // Disable the wall
            }
            
        }
    }
}
