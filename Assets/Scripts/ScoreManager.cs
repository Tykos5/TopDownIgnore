using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text ScoreText;

    public int score = 0;

    private void Start()
    {
        // Load score from static data
        score = StaticData.score;
    }

    public void FixedUpdate()
    {
        ScoreText.text = "Score: " + score;
    }
}
