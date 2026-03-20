#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts")]
    public static void FindMissing()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int count = 0;

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component == null)
                {
                    Debug.LogWarning("Missing script on: " + go.name, go);
                    count++;
                }
            }
        }

        if (count == 0)
            Debug.Log("No missing scripts found in current scene!");
        else
            Debug.Log(count + " missing scripts found!");
    }
}
#endif