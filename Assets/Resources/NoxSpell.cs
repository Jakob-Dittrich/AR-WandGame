using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NoxStates
{
    Idle,
    Start,
    ChangeToRight,
    RightPanel,
    ChangeToLeft,
    LeftPanel,
    ChangeToEnd,
    EndPanel,
    End,
    Error
}

public class NoxSpell : MonoBehaviour
{
    public GameObject imageTarget; // Reference to your Vuforia Image Target
    public ParticleSystem particleSystem;

    public Button btnLumos;
    public Button btnNox;

    private GameObject startPanel;
    private GameObject rightPanel;
    private GameObject leftPanel;
    private GameObject endPanel;

    private NoxStates currentState;

    private Color activeColor = new Color(0, 1, 0, 0.5f); // Green transparent
    private Color passiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Grey transparent

    void Start()
    {
        currentState = NoxStates.Idle;
    }

    void Update()
    {
        CheckTargetPosition();
    }

    public void StartNox()
    {
        if (startPanel != null || rightPanel != null || leftPanel != null || endPanel != null)
        {
            Debug.LogError("Error Creating Panels, they are not null");
            currentState = NoxStates.Error;
            return;
        }

        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        CreatePanels();

        currentState = NoxStates.Start;
    }

    private void CreatePanels()
    {
        // Adjust the panel creation logic for right to left movement

        // Start Panel
        startPanel = CreatePanel("StartPanel", 1, false);

        // Right Panel
        rightPanel = CreatePanel("RightPanel", 1, true);

        // Left Panel
        leftPanel = CreatePanel("LeftPanel", -1, true);

        // End Panel
        endPanel = CreatePanel("EndPanel", -1, false);
    }

    private GameObject CreatePanel(string name, int direction, bool rotate)
    {
        GameObject panel = new GameObject(name, typeof(Image), typeof(RectTransform));
        panel.transform.SetParent(GameObject.Find("Canvas").transform);
        panel.GetComponent<Image>().color = passiveColor;
        SetPanelPosition(panel, direction, rotate);
        return panel;
    }

    public void DestroyPanels()
    {
        Destroy(startPanel);
        Destroy(rightPanel);
        Destroy(leftPanel);
        Destroy(endPanel);
    }

    void SetPanelPosition(GameObject panel, int direction, bool rotate)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f * direction);

        if (rotate)
        {
            rectTransform.sizeDelta = new Vector2(130, 350);
            rectTransform.anchoredPosition = new Vector2(direction * 106, 300);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(130, 100);
            rectTransform.anchoredPosition = new Vector2(direction * 324, 83);
        }
    }

    bool IsTargetCompletelyInArea(Transform targetTransform, RectTransform areaRectTransform)
    {
        Vector2 targetScreenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        return RectTransformUtility.RectangleContainsScreenPoint(areaRectTransform, targetScreenPosition);
    }

    void CheckTargetPosition()
    {
        switch (currentState)
        {
            case NoxStates.Idle:
                break;
            case NoxStates.Start:
                startPanel.GetComponent<Image>().color = passiveColor;
                leftPanel.GetComponent<Image>().color = passiveColor;
                rightPanel.GetComponent<Image>().color = passiveColor;
                endPanel.GetComponent<Image>().color = passiveColor;
                if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>()))
                {
                    startPanel.GetComponent<Image>().color = activeColor;
                    currentState = NoxStates.ChangeToRight;
                }
                break;
            case NoxStates.ChangeToRight:
                Debug.Log("ChangeToRigth");
                if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>()))
                {
                    // Perform actions for the "Start" state
                    startPanel.GetComponent<Image>().color = activeColor;

                }
                else
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, rightPanel.GetComponent<RectTransform>()))
                    {
                        rightPanel.GetComponent<Image>().color = activeColor;
                        currentState = NoxStates.RightPanel;
                    }
                    else
                    {
                        currentState = NoxStates.Start;
                    }
                }

                break;
            case NoxStates.RightPanel:
                if (!IsTargetCompletelyInArea(imageTarget.transform, rightPanel.GetComponent<RectTransform>()))
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, leftPanel.GetComponent<RectTransform>()))
                    {
                        leftPanel.GetComponent<Image>().color = activeColor;
                        currentState = NoxStates.LeftPanel;
                    }
                    else
                    {
                        currentState = NoxStates.Start;
                    }
                }
                break;
            case NoxStates.LeftPanel:
                if (!IsTargetCompletelyInArea(imageTarget.transform, leftPanel.GetComponent<RectTransform>()))
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, endPanel.GetComponent<RectTransform>()))
                    {
                        endPanel.GetComponent<Image>().color = activeColor;
                        currentState = NoxStates.EndPanel;
                    }
                    else
                    {
                        currentState = NoxStates.Start;
                    }
                }
                break;
            case NoxStates.EndPanel:
                DestroyPanels();
                currentState = NoxStates.End;
                TriggerFunction();
                break;
            case NoxStates.End:
                // Final State
                break;
            case NoxStates.Error:
                // Error State
                break;
        }
    }

    void TriggerFunction()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

        if (btnLumos != null)
        {
            btnLumos.gameObject.SetActive(true);
        }
        if (btnNox != null)
        {
            btnNox.gameObject.SetActive(false);
        }
    }
}

