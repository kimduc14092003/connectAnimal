using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThreeMatchEffect : MonoBehaviour
{
    public Image comboImg;
    public Image xImg;
    public Image num1Img;
    public Image num2Img;

    public Transform comboTarget;
    public Transform xTarget;
    public Transform num1Target;
    public Transform num2Target;

    private Vector3 oldPosCombo;
    private Vector3 oldPosX;
    private Vector3 oldPosNum1;
    private Vector3 oldPosNum2;
    public Sprite[] listNum;
    private int countCombo;

    private void Awake()
    {
        oldPosCombo = comboImg.transform.position;
        oldPosX = xImg.transform.position;
        oldPosNum1 = num1Img.transform.position;
        oldPosNum2 = num2Img.transform.position;
    }
    private void OnEnable()
    {
        Invoke("DisableGameObject", 0.75f);
        countCombo = transform.parent.GetComponent<LevelManagerThreeMatch>().countCombo;
        if (countCombo < 10)
        {
            num1Img.sprite = listNum[countCombo];
            num2Img.gameObject.SetActive(false);
        }
        else
        {
            num1Img.sprite = listNum[countCombo/10];
            num2Img.sprite = listNum[countCombo%10];
            num2Img.gameObject.SetActive(true);
        }

        comboImg.transform.DOMove(comboTarget.position, 0.2f).OnComplete(() =>
        {
            
        });
        xImg.transform.DOMove(xTarget.position, 0.2f);
        num1Img.transform.DOMove(num1Target.position, 0.2f);
        num2Img.transform.DOMove(num2Target.position, 0.2f);
        
    }
    
    private void DisableGameObject() { 
        
        gameObject.SetActive(false);
    }

    // Update is called once per frame

    private void OnDisable()
    {
        comboImg.transform.position = oldPosCombo;
        xImg.transform.position = oldPosX;
        num1Img.transform.position = oldPosNum1;
        num2Img.transform.position = oldPosNum2;
    }
}
