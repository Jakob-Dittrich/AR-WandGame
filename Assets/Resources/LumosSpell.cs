

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum LumosStates
{
    Idle,
    Start,
    ChangeToUp,
    UpPanel,
    ChangeToDown,
    DownPanel,
    ChangeToEnd,
    EndPanel,
    End,
    Error
}


public class LumosSpell : MonoBehaviour
{
    // public Camera arCamera; // Reference to your AR camera
    public GameObject imageTarget; // Reference to your Vuforia Image Target
    public ParticleSystem particleSystem;

    public Button btnLumos;
    public Button btnNox;

    private GameObject startPanel;
    private GameObject upPanel;
    private GameObject downPanel;
    private GameObject endPanel;


    private LumosStates currentState;

    private Color activeColor = new Color(0, 1, 0, 0.5f); // Green transparent
    private Color passiveColor = new Color(0.5f, 0.5f, 0.5f, 0.9f); // Grey transparent

    void Start()
    {
        currentState = LumosStates.Idle;
        particleSystem.Stop();
    }


    void Update()
    {

        CheckTargetPosition();



    }

    public void StartLumos()
    {
        if (startPanel != null || upPanel != null || downPanel != null || endPanel != null)
        {
            Debug.LogError("Error Creating Panels, they are not null");
            currentState = LumosStates.Error;
            return;
        }

        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

        CreatePanels();

        currentState = LumosStates.Start;

    }


    private void CreatePanels()
    {
        // Adjust the panel creation logic for right to left movement


        // Left Panel
        upPanel = CreatePanel("UpPanel", -1, true, new Vector2(0, 1));

        // Right Panel
        downPanel = CreatePanel("DownPanel", 1, true, new Vector2(1, 1));

        // Start Panel
        startPanel = CreatePanel("StartPanel", -1, false, new Vector2(0, 1));

        // End Panel
        endPanel = CreatePanel("EndPanel", 1, false, new Vector2(0, 1));
    }


    private GameObject CreatePanel(string name, int direction, bool rotate, Vector2 pivot)
    {
        GameObject panel = new GameObject(name, typeof(Image), typeof(RectTransform));
        panel.transform.SetParent(GameObject.Find("Canvas").transform);
        if(rotate)
        {
            panel.GetComponent<Image>().color = passiveColor;
        }
        else
        {
            panel.GetComponent<Image>().color = activeColor;
        }
        SetPanelPosition(panel, direction, rotate, pivot);
        return panel;
    }

    void SetPanelPosition(GameObject panel, int direction, bool rotate, Vector2 pivot)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.pivot = pivot;

