using UnityEngine;
using UnityEngine.UI;

public class TogglePanels : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;

    public Button button1;
    public Button button2;

    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;

    private void Start()
    {
        SetPanelState(panel1, button1, true);
        SetPanelState(panel2, button2, false);
        
        button1.onClick.AddListener(() => SwitchPanel(panel1, button1, panel2, button2));
        button2.onClick.AddListener(() => SwitchPanel(panel2, button2, panel1, button1));
    }

    private void SwitchPanel(GameObject activatePanel, Button activateButton, GameObject deactivatePanel, Button deactivateButton)
    {
        SetPanelState(activatePanel, activateButton, true);
        SetPanelState(deactivatePanel, deactivateButton, false);
    }

    private void SetPanelState(GameObject panel, Button button, bool isActive)
    {
        panel.SetActive(isActive);
        
        var colors = button.colors;
        colors.normalColor = isActive ? activeColor : inactiveColor;
        colors.highlightedColor = colors.normalColor;
        colors.pressedColor = Color.Lerp(colors.normalColor, Color.black, 0.3f);
        colors.selectedColor = colors.normalColor;
        
        button.colors = colors;
    }
}