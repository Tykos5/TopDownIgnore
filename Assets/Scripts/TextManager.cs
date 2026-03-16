using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TMP_Text ScoreText;

    public void UpdateScoreText(Text scoreText)
    {
        ScoreText.text = "Score: " + StaticData.score.ToString();
    }
}