        if (rotate)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f * direction);
            rectTransform.sizeDelta = new Vector2(130, 350);
            rectTransform.anchoredPosition = new Vector2(0, 500);
        }
        else
        {
            GameObject parent = direction == -1 ? upPanel : downPanel;
            panel.transform.SetParent(parent.transform, false);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            rectTransform.localRotation = Quaternion.Euler(0f, 0f, 0);
            rectTransform.localScale = new Vector3(1, 1,1);

            rectTransform.sizeDelta = new Vector2(130, 100);
            rectTransform.anchoredPosition = new Vector2(0,0);
        }
    }



    //private void CreatePanels()
    //{
    //    // Create the start panel
    //    startPanel = new GameObject("StartPanel", typeof(Image), typeof(RectTransform));
    //    startPanel.transform.SetParent(GameObject.Find("Canvas").transform);
    //    startPanel.GetComponent<Image>().color = passiveColor; // Grey transparent
    //    SetPanelPosition(startPanel, -1, false);

    //    // Create the end panel
    //    endPanel = new GameObject("EndPanel", typeof(Image), typeof(RectTransform));
    //    endPanel.transform.SetParent(GameObject.Find("Canvas").transform);
    //    endPanel.GetComponent<Image>().color = passiveColor; // Grey transparent
    //    SetPanelPosition(endPanel, 1, false);


    //    // Create the start panel
    //    upPanel = new GameObject("UpPanel", typeof(Image), typeof(RectTransform));
    //    upPanel.transform.SetParent(GameObject.Find("Canvas").transform);
    //    upPanel.GetComponent<Image>().color = passiveColor; // Green transparent
    //    SetPanelPosition(upPanel, -1, true);

    //    // Create the end panel
    //    downPanel = new GameObject("DownPanel", typeof(Image), typeof(RectTransform));
    //    downPanel.transform.SetParent(GameObject.Find("Canvas").transform);
    //    downPanel.GetComponent<Image>().color = passiveColor; // Grey transparent
    //    SetPanelPosition(downPanel, 1, true);


    //}

    public void DestroyPanels()
    {
        Destroy(startPanel);
        Destroy(endPanel);
        Destroy(upPanel);
        Destroy(downPanel);
    }

    //void SetPanelPosition(GameObject panel, int direction, bool rotate)
    //{
    //    RectTransform rectTransform = panel.GetComponent<RectTransform>(); // Middle of the canvas horizontally

    //    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    //    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    //    rectTransform.localRotation = Quaternion.Euler(0f, 0f, 45f * direction); // Rotate around the Z-axis

    //    if (rotate)
    //    {
    //        rectTransform.sizeDelta = new Vector2(60, 200); // Size of a normal button
    //        rectTransform.anchoredPosition = new Vector2(direction * 50, 145);

    //    }
    //    else
    //    {
    //        rectTransform.sizeDelta = new Vector2(60, 60); // Size of a normal button
    //        rectTransform.anchoredPosition = new Vector2(direction * 142, 53); // Middle of the canvas horizontally
    //    }
    //}

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
            case LumosStates.Idle:
                return;
            case LumosStates.Start:
                Debug.Log("Start");
                startPanel.GetComponent<Image>().color = passiveColor;
                upPanel.GetComponent<Image>().color = passiveColor;
                downPanel.GetComponent<Image>().color = passiveColor;
                endPanel.GetComponent<Image>().color = passiveColor;
                if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>()))
                {
                    // Perform actions for the "Start" state
                    startPanel.GetComponent<Image>().color = activeColor;
                    currentState = LumosStates.ChangeToUp;

                }
                break;
            case LumosStates.ChangeToUp:
                Debug.Log("ChangeToUp");
                if (IsTargetCompletelyInArea(imageTarget.transform, startPanel.GetComponent<RectTransform>()))
                {
                    // Perform actions for the "Start" state
                    startPanel.GetComponent<Image>().color = activeColor;

                } else
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, upPanel.GetComponent<RectTransform>()))
                    {
                        upPanel.GetComponent<Image>().color = activeColor;
                        currentState = LumosStates.UpPanel;
                    }
                    else
                    {
                        currentState = LumosStates.Start;
                    }
                }

                break;

            case LumosStates.UpPanel:
                Debug.Log("UpPanel");
                if (IsTargetCompletelyInArea(imageTarget.transform, upPanel.GetComponent<RectTransform>()))
                {
                    // Perform actions for the "Start" state
                    upPanel.GetComponent<Image>().color = activeColor;

                }
                else
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, downPanel.GetComponent<RectTransform>()))
                    {
                        downPanel.GetComponent<Image>().color = activeColor;
                        currentState = LumosStates.DownPanel;
                    }
                    else
                    {
                        currentState = LumosStates.Start;
                    }
                }
                break;
            case LumosStates.DownPanel:
                if (IsTargetCompletelyInArea(imageTarget.transform, downPanel.GetComponent<RectTransform>()))
                {
                    // Perform actions for the "Start" state
                    downPanel.GetComponent<Image>().color = activeColor;

                }
                else
                {
                    if (IsTargetCompletelyInArea(imageTarget.transform, endPanel.GetComponent<RectTransform>()))
                    {
                        endPanel.GetComponent<Image>().color = activeColor;
                        currentState = LumosStates.EndPanel;
                    }
                    else
                    {
                        currentState = LumosStates.Start;
                    }
                }
                break;

            case LumosStates.EndPanel:
                DestroyPanels();
                currentState = LumosStates.End;
                TriggerFunction();
                break;

            case LumosStates.End:
                Debug.Log("Ending...");
                // Perform actions for the "End" state
                // No transition needed as it's the final state
                break;
            case LumosStates.Error:
                Debug.Log("Error...");
                break;

        }
    }

    void TriggerFunction()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        if (btnLumos != null)
        {
            btnLumos.gameObject.SetActive(false);
        }
        if (btnNox != null)
        {
            btnNox.gameObject.SetActive(true);
        }

    }
}
