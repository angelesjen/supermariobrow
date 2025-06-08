using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CornTracker : MonoBehaviour
{
    public static CornTracker Instance;
    public int totalCorns = 3;
    private HashSet<string> collectedCorns = new HashSet<string>();

    
    public UnityEvent<int> OnCornCollected;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectCorn(string id)
    {
        if (!collectedCorns.Contains(id))
        {
            collectedCorns.Add(id);
            Debug.Log($"Corn collected! Total: {collectedCorns.Count}");
            int count = collectedCorns.Count;

            
            OnCornCollected.Invoke(collectedCorns.Count);


            if (count >= totalCorns)
            {
                Debug.Log("All corns collected!");
            }
        }
    }


    public void ResetTracker()
    {
        collectedCorns.Clear();
    }
}