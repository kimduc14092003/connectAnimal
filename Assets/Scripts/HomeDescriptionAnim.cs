using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HomeDescriptionAnim : MonoBehaviour
{
    public Vector3 targerRotate;
    public float timeFade,timeRotate;
    private void Start()
    {
        transform.DOLocalRotate(targerRotate, timeRotate).SetLoops(-1,LoopType.Yoyo);
        GetComponent<Image>().DOFade(0.5f, timeFade)
            .SetLoops(-1,LoopType.Yoyo);
    }
}
