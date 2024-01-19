using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LeviosaControl {
    left,
    up,
    right,
    down,
    none
}

public class LeviosaManager : MonoBehaviour
{

    private GameObject leftPanel;
    private GameObject rightPanel;
    private GameObject upPanel;
    private GameObject downPanel;

    public Button btnLeviosa;
    public Button btnStop;

    public GameObject objectToMove; // The GameObject to move
    public float moveSpeed = 0.5f; // Speed at which the object moves
    public ObjectMover mover;

    public GameObject imageTarget;

    public FloatingEffect floatingScript;

    private bool isMovementActivated = false;


    Color transparentGreen = new Color(0, 1, 0, 0.1f); // Green transparent
    Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Default color (grey transparent)

    void Start()
    {
        //CreatePanels();
        MovementActivation(false);
    }

    void Update()
    {

        //mover.MoveLeft();
        if (isMovementActivated)
        {
            MoveInDirection(GetMovement());
        }
    }

    public void StopButtonPressed()
    {
        MovementActivation(false);
        if (btnLeviosa != null)
        {
            btnLeviosa.gameObject.SetActive(true);
        }
        if (btnStop != null)
        {
            btnStop.gameObject.SetActive(false);
        }
    }

    public void MovementActivation(bool activate)
    {
        isMovementActivated = activate;
        floatingScript.SetFloating(activate);
        if (activate)
        {
            CreatePanels();
            if (btnLeviosa != null)
            {
                btnLeviosa.gameObject.SetActive(false);
            }
            if (btnStop != null)
            {
                btnStop.gameObject.SetActive(true);
            }
        }
        else
        {
            DestroyPanels();
        }
    }

    private void CreatePanels()
    {
        // Create each panel
        leftPanel = CreatePanel("LeftPanel");
        rightPanel = CreatePanel("RightPanel");
        upPanel = CreatePanel("UpPanel");
        downPanel = CreatePanel("DownPanel");

        // Set the position, size, and pivot of each panel
        SetPanelPosition(leftPanel, -Screen.width / 2, 0, Screen.width / 12 *5, Screen.height/3, new Vector2(0.4f, 0.5f));
        SetPanelPosition(rightPanel, Screen.width / 2, 0, Screen.width / 12 *5, Screen.height/3, new Vector2(0.6f, 0.5f));
        SetPanelPosition(upPanel, 0, Screen.height / 2, Screen.width, Screen.height / 3, new Vector2(0.5f, 0.6f));
        SetPanelPosition(downPanel, 0, -Screen.height /2 + 40, Screen.width, Screen.height / 3 -60, new Vector2(0.5f, 0.35f));
    }

    private GameObject CreatePanel(string name)
    {
        GameObject panel = new GameObject(name, typeof(Image), typeof(RectTransform));
        panel.transform.SetParent(GameObject.Find("Canvas").transform);
        panel.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Grey transparent
        return panel;
    }

    void SetPanelPosition(GameObject panel, float xPos, float yPos, float width, float height, Vector2 pivot)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.pivot = pivot;
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
    }

    public void DestroyPanels()
    {
        Destroy(leftPanel);
        Destroy(rightPanel);
        Destroy(upPanel);
        Destroy(downPanel);
    }



    bool IsTargetCompletelyInArea(Transform targetTransform, RectTransform areaRectTransform)
    {
        Vector2 targetScreenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        return RectTransformUtility.RectangleContainsScreenPoint(areaRectTransform, targetScreenPosition);
    }

    LeviosaControl GetMovement()
    {
        ResetPanelColors(); // Resets all panel colors to default

        if (IsTargetCompletelyInArea(imageTarget.transform, leftPanel.GetComponent<RectTransform>()))
        {
            //leftPanel.GetComponent<Image>().color = transparentGreen;
            return LeviosaControl.left;
        }
        else if (IsTargetCompletelyInArea(imageTarget.transform, upPanel.GetComponent<RectTransform>()))
        {
            upPanel.GetComponent<Image>().color = transparentGreen;
            return LeviosaControl.up;
        }
        else if (IsTargetCompletelyInArea(imageTarget.transform, rightPanel.GetComponent<RectTransform>()))
        {
            rightPanel.GetComponent<Image>().color = transparentGreen;
            return LeviosaControl.right;
        }
        else if (IsTargetCompletelyInArea(imageTarget.transform, downPanel.GetComponent<RectTransform>()))
        {
            downPanel.GetComponent<Image>().color = transparentGreen;
            return LeviosaControl.down;
        }
        else
        {
            return LeviosaControl.none;
        }
    }

    void ResetPanelColors()
    {
        leftPanel.GetComponent<Image>().color = defaultColor;
        upPanel.GetComponent<Image>().color = defaultColor;
        rightPanel.GetComponent<Image>().color = defaultColor;
        downPanel.GetComponent<Image>().color = defaultColor;
    }

    public void MoveInDirection(LeviosaControl direction)
    {
        switch (direction)
        {
            case LeviosaControl.left:
                //objectToMove.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                mover.MoveLeft();

                leftPanel.GetComponent<Image>().color = transparentGreen;
                break;
            case LeviosaControl.up:
                mover.MoveUp();
                break;
            case LeviosaControl.right:
                mover.MoveRight();
                break;
            case LeviosaControl.down:
                mover.MoveDown();
                break;
            case LeviosaControl.none:
                // Do nothing for 'none'
                break;
        }
    }



}

