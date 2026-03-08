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
        StaticData.difficulty = difficulty;
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
