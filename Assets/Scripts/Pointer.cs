using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Transform targetPosition;
    public Transform playerTransform; // Drag your player in via Inspector
    public float radius = 100f; // Distance from player in screen pixels

    private RectTransform pointerRectTransform;
    private Canvas canvas;

    private void Awake()
    {
        pointerRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        // Get direction from player to target
        Vector2 toPosition = targetPosition.position;
        Vector2 fromPosition = playerTransform.position;

        Vector2 dir = (toPosition - fromPosition).normalized;

        // Rotate arrow to face target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pointerRectTransform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Move arrow in a circle around the player's screen position
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(playerTransform.position);

        // Convert to canvas space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            playerScreenPos,
            canvas.worldCamera,
            out Vector2 playerCanvasPos
        );

        // Place arrow on the circle around the player
        pointerRectTransform.anchoredPosition = playerCanvasPos + dir * radius;
    }

    public void SetTarget(Transform newTarget)
    {
        targetPosition = newTarget;
    }
}