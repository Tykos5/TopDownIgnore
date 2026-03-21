using UnityEngine;
using System.Collections;

public class CoffinManager : MonoBehaviour  // Handles the swapping of coffin sprites
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

    public void NextForm() //change shape when hit
    {
        if (isChanging) return;

        StartCoroutine(ChangeForm());
    }

    IEnumerator ChangeForm()    //handles the swap with a small time delay
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
