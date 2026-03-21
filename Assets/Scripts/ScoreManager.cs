using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour   //Handles the score text 
{
    public TMP_Text ScoreText;

    public int score = 0;

    private void Start()
    {
        // Load score from static data
        score = StaticData.score;
    }

    public void FixedUpdate() //Updates the score text
    {
        ScoreText.text = "Score: " + score;
    }
}
