using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    public string CellID;
    public bool isSelected;
    private bool isHint;
    public int posX, posY;
    public GameObject starEffect;
    Image image;

    private LevelManager levelManager;
    public GameObject listCellController;
    private bool isShuffle = false;
    private void Awake()
    {
        listCellController = gameObject.transform.parent.gameObject;
    }
    private void Start()
    {
        isSelected = false;
        isHint = false;
        image = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if(CellID == "wall")
        {
            return;
        }
        if(listCellController == null )
        {
            return;
        }
        
        if (listCellController.GetComponent<ListCellController>())
        {
            levelManager = listCellController.GetComponent<ListCellController>().levelManager;
            isShuffle= levelManager.isShuffle;
        }
        else if(listCellController.GetComponent<RelaxPuzzleLevelManager>())
        {
            isShuffle = listCellController.GetComponent<RelaxPuzzleLevelManager>().isShuffle;
        }
        if (!image)
        {
            return;
        }
        if (this.isSelected||isHint)
        {
            if (isSelected|| isShuffle)
            {
                isHint = false;
            }
            image.color = new Color32(12, 153, 167, 255);
        }
        else
        {
            image.color = new Color32(255, 255, 225, 255);
        }
    }

    public void SetHintColor()
    {
        isHint = true;
        Invoke("StopHint", 3f);
    }

    private void StopHint()
    {
        isHint = false;
    }

    public void ToggleIsSelected()
    {
        this.isSelected = !this.isSelected;
        if (listCellController.GetComponent<ListCellController>())
        {
            listCellController.GetComponent<ListCellController>().CellClicked(gameObject);
        }
        else if(listCellController.GetComponent<EndlessModeController>())
        {
            listCellController.GetComponent<EndlessModeController>().CellClicked(gameObject);
        } 
        else if (listCellController.GetComponent<PuzzleModeController>())
        {
            listCellController.GetComponent<PuzzleModeController>().CellClicked(gameObject);
        }
        else if (listCellController.GetComponent<ShadowModeController>())
        {
            listCellController.GetComponent<ShadowModeController>().CellClicked(gameObject);
        }
        else if (listCellController.GetComponent<ListCellEscapeController>())
        {
            listCellController.GetComponent<ListCellEscapeController>().CellClicked(gameObject);
        }
        else
        {
            print("nothing");
        }
    }
    // Đã test chạy okela lắm bạn tôi ơi
    public void ConnectFailAnim()
    {
        transform.DORotate(new Vector3(0, 0, 25f), 0.125f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, -14f), 0.125f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0, 5f), 0.125f, RotateMode.Fast).OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0, 0, 0), 0.125f, RotateMode.FastBeyond360);
                });
            });
        });
    }
    public void ConnectSuccess(float delayTime)
    {
        transform.DOScale(new Vector3(0f, 0f, 1), delayTime);
        Instantiate(starEffect, transform.position, Quaternion.EulerAngles(Vector3.zero));

        CanvasGroup canvasGroup =GetComponent<CanvasGroup>();
        if(canvasGroup != null )
        {
            canvasGroup.DOFade(0, delayTime);
        }
        
    }
}
