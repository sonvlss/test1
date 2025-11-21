using UnityEngine;

public class AdCounter : MonoBehaviour
{
    public static AdCounter Instance;
    public int restartCount = 0;

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
}
