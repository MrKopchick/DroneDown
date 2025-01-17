using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int totalResources = 0;

    private void Awake()
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

    public void AddResources(int amount)
    {
        totalResources += amount;
        //Debug.Log($"Total resources: {totalResources}");
    }

    public int GetTotalResources()
    {
        return totalResources;
    }
}