using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsMovement : MonoBehaviour
{
    private string[,] Matrix;
    private int totalCol,totalRow;
    private List<GameObject> listCell;
    private void Start()
    {
        totalCol = gameObject.GetComponent<ListCellController>().col;
        totalRow= gameObject.GetComponent<ListCellController>().row;
        Matrix = gameObject.GetComponent<ListCellController>().Matrix;
        listCell= gameObject.GetComponent<ListCellController>().listCell;
    }
    public void MoveTopCellNull(GameObject cell,bool isLimitCenter)
    {
        CellController cellController = cell.GetComponent<CellController>();
        
        while (Matrix[cellController.posX, cellController.posY + 1] != "0")
        {
            if (isLimitCenter)
            {
               if( cellController.posY + 1 >= totalRow * 0.5f)
                {
                    break;
                }
            }
            CellController cellNeedToChange = gameObject.transform.GetChild((cellController.posY+1) * totalCol + cellController.posX).GetComponent<CellController>();
            // Đổi chỗ trong ma trận
            var temp = Matrix[cellController.posX, cellController.posY + 1];
            Matrix[cellController.posX, cellController.posY + 1] = Matrix[cellController.posX, cellController.posY];
            Matrix[cellController.posX, cellController.posY] = temp;

            // Đổi chỗ trong ListCell
            GameObject gameObjectTemp = listCell[cell.transform.GetSiblingIndex()];
            listCell[cell.transform.GetSiblingIndex()] = listCell[cellNeedToChange.transform.GetSiblingIndex()];
            listCell[cellNeedToChange.transform.GetSiblingIndex()] = gameObjectTemp;

            // Đổi chỗ trong hierachy

            int indexTemp = cell.transform.GetSiblingIndex();
            cell.transform.SetSiblingIndex(cellNeedToChange.transform.GetSiblingIndex());
            cellNeedToChange.transform.SetSiblingIndex(indexTemp);


            // Đổi chỗ trong cell controller

            int posTemp = cellController.posY;
            cellController.posY = cellNeedToChange.posY;
            cellNeedToChange.posY = posTemp;
        }
    }

    public void MoveBottomCellNull(GameObject cell, bool isLimitCenter)
    {
        CellController cellController = cell.GetComponent<CellController>();
        while (Matrix[cellController.posX, cellController.posY - 1] != "0")
        {
            if (isLimitCenter)
            {
                if (cellController.posY - 1 < totalRow * 0.5f)
                {
                    break;
                }
            }
            CellController cellNeedToChange = gameObject.transform.GetChild((cellController.posY - 1) * totalCol + cellController.posX).GetComponent<CellController>();
            // Đổi chỗ trong ma trận
            var temp = Matrix[cellController.posX, cellController.posY - 1];
            Matrix[cellController.posX, cellController.posY - 1] = Matrix[cellController.posX, cellController.posY];
            Matrix[cellController.posX, cellController.posY] = temp;


            // Đổi chỗ trong ListCell
            GameObject gameObjectTemp = listCell[cell.transform.GetSiblingIndex()];
            listCell[cell.transform.GetSiblingIndex()] = listCell[cellNeedToChange.transform.GetSiblingIndex()];
            listCell[cellNeedToChange.transform.GetSiblingIndex()] = gameObjectTemp;

            // Đổi chỗ trong hierachy

            int indexTemp = cell.transform.GetSiblingIndex();
            cell.transform.SetSiblingIndex(cellNeedToChange.transform.GetSiblingIndex());
            cellNeedToChange.transform.SetSiblingIndex(indexTemp);

            // Đổi chỗ trong cell controller

            int posTemp = cellController.posY;
            cellController.posY = cellNeedToChange.posY;
            cellNeedToChange.posY = posTemp;
        }
    }

    public void MoveLeftCellNull(GameObject cell,bool isLimitCenter)
    {

        CellController cellController = cell.GetComponent<CellController>();
        while (Matrix[cellController.posX - 1, cellController.posY] != "0")
        {
            if (isLimitCenter)
            {
                if (cellController.posX - 1 < totalCol * 0.5f)
                {
                    break;
                }
            }
            CellController cellNeedToChange =
                gameObject.transform.GetChild((cellController.posY) * totalCol + cellController.posX - 1).GetComponent<CellController>();
            // Đổi chỗ trong ma trận
            var temp = Matrix[cellController.posX - 1, cellController.posY];
            Matrix[cellController.posX - 1, cellController.posY] = Matrix[cellController.posX, cellController.posY];
            Matrix[cellController.posX, cellController.posY] = temp;

            // Đổi chỗ trong ListCell
            GameObject gameObjectTemp = listCell[cell.transform.GetSiblingIndex()];
            listCell[cell.transform.GetSiblingIndex()] = listCell[cellNeedToChange.transform.GetSiblingIndex()];
            listCell[cellNeedToChange.transform.GetSiblingIndex()] = gameObjectTemp;

            // Đổi chỗ trong hierachy

            int indexTemp = cell.transform.GetSiblingIndex();
            cell.transform.SetSiblingIndex(cellNeedToChange.transform.GetSiblingIndex());
            cellNeedToChange.transform.SetSiblingIndex(indexTemp);

            // Đổi chỗ trong cell controller

            int posTemp = cellController.posX;
            cellController.posX = cellNeedToChange.posX;
            cellNeedToChange.posX = posTemp;
        }
    }

    public void MoveRightCellNull(GameObject cell,bool isLimitCenter)
    {

        CellController cellController = cell.GetComponent<CellController>();
        while (Matrix[cellController.posX + 1, cellController.posY] != "0")
        {

            if (isLimitCenter)
            {
                if (cellController.posX + 1 >= totalCol * 0.5f)
                {
                    break;
                }
            }

            CellController cellNeedToChange =
                gameObject.transform.GetChild((cellController.posY) * totalCol + cellController.posX + 1).GetComponent<CellController>();
            // Đổi chỗ trong ma trận
            var temp = Matrix[cellController.posX + 1, cellController.posY];
            Matrix[cellController.posX + 1, cellController.posY] = Matrix[cellController.posX, cellController.posY];
            Matrix[cellController.posX, cellController.posY] = temp;

            // Đổi chỗ trong ListCell
            GameObject gameObjectTemp = listCell[cell.transform.GetSiblingIndex()];
            listCell[cell.transform.GetSiblingIndex()] = listCell[cellNeedToChange.transform.GetSiblingIndex()];
            listCell[cellNeedToChange.transform.GetSiblingIndex()] = gameObjectTemp;

            // Đổi chỗ trong hierachy

            int indexTemp = cell.transform.GetSiblingIndex();
            cell.transform.SetSiblingIndex(cellNeedToChange.transform.GetSiblingIndex());
            cellNeedToChange.transform.SetSiblingIndex(indexTemp);

            // Đổi chỗ trong cell controller

            int posTemp = cellController.posX;
            cellController.posX = cellNeedToChange.posX;
            cellNeedToChange.posX = posTemp;
        }
    }
}
