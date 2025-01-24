using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CityPanelUI : MonoBehaviour
{
    [Header("City UI Elements")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(0, -Screen.height);
    [SerializeField] private Vector2 visiblePosition = new Vector2(0, 0);
    [SerializeField] private TMP_Text cityNameText;
    [SerializeField] private TMP_Text cityDescriptionText;
    [SerializeField] private Button buildBunkerButton;
    [SerializeField] private Button buildHospitalButton;
    [SerializeField] private Button repairInfrastructureButton;

    private CityStats currentCityStats;
    private bool isPanelOpen = false;

    public static CityPanelUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        panel.anchoredPosition = hiddenPosition;

        buildBunkerButton.onClick.AddListener(OnBuildBunkerClicked);
        buildHospitalButton.onClick.AddListener(OnBuildHospitalClicked);
        repairInfrastructureButton.onClick.AddListener(OnRepairInfrastructureClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideCityPanel();
        }
    }

    public void ShowCityPanel(CityStats cityStats)
    {
        if (IDCardManager.Instance != null && IDCardManager.Instance.IsPanelOpen)
        {
            IDCardManager.Instance.HideIDCard(() => OpenCityPanel(cityStats));
        }
        else if (currentCityStats == cityStats && isPanelOpen)
        {
            Debug.Log("City panel is already open for this city.");
            return;
        }
        else
        {
            OpenCityPanel(cityStats);
        }
    }

    private void OpenCityPanel(CityStats cityStats)
    {
        currentCityStats = cityStats;
        UpdateCityDetails();
        panel.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
        isPanelOpen = true;
        GameManager.Instance.isPauseReady = false;
    }

    public void HideCityPanel(TweenCallback onComplete = null)
    {
        if (!isPanelOpen) return;

        panel.DOAnchorPos(hiddenPosition, animationDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            currentCityStats = null;
            isPanelOpen = false;
            if (IDCardManager.Instance != null)
            {
                IDCardManager.Instance.RestorePanel();
            }
            onComplete?.Invoke();
            GameManager.Instance.isPauseReady = true;
        });
    }

    public void RestorePanel()
    {
        if (!isPanelOpen)
        {
            panel.DOAnchorPos(visiblePosition, animationDuration).SetEase(Ease.OutQuad);
            isPanelOpen = true;
        }
    }

    private void UpdateCityDetails()
    {
        if (currentCityStats == null) return;

        cityNameText.text = currentCityStats.cityName;
        cityDescriptionText.text = $"Population: {currentCityStats.population}\n" +
                                    $"Quality of Life: {currentCityStats.qualityOfLife}%\n" +
                                    $"Bunkers: {currentCityStats.bunkers}/{currentCityStats.maxBunkers}\n" +
                                    $"Hospitals: {currentCityStats.hospitals}/{currentCityStats.maxHospitals}\n" +
                                    $"Infrastructure: {currentCityStats.infrastructureIntegrity}%";
    }

    private void OnBuildBunkerClicked()
    {
        if (currentCityStats != null)
        {
            currentCityStats.BuildBunker(1000);
            UpdateCityDetails();
        }
    }

    private void OnBuildHospitalClicked()
    {
        if (currentCityStats != null)
        {
            currentCityStats.BuildHospital(1500);
            UpdateCityDetails();
        }
    }

    private void OnRepairInfrastructureClicked()
    {
        if (currentCityStats != null)
        {
            currentCityStats.RepairInfrastructure(500);
            UpdateCityDetails();
        }
    }

    public bool IsPanelOpen => isPanelOpen;
}