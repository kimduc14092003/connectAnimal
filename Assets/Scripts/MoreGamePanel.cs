using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MoreGamePanel : MonoBehaviour
{
    public GameObject cameraGO;
    private bool isRotate;


    public List<GameObject> listItemMoreGame;
    public List<RectTransformData> listRectTransformData;

    public int currentChooseIndex;

    private void Start()
    {
        //Khởi tạo danh sách vị trí các item
        listRectTransformData = new List<RectTransformData>();
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform itemRectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            RectTransformData rectTransformData = new RectTransformData();

            rectTransformData = new RectTransformData();
            rectTransformData.position = itemRectTransform.position;
            rectTransformData.rotation = itemRectTransform.rotation.eulerAngles;
            rectTransformData.anchoredPosition = itemRectTransform.anchoredPosition;
            rectTransformData.anchorMin = itemRectTransform.anchorMin;
            rectTransformData.anchorMax = itemRectTransform.anchorMax;

            listRectTransformData.Add(rectTransformData);
        }

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
            GameObject item = Instantiate(listItemMoreGame[i], transform);
            item.transform.position = child.position;
            item.transform.rotation = child.rotation;
            item.transform.SetAsLastSibling();


            RectTransform rectTransformChild = child.GetComponent<RectTransform>();
            RectTransform rectTransformItem = item.GetComponent<RectTransform>();
            rectTransformItem.anchoredPosition = rectTransformChild.anchoredPosition;
            rectTransformItem.anchorMin = rectTransformChild.anchorMin;
            rectTransformItem.anchorMax = rectTransformChild.anchorMax;

        }
        for (int i = 0; i < count; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void NextButton()
    {
        if (!isRotate)
        {
            AudioManager.Instance.PlaySFX("click_button");

            isRotate = true;
            currentChooseIndex++;
            int childCount = transform.childCount;

            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();

                try
                {
                    child.transform.DORotate(listRectTransformData[i - 1].rotation, 1f);
                    child.transform.DOMove(listRectTransformData[i - 1].position, 1f).OnComplete(() =>
                    {
                        if (currentChooseIndex >= listItemMoreGame.Count)
                        {
                            currentChooseIndex = 0;
                        }
                        isRotate = false;
                    });
                }
                catch
                {
                    Debug.Log("Error " + child);
                    Destroy(child.gameObject);
                    ChangeLastButton(true);
                }
            }

        }
    }
    public void BackButton()
    {
        if (!isRotate)
        {
            AudioManager.Instance.PlaySFX("click_button");

            isRotate = true;
            currentChooseIndex--;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
                try
                {
                    child.transform.DORotate(listRectTransformData[i + 1].rotation, 1f);
                    child.transform.DOMove(listRectTransformData[i + 1].position, 1f).OnComplete(() =>
                    {
                        if (currentChooseIndex < 0)
                        {
                            currentChooseIndex = listItemMoreGame.Count - 1;
                        }
                        isRotate = false;
                    });
                }
                catch
                {
                    Debug.Log("Error " + child);
                    Destroy(child.gameObject);
                    ChangeLastButton(false);
                }
            }


        }

    }

    private void ChangeLastButton(bool isInRight)
    {
        /*// Tìm gameobject Child cần thay đổi
        Transform childNeedToChange = transform.GetChild(0); ;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (childNeedToChange.position.z > transform.GetChild(i).position.z)
            {
                childNeedToChange = transform.GetChild(i);
            }
        }*/
        // Sinh ra GO mới trong List

        if (isInRight)
        {
            int index = (currentChooseIndex + 2) % listItemMoreGame.Count;
            GameObject item = Instantiate(listItemMoreGame[index], transform);

            RectTransformData rightRectTransformData = listRectTransformData[listRectTransformData.Count - 1];
            item.transform.position = rightRectTransformData.position;
            item.transform.rotation = Quaternion.Euler(rightRectTransformData.rotation);

            RectTransform rectTransformItem = item.GetComponent<RectTransform>();
            rectTransformItem.anchoredPosition = rightRectTransformData.anchoredPosition;
            rectTransformItem.anchorMin = rightRectTransformData.anchorMin;
            rectTransformItem.anchorMax = rightRectTransformData.anchorMax;

        }
        else
        {
            try
            {
                int index = (currentChooseIndex - 2);
                if (index < 0)
                {
                    index = listItemMoreGame.Count + index;
                }
                GameObject item = Instantiate(listItemMoreGame[index], transform);

                RectTransformData leftRectTransformData = listRectTransformData[0];

                item.transform.position = leftRectTransformData.position;
                item.transform.rotation = Quaternion.Euler(leftRectTransformData.rotation);
                item.transform.SetAsFirstSibling();

                RectTransform rectTransformItem = item.GetComponent<RectTransform>();
                rectTransformItem.anchoredPosition = leftRectTransformData.anchoredPosition;
                rectTransformItem.anchorMin = leftRectTransformData.anchorMin;
                rectTransformItem.anchorMax = leftRectTransformData.anchorMax;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

        }
    }
}

public class RectTransformData
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector2 anchoredPosition;
    public Vector2 anchorMax;
    public Vector2 anchorMin;
}
