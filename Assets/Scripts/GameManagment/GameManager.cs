using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGamePaused = false;
    public bool isPauseReady = true;
    private int totalResources = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            isPauseReady = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddResources(int amount)
    {
        totalResources += amount;
    }

    public int GetTotalResources()
    {
        return totalResources;
    }
}