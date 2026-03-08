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


    void Start()
    {
        difficulty = StaticData.difficulty; // Get the difficulty from the static data

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

        else if (enemyType == EnemyType.Zombie)
        {
            foreach (GameObject zombie in zombies)
            {
                if (zombie != null)
                    zombie.SetActive(false); // Disable all zombies at the start
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
                            break;

                        case 1: // Medium
                            for (int i = 0; i < 3; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            break;

                        case 2: // Hard
                            for (int i = 0; i < 5; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            break;

                        case 3: // Insane
                            for (int i = 0; i < 7; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            break;

                        case 4:
                            for (int i = 0; i < 7; i++)
                            {
                                zombies[i].SetActive(true);
                            }
                            break;
                    }
                }
            }
        }
    }
}
