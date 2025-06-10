using UnityEngine;

public class Corn : MonoBehaviour
{
    AudioManager AudioManager;
    [Header("Corn Settings")]
    public string cornID;

    private void Awake()
    {
        AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        Debug.Log("collected");
        AudioManager.PlaySFX(AudioManager.coin, 0.1f);
        CornTracker.Instance.CollectCorn(cornID);
        Destroy(gameObject);
    }
}