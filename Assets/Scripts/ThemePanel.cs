using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ThemePanel : MonoBehaviour
{
    public GameObject listThemeContent;
    private GameObject canvasHome;
    public List<Vector3> listPosItem;
    public List<GameObject> itemThemes;
    public GameObject listTheme;
    public int index=0;
    public float moveTime;
    private void Awake()
    {
        canvasHome = transform.parent.root.gameObject;

        //Thay đổi listSpite default
        string currentListSprite= PlayerPrefs.GetString("currentListSprite", "Animal");
        GameObject target= itemThemes[0];
        for (int i = 0; i < itemThemes.Count; i++)
        {
            if (itemThemes[i].name == currentListSprite)
            {
                target = itemThemes[i];
                break;
            }
        }
        if(target != null)
        {
            HandleTickOnButton(target);
        }
        else
        {
            print("Awake in theme error");
        }

        // Lấy vị trí tất cả các gameobject Item
        for(int i = 0;i<listTheme.transform.childCount;i++)
        {
            listPosItem.Add(listTheme.transform.GetChild(i).position);
        }
        
    }

    public void NextButton()
    {
        if (index >= 1)
        {
            index = 1;
            Debug.Log("?");
            return;
        }
        for (int i = 0;i<listTheme.transform.childCount; i++)
        {
            int pos = (i - index - 1);
            try
            {
                if(pos < 0)
                {
                   pos= listPosItem.Count + pos;
                }
                listTheme.transform.GetChild(i).DOMove(listPosItem[pos], moveTime);
            }
            catch
            {
                Debug.Log(i - 1 +" | "+pos);
            }
        }
        index++;
    }
    public void BackButton()
    {
        if (index <= 0)
        {
            index = 0;
            return;
        }
        for (int i = 0; i < listTheme.transform.childCount; i++)
        {
            int pos = (i - index + 1);

            try
            {
                if (pos >= listPosItem.Count)
                {
                    pos = pos % listPosItem.Count;
                }
                listTheme.transform.GetChild(i).DOMove(listPosItem[pos], moveTime);
            }
            catch
            {
                Debug.Log(i + 1);
            }
        }
        index--;

    }

    

    public void GetSelected()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        PlayerPrefs.SetString("currentListSprite", clickedObject.name);
        HandleTickOnButton(clickedObject);
        canvasHome.GetComponent<PrefabController>().ChangePrefabToCurrentTheme();
        //Tắt ThemePanel sau khi chọn chủ đề
        Invoke("ClosePanel", 0.15f);
    }

    private void ClosePanel()
    {
        canvasHome.GetComponent<HomePanel>().CloseThemePanel();
    }

    private void HandleTickOnButton(GameObject target)
    {
        for(int i = 0; i < itemThemes.Count; i++)
        {
            itemThemes[i].transform.Find("Selected").gameObject.SetActive(false);
        }

        target.transform.GetChild(0).gameObject.SetActive(true);
    }
    
}
