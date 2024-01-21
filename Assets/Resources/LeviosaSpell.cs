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

        // Down Panel
        downLeftPanel = CreatePanel("DownLeftPanel");
        RectTransform rectTransformDownLeft = downLeftPanel.GetComponent<RectTransform>();
        rectTransformDownLeft.pivot = new Vector2(0, 0);

        rectTransformDownLeft.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransformDownLeft.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransformDownLeft.localRotation = Quaternion.Euler(0f, 0f, 45f);
        rectTransformDownLeft.sizeDelta = new Vector2(130, 350);
        rectTransformDownLeft.anchoredPosition = new Vector2(0, 0);

        // Up Panel
        upLeftPanel = CreatePanel("UpLeftPanel");
        upLeftPanel.transform.SetParent(downLeftPanel.transform, false);

        RectTransform rectTransformUpLeft = upLeftPanel.GetComponent<RectTransform>();
        rectTransformUpLeft.pivot = new Vector2(1, 0);

        rectTransformUpLeft.anchorMin = new Vector2(1, 01);
        rectTransformUpLeft.anchorMax = new Vector2(1, 1);
        rectTransformUpLeft.localRotation = Quaternion.Euler(0f, 0f, 90f);
        rectTransformUpLeft.sizeDelta = new Vector2(130, 250);
        rectTransformUpLeft.anchoredPosition = new Vector2(0, 0);
        rectTransformUpLeft.localScale = new Vector3(1, 1, 1);

        // Start Panel
        startPanel = CreatePanel("StartPanel");
        startPanel.transform.SetParent(upLeftPanel.transform, false);

        RectTransform rectTransStart = startPanel.GetComponent<RectTransform>();
        rectTransStart.pivot = new Vector2(1, 0);

        rectTransStart.anchorMin = new Vector2(1, 01);
        rectTransStart.anchorMax = new Vector2(1, 1);
        rectTransStart.localRotation = Quaternion.Euler(0f, 0f, 0);
        rectTransStart.sizeDelta = new Vector2(130, 100);
        rectTransStart.anchoredPosition = new Vector2(0, 0);
        rectTransStart.localScale = new Vector3(1, 1, 1);

        // Up Again Panel
        upRightPanel = CreatePanel("UpRigthPanel");
        RectTransform rectTransformUpRigth = upRightPanel.GetComponent<RectTransform>();
        rectTransformUpRigth.pivot = new Vector2(1, 0);

        rectTransformUpRigth.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransformUpRigth.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransformUpRigth.localRotation = Quaternion.Euler(0f, 0f, -45f);
        rectTransformUpRigth.sizeDelta = new Vector2(130, 350);
        rectTransformUpRigth.anchoredPosition = new Vector2(0, 0);



        downPanel = CreatePanel("DownPanel");
        downPanel.transform.SetParent(upRightPanel.transform, false);

        RectTransform rectTransformDownRigth = downPanel.GetComponent<RectTransform>();
        rectTransformDownRigth.pivot = new Vector2(1, 1);

        rectTransformDownRigth.anchorMin = new Vector2(1, 01);
        rectTransformDownRigth.anchorMax = new Vector2(1, 1);
        rectTransformDownRigth.localRotation = Quaternion.Euler(0f, 0f, 45f);
        rectTransformDownRigth.sizeDelta = new Vector2(130, 300);
        rectTransformDownRigth.anchoredPosition = new Vector2(0, 0);
        rectTransformDownRigth.localScale = new Vector3(1, 1, 1);

        // End Panel
        endPanel = CreatePanel("EndPanel");
        endPanel.transform.SetParent(downPanel.transform, false);

        RectTransform rectTransEnd = endPanel.GetComponent<RectTransform>();
        rectTransEnd.pivot = new Vector2(0, 1);

        rectTransEnd.anchorMin = new Vector2(0, 0);
        rectTransEnd.anchorMax = new Vector2(0, 0);
        rectTransEnd.localRotation = Quaternion.Euler(0f, 0f, 0);
        rectTransEnd.sizeDelta = new Vector2(130, 100);
        rectTransEnd.anchoredPosition = new Vector2(0, 0);
        rectTransEnd.localScale = new Vector3(1, 1, 1);
    }

    private GameObject CreatePanel(string name)
    {
        GameObject panel = new GameObject(name, typeof(Image), typeof(RectTransform));
        panel.transform.SetParent(GameObject.Find("Canvas").transform);
        panel.GetComponent<Image>().color = passiveColor;
        //SetPanelPosition(panel, point, left);
        return panel;
    }

    void SetPanelPosition2(GameObject panel, int direction, bool rotate, Vector2 pivot)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.pivot = pivot;

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f * direction);
        rectTransform.sizeDelta = new Vector2(130, 350);
        rectTransform.anchoredPosition = new Vector2(0, 500);
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

