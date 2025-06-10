using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeTrackerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image[] lifeIcons;
    [SerializeField] private float loseLifeAnimDuration = 0.5f;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            Initialize();
            for (int i = 0; i < lifeIcons.Length; i++)
            {
                if (i >= GameManager.Instance.lives && lifeIcons[i].enabled)
                {
                    lifeIcons[i].enabled = false;
                }
                else
                {
                    lifeIcons[i].enabled = true;
                }
            }
        }
        else
        {
            Invoke(nameof(DelayedInitialize), 0.1f);
        }
    }

    private void DelayedInitialize()
    {
        if (GameManager.Instance != null)
        {
            Initialize();
        }
        else
        {
            Debug.LogError("GameManager instance not found after delay!");
        }
    }

    private void Initialize()
    {
        GameManager.Instance.OnLivesChanged += UpdateLifeIcons;

        UpdateLifeIcons(GameManager.Instance.lives);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged -= UpdateLifeIcons;
        }
    }


    public void UpdateLifeIcons(int currentLives)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i >= currentLives && lifeIcons[i].enabled)
            {
                StartCoroutine(AnimateLifeLoss(lifeIcons[i]));
            }
            else
            {
                lifeIcons[i].enabled = i < currentLives;
            }
        }
    }

    private IEnumerator AnimateLifeLoss(Image lifeIcon)
    {
        float timer = 0;
        Vector3 startScale = lifeIcon.transform.localScale;

        while (timer < loseLifeAnimDuration)
        {
            float progress = timer / loseLifeAnimDuration;
            lifeIcon.transform.localScale = startScale * (1 - progress);
            timer += Time.deltaTime;
            yield return null;
        }

        lifeIcon.enabled = false;
        lifeIcon.transform.localScale = startScale;
    }
}