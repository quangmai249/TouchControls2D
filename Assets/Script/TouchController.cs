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
    [SerializeField] SceneName sceneManager;
    [SerializeField] TextMeshProUGUI txtTouchCount;
    [SerializeField] GameObject square;
    [SerializeField] int count;
    [SerializeField] Vector2 startTouchPos, endTouchPos;
    [SerializeField] PathType pathType;
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
        throw new NotImplementedException();
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
