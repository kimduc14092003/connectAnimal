using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EndlessModeController : MonoBehaviour
{
    public LineController lineController;
    public string[,] Matrix;
    public List< List<GameObject>> listCell;
    public int colPlay, rowPlay, col, row;

    public int numOfDifferentCell;

    public float delayTimeToDeleteCell;

    public LevelManagerEndless levelManager;

    public int limitCellAdjacent;

    public GameObject cellAnim1, cellAnim2;

    [SerializeField]
    private List<CellController> spawnDefaultCell;
    [SerializeField]
    private List<GameObject> listCellPrepare;
    private int countCellAdjacent = 0;

    [SerializeField]
    private List<GameObject> cells;

    [SerializeField]
    private GameObject wallCell;
    private GridLayoutGroup layoutGroup;
    private GameObject CellSelected1, CellSelected2;
    private AlgorithmEndless algorithm;
    private int cellRemain;
    private int currentLevel;
    private int spawnMode;
    private GameObject[] hintCells;

    private Vector3[,] listPos;
    List<GameObject> tempListCell;
    private void Awake()
    {
        levelManager=transform.parent.GetComponent<LevelManagerEndless>();
        algorithm = GetComponent<AlgorithmEndless>();
        layoutGroup = GetComponent<GridLayoutGroup>();
        colPlay = 24;
        rowPlay = 8;
        col = colPlay + 2;
        row = rowPlay + 2;
        Matrix = new string[col, row];
        listPos = new Vector3[col, row];
        cellRemain = colPlay * rowPlay;
        spawnMode = 1;

        //Test chỉnh cell size
       /* BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector3 size = collider.bounds.size;
        print(size);*/
        //layoutGroup.cellSize = new Vector2(size.x/col,size.y/row);

    }

    private void Start()
    {
        float panelWidth= GetComponent<RectTransform>().rect.width;
        float panelHeight = GetComponent<RectTransform>().rect.height;
        float ratioWidthHeight=panelWidth/panelHeight;
        if (ratioWidthHeight > 2)
        {
            layoutGroup.cellSize =new Vector2( panelWidth / col, (panelWidth / col) * 1.2f);
        }
        else
        {
            layoutGroup.cellSize = new Vector2(panelWidth / colPlay, (panelWidth / colPlay) * 1.25f);
        }
        layoutGroup.constraintCount = row;
        listCell = new List<List<GameObject>>();
        for(int i = 0; i < row; i++)
        {
            List<GameObject> cells = new List<GameObject>();
            for(int j = 0; j < col; j++)
            {
                cells.Add(wallCell);
            }
            listCell.Add(cells);
        }

        SpawnDefaultCell();

        //Tắt layout group
        //Invoke("TurnOffLayoutGroup",0.1f);
    }

    private void TurnOffLayoutGroup()
    {
        gameObject.GetComponent<GridLayoutGroup>().enabled = false;
    }

    private void PrepareCell(List<GameObject> list)
    {
        listCellPrepare.AddRange(list);
        listCellPrepare.AddRange(list);
    }

    private void SpawnDefaultCell()
    {
        //Thêm toàn bộ child vào listCell
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                GameObject cell = gameObject.transform.GetChild(i * col + j).gameObject;
                CellController cellController = cell.GetComponent<CellController>();
                cellController.posX = j;
                cellController.posY = i;
                Matrix[j, i] = cellController.CellID;
                listCell[i][j] = cell;
                listPos[j,i]=cell.transform.position;
            }
        }

        // Lấy ngẫu nhiên các loại ô vào tempCells
        List<GameObject> tempCells = new List<GameObject>();
        for (int i = 0; i < numOfDifferentCell; i++)
        {
            int randomValue = Random.Range(0, cells.Count);
            tempCells.Add(cells[randomValue]);
            cells.Remove(cells[randomValue]);
        }


        //Tạo một danh sách tạm để lấy random từ danh sách này ra 
        tempListCell = new List<GameObject>();
        tempListCell.AddRange(tempCells);

        PrepareCell(tempListCell);
        

        for(int i = 0; i < spawnDefaultCell.Count; i++)
        {
            int a = Random.Range(0, 101);
            // tỷ lệ random 75% sẽ spawn ra ô
            if (a <= 75)
            {
                int randomValue = Random.Range(0, listCellPrepare.Count);
                GameObject cell = Instantiate(listCellPrepare[randomValue],transform);
                cell.transform.SetSiblingIndex(spawnDefaultCell[i].transform.GetSiblingIndex());
                
                CellController cellController= cell.GetComponent<CellController>();
                cellController.listCellController = gameObject;
                cellController.posX = spawnDefaultCell[i].posX;
                cellController.posY = spawnDefaultCell[i].posY;
                listCell[cellController.posY][cellController.posX] = cell;
                Destroy(spawnDefaultCell[i].gameObject);

                listCellPrepare.RemoveAt(randomValue);
                if(listCellPrepare.Count== 0)
                {
                    PrepareCell(tempListCell);
                }
                
            }
        }
    }

    public void MoveListCell(int moveX,int mode)
    {
        spawnMode = mode;
        if (mode == 1)
        {
            for(int i = 0; i < listCell.Count; i++)
            {
                for(int j=0;j< listCell[i].Count;j++)
                {
                    CellController cellController = listCell[i][j].GetComponent<CellController>();
                    if (cellController.CellID != "0" && cellController.CellID != "wall")
                    {
                        //listCell[i][j+moveX].GetComponent<CellController>().posX -= moveX;
                        GameObject cellNeedChange = listCell[i][j + moveX];
                        if (cellNeedChange.GetComponent<CellController>().CellID == "wall")
                        {
                            Image image = cellNeedChange.GetComponent<Image>();
                            Color color = image.color; // Lấy giá trị màu hiện tại của đối tượng
                            color.a = 0;
                            image.color = color;

                            //hiển thị màn hình thua game
                            Invoke("LoseGameManager", 0.3f);
                        }
                        //Đổi chỗ trong ma trận
                        GameObject temp = listCell[cellController.posY][cellController.posX];
                   
                        listCell[cellController.posY][cellController.posX] = listCell[cellController.posY][cellController.posX+ moveX];
                        listCell[cellController.posY][cellController.posX + moveX] = temp;
                    
                        cellController.posX += moveX;
                        //đổi chỗ trong hierachy
                        cellNeedChange.transform.SetSiblingIndex(cellController.gameObject.transform.GetSiblingIndex());
                        cellController.gameObject.transform.SetSiblingIndex(cellController.posY*col+cellController.posX);
                    
                    }
                }
            }
        } else
        if (mode == 2)
        {
            for (int i = 0; i < listCell.Count; i++)
            {
                for (int j = listCell[i].Count-1; j >= 0; j--)
                {
                    CellController cellController = listCell[i][j].GetComponent<CellController>();
                    if (cellController.CellID != "0" && cellController.CellID != "wall")
                    {
                        //listCell[i][j+moveX].GetComponent<CellController>().posX -= moveX;
                        GameObject cellNeedChange = listCell[i][j + moveX];
                        if (cellNeedChange.GetComponent<CellController>().CellID == "wall")
                        {
                            Image image = cellNeedChange.GetComponent<Image>();
                            Color color = image.color; // Lấy giá trị màu hiện tại của đối tượng
                            color.a = 0;
                            image.color = color;

                            //hiển thị màn hình thua game
                             Invoke("LoseGameManager", 0.3f);
                        }
                        //Đổi chỗ trong ma trận
                        GameObject temp = listCell[cellController.posY][cellController.posX];

                        listCell[cellController.posY][cellController.posX] = listCell[cellController.posY][cellController.posX + moveX];
                        listCell[cellController.posY][cellController.posX + moveX] = temp;

                        cellController.posX += moveX;
                        //đổi chỗ trong hierachy
                        cellNeedChange.transform.SetSiblingIndex(cellController.gameObject.transform.GetSiblingIndex());
                        cellController.gameObject.transform.SetSiblingIndex(cellController.posY * col + cellController.posX);

                    }
                }
            }
        }
        else if (mode == 3)
        {
            for (int i = 0; i < listCell.Count; i++)
            {
                if (i %2!=0)
                {
                    for (int j = 0; j < listCell[i].Count; j++)
                    {
                        CellController cellController = listCell[i][j].GetComponent<CellController>();
                        if (cellController.CellID != "0" && cellController.CellID != "wall")
                        {
                            int moveX2 = moveX;
                            if (i%2 !=0)
                            {
                                moveX2 = -moveX;
                            }

                            //listCell[i][j+moveX].GetComponent<CellController>().posX -= moveX;
                            GameObject cellNeedChange = listCell[i][j + moveX2];
                            if (cellNeedChange.GetComponent<CellController>().CellID == "wall")
                            {
                                Image image = cellNeedChange.GetComponent<Image>();
                                Color color = image.color; // Lấy giá trị màu hiện tại của đối tượng
                                color.a = 0;
                                image.color = color;

                                //hiển thị màn hình thua game
                                Invoke("LoseGameManager", 0.3f);
                            }
                            //Đổi chỗ trong ma trận
                            GameObject temp = listCell[cellController.posY][cellController.posX];

                            listCell[cellController.posY][cellController.posX] = listCell[cellController.posY][cellController.posX + moveX2];
                            listCell[cellController.posY][cellController.posX + moveX2] = temp;

                            cellController.posX += moveX2;
                            //đổi chỗ trong hierachy
                            cellNeedChange.transform.SetSiblingIndex(cellController.gameObject.transform.GetSiblingIndex());
                            cellController.gameObject.transform.SetSiblingIndex(cellController.posY * col + cellController.posX);

                        }
                    }
                }
                else
                {
                    for (int j = listCell[i].Count - 1; j >= 0; j--)
                    {
                        CellController cellController = listCell[i][j].GetComponent<CellController>();
                        if (cellController.CellID != "0" && cellController.CellID != "wall")
                        {
                            int moveX2 = moveX;
                            if (i % 2 != 0)
                            {
                                moveX2 = -moveX;
                            }

                            //listCell[i][j+moveX].GetComponent<CellController>().posX -= moveX;
                            GameObject cellNeedChange = listCell[i][j + moveX2];
                            if (cellNeedChange.GetComponent<CellController>().CellID == "wall")
                            {
                                Image image = cellNeedChange.GetComponent<Image>();
                                Color color = image.color; // Lấy giá trị màu hiện tại của đối tượng
                                color.a = 0;
                                image.color = color;

                                //hiển thị màn hình thua game
                                Invoke("LoseGameManager", 0.3f);
                            }
                            //Đổi chỗ trong ma trận
                            GameObject temp = listCell[cellController.posY][cellController.posX];

                            listCell[cellController.posY][cellController.posX] = listCell[cellController.posY][cellController.posX + moveX2];
                            listCell[cellController.posY][cellController.posX + moveX2] = temp;

                            cellController.posX += moveX2;
                            //đổi chỗ trong hierachy
                            cellNeedChange.transform.SetSiblingIndex(cellController.gameObject.transform.GetSiblingIndex());
                            cellController.gameObject.transform.SetSiblingIndex(cellController.posY * col + cellController.posX);

                        }
                    }
                }
            }
        }
        else if (mode == 4)
        {
            for (int i = 0; i < listCell.Count; i++)
            {
                if(i >= row / 2)
                {
                 for (int j = 0; j < listCell[i].Count; j++)
                    {
                    CellController cellController = listCell[i][j].GetComponent<CellController>();
                    if (cellController.CellID != "0" && cellController.CellID != "wall")
                    {
                        int moveX2 = moveX;
                        if (cellController.posY>=row/2)
                        {
                            moveX2 = -moveX;
                        }

                        //listCell[i][j+moveX].GetComponent<CellController>().posX -= moveX;
                        GameObject cellNeedChange = listCell[i][j + moveX2];
                        if (cellNeedChange.GetComponent<CellController>().CellID == "wall")
                        {
                            Image image = cellNeedChange.GetComponent<Image>();
                            Color color = image.color; // Lấy giá trị màu hiện tại của đối tượng
                            color.a = 0;
                            image.color = color;

                            //hiển thị màn hình thua game
                            Invoke("LoseGameManager", 0.3f);
                        }
                        //Đổi chỗ trong ma trận
                        GameObject temp = listCell[cellController.posY][cellController.posX];

                        listCell[cellController.posY][cellController.posX] = listCell[cellController.posY][cellController.posX + moveX2];
                        listCell[cellController.posY][cellController.posX + moveX2] = temp;

                        cellController.posX += moveX2;
                        //đổi chỗ trong hierachy
                        cellNeedChange.transform.SetSiblingIndex(cellController.gameObject.transform.GetSiblingIndex());
                        cellController.gameObject.transform.SetSiblingIndex(cellController.posY * col + cellController.posX);

                    }
                    }
                }
                else
                {
                    for (int j = listCell[i].Count-1; j >=0; j--)
                    {
                        CellController cellController = listCell[i][j].GetComponent<CellController>();
                        if (cellController.CellID != "0" && cellController.CellID != "wall")
                        {
                            int moveX2 = moveX;
                            if (cellController.posY >= row / 2)
                            {
                                moveX2 = -moveX;
                            }

                            //listCell[i][j+moveX].GetComponent<CellController>().posX -= moveX;
                            GameObject cellNeedChange = listCell[i][j + moveX2];
                            if (cellNeedChange.GetComponent<CellController>().CellID == "wall")
                            {
                                Image image = cellNeedChange.GetComponent<Image>();
                                Color color = image.color; // Lấy giá trị màu hiện tại của đối tượng
                                color.a = 0;
                                image.color = color;

                                //hiển thị màn hình thua game
                                Invoke("LoseGameManager", 0.3f);
                            }
                            //Đổi chỗ trong ma trận
                            GameObject temp = listCell[cellController.posY][cellController.posX];

                            listCell[cellController.posY][cellController.posX] = listCell[cellController.posY][cellController.posX + moveX2];
                            listCell[cellController.posY][cellController.posX + moveX2] = temp;

                            cellController.posX += moveX2;
                            //đổi chỗ trong hierachy
                            cellNeedChange.transform.SetSiblingIndex(cellController.gameObject.transform.GetSiblingIndex());
                            cellController.gameObject.transform.SetSiblingIndex(cellController.posY * col + cellController.posX);

                        }
                    }
                }
            }
        }
        SetPosXAllCell();
        AddNewCellAfterMoving();
    }

    private void LoseGameManager()
    {
        levelManager.LoseGameNotification();
        gameObject.SetActive(false);
    }

    private void SetPosXAllCell()
    {
        for (int i = 0; i < listCell.Count; i++)
        {
            for (int j = 0; j < listCell[i].Count; j++)
            {
                CellController cellController = listCell[i][j].GetComponent<CellController>();
                cellController.posX = listCell[i][j].transform.GetSiblingIndex() % col;
            }
        }

        //Sửa lại vị trí danh sách
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject cell= transform.GetChild(i).gameObject;
            CellController cellController= cell.GetComponent<CellController>();
            listCell[cellController.posY][cellController.posX] = cell;
          switch (spawnMode)
            {
                case 1:
                    {
                        if (cellController.posX <= 6 && cellController.CellID != "0" && cellController.CellID != "wall")
                        {
                            WarningCell(cell);
                        }
                        break;
                    }
                case 2:
                    {
                        if (cellController.posX >= col-7 && cellController.CellID != "0" && cellController.CellID != "wall")
                        {
                            WarningCell(cell);
                        }
                        break;
                    }
                case 3:
                    {

                        if (cellController.posY % 2 != 0)
                        {
                            if (cellController.posX <= 6 && cellController.CellID != "0" && cellController.CellID != "wall")
                            {
                                WarningCell(cell);
                            }
                        }
                        else
                        {
                            if (cellController.posX >= col - 7 && cellController.CellID != "0" && cellController.CellID != "wall")
                            {
                                WarningCell(cell);
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        if (cellController.posY >= row / 2)
                        {
                            if (cellController.posX <= 6 && cellController.CellID != "0" && cellController.CellID != "wall")
                            {
                                WarningCell(cell);
                            }
                            break;
                        }
                        else
                        {
                            if (cellController.posX >= col - 7 && cellController.CellID != "0" && cellController.CellID != "wall")
                            {
                                WarningCell(cell);
                            }
                        }
                        break;
                    }
            }
        }
    }

    private void AddNewCellAfterMoving()
    {
        switch (spawnMode)
        {
            case 1:
                {
                    for (int i = 0; i < listCell.Count; i++)
                    {
                        for (int j = 0; j < listCell[i].Count; j++)
                        {
                            CellController cellController = listCell[i][j].GetComponent<CellController>();
                            if (cellController.posY >= 1 && cellController.posY < listCell.Count - 1 && cellController.posX >= listCell[i].Count - 3)
                            {
                                int a = Random.Range(0, 101);
                                // tỷ lệ random 75% sẽ spawn ra ô
                                if (a <= 75)
                                {
                                    int randomValue = Random.Range(0, listCellPrepare.Count);
                                    GameObject cell = Instantiate(listCellPrepare[randomValue], transform);
                                    cell.transform.SetSiblingIndex(listCell[i][j].transform.GetSiblingIndex());

                                    CellController cellController2 = cell.GetComponent<CellController>();
                                    cellController2.listCellController = gameObject;
                                    cellController2.posX = listCell[i][j].GetComponent<CellController>().posX;
                                    cellController2.posY = listCell[i][j].GetComponent<CellController>().posY;

                                    Destroy(listCell[i][j]);
                                    listCell[i][j] = cell;

                                    listCellPrepare.RemoveAt(randomValue);
                                    if (listCellPrepare.Count == 0)
                                    {
                                        PrepareCell(tempListCell);
                                    }

                                }
                            }
                        }
                    }
                    break;
                }
            case 2:
                {
                    for (int i = 0; i < listCell.Count; i++)
                    {
                        for (int j = 0; j < listCell[i].Count; j++)
                        {
                            CellController cellController = listCell[i][j].GetComponent<CellController>();
                            if (cellController.posY >= 1 && cellController.posY < listCell.Count - 1 && cellController.posX < 3)
                            {
                                int a = Random.Range(0, 101);
                                // tỷ lệ random 75% sẽ spawn ra ô
                                if (a <= 75)
                                {
                                    int randomValue = Random.Range(0, listCellPrepare.Count);
                                    GameObject cell = Instantiate(listCellPrepare[randomValue], transform);
                                    cell.transform.SetSiblingIndex(listCell[i][j].transform.GetSiblingIndex());

                                    CellController cellController2 = cell.GetComponent<CellController>();
                                    cellController2.listCellController = gameObject;
                                    cellController2.posX = listCell[i][j].GetComponent<CellController>().posX;
                                    cellController2.posY = listCell[i][j].GetComponent<CellController>().posY;

                                    Destroy(listCell[i][j]);
                                    listCell[i][j] = cell;

                                    listCellPrepare.RemoveAt(randomValue);
                                    if (listCellPrepare.Count == 0)
                                    {
                                        PrepareCell(tempListCell);
                                    }

                                }
                            }
                        }
                    }
                    break;
                }
            case 3:
                {
                    for (int i = 0; i < listCell.Count; i++)
                    {
                        for (int j = 0; j < listCell[i].Count; j++)
                        {
                            CellController cellController = listCell[i][j].GetComponent<CellController>();
                            if (cellController.posY >= 1 && cellController.posY < listCell.Count - 1 && cellController.CellID!="wall")
                            {
                                if(cellController.posX <3 || cellController.posX >= listCell[i].Count - 3)
                                {
                                    int a = Random.Range(0, 101);
                                    // tỷ lệ random 75% sẽ spawn ra ô
                                    if (a <= 75)
                                    {
                                        int randomValue = Random.Range(0, listCellPrepare.Count);
                                        GameObject cell = Instantiate(listCellPrepare[randomValue], transform);
                                        cell.transform.SetSiblingIndex(listCell[i][j].transform.GetSiblingIndex());

                                        CellController cellController2 = cell.GetComponent<CellController>();
                                        cellController2.listCellController = gameObject;
                                        cellController2.posX = listCell[i][j].GetComponent<CellController>().posX;
                                        cellController2.posY = listCell[i][j].GetComponent<CellController>().posY;

                                        Destroy(listCell[i][j]);
                                        listCell[i][j] = cell;

                                        listCellPrepare.RemoveAt(randomValue);
                                        if (listCellPrepare.Count == 0)
                                        {
                                            PrepareCell(tempListCell);
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                    break;
                }
            case 4:
                {
                    for (int i = 0; i < listCell.Count; i++)
                    {
                        for (int j = 0; j < listCell[i].Count; j++)
                        {
                            CellController cellController = listCell[i][j].GetComponent<CellController>();
                            if (cellController.posY >= 1 && cellController.posY < listCell.Count - 1 && cellController.CellID != "wall")
                            {
                                if (cellController.posX < 3 || cellController.posX >= listCell[i].Count - 3)
                                {
                                    int a = Random.Range(0, 101);
                                    // tỷ lệ random 75% sẽ spawn ra ô
                                    if (a <= 75)
                                    {
                                        int randomValue = Random.Range(0, listCellPrepare.Count);
                                        GameObject cell = Instantiate(listCellPrepare[randomValue], transform);
                                        cell.transform.SetSiblingIndex(listCell[i][j].transform.GetSiblingIndex());

                                        CellController cellController2 = cell.GetComponent<CellController>();
                                        cellController2.listCellController = gameObject;
                                        cellController2.posX = listCell[i][j].GetComponent<CellController>().posX;
                                        cellController2.posY = listCell[i][j].GetComponent<CellController>().posY;

                                        Destroy(listCell[i][j]);
                                        listCell[i][j] = cell;

                                        listCellPrepare.RemoveAt(randomValue);
                                        if (listCellPrepare.Count == 0)
                                        {
                                            PrepareCell(tempListCell);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    break;
                }
        }
    }

    private void WarningCell(GameObject cell)
    {
        if (cell.GetComponent<Image>())
        {
            cell.GetComponent<Image>().DOColor(Color.red, 0.5f).SetLoops(-1,LoopType.Yoyo);
        }
    }

    public void CellClicked(GameObject cellClicked)
    {
        if (CellSelected1 == null)
        {
            CellSelected1 = cellClicked;
            AudioManager.Instance.PlaySFX("click");
        }
        else
        if (cellClicked != CellSelected1) //Kiểm tra người chơi có click lại vào ô vừa chọn không
        {
            CellSelected2 = cellClicked;
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
                    

                    // Cộng điểm khi ăn được ô
                    levelManager.AddCurrentScore(10);

                    //Loại bỏ các phần tử trùng nhau
                    result = result.Distinct().ToArray();

                    //Bỏ gợi ý
                    //levelManager.isFinded = false;


                    //Tạo đường kẻ kết nối các ô đã ăn
                    if (result.Length == 2)
                    {
                        lineController.CreateLine(result[0], result[1]);
                    }
                    else if (result.Length == 3)
                    {
                        lineController.CreateLine(result[0], result[1], result[2]);
                    }
                    else if (result.Length == 4)
                    {
                        lineController.CreateLine(result[0], result[1], result[2], result[3]);
                    }
                    // Kiểm tra xem còn ô nào có thể ăn được nữa không
                    //Invoke("HandleCantPlayWhenScored", delayTimeToDeleteCell + 0.1f);
                    StartCoroutine(DeleteCell(CellSelected1, true));
                    StartCoroutine(DeleteCell(CellSelected2, false));

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

    // Chức năng tìm kiếm ô có thể ăn được
   
    public void OnPointerClickFindFunctional()
    {
        CellController cellController1 = hintCells[0].GetComponent<CellController>();
        CellController cellController2 = hintCells[1].GetComponent<CellController>();
        cellController1.SetHintColor();
        cellController2.SetHintColor();
    }

    IEnumerator DeleteCell(GameObject cell, bool isCell1)
    {

        CellController cellController = cell.GetComponent<CellController>();
        cellController.CellID = "0";
        //cellController.ConnectSuccess(delayTimeToDeleteCell);
        Matrix[cellController.posX, cellController.posY] = "0";
        if (cell) {
            cell.GetComponent<CanvasGroup>().alpha = 0;
        }
        if (cell)
        {
            cell.GetComponent<Button>().interactable = false;
        }
        if (isCell1)
        {
            StartCoroutine(TestDeleteCell1(cellAnim1, cell));
        }
        else
        {
            StartCoroutine(TestDeleteCell1(cellAnim2, cell));
        }

        GameObject nothingGameObject = Instantiate(wallCell,transform);
        nothingGameObject.transform.SetSiblingIndex(cell.transform.GetSiblingIndex());
       
        CellController cellController1=nothingGameObject.GetComponent<CellController>();
        cellController1.posX = cellController.posX;
        cellController1.posY = cellController.posY;

        listCell[cellController.posY][cellController.posX] = nothingGameObject;
        Destroy(cell);
        yield return new WaitForSeconds(delayTimeToDeleteCell);

    }
    IEnumerator TestDeleteCell1(GameObject cellAnim, GameObject cellTarget)
    {
        cellAnim.transform.localScale = Vector3.one;
        cellAnim.GetComponent<CanvasGroup>().alpha = 1.0f;
        cellAnim.transform.position = cellTarget.transform.position;

        cellAnim.transform.GetChild(0).GetComponent<Image>().sprite = cellTarget.transform.GetChild(0).GetComponent<Image>().sprite;
        cellAnim.SetActive(true);
        cellAnim.GetComponent<CellController>().ConnectSuccess(delayTimeToDeleteCell);
        yield return new WaitForSeconds(delayTimeToDeleteCell);
        cellAnim.SetActive(false);

    }


}
