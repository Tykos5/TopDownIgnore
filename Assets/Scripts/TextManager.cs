using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour    //Handles score text for the death and victory scene
{
    public TMP_Text ScoreText;

    void Start()
    {
        ScoreText.text = "Score: " + StaticData.score.ToString();
    }
}
