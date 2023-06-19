using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MoreGamePanel : MonoBehaviour
{
    public Transform leftTransform, rightTransform;
    public GameObject cameraGO;
    private bool isRotate;

    private Vector3 leftPosition, leftRotation;
    private Vector3 rightPosition, rightRotation;

    public List<GameObject> listItemMoreGame;

    public int currentChooseIndex;

    private void Start()
    {
        leftPosition = leftTransform.position;
        leftRotation = leftTransform.rotation.eulerAngles;

        rightPosition = rightTransform.position;
        rightRotation = rightTransform.rotation.eulerAngles;

        leftTransform = null; rightTransform=null;
        isRotate = false;
        transform.position = new Vector3(cameraGO.transform.position.x, transform.position.y, cameraGO.transform.position.z);
        SetDefaultItem();
        currentChooseIndex = 2;
    }

    private void SetDefaultItem()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            GameObject item = Instantiate(listItemMoreGame[i],transform);
            item.transform.position = child.position;
            item.transform.rotation = child.rotation;
            item.transform.SetAsLastSibling();
        }
        for( int i = 0; i < count; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void NextButton()
    {
        if (!isRotate)
        {
            isRotate = true;
            transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y - 30, 0), 1f).OnComplete(() =>
            {
                currentChooseIndex++;
                if (currentChooseIndex >= listItemMoreGame.Count)
                {
                    currentChooseIndex = 0;
                }
                isRotate = false;
                ChangeLastButton();
            });
        }
    }
    public void BackButton()
    {
        if (!isRotate)
        {
            isRotate = true;
            transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y + 30, 0), 1f).OnComplete(() =>
            {
                currentChooseIndex--;
                if(currentChooseIndex < 0)
                {
                    currentChooseIndex=listItemMoreGame.Count - 1;
                }
                isRotate = false;
                ChangeLastButton();
            });
        }

    }

    private void ChangeLastButton()
    {
        // Tìm gameobject Child cần thay đổi
        Transform childNeedToChange = transform.GetChild(0); ;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (childNeedToChange.position.z > transform.GetChild(i).position.z)
            {
                childNeedToChange = transform.GetChild(i);
            }
        }
        // Sinh ra GO mới trong List

        if (childNeedToChange.transform.position.x < 0)
        {
            int index = (currentChooseIndex + 2) % listItemMoreGame.Count;
            GameObject item = Instantiate(listItemMoreGame[index],transform);

            item.transform.position = rightPosition;
            item.transform.rotation = Quaternion.Euler(rightRotation);
            Destroy(childNeedToChange.gameObject);
        }
        else
        {
            int index = (currentChooseIndex - 2);
            if (index < 0)
            {
                index = listItemMoreGame.Count +index;
            }
            GameObject item = Instantiate(listItemMoreGame[index], transform);

            item.transform.position = leftPosition;
            item.transform.rotation = Quaternion.Euler(leftRotation);
            Destroy(childNeedToChange.gameObject);

        }
    }
}
