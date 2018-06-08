using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public Image healthUI;

    public Image keyBackground;
    public Image keyUI;

    public Button resumeButton;
    public Button backToMenuButton;

    bool isEscape = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isEscape = !isEscape;

            resumeButton.gameObject.SetActive(isEscape);
            backToMenuButton.gameObject.SetActive(isEscape);
        }
    }

    public void UpdateHealthUI(float max, float current)
    {
        healthUI.fillAmount = 1 - ((max - current) / max);
    }

    public void UpdateHealthUI(float percent)
    {
        healthUI.fillAmount = 1 - (percent / 100);
    }

    public void ActivateKeyUI()
    {
        healthUI.enabled = true;
    }
    
    public void PressResume()
    {
        resumeButton.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
        isEscape = !isEscape;
    }

    public void PressBackToMenu()
    {
        SceneManager.LoadScene("MainMenu Scene", LoadSceneMode.Single);
    }

    public void ActivateKey()
    {
        keyBackground.gameObject.SetActive(false);
        keyUI.gameObject.SetActive(true);
    }
}
