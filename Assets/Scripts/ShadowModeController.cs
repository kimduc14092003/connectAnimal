using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShadowModeController : MonoBehaviour
{
    public LineController lineController;
    public string[,] Matrix;
    public List<GameObject> listCell;
    public int colPlay, rowPlay, col, row;

    public int numOfDifferentCell;

    public float delayTimeToDeleteCell;

    public ShadowModeLevelManager levelManager;

    public int limitCellAdjacent;
    private int countCellAdjacent=0;

    public List<GameObject> cellsShadow;
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
    private List<GameObject> listCellNeedToSpawn;
    private Vector3[,] listPos;

    
    private void Awake()
    {
        algorithm=GetComponent<Algorithm>();
        layoutGroup = GetComponent<GridLayoutGroup>();
        col = colPlay + 2;
        row = rowPlay + 2;
        Matrix = new string[col,row];
        listPos = new Vector3[col, row];
        //set level cho random mode
        randomLevel = Random.Range(0, 8);
    }

    private void Start()
    {
        float panelWidth = GetComponent<RectTransform>().rect.width;
        float panelHeight = GetComponent<RectTransform>().rect.height;
        layoutGroup.cellSize = new Vector2(panelWidth / colPlay, (panelWidth / colPlay) * 1.25f);
        if ((panelWidth / colPlay) * 1.25f * rowPlay >= panelHeight)
        {
            layoutGroup.cellSize = new Vector2((panelHeight / rowPlay) * 0.8f, panelHeight / rowPlay);
        }
        layoutGroup.constraintCount = row;
        listCell = new List<GameObject>();
       // currentLevel = levelManager.currentLevel;
        SpawnDefaultCell();

    }


    private void SpawnDefaultCell()
    {
        // sinh các ô cần đổ dữ liệu
        listCellNeedToSpawn = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cell = transform.GetChild(i).gameObject;
            CellController cellController = cell.GetComponent<CellController>();
            if (cellController.CellID == "spawn")
            {
                listCellNeedToSpawn.Add(cell);
            }
        }

        //Đếm các ô còn lại
        cellRemain = listCellNeedToSpawn.Count;

        // lấy danh sách các ô sẽ có trong danh sách tất cả các ô
        List<GameObject> tempCells = new List<GameObject>();

        //Lấy danh sách các shadow cell 
        List<GameObject> tempShadowCells=new List<GameObject>();

        for(int i = 0; i < numOfDifferentCell; i++)
        {
            int randomValue = Random.Range(0, cells.Count);
            tempCells.Add(cells[randomValue]);
            cells.Remove(cells[randomValue]);

            tempShadowCells.Add(cellsShadow[randomValue]);
            cellsShadow.Remove(cellsShadow[randomValue]);
        }
        //Tạo ra danh sách tạm để sinh ra các ô 
        List<GameObject> listCellTemp = new List<GameObject>();


        //Tạo một danh sách tạm để lấy random từ danh sách này ra 
        List<GameObject> tempListCell = new List<GameObject>();
        tempListCell.AddRange(tempCells);

        //Tao danh sach tam shadow cell
        List<GameObject> tempListShadowCell = new List<GameObject>();
        tempListShadowCell.AddRange(tempShadowCells);

        int listCellNeedToSpawnCount = listCellNeedToSpawn.Count;
        for (int i = 0; i < listCellNeedToSpawnCount/2; i++)
        {
            int randomValue = Random.Range(0, tempListCell.Count);
           
            GameObject spawnCell = tempListCell[randomValue];
            GameObject spawnCellShadow = tempListShadowCell[randomValue];
           
            tempListCell.Remove(spawnCell);
            tempListShadowCell.Remove(spawnCellShadow);
            
            // Nếu danh sách rỗng thì lại sao chép từ mảng ban đầu
            if(tempListCell.Count <= 0)
            {
                tempListCell.AddRange(tempCells);
                tempListShadowCell.AddRange(tempShadowCells);
            }

            listCellTemp.Add(spawnCell);
            listCellTemp.Add(spawnCellShadow);
            
        }
        // Danh sách chứa các ô cần thêm sinh ô
        for(int i=0;i< listCellNeedToSpawnCount; i++)
        {
            int randomValue = Random.Range(0, listCellTemp.Count);
            GameObject cell= Instantiate(listCellTemp[randomValue],transform);
            cell.transform.SetSiblingIndex(listCellNeedToSpawn[i].transform.GetSiblingIndex());
            CellController cellController = cell.GetComponent<CellController>();
            cellController.listCellController = gameObject;
            Destroy(listCellNeedToSpawn[i]);
            listCellNeedToSpawn[i] = cell;
            listCellTemp.RemoveAt(randomValue);
        }


        //Thêm toàn bộ vào ma trận gameobject và thêm giá trị tọa độ cho cell
        Invoke("AddValueToMatrix", 0.1f);
    }

    private void AddValueToMatrix()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject cell = transform.GetChild(i).gameObject;
            CellController cellController = cell.GetComponent<CellController>();
            cellController.posX = cell.transform.GetSiblingIndex() % col;
            cellController.posY = cell.transform.GetSiblingIndex() / col;

            listCell.Add(cell);
            Matrix[cellController.posX, cellController.posY] = cellController.CellID;
        }

        if (!FindCellToScore())
        {
            ShuffleAllCell(0);
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
            string id1= CellSelected1.GetComponent<CellController>().CellID.Substring(0, CellSelected1.GetComponent<CellController>().CellID.Length - 1);
            string id2= CellSelected2.GetComponent<CellController>().CellID.Substring(0, CellSelected2.GetComponent<CellController>().CellID.Length - 1);
            if ( id1== id2&& CellSelected1.GetComponent<CellController>().CellID !=CellSelected2.GetComponent<CellController>().CellID)
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
            if (listCell[i].GetComponent<CellController>().CellID != "0"&& listCell[i].GetComponent<CellController>().CellID !="wall")
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
            if (cellController.CellID != "0"&&cellController.CellID!="wall")
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
                cellController.gameObject.transform.SetSiblingIndex(i);
                if (cellController.CellID == "wall") continue;
                CellAnimMoveToPos(listCell[i], listPos[cellController.posX, cellController.posY]);
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
            if (listCell[i].GetComponent<CellController>().CellID != "0"&& listCell[i].GetComponent<CellController>().CellID!="wall")
            {
                for(int j = i + 1; j < listCell.Count; j++)
                {
                    CellController cellController1 = listCell[j].GetComponent<CellController>();
                    CellController cellController2 = listCell[i].GetComponent<CellController>();
                    string id1 = cellController1.CellID.Substring(0, cellController1.CellID.Length - 1);
                    string id2 = cellController2.CellID.Substring(0, cellController2.CellID.Length - 1);
                    if (id1 == id2 && cellController1.CellID != cellController2.CellID)
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
        if (hintCells.Length == 0)
        {
            print("Error to get hint cell!");
            return;
        }
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
