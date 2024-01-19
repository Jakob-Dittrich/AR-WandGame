using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LeviosaStates
{
    Idle,
    Start,
    ChangeToUpLeft,
    UpLeftPanel,
    DownLeftPanel,
    UpRightPanel,
    DownPanel,
    EndPanel,
    End,
    Error
}

public class LeviosaSpell : MonoBehaviour
{
    public GameObject imageTarget; // Reference to your Vuforia Image Target

    public LeviosaManager leviosaMgr;


    private GameObject startPanel;
    private GameObject upLeftPanel;
    private GameObject downLeftPanel;
    private GameObject upRightPanel;
    private GameObject downPanel;
    private GameObject endPanel;

    private LeviosaStates currentState;

    private Color activeColor = new Color(0, 1, 0, 0.5f); // Green transparent
    private Color passiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Grey transparent


    private bool isLeviosaActivated = false;


    void Start()
    {
        currentState = LeviosaStates.Idle;
    }

    void Update()
    {
        CheckTargetPosition();
    }

    public void LeviosaButtonPressed()
    {
        isLeviosaActivated = !isLeviosaActivated;
        if (isLeviosaActivated)
        {
            StartLeviosa();
        }
        else
        {
            DestroyPanels();
            leviosaMgr.MovementActivation(false);
        }

    }

    public void StartLeviosa()
    {
        if (startPanel != null || upLeftPanel != null || downLeftPanel != null || upRightPanel != null || downPanel != null || endPanel != null)
        {
            Debug.LogError("Error Creating Panels, they are not null");
            currentState = LeviosaStates.Error;
            return;
        }

        CreatePanels();

        currentState = LeviosaStates.Start;
    }

    private void CreatePanels()
    {
        // Start Panel
        startPanel = CreatePanel("StartPanel");
        SetPanelPosition(startPanel,true, -1, 100, -416, 177);

        // Up Panel
        upLeftPanel = CreatePanel("UpLeftPanel");
        SetPanelPosition(upLeftPanel,true, -1, 250, -247, 347);

        // Down Panel
        downLeftPanel = CreatePanel("DownLeftPanel");
        SetPanelPosition(downLeftPanel, true, 1, 350, -80, 300);

        // Up Again Panel
        upRightPanel = CreatePanel("UpRigthPanel");
        SetPanelPosition(upRightPanel, true, -1, 350, 135, 300);


        downPanel = CreatePanel("DownPanel");
        SetPanelPosition(downPanel, false, -1, 300, 280, 250);

        // End Panel
        endPanel = CreatePanel("EndPanel");
        SetPanelPosition(endPanel, false, -1, 100, 280, -23);
    }

    private GameObject CreatePanel(string name)
    {
        GameObject panel = new GameObject(name, typeof(Image), typeof(RectTransform));
        panel.transform.SetParent(GameObject.Find("Canvas").transform);
        panel.GetComponent<Image>().color = passiveColor;
        //SetPanelPosition(panel, point, left);
        return panel;
    }

    void SetPanelPosition(GameObject panel, bool rotate, int direction, float heigth, float x, float y)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(130, heigth);
        rectTransform.anchoredPosition = new Vector2(x, y);

        if(rotate)
        {
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f * direction);
        }
    }

    public void DestroyPanels()
    {
        Destroy(startPanel);
        Destroy(upLeftPanel);
        Destroy(downLeftPanel);
        Destroy(upRightPanel);
        Destroy(downPanel);
        Destroy(endPanel);
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
            case LeviosaStates.Idle:
                break;
            case LeviosaStates.Start:
                startPanel.GetComponent<Image>().color = passiveColor;
                upLeftPanel.GetComponent<Image>().color = passiveColor;
                downLeftPanel.GetComponent<Image>().color = passiveColor;
                upRightPanel.GetComponent<Image>().color = passiveColor;
                downPanel.GetComponent<Image>().color = passiveColor;
                endPanel.GetComponent<Image>().color = passiveColor;

                if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>()))
                {
                    startPanel.GetComponent<Image>().color = activeColor;
                    currentState = LeviosaStates.ChangeToUpLeft;
                }
                break;
            case LeviosaStates.ChangeToUpLeft:
                if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>()))
                {
                    // Perform actions for the "Start" state
                    startPanel.GetComponent<Image>().color = activeColor;

                }
                else
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, upLeftPanel.GetComponent<RectTransform>()))
                    {
                        upLeftPanel.GetComponent<Image>().color = activeColor;
                        currentState = LeviosaStates.UpLeftPanel;
                    }
                    else
                    {
                        currentState = LeviosaStates.Start;
                    }
                }

                break;
            case LeviosaStates.UpLeftPanel:
                if (!IsTargetCompletelyInArea(imageTarget.transform, upLeftPanel.GetComponent<RectTransform>()))
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, downLeftPanel.GetComponent<RectTransform>()))
                    {
                        downLeftPanel.GetComponent<Image>().color = activeColor;
                        currentState = LeviosaStates.DownLeftPanel;
                    }
                    else
                    {
                        currentState = LeviosaStates.Start;
                    }
                }
                break;
            case LeviosaStates.DownLeftPanel:
                if (!IsTargetCompletelyInArea(imageTarget.transform, downLeftPanel.GetComponent<RectTransform>()))
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, upRightPanel.GetComponent<RectTransform>()))
                    {
                        upRightPanel.GetComponent<Image>().color = activeColor;
                        currentState = LeviosaStates.UpRightPanel;
                    }
                    else
                    {
                        currentState = LeviosaStates.Start;
                    }
                }
                break;
            case LeviosaStates.UpRightPanel:
                if (!IsTargetCompletelyInArea(imageTarget.transform, upRightPanel.GetComponent<RectTransform>()))
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, downPanel.GetComponent<RectTransform>()))
                    {
                        downPanel.GetComponent<Image>().color = activeColor;
                        currentState = LeviosaStates.DownPanel;
                    }
                    else
                    {
                        currentState = LeviosaStates.Start;
                    }
                }
                break;
            case LeviosaStates.DownPanel:
                if (!IsTargetCompletelyInArea(imageTarget.transform, downPanel.GetComponent<RectTransform>()))
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, endPanel.GetComponent<RectTransform>()))
                    {
                        endPanel.GetComponent<Image>().color = activeColor;
                        currentState = LeviosaStates.EndPanel;
                    }
                    else
                    {
                        currentState = LeviosaStates.Start;
                    }
                }
                break;
            case LeviosaStates.EndPanel:
                DestroyPanels();
                currentState = LeviosaStates.End;
                TriggerFunction();
                break;
            case LeviosaStates.End:
                // Final State
                break;
        }

    }

    void TriggerFunction()
    {
        //positionScript.SetPositionUpdating(true);
        leviosaMgr.MovementActivation(true);
    }
}

