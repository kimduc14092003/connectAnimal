using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListCellEscapeController : MonoBehaviour
{
    public LineController lineController;
    public string[,] Matrix;
    public List<GameObject> listCell;
    public int colPlay, rowPlay, col, row;

    public int numOfDifferentCell;

    public float delayTimeToDeleteCell;

    public LevelManagerEscapeMode levelManager;

    public int limitCellAdjacent,limitMoveTurn;
    public float timeToMove;
    private bool isMoving;
    public bool isWin;

    private List<string> rabbitMovingQueue;
    [SerializeField]
    private List<Sprite> cells;

    [SerializeField]
    private GameObject CellSelected1, CellSelected2;
    private Algorithm algorithm;
    private int cellRemain;
    private GridLayoutGroup layoutGroup;
    public GameObject rabbit;
    public GameObject rabbitDefaultTras;
    public Vector3 rabbitDefaultPos;

    private void Awake()
    {
        layoutGroup=GetComponent<GridLayoutGroup>();
        rabbitMovingQueue = new List<string>();
        algorithm =GetComponent<Algorithm>();
        isMoving = false;
        isWin = false;
        col = colPlay + 2;
        row = rowPlay + 2;
        Matrix = new string[col,row];
        cellRemain = colPlay*rowPlay;
    }

    private void Start()
    {
        float panelWidth = GetComponent<RectTransform>().rect.width;
        float panelHeight = GetComponent<RectTransform>().rect.height;
        layoutGroup.cellSize = new Vector2(panelWidth / col, (panelWidth / col) * 1.25f);
        if ((panelWidth / col) * 1.25f * row >= panelHeight)
        {
            layoutGroup.cellSize = new Vector2((panelHeight / row) * 0.8f, panelHeight / row);
        }
        levelManager =transform.parent.GetComponent<LevelManagerEscapeMode>();
        listCell = new List<GameObject>();
        StartCoroutine(SetDefaultRabbitPos());
        GetMatrix();
        SetDifferentCellImage();
    }
    IEnumerator SetDefaultRabbitPos()
    {
        yield return new WaitForSeconds(0.1f);
        rabbitDefaultPos = rabbitDefaultTras.transform.position;
        rabbit.transform.position= rabbitDefaultTras.transform.position;

    }
    

    private void GetMatrix()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject cell = transform.GetChild(i).gameObject;
            listCell.Add(cell);
            CellController cellController = cell.GetComponent<CellController>();
            cellController.posX = i % col;
            cellController.posY = i / col;
            Matrix[cellController.posX, cellController.posY] = cellController.CellID;
        }
    }

    private void SetDifferentCellImage()
    {
        List<Sprite> listSpiteWillAppear = new List<Sprite>();
        for (int i = 0; i < numOfDifferentCell; i++)
        {
            int randomValue = Random.Range(0, cells.Count);
            listSpiteWillAppear.Add(cells[randomValue]);
            cells.Remove(cells[randomValue]);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cell = transform.GetChild(i).gameObject;
            CellController cellController = cell.GetComponent<CellController>();
            switch (cellController.CellID)
            {
                case "up":
                    {
                        cell.transform.GetChild(0).GetComponent<Image>().sprite = listSpiteWillAppear[0];
                        break;
                    }
                case "down":
                    {
                        cell.transform.GetChild(0).GetComponent<Image>().sprite = listSpiteWillAppear[1];
                        break;
                    }
                case "left":
                    {
                        cell.transform.GetChild(0).GetComponent<Image>().sprite = listSpiteWillAppear[2];
                        break;
                    }
                case "right":
                    {
                        cell.transform.GetChild(0).GetComponent<Image>().sprite = listSpiteWillAppear[3];
                        break;
                    }

            }
        }
    }

    private void FixedUpdate()
    {
        if (rabbitMovingQueue.Count > 0&&!isMoving)
        {
            string nextMove= rabbitMovingQueue[0];
            HandleRabbitMovement(nextMove);
            rabbitMovingQueue.RemoveAt(0);
        }
    }

    public void CellClicked(GameObject cellClicked)
    {
        if(CellSelected1 == null)
        {
            CellSelected1 = cellClicked;
            AudioManager.Instance.PlaySFX("click");
        }
        else 
        if (cellClicked != CellSelected1) //Kiểm tra người chơi có click lại vào ô vừa chọn không
        {
            CellSelected2= cellClicked;
            //Kiểm tra ID của CellSelected 1 và CellSelected 2
            if (CellSelected1.GetComponent<CellController>().CellID == CellSelected2.GetComponent<CellController>().CellID)
            {
             
                CellController cellController1 = CellSelected1.GetComponent<CellController>();
                CellController cellController2 = CellSelected2.GetComponent<CellController>();

                Vector2Int[] result = algorithm.CheckResultCell(cellController1.posX, cellController2.posX,
                                             cellController1.posY, cellController2.posY);

                if (result != null)
                {
                    AudioManager.Instance.PlaySFX("connect");
                    rabbitMovingQueue.Add(cellController1.CellID);
                    cellRemain -= 2;
                    levelManager.MinusTurnMove();
                    StartCoroutine(DeleteCell(CellSelected1));
                    StartCoroutine(DeleteCell(CellSelected2));
                    StartCoroutine(CheckWinLevel(0.25f));


                    //Loại bỏ các phần tử trùng nhau
                    result = result.Distinct().ToArray();

                    //Bỏ gợi ý
                    //levelManager.isFinded = false;


                    //Tạo đường kẻ kết nối các ô đã ăn
                    if (result.Length == 2)
                    {
                        lineController.CreateLine(result[0], result[1]);
                    }
                    else if(result.Length == 3) 
                    {
                        lineController.CreateLine(result[0], result[1], result[2]);
                    }
                    else if(result.Length == 4)
                    {
                        lineController.CreateLine(result[0], result[1], result[2], result[3]);
                    }
                    // Kiểm tra xem còn ô nào có thể ăn được nữa không
                   // Invoke("HandleCantPlayWhenScored", delayTimeToDeleteCell+0.1f);

                }
                else
                {
                    AudioManager.Instance.PlaySFX("connect_fail");
                    CellSelected1.GetComponent<CellController>().isSelected = false;
                    CellSelected1.GetComponent<CellController>().ConnectFailAnim();
                    
                    CellSelected2.GetComponent<CellController>().isSelected = false;
                    CellSelected2.GetComponent<CellController>().ConnectFailAnim();

                }


            }
            else
            {
                AudioManager.Instance.PlaySFX("connect_fail");
                CellSelected1.GetComponent<CellController>().isSelected = false;
                CellSelected1.GetComponent<CellController>().ConnectFailAnim();
                
                CellSelected2.GetComponent<CellController>().isSelected = false;
                CellSelected2.GetComponent<CellController>().ConnectFailAnim();

            }



            //Reset Cell selected
            CellSelected1 = null;
            CellSelected2 = null;
        }

        else
        {
            //Reset Cell selected khi người dùng click lại vào ô đã click
            CellSelected1 = null;
            CellSelected2 = null;
        }
    }

    IEnumerator DeleteCell(GameObject cell)
    {

        CellController cellController = cell.GetComponent<CellController>();
        cellController.CellID = "0";
        Matrix[cellController.posX, cellController.posY] = "0";
        cellController.GetComponent<BoxCollider2D>().enabled = false;
        cellController.ConnectSuccess(delayTimeToDeleteCell);
        yield return new WaitForSeconds(delayTimeToDeleteCell);

        cell.GetComponent<CanvasGroup>().alpha = 0;
        cell.GetComponent<Button>().interactable = false;
    }

    IEnumerator CheckWinLevel(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (cellRemain <= 0)
        {
            levelManager.WinLevelNotification();
        }
    }

    private void HandleRabbitMovement(string cellID)
    {
        isMoving = true;
        Invoke("MovingDone", timeToMove);
        switch(cellID)
        {
            case "up":
                {
                    RabbitMoveUp();
                    break;
                }
            case "down":
                {
                    RabbitMoveDown();
                    break;
                }
            case "left":
                {
                    RabbitMoveLeft();
                    break;
                }
            case "right":
                {
                    RabbitMoveRight();
                    break;
                }
        }
    }

    private void MovingDone()
    {
        isMoving = false;
        if (isWin)
        {
            levelManager.WinLevelNotification();
        }
        else
        {
            if (levelManager.limitMoveTurn <= 0)
            {
                levelManager.LoseGameNotification();
            }
        }
    }

    public void DevModeUpdateDataMatrix()
    {
        for(int i=0;i<listCell.Count;i++)
        {
            CellController cellController = listCell[i].GetComponent<CellController>();
            if (cellController.CellID != Matrix[cellController.posX, cellController.posY])
            {
                Matrix[cellController.posX, cellController.posY]=cellController.CellID;
            }
        }
    }

    public void RabbitMoveUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(rabbit.transform.position, Vector2.up);
        if (hit.collider != null)
        {
            Vector2 target = new Vector2(hit.transform.position.x, hit.transform.position.y);
            if (hit.transform.GetComponent<CellController>().CellID != "home")
            {
                target.y -= 0.93f;

            }
            else
            {
                isWin = true;
            }
            Vector2 currentPos = transform.position;
            if (target == currentPos)
            {
                return;
            }
            rabbit.transform.DOMove(target, timeToMove);
        }
    }
    public void RabbitMoveDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(rabbit.transform.position, Vector2.down);
        if (hit.collider != null)
        {
            Vector2 target = new Vector2(hit.transform.position.x, hit.transform.position.y);

            if (hit.transform.GetComponent<CellController>().CellID != "home")
            {
                target.y += 0.93f;

            }
            else
            {
                isWin = true;
            }
            Vector2 currentPos = transform.position;
            if (target == currentPos)
            {
                return;
            }
            rabbit.transform.DOMove(target, timeToMove);
        }
    }
    public void RabbitMoveLeft()
    {
        RaycastHit2D hit = Physics2D.Raycast(rabbit.transform.position, Vector2.left);
        if (hit.collider != null)
        {
            Vector2 target = new Vector2(hit.transform.position.x, hit.transform.position.y);
            if (hit.transform.GetComponent<CellController>().CellID != "home")
            {
                target.x += 0.83f;

            }
            else
            {
                isWin = true;
            }
            Vector2 currentPos = transform.position;
            if (target == currentPos)
            {
                return;
            }
            rabbit.transform.DOMove(target, timeToMove);
        }
    }
    public void RabbitMoveRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(rabbit.transform.position, Vector2.right);
        if (hit.collider != null)
        {
            Vector2 target = new Vector2(hit.transform.position.x, hit.transform.position.y);
            if (hit.transform.GetComponent<CellController>().CellID != "home")
            {
                target.x -= 0.83f;
            }
            else
            {
                isWin = true;
            }
            Vector2 currentPos = transform.position;
            if (target == currentPos)
            {
                return;
            }
            rabbit.transform.DOMove(target, timeToMove);
        }
    }

}
