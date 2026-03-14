using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonUI : MonoBehaviour
{
    public TMP_Text difficultyText;

    public enum buttonType
    {
        Difficulty
    }

    // Swap to level 1
    public void NewGameButton()
    {
        // Set the difficulty in static data
        StaticData.difficulty = difficulty;

        // Set zombie damage based on difficulty
        switch (difficulty)
        {
            case 0:                 // Easy 
                StaticData.zombieDamage = 1;
                StaticData.zombieHealth = 3;

                StaticData.meleeReaperDamage = 1;
                StaticData.meleeReaperHealth = 4;

                StaticData.rangedReaperDamage = 1;
                StaticData.rangedReaperHealth = 3;
                StaticData.orbDespawnTime = 5;

                StaticData.rockBossHealth = 15;
                StaticData.RockBossMeleeDMG = 1;
                StaticData.RockBossRangedDMG = 1;
                StaticData.RockBossOrbDespawnTime = 4;
                StaticData.RockBossRangedCD = 6;
                break;
            case 1:                 // Medium
                StaticData.zombieDamage = 1;
                StaticData.zombieHealth = 4;

                StaticData.meleeReaperDamage = 1;
                StaticData.meleeReaperHealth = 6;

                StaticData.rangedReaperDamage = 1;
                StaticData.rangedReaperHealth = 4;
                StaticData.orbDespawnTime = 5;

                StaticData.rockBossHealth = 20;
                StaticData.RockBossMeleeDMG = 2;
                StaticData.RockBossRangedDMG = 1;
                StaticData.RockBossOrbDespawnTime = 5;
                StaticData.RockBossRangedCD = 6;
                break;
            case 2:                 // Hard
                StaticData.zombieDamage = 2;
                StaticData.zombieHealth = 5;
                 
                StaticData.meleeReaperDamage = 2;
                StaticData.meleeReaperHealth = 8;

                StaticData.rangedReaperDamage = 2;
                StaticData.rangedReaperHealth = 5;
                StaticData.orbDespawnTime = 7;

                StaticData.rockBossHealth = 25;
                StaticData.RockBossMeleeDMG = 3;
                StaticData.RockBossRangedDMG = 2;
                StaticData.RockBossOrbDespawnTime = 8;
                StaticData.RockBossRangedCD = 6;
                break;
            case 3:                 // Expert
                StaticData.zombieDamage = 3;
                StaticData.zombieHealth = 6;

                StaticData.meleeReaperDamage = 3;
                StaticData.meleeReaperHealth = 8;

                StaticData.rangedReaperDamage = 4;
                StaticData.rangedReaperHealth = 5;
                StaticData.orbDespawnTime = 8;

                StaticData.rockBossHealth = 30;
                StaticData.RockBossMeleeDMG = 3;
                StaticData.RockBossRangedDMG = 2;
                StaticData.RockBossOrbDespawnTime = 10;
                StaticData.RockBossRangedCD = 4.5f;
                break;
            case 4:                 // Nightmare
                StaticData.zombieDamage = 100;
                StaticData.zombieHealth = 7;

                StaticData.meleeReaperDamage = 100;
                StaticData.meleeReaperHealth = 10;

                StaticData.rangedReaperDamage = 100;
                StaticData.rangedReaperHealth = 7;
                StaticData.orbDespawnTime = 10;

                StaticData.rockBossHealth = 30;
                StaticData.RockBossMeleeDMG = 100;
                StaticData.RockBossRangedDMG = 100;
                StaticData.RockBossOrbDespawnTime = 10;
                StaticData.RockBossRangedCD = 4.5f;
                break;
        }

        

        SceneManager.LoadScene("Level1");
    }

    public int difficulty = 2;

    // Swap to main menu
    public void MainMenuButton()
    {
        StaticData.difficulty = difficulty;
        SceneManager.LoadScene("MainMenu");
    }

    // Swap to difficulty menu
    public void DifficultyButton()
    {
        if (difficulty < 4)
            difficulty++;
        else
            difficulty = 0;
        switch (difficulty)
        {
            case 0:
                difficultyText.text = "Difficulty: Easy";
                break;
            case 1:
                difficultyText.text = "Difficulty: Medium";
                break;
            case 2:
                difficultyText.text = "Difficulty: Hard";
                break;
            case 3:
                difficultyText.text = "Difficulty: Expert";
                break;
            case 4:
                difficultyText.text = "Difficulty: Nightmare";
                break;
        }
    }


    public void SetDifficulty(int diff)
    {
        difficulty = diff;
    }
}
