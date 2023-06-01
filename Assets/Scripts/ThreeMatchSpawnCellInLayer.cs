using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeMatchSpawnCellInLayer : MonoBehaviour
{
    private int remainingCellInLayer;
    public List<GameObject> listCellNeedToSpawn;

    private List<GameObject> listCell;
    // Start is called before the first frame update
    void Start()
    {
        listCell = transform.parent.parent.GetComponent<LevelManagerThreeMatch>().listCell;
        SpawnCell();
        Invoke("TurnOffGridLayout", 0.02f);
    }

    private void TurnOffGridLayout()
    {
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.enabled = false;
    }
    private void SpawnCell()
    {
        listCellNeedToSpawn=new List<GameObject>();

        //Lấy tất cả các ô cần spawn đưa vào List
        for (int i = 0;i<transform.childCount;i++)
        {
            GameObject cell=transform.GetChild(i).gameObject;

            if (cell != null)
            {
                CellController cellController = cell.GetComponent<CellController>();
                if (cellController)
                {
                    if(cellController.CellID== "spawn")
                    {
                        listCellNeedToSpawn.Add(cell);
                    }

                }
            }
        }
        //lấy số các ô ở layer
        remainingCellInLayer= listCellNeedToSpawn.Count;

        for(int i = 0; i < listCellNeedToSpawn.Count; i++)
        {
             int randomNumber=Random.Range(0,listCell.Count);
            GameObject cell = listCell[randomNumber];
            cell.transform.SetParent(transform);
            cell.transform.SetSiblingIndex(listCellNeedToSpawn[i].transform.GetSiblingIndex());

            cell.transform.position = listCellNeedToSpawn[i].transform.position;
            /*
             GameObject cell = Instantiate(tempListCell[randomNumber],transform);

             cell.transform.SetSiblingIndex(listCellNeedToSpawn[i].transform.GetSiblingIndex());

             cell.transform.position = listCellNeedToSpawn[i].transform.position;
            */

            //Thay đổi component cellcontroller cho nó
            /* CellControllerThreeMatch cellControllerThreeMatch = cell.GetComponent<CellControllerThreeMatch>();
             cellControllerThreeMatch.levelManager=transform.parent.parent.GetComponent<LevelManagerThreeMatch>();
             cellControllerThreeMatch.listChooseCell = transform.parent.parent.GetComponent<LevelManagerThreeMatch>().listChooseCellGameObject;*/

            DestroyImmediate(listCellNeedToSpawn[i]);
            listCellNeedToSpawn[i] = cell;
            listCell.RemoveAt(randomNumber);
        }


    }

    public void ReCheckCell()
    {
        for (int i = 0; i < listCellNeedToSpawn.Count; i++)
        {
            if (!listCellNeedToSpawn[i])
            {
                continue;
            }
            CellControllerThreeMatch cellControllerThreeMatch = listCellNeedToSpawn[i].GetComponent<CellControllerThreeMatch>();
            if (cellControllerThreeMatch)
            {
                cellControllerThreeMatch.Check();
            }
        }
    }
}
