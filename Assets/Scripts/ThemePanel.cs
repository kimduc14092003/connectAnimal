using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ThemePanel : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject listThemeContent;
    public float scrollSpeed;
    private GameObject canvasHome;
    private bool isScrollingToBegin,isScrollingToEnd;

    private void Awake()
    {
        canvasHome = transform.parent.gameObject;
        string currentListSprite= PlayerPrefs.GetString("currentListSprite", "Animal");
        GameObject target = GameObject.Find(currentListSprite);
        if(target != null)
        {
            HandleTickOnButton(target);
        }
        else
        {
            print("Awake in theme error");
        }
    }

    private void FixedUpdate()
    {
        if (isScrollingToBegin)
        {
            scrollRect.normalizedPosition-= Vector2.right*Time.deltaTime* scrollSpeed;
            if (scrollRect.normalizedPosition.x <= 0)
            {
                isScrollingToBegin = false;
            }
        }
        if (isScrollingToEnd)
        {
            scrollRect.normalizedPosition += Vector2.right * Time.deltaTime* scrollSpeed;
            if (scrollRect.normalizedPosition.x >= 1)
            {
                isScrollingToEnd = false;
            }
        }
    }

    public void GetSelected()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        PlayerPrefs.SetString("currentListSprite", clickedObject.name);
        HandleTickOnButton(clickedObject);
        canvasHome.GetComponent<PrefabController>().ChangePrefabToCurrentTheme();
    }

    private void HandleTickOnButton(GameObject target)
    {
        for(int i = 0; i < listThemeContent.transform.childCount; i++)
        {
            listThemeContent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }

        target.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void ScrollToBeginButtonOnClick()
    {
        isScrollingToBegin=true;
    }
    public void ScrollToEndButtonOnClick()
    {
        isScrollingToEnd = true;
    }
}
