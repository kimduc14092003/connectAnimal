using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MoreGamePanel : MonoBehaviour
{
    public GameObject pinWheel;
    public GameObject[] star;
    public ScrollRect scrollRect;
    public float scrollSpeed;
    public Ease ease;
    private bool isScrollingToBegin, isScrollingToEnd;
    // Start is called before the first frame update
    void Start()
    {
        StartRotationPinWheel();
        for(int i = 0; i < star.Length; i++)
        {
            StartAnimStar(star[i]);

        }
    }
    private void FixedUpdate()
    {
        if (isScrollingToBegin)
        {
            scrollRect.normalizedPosition -= Vector2.right * Time.deltaTime * scrollSpeed;
            if (scrollRect.normalizedPosition.x <= 0)
            {
                isScrollingToBegin = false;
            }
        } else
        if (isScrollingToEnd)
        {
            scrollRect.normalizedPosition += Vector2.right * Time.deltaTime * scrollSpeed;
            if (scrollRect.normalizedPosition.x >= 1)
            {
                isScrollingToEnd = false;
            }
        }
    }


    void StartRotationPinWheel()
    {
        // Xoay chong chóng đi
        pinWheel.transform.DORotate(new Vector3(0,0,360f), 10f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetUpdate(true);
    }

    private void StartAnimStar(GameObject target)
    {
        float delay = Random.Range(0f,0.5f);
        target.transform.DOScale(new Vector3(1, 1, 1), 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease).SetDelay(delay); ;
        target.transform.DORotate(new Vector3(0, 0, 60f), 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease).SetDelay(delay); ;
    }

    public void ScrollToBeginButtonOnClick()
    {
        isScrollingToBegin = true;
    }
    public void ScrollToEndButtonOnClick()
    {
        isScrollingToEnd = true;
    }
}
