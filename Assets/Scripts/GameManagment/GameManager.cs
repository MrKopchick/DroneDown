using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGamePaused = false;
    public bool isPauseReady = true;
    private int totalResources = 0;
    //private RadarManager radarManager;
    //private MiniMapRenderer miniMapRenderer;

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

    private void Start()
    {
       // radarManager = new RadarManager();
        //miniMapRenderer = new MiniMapRenderer();

        //radarManager.AddRadar(new Radar(new Vector3(0, 0, 0), 50f, 1f));

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

    public void Update(){
        if(isPauseReady){
            Debug.Log("pause ready");
        }else{
            Debug.Log("pause not ready");
        }

        //radarManager.UpdateState(Time.deltaTime);
        //miniMapRenderer.RenderRadarData(radarManager.GetRadars());
    }
}