using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{
    public GameObject treasure;
    public Sprite openTreasure;
    public GameObject lightTreasure;
    public GameObject item;
    public Image itemImg;
    public Sprite[] listItem;
    public GameObject buttonConfirm;
    // Start is called before the first frame update
    void Start()
    {
        treasure.transform.DOMove(new Vector3(0,1.5f,0), 0.3f);
        treasure.transform.DOScale(new Vector3(0.5f,1f), 0.3f).OnComplete(() =>
        {
            treasure.transform.DOScaleX(1, 0.2f);
            treasure.transform.DOMove(new Vector3(0, -0.5f, 0), 0.2f).OnComplete(() =>
            {
                StartCoroutine(DoAnimTreasure());
            });
        });
        //treasure.transform.DOScaleY(1, 0.3f);
    }

    IEnumerator DoAnimTreasure()
    {
        yield return new WaitForSeconds(1 / 30);
        treasure.transform.DORotate(new Vector3(0, 0, -15f), 0.06f);
        yield return new WaitForSeconds(0.075f);
        Tween rotateTween = treasure.transform.DORotate(new Vector3(0, 0, 15f), 0.12f).SetLoops(-1, LoopType.Yoyo);
        treasure.transform.DOScale(new Vector3(0.8f, 0.8f), 0.2f).OnComplete(() =>
        {
            treasure.transform.DOScale(new Vector3(1, 1), 0.4f).OnComplete(() =>
            {
                rotateTween.Kill();
                treasure.transform.DORotate(new Vector3(0, 0, 0), 0.1f).OnComplete(() =>
                {
                    treasure.GetComponent<Image>().sprite = openTreasure;
                    ItemAppear();
                });
            });

        });
    }

    private void ItemAppear()
    {
        int randomNumber = Random.Range(0, listItem.Length);
        itemImg.sprite = listItem[randomNumber];
        lightTreasure.SetActive(true);
        item.SetActive(true);
        item.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.5f);
        item.transform.DOMove(item.transform.position + Vector3.up * 2.5f, 0.5f);
        buttonConfirm.SetActive(true);
        HandleItem(listItem[randomNumber].name);
    }

    private void HandleItem(string id)
    {
        switch (id)
        {
            case "bom":
                {
                    int current = PlayerPrefs.GetInt("bombRemainingThreeMatchMode", 5);
                    PlayerPrefs.SetInt("bombRemainingThreeMatchMode", ++current);
                    break;
                }
            case "icon_kinh_lup":
                {
                    int current = PlayerPrefs.GetInt("findRemainingThreeMatchMode", 5);
                    PlayerPrefs.SetInt("findRemainingThreeMatchMode", ++current);
                    break;
                }
            case "ic_reroll":
                {
                    int current = PlayerPrefs.GetInt("shuffleRemainingThreeMatchMode", 5);
                    PlayerPrefs.SetInt("shuffleRemainingThreeMatchMode", ++current);
                    break;
                }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
