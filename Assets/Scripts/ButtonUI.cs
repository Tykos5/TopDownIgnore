using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "Level1";
    public void NewGameButton()
    {
        SceneManager.LoadScene("Level1");
    }
}
