using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoseGamePanel : MonoBehaviour
{
    public GameObject loseLight;
    public bool isRight = false;

    private void Start()
    {
        if (isRight)
        {
            loseLight.transform.DOLocalRotate(new Vector3(0, 0, -360), 60, RotateMode.FastBeyond360)
                                    .SetEase(Ease.Linear)
                                    .SetLoops(-1);
        }
        else
        {
            loseLight.transform.DORotate(new Vector3(0, 0, 360), 60, RotateMode.FastBeyond360)
                                   .SetEase(Ease.Linear)
                                   .SetLoops(-1);
        }
    }
}
