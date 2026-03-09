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
            case 0:
                StaticData.zombieDamage = 1;
                StaticData.zombieHealth = 3;
                break;
            case 1:
                StaticData.zombieDamage = 1;
                StaticData.zombieHealth = 4;
                break;
            case 2:
                StaticData.zombieDamage = 2;
                StaticData.zombieHealth = 5;
                break;
            case 3:
                StaticData.zombieDamage = 3;
                StaticData.zombieHealth = 6;
                break;
            case 4:
                StaticData.zombieDamage = 100;
                StaticData.zombieHealth = 7;
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

    void Update()
    {

    }
}
