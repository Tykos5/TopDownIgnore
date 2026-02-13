using UnityEngine;
using System.Collections;

public class CoffinManager : MonoBehaviour
{
    public GameObject[] coffins;
    public float formDelay = 0.2f;

    private int currentIndex = 0;
    private bool isChanging = false;

    void Start()
    {
        for (int i = 0; i < coffins.Length; i++)
        {
            coffins[i].SetActive(i == 0);
        }
    }

    public void NextForm()
    {
        if (isChanging) return;

        StartCoroutine(ChangeForm());
    }

    IEnumerator ChangeForm()
    {
        isChanging = true;

        coffins[currentIndex].SetActive(false);

        yield return new WaitForSeconds(formDelay);

        currentIndex++;

        if (currentIndex < coffins.Length)
        {
            coffins[currentIndex].SetActive(true);
        }
        else
        {
            Debug.Log("All coffin forms finished!");
        }

        isChanging = false;
    }
}
