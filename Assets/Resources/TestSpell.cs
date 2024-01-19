using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum State
{
    Idle,
    StartFirstPanel,
    EndFirstPanel,
    StartSecondPanel,
    EndSecondPanel
}


public class TestSpell : MonoBehaviour
{
    // public Camera arCamera; // Reference to your AR camera
    public GameObject imageTarget; // Reference to your Vuforia Image Target
    private GameObject startPanel;
    private GameObject endPanel;

    private State currentState;

    private Color startColor = new Color(0, 1, 0, 0.5f); // Green transparent
    private Color endColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Grey transparent



    void Start()
    {
        // CreatePanels();
        currentState = State.Idle;
    }

    void Update()
    {
        //CheckTargetPosition();

        if (currentState == State.Idle)
        {
            return;
        }

        if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>())){

            //startPanel.GetComponent<Image>().color = new Color(0, 1, 0, 0.5f); // Green transparent
        } else
        {
            //startPanel.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Grey transparent

        }

    }

    public void CreatePanels()
    {
        // Create the start panel
        startPanel = new GameObject("StartPanel", typeof(Image), typeof(RectTransform));
        startPanel.transform.SetParent(GameObject.Find("Canvas").transform);
        startPanel.GetComponent<Image>().color = new Color(0, 1, 0, 0.5f); // Green transparent
        SetPanelPosition(startPanel, -1);

        // Create the end panel
        endPanel = new GameObject("EndPanel", typeof(Image), typeof(RectTransform));
        endPanel.transform.SetParent(GameObject.Find("Canvas").transform);
        endPanel.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Grey transparent
        SetPanelPosition(endPanel, 1);

        currentState = State.StartFirstPanel;

    }

    void SetPanelPosition(GameObject panel, int direction)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(100, 300); // Size of a normal button
        rectTransform.anchoredPosition = new Vector2(direction * 200, 0);
        // Middle of the canvas horizontally
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f * direction); // Rotate around the Z-axis
    }

    bool IsTargetCompletelyInArea(Transform targetTransform, RectTransform areaRectTransform)
    {
        // Convert the target's world position to screen space
        Vector2 targetScreenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);

        // Check if the target's position is within the area's bounds
        return RectTransformUtility.RectangleContainsScreenPoint(areaRectTransform, targetScreenPosition);
    }


    void CheckTargetPosition()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.StartFirstPanel:
                startPanel.GetComponent<Image>().color = startColor;
                endPanel.GetComponent<Image>().color = endColor;

                if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>()))
                {
                    currentState = State.StartSecondPanel;
                }

                break;

            case State.EndFirstPanel:
                // Execute actions for ending the first panel
                Debug.Log("Ending First Panel");
                // Transition to the next state when ready
                currentState = State.StartSecondPanel;
                break;

            case State.StartSecondPanel:
                startPanel.GetComponent<Image>().color = endColor;
                endPanel.GetComponent<Image>().color = startColor;

                if (IsTargetCompletelyInArea(imageTarget.transform, endPanel.GetComponent<RectTransform>()))
                {
                    currentState = State.EndSecondPanel;
                }
                break;

            case State.EndSecondPanel:
                // Execute actions for ending the second panel
                Debug.Log("You got it!!!!");
                // Transition to the next state when ready
                currentState = State.StartFirstPanel;
                break;

            default:
                break;
        }
    }

    void TriggerFunction()
    {
        // Your function that gets called when the target is in the end area
        Debug.Log("Function Triggered!");
    }
}
