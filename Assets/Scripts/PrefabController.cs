using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabController : MonoBehaviour
{
    public GameObject[] listGameobjectPrefab;
    public GameObject[] listGameobjectShadow;
    public GameObject[] listGameobjectBlock;

    public Sprite[] listSpriteAnimal;
    public Sprite[] listSpritePokemon;
    public Sprite[] listSpriteFood;
    public Sprite[] listSpriteFruit;
    public Sprite[] listSpriteCandy;
    public Sprite[] listSpriteTravel;
    public Sprite[] listSpriteFlower;
    public Sprite[] listSpriteInsect;
    public Sprite[] listSpriteJapan;
    public Sprite[] listSpriteKorean;
    public Sprite[] listSpriteSomething;

    private string currentListSpriteString;
    private Sprite[] currentListSprite;
    private void Awake()
    {
        //ChangePrefabToCurrentTheme();
    }

    public void ChangePrefabToCurrentTheme()
    {
        currentListSpriteString = PlayerPrefs.GetString("currentListSprite", "Animal");
        GetCurrentListSprite();
        ChangeSpritePrefabs();
    }

    private void GetCurrentListSprite()
    {
        switch(currentListSpriteString)
        {
            case "Animal":
                {
                    currentListSprite = listSpriteAnimal;
                    break;
                }
            case "Pokemon":
                {
                    currentListSprite = listSpritePokemon;
                    break;
                }
            case "Food":
                {
                    currentListSprite = listSpriteFood;
                    break;
                }
            case "Fruit":
                {
                    currentListSprite = listSpriteFruit;
                    break;
                }
            case "Candy":
                {
                    currentListSprite = listSpriteCandy;
                    break;
                }
            case "Travel":
                {
                    currentListSprite = listSpriteTravel;
                    break;
                }
            case "Flower":
                {
                    currentListSprite = listSpriteFlower;
                    break;
                }
            case "Insect":
                {
                    currentListSprite = listSpriteInsect;
                    break;
                }
            case "Japan":
                {
                    currentListSprite = listSpriteJapan;
                    break;
                }
            case "Korean":
                {
                    currentListSprite = listSpriteKorean;
                    break;
                }
            case "Something":
                {
                    currentListSprite = listSpriteSomething;
                    break;
                }
        }    
    }
    private void ChangeSpritePrefabs()
    {
        for(int i = 0; i < listGameobjectPrefab.Length; i++)
        {
            listGameobjectPrefab[i].transform.GetChild(0).GetComponent<Image>().sprite = currentListSprite[i];
            listGameobjectShadow[i].transform.GetChild(0).GetComponent<Image>().sprite= currentListSprite[i];
            if (listGameobjectBlock.Length>0)
            {
                listGameobjectBlock[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = currentListSprite[i];
            }
        }
    }
}
