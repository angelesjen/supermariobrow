using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CornTrackerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image[] cornIcons;

    [Header("Color Settings")]
    [SerializeField] private Color emptyColor = new Color(0.2235294f, 0.2235294f, 0.2235294f, 0.7372549f);
    [SerializeField] private Color filledColor = Color.white;

    private void OnEnable()
    {
        ResetTrackerUI();
        if (CornTracker.Instance != null)
        {
            CornTracker.Instance.OnCornCollected.AddListener(UpdateCornUI);
        }
    }

    private void OnDisable()
    {
        if (CornTracker.Instance != null)
        {
            CornTracker.Instance.OnCornCollected.RemoveListener(UpdateCornUI);
        }
    }

    public void UpdateCornUI(int collectedCount)
    {
        for (int i = 0; i < cornIcons.Length; i++)
        {
            cornIcons[i].color = i < collectedCount ? filledColor : emptyColor;
        }
    }

    public void ResetTrackerUI()
    {
        if (cornIcons == null) return;

        foreach (Image icon in cornIcons)
        {
            if (icon != null) icon.color = emptyColor;
        }
    }
}