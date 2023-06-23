using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListCellController : MonoBehaviour
{
    public LineController lineController;
    public string[,] Matrix;
    public List<GameObject> listCell;
    public int colPlay, rowPlay, col, row;

    public int numOfDifferentCell;

    public float delayTimeToDeleteCell;

    public LevelManager levelManager;

    public int limitCellAdjacent;
    private int countCellAdjacent=0;

    [SerializeField]
    private List<GameObject> cells;

    [SerializeField]
    private GameObject wallCell;
    private GridLayoutGroup layoutGroup;
    private GameObject CellSelected1, CellSelected2;
    private Algorithm algorithm;
    private int cellRemain;
    private int currentLevel;
    private int randomLevel;
    private GameObject[] hintCells;

    private Vector3[,] listPos;

    private void Awake()
    {
        if (!levelManager.enabled)
        {
            GetComponent<ListCellController>().enabled = false;
            return;
        }

    }

    private void Start()
    {
        layoutGroup = GetComponent<GridLayoutGroup>();
        col = colPlay + 2;
        row = rowPlay + 2;
        Matrix = new string[col, row];
        listPos = new Vector3[col, row];
        cellRemain = colPlay * rowPlay;
        algorithm = GetComponent<Algorithm>();

        //set level cho random mode
        randomLevel = Random.Range(0, 8);

        float panelWidth = GetComponent<RectTransform>().rect.width;
        float panelHeight = GetComponent<RectTransform>().rect.height;
        layoutGroup.cellSize = new Vector2(panelWidth / colPlay, (panelWidth / colPlay) * 1.25f);
        if ((panelWidth / colPlay) * 1.25f * rowPlay >= panelHeight)
        {
            layoutGroup.cellSize = new Vector2((panelHeight / rowPlay) * 0.8f, panelHeight / rowPlay);
        }
        layoutGroup.constraintCount = row;
        listCell = new List<GameObject>();
        currentLevel = levelManager.currentLevel;
        SpawnDefaultCell();
        Invoke("ShuffleAllIfNothingDefault", Time.deltaTime);
        print(countCellAdjacent);
    }

    private void ShuffleAllIfNothingDefault()
    {
        if (!FindCellToScore())
        {
            ShuffleAllCell(0);
        }
    }


    private void SpawnDefaultCell()
    {
        List<GameObject> tempCells = new List<GameObject>();
        for(int i = 0; i < numOfDifferentCell; i++)
        {
            int randomValue = Random.Range(0, cells.Count);
            tempCells.Add(cells[randomValue]);
            cells.Remove(cells[randomValue]);
        }


        //Tạo một danh sách tạm để lấy random từ danh sách này ra 
        List<GameObject> tempListCell = new List<GameObject>();
        tempListCell.AddRange(tempCells);

        for(int i = 0; i < rowPlay; i++)
        {
            for(int j = 0; j < colPlay/2; j++)
            {
                int randomValue = Random.Range(0, tempListCell.Count);
                GameObject spawnCell = tempListCell[randomValue];
                tempListCell.Remove(spawnCell);

                // Nếu danh sách rỗng thì lại sao chép từ mảng ban đầu
                if(tempListCell.Count <= 0)
                {
                    tempListCell.AddRange(tempCells);
                }

                listCell.Add( spawnCell);
            }
        }
        //Nhân đôi danh sách để số ô không bị lẻ
        listCell= listCell.Concat(listCell).ToList();

        //Tạo ra danh sách tạm để sinh ra các ô 
        List<GameObject> listCellTemp = new List<GameObject>(listCell);

        //Xóa danh sách đi để xíu thêm phần tử tuần tự vào danh sách
        listCell.Clear();
        int listCellTempCount = listCellTemp.Count;

        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                if (i == 0|| i == row - 1||j == 0 || j == col - 1)
                {
                    //Thêm gameObject Wall vào viền bảng
                    GameObject spawnCellWall = Instantiate(wallCell, this.transform);
                    CellController cellController = spawnCellWall.GetComponent<CellController>();
                    cellController.listCellController = this.gameObject;
                    cellController.posX = j;
                    cellController.posY = i;
                    Matrix[j, i] = cellController.CellID;
                    listCell.Add(spawnCellWall);
                }

                else

                { 
                    //Lấy random vị trí trong danh sách phụ để sinh ra cũng như thêm vào ma trận, danh sách chính
                    int randomValue = Random.Range(0, listCellTemp.Count);
                    GameObject spawnCell = Instantiate(listCellTemp[randomValue], this.transform);
                    CellController cellController=spawnCell.GetComponent<CellController>();

                    // Set giá trị ban đầu cho gameObject vừa sinh ra
                    cellController.listCellController = this.gameObject;
                    cellController.posX = j;
                    cellController.posY = i;
                    if (cellController.CellID == Matrix[j, i - 1] || cellController.CellID == Matrix[j - 1, i])
                    {
                        if (countCellAdjacent >= limitCellAdjacent)
                        {
                            j--;
                            Destroy(spawnCell);
                            continue;
                        }
                        else
                        {
                            countCellAdjacent++;
                        }
                    }

                    Matrix[j,i] = cellController.CellID;
                    listCellTemp.RemoveAt(randomValue);
                    listCell.Add(spawnCell);

                }
            }
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
                    cellRemain -= 2;
                    StartCoroutine(DeleteCell(CellSelected1));
                    StartCoroutine(DeleteCell(CellSelected2));
                    StartCoroutine(CheckWinLevel(0.25f));

                    // Cộng điểm khi ăn được ô
                    levelManager.AddCurrentScore(10);

                    //Loại bỏ các phần tử trùng nhau
                    result = result.Distinct().ToArray();

                    //Bỏ gợi ý
                    levelManager.isFinded = false;


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
                    Invoke("HandleCantPlayWhenScored", delayTimeToDeleteCell+0.1f);

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

    private void HandleCantPlayWhenScored()
    {
        // Xử lý khi không còn ô nào có thể ăn 
        if (!FindCellToScore() && cellRemain > 0)
        {
            print("trừ HP!");
            levelManager.HandleMinusHP();
            Invoke("ShuffleAllCell", 1f);
        }
    }

    private void ShuffleAllCell()
    {
        ShuffleAllCell(0.45f);
    }


    // Chức năng xáo trộn các ô còn lại 
    public void ShuffleAllCell(float delayToAnim)
    {
        if (cellRemain <= 0) return;
        //tắt grid layout để thay đổi vị trí các gameobject con vào trung tâm
         gameObject.GetComponent<GridLayoutGroup>().enabled = false;

        //Tạo danh sách tạm chứa các ô còn lại mà người chơi chưa ăn được
        List<GameObject> listCellTemp = new List<GameObject>();

        for (int i = 0; i < listCell.Count; i++)
        {
            if (listCell[i].GetComponent<CellController>().CellID != "0")
            {
                //Lưu tọa độ của ô vào mảng
                listPos[listCell[i].GetComponent<CellController>().posX, listCell[i].GetComponent<CellController>().posY] = listCell[i].transform.position;

                // thêm các ô còn lại mà người chơi chưa ăn được cũng như set pos về trung tâm tạo hiệu ứng
                listCellTemp.Add(listCell[i]);
                CellsAnimWhenShuffle(listCell[i]);
            }
        }
        // tạo array clone lại mảng vì khi xóa GameObject trong scene thì trong List bị mất

        // Lấy random giá trị danh sách tạm thay vào danh sách chính
        for (int i = 0; i < listCell.Count; i++)
        {
            CellController cellController = listCell[i].GetComponent<CellController>();
            if (cellController.CellID != "0")
            {
                int RandomNumber = Random.Range(0, listCellTemp.Count);
                listCell[i] = listCellTemp[RandomNumber];
                listCellTemp.RemoveAt(RandomNumber);
            }

        }

        // Đồng bộ hóa dữ liệu trong ma trận
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                CellController cellController = listCell[i * col + j].GetComponent<CellController>();
                Matrix[j, i] = cellController.CellID;
                cellController.posX = j;
                cellController.posY = i;
            }
        }
        // Đặt lại vị trí chính xác cho ô theo tọa độ
        Invoke("ShuffleComplete", delayToAnim);
    }


    private void ShuffleComplete()
    {

        //Tìm kiếm xem ô nào có thể ăn sau khi xáo trộn

        if (!FindCellToScore())
        {
            print("xáo trộn xong nhưng không có ô nào có thể ăn");
            ShuffleAllCell(0);
        }
        else
        {
            // Set lại thứ tự các gameobject cho khớp với trong listCell
            for (int i = 0; i < listCell.Count; i++)
            {
                CellController cellController = listCell[i].GetComponent<CellController>();
                CellAnimMoveToPos(listCell[i], listPos[cellController.posX, cellController.posY]);
                cellController.gameObject.transform.SetSiblingIndex(i);
            }
            // bật lại grid layout để set lại panel chơi
            Invoke("TurnOnGridLayout", 0.35f);
        }
    }

    private void TurnOnGridLayout()
    {
        gameObject.GetComponent<GridLayoutGroup>().enabled = true;
        levelManager.isShuffle = false;
    }

    private void CellsAnimWhenShuffle(GameObject cell)
    {
        cell.transform.DOMove(Vector3.zero, 0.3f);
    }
    private void CellAnimMoveToPos(GameObject cell, Vector3 target)
    {
        cell.transform.DOMove(target, 0.3f);
    }

    // Chức năng tìm kiếm ô có thể ăn được
    private bool FindCellToScore()
    {
        hintCells = new GameObject[2];
        for(int i=0;i<listCell.Count; i++)
        {
            if (listCell[i].GetComponent<CellController>().CellID != "0")
            {
                for(int j = i + 1; j < listCell.Count; j++)
                {
                    CellController cellController1 = listCell[j].GetComponent<CellController>();
                    CellController cellController2 = listCell[i].GetComponent<CellController>();
                    if (cellController1.CellID== cellController2.CellID)
                    {
                        if (algorithm.CheckResultCell(cellController1.posX, cellController2.posX,
                                                     cellController1.posY, cellController2.posY) !=null)
                        {
                            hintCells[0] = cellController1.gameObject;
                            hintCells[1] = cellController2.gameObject;

                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public void OnPointerClickFindFunctional()
    {
        CellController cellController1 = hintCells[0].GetComponent<CellController>();
        CellController cellController2 = hintCells[1].GetComponent<CellController>();
        cellController1.SetHintColor();
        cellController2.SetHintColor();
    }

    IEnumerator DeleteCell(GameObject cell)
    {

        CellController cellController = cell.GetComponent<CellController>();
        cellController.CellID = "0";
        Matrix[cellController.posX, cellController.posY] = "0";
        cellController.ConnectSuccess(delayTimeToDeleteCell);
        yield return new WaitForSeconds(delayTimeToDeleteCell);

        cell.GetComponent<CanvasGroup>().alpha = 0;
        cell.GetComponent<Button>().interactable = false;
        StartCoroutine(HandleMovementCell(cell,0));
        StartCoroutine(HandleMovementCell(cell, 0.02f));
    }

    IEnumerator HandleMovementCell(GameObject cell,float delay)
    {
        yield return new WaitForSeconds(delay);
        CellsMovement cellsMovement = gameObject.GetComponent<CellsMovement>();
        CellController cellController= cell.GetComponent<CellController>();

        switch (PlayerPrefs.GetString("PlayeMode"))
        {
            case "RelaxRandomMode":
                {
                    switch (randomLevel)
                    {
                        case 1:
                            {
                                cellsMovement.MoveTopCellNull(cell, false);
                                break;
                            }
                        case 2:
                            {

                                cellsMovement.MoveBottomCellNull(cell, false);
                                break;
                            }
                        case 3:
                            {
                                cellsMovement.MoveLeftCellNull(cell, false);
                                break;
                            }
                        case 4:
                            {
                                cellsMovement.MoveRightCellNull(cell, false);
                                break;
                            }
                        case 5:
                            {
                                // giữa dãn ra trên dưới
                                if (cellController.posY < row * 0.5f)
                                {
                                    cellsMovement.MoveTopCellNull(cell, true);
                                }
                                else
                                {
                                    cellsMovement.MoveBottomCellNull(cell, true);
                                }
                                break;
                            }
                        case 6:
                            {
                                // trên dưới gộp vào giữa
                                if (cellController.posY < row * 0.5f)
                                {
                                    cellsMovement.MoveBottomCellNull(cell, false);
                                }
                                else
                                {
                                    cellsMovement.MoveTopCellNull(cell, false);
                                }
                                break;
                            }
                        case 7:
                            {
                                // giữa dãn ra trái phải
                                if (cellController.posX < col * 0.5f)
                                {
                                    cellsMovement.MoveRightCellNull(cell, true);
                                }
                                else
                                {
                                    cellsMovement.MoveLeftCellNull(cell, true);
                                }
                                break;
                            }
                        case 0:
                            {
                                // trái phải gộp vào giữa
                                if (cellController.posX >= col * 0.5f)
                                {
                                    cellsMovement.MoveRightCellNull(cell, false);
                                }
                                else
                                {
                                    cellsMovement.MoveLeftCellNull(cell, false);
                                }
                                break;
                            }
                        default:
                            {
                                // trái phải gộp vào giữa
                                if (cellController.posX >= col * 0.5f)
                                {
                                    cellsMovement.MoveRightCellNull(cell, false);
                                }
                                else
                                {
                                    cellsMovement.MoveLeftCellNull(cell, false);
                                }
                                break;
                            }
                    }
                    break;
                }
            default:
                {
                    switch (currentLevel % 9)
                    {
                        case 1:
                            {
                                break;
                            }
                        case 2:
                            {
                                cellsMovement.MoveTopCellNull(cell, false);
                                break;
                            }
                        case 3:
                            {

                                cellsMovement.MoveBottomCellNull(cell, false);
                                break;
                            }
                        case 4:
                            {
                                cellsMovement.MoveLeftCellNull(cell, false);
                                break;
                            }
                        case 5:
                            {
                                cellsMovement.MoveRightCellNull(cell, false);
                                break;
                            }
                        case 6:
                            {
                                // giữa dãn ra trên dưới
                                if (cellController.posY < row * 0.5f)
                                {
                                    cellsMovement.MoveTopCellNull(cell, true);
                                }
                                else
                                {
                                    cellsMovement.MoveBottomCellNull(cell, true);
                                }
                                break;
                            }
                        case 7:
                            {
                                // trên dưới gộp vào giữa
                                if (cellController.posY < row * 0.5f)
                                {
                                    cellsMovement.MoveBottomCellNull(cell, false);
                                }
                                else
                                {
                                    cellsMovement.MoveTopCellNull(cell, false);
                                }
                                break;
                            }
                        case 8:
                            {
                                // giữa dãn ra trái phải
                                if (cellController.posX < col * 0.5f)
                                {
                                    cellsMovement.MoveRightCellNull(cell, true);
                                }
                                else
                                {
                                    cellsMovement.MoveLeftCellNull(cell, true);
                                }
                                break;
                            }
                        case 0:
                            {
                                // trái phải gộp vào giữa
                                if (cellController.posX >= col * 0.5f)
                                {
                                    cellsMovement.MoveRightCellNull(cell, false);
                                }
                                else
                                {
                                    cellsMovement.MoveLeftCellNull(cell, false);
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }
        }

       
    }

    IEnumerator CheckWinLevel(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (cellRemain <= 0)
        {
            levelManager.WinLevelNotification();
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

}
