using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum SceneName
{
    MULTITAPSCENE,
    SWIPESCENE,
    PINCHTOZOOMSCENE
}

public class TouchController : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] int count;
    [SerializeField] GameObject square;
    [SerializeField] TextMeshProUGUI txtTouchCount;
    [SerializeField] SceneName sceneManager;

    [Header("Multi Tap")]
    [SerializeField] PathType pathType;
    [SerializeField] Vector2 startTouchPos, endTouchPos;

    [Header("Pinch To Zoom")]
    [SerializeField] float previousDistance;
    [SerializeField] Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (sceneManager == SceneName.MULTITAPSCENE)
            this.MultiTouchOnTheScreen();

        if (sceneManager == SceneName.SWIPESCENE)
            this.SwipeOnScreen();

        if (sceneManager == SceneName.PINCHTOZOOMSCENE)
            this.PrinchToZoomScreen();
    }

    private void PrinchToZoomScreen()
    {
        if (Input.touchCount == 2)
        {
            float currentDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (previousDistance > 0)
                {
                    mainCamera.orthographicSize += (previousDistance - currentDistance) * 0.01f;
                    mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2, 10);
                    txtTouchCount.text = $"{mainCamera.orthographicSize}";
                }
                previousDistance = currentDistance;
            }
        }
        else
        {
            previousDistance = 0;
        }
    }

    private void SwipeOnScreen()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPos = this.GetTouchPosition(0);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPos = this.GetTouchPosition(0);
                this.StartMovingWipe();
                //this.StartMovingFlDOPath(startTouchPos, endTouchPos, pathType);
            }

            return;
        }
    }

    private void StartMovingWipe()
    {
        Vector2 swipeDelta = endTouchPos - startTouchPos;

        if (swipeDelta.magnitude > 0.3)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0)
                    square.transform.DOMoveX(square.transform.position.x + 1, .25f);
                else
                    square.transform.DOMoveX(square.transform.position.x - 1, .25f);
            }
            else
            {
                if (swipeDelta.y > 0)
                    square.transform.DOMoveY(square.transform.position.y + 1, .25f);
                else
                    square.transform.DOMoveY(square.transform.position.y - 1, .25f);
            }
        }

        count++;
        txtTouchCount.text = $"{count}";
    }

    private void StartMovingFlDOPath(Vector3 startPos, Vector3 endPos, PathType pathType)
    {
        square.transform.DOPath(new Vector3[] { startPos, endPos }, 1f, pathType)
            .SetEase(Ease.Linear);
    }

    private void MultiTouchOnTheScreen()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                count++;
                txtTouchCount.text = $"{count}";
                square.transform.position = this.GetTouchPosition(i);
                return;
            }

            if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                count++;
                txtTouchCount.text = $"{count}";
                square.transform.position = this.GetTouchPosition(i);
                return;
            }
        }
    }

    private Vector3 GetTouchPosition(int indexTouch)
    {
        return new Vector3(
            Camera.main.ScreenToWorldPoint(Input.GetTouch(indexTouch).position).x,
            Camera.main.ScreenToWorldPoint(Input.GetTouch(indexTouch).position).y,
            0);
    }
}
