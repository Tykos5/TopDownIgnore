using UnityEngine;

public class DontDestroy : MonoBehaviour    //sript to not destroy object when swapping scene
{
    private void Awake()
    {
        DontDestroy existing = Object.FindAnyObjectByType<DontDestroy>();

        if (existing != null && existing != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
