using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CellControllerThreeMatch : MonoBehaviour
{
    public string CellID;
    public Vector3 target; // quy ước target z sẽ là vị trí
    public LevelManagerThreeMatch levelManager;
    public GameObject listChooseCell;
    private List<GameObject> listChooseCellID;
    private Tween currentTween;
    public bool isDisable;

    private void Start()
    {
        if (CellID != "0")
        {
            levelManager = transform.parent.parent.parent.GetComponent<LevelManagerThreeMatch>();
            listChooseCell = levelManager.listChooseCellGameObject;
            listChooseCellID = levelManager.listChooseCellID;
            Button button = GetComponent<Button>();
            button.onClick.AddListener(GetPosOfCell);
            GameObject child = transform.GetChild(0).gameObject;

        }
        //child.transform.SetParent(transform, false);
        Invoke("Check", 0.1f);
        //StartCoroutine(CallFunctionRepeatedly());
    }

    private void FixedUpdate()
    {
        if(isDisable)
        {
            DisableButton();
        }
        else
        {
            EnableButton();
        }
        if(transform.parent.name== "ListChooseCell")
        {
            isDisable = false;
        }
    }

    public void GetPosOfCell()
    {
        try
        {
            AudioManager.Instance.PlaySFX("click");
        }
        catch
        {
            Debug.Log("Audio Not Found!");
        }

        
        Button buttonCell = GetComponent<Button>();
        buttonCell.interactable = false;
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D)
        {
            boxCollider2D.enabled = false;
            LevelManagerThreeMatch levelManagerThreeMatch = transform.parent.parent.parent.GetComponent<LevelManagerThreeMatch>();
            for(int i=0;i < levelManagerThreeMatch.listLayer.Length; i++)
            {
                levelManagerThreeMatch.listLayer[i].ReCheckCell();
            }
           
        }

        target = levelManager.GetPosCellToMove(CellID);
        if ((int)target.z >= listChooseCellID.Count)
        {
            listChooseCellID.Add(gameObject);

        }else
        if (listChooseCellID[(int)target.z].GetComponent<CellControllerThreeMatch>().CellID == "0")
        {
            listChooseCellID[(int)target.z] = gameObject;
        }
        else
        {
            listChooseCellID.Insert((int)target.z, gameObject);
            listChooseCellID.RemoveAt(listChooseCellID.Count - 1);
        }
        MoveToListChooseCell();
    }

    public void MoveToListChooseCell()
    {
        //Invoke("ResetPos", 0.2f);
        currentTween = transform.DOMove(target, 0.5f).OnComplete(() =>
        {
            transform.SetParent(listChooseCell.transform, true);
            levelManager.ResetPosOfChosenCell();
        });
    }
    public void Check() 
    {
        if (CellID == "0")
        {
            return;
        }
        Collider2D[] colliders= Physics2D.OverlapBoxAll(transform.position,new Vector2(0.6f, 0.6f), 0f);
        foreach (Collider2D item in colliders)
        {
            if (gameObject.transform.parent.GetSiblingIndex() < item.transform.parent.GetSiblingIndex())
            {
                isDisable = true;
                return;
            }
        }
        isDisable = false;
    }


    private void DisableButton()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup)
        {
            canvasGroup.alpha = 0.4f;
            canvasGroup.interactable = false;
        }
    }

    public void EnableButton()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }
    }

    public void DoAnim()
    {
        Invoke("DoAnimationScore", 0.2f);
    }
    private void DoAnimationScore()
    {
        transform.DORotate(new Vector3(0, 0, 25f), 0.125f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, -14f), 0.125f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0, 5f), 0.125f, RotateMode.Fast).OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0, 0, 0), 0.125f, RotateMode.FastBeyond360).OnComplete(() =>
                    {
                        DOTween.Kill(gameObject);
                        Destroy(gameObject);
                    });
                });
            });
        });
    }

}
