using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    private string[,] Matrix; 
    private Vector2Int cell1, cell2;

    private void Start()
    {
        if (gameObject.GetComponent<ListCellController>())
        {
            Matrix=gameObject.GetComponent<ListCellController>().Matrix;

        }
        else if(gameObject.GetComponent<PuzzleModeController>())
        {
            Matrix=gameObject.GetComponent<PuzzleModeController>().Matrix;
        }else if (gameObject.GetComponent<ShadowModeController>())
        {
            Matrix = gameObject.GetComponent<ShadowModeController>().Matrix;
        }
        else if (gameObject.GetComponent<ListCellEscapeController>())
        {
            Matrix = gameObject.GetComponent<ListCellEscapeController>().Matrix;
        }
        if (Matrix == null)
        {
        }
    }

    public Vector2Int[] CheckResultCell(int x1,int x2,int y1,int y2)
    {
        cell1 = new Vector2Int(x1,y1);
        cell2 = new Vector2Int(x2,y2);
        if (x1 == x2)
        {
            if(CheckLineY(x1, y1, y2))
            {
                return new Vector2Int[] { new Vector2Int(x1, y1), new Vector2Int(x2, y2) };
            }
        }
        if(y1 == y2)
        {
            if (CheckLineX(y1, x1, x2))
            {
                return new Vector2Int[] { new Vector2Int(x1, y1), new Vector2Int(x2, y2) };

            }
        }
        Vector2Int[] result= CheckRectX(new Vector2Int(x1,y1),new Vector2Int(x2,y2));
        if (result != null)
        {
            return result;
        }

        result = CheckRectY(new Vector2Int(x1, y1), new Vector2Int(x2, y2));
        if(result != null)
        {
            return result;
        }


        return null;
    }

    

    private bool CheckLineX(int y, int x1, int x2)
    {
        // Kiểm tra 2 ô đã chọn có liền kề nhau không
        if (cell1.y == cell2.y)
        {
            if (cell1.x - cell2.x <= 1 && cell1.x - cell2.x >= -1)

            {
                return true;
            }
        }
        //===============================================
        int min, max;
        if (x1 <= x2)
        {
            min = x1; max = x2;
        }
        else
        {
            min = x2; max = x1;
        }


        for (int x = min; x <= max; x++)
        {
            if (Matrix[x, y] != "0")
            {

                if ( x==min|| x == max)
                {
                    if (y == cell1.y&&x==cell1.x || y == cell2.y&&x==cell2.x)
                    {
                        continue;
                    }
                }

                return false; //Có vật cản theo chiều X
            }
        }
        //Không có vật cản theo chiều X
        return true;
    }
    //42 -> 43
    private bool CheckLineY(int x, int y1, int y2)
    {

        // Kiểm tra 2 ô đã chọn có liền kề nhau không
        if (cell1.x == cell2.x)
        {
            if(cell1.y-cell2.y<=1&& cell1.y - cell2.y >= -1)
            {
                return true;
            }  
        }
        //===============================================
        int min, max;
        if (y1 <= y2)
        {
            min = y1; max = y2;
        }
        else
        {
            min = y2; max = y1;
        }
        //=========================
        for (int y = min; y <= max ; y++)
        {
            if (y==min||y == max)
            {
                if (x == cell1.x&&y==cell1.y || x == cell2.x&&y==cell2.y)
                {
                    continue;
                }
            }
            if (Matrix[x, y] != "0")
            {
                return false; //Có vật cản theo chiều Y
            }
        }
        //Không có vật cản theo chiều Y
        return true;
    }

    //Kiểm tra đường chữ Z
    private Vector2Int[] CheckRectX(Vector2Int p1, Vector2Int p2)
    {
        Vector2Int[] result1 = null; 
        Vector2Int[] result2 = null;
        // Tìm điểm có x nhỏ hơn, lớn hơn
        Vector2Int pMinX = p1, pMaxX = p2;
        if (p1.x > p2.x)
        {
            pMinX = p2;
            pMaxX = p1;
        }

        //======== Code cua tui ==================
        for (int x = pMinX.x; x < Matrix.GetLength(0); x++)
        {
            // check three line
            if (CheckLineX((int)pMinX.y, x, (int)pMinX.x)
                    && CheckLineY(x, (int)pMinX.y, pMaxX.y)
                    && CheckLineX(pMaxX.y, x, (int)pMaxX.x))

            {

/*                print("Rect X: (" + pMinX.x + "," + pMinX.y + ") -> ("
                        + x + "," + pMinX.y + ") -> (" + x + "," + pMaxX.y
                        + ") -> (" + pMaxX.x + "," + pMaxX.y + ")");*/
                // if three line is true return column y
                result1= new Vector2Int[] { pMinX ,new Vector2Int(x,pMinX.y),new Vector2Int(x,pMaxX.y),pMaxX };
                break;
            }
        }
        for (int x = pMinX.x; x >= 0; x--)
        {
            // check three line
            if (CheckLineX((int)pMinX.y, x, (int)pMinX.x)
                    && CheckLineY(x, (int)pMinX.y, pMaxX.y)
                    && CheckLineX(pMaxX.y, x, (int)pMaxX.x))

            {

                /*                print("Rect X: (" + pMinX.x + "," + pMinX.y + ") -> ("
                                        + x + "," + pMinX.y + ") -> (" + x + "," + pMaxX.y
                                        + ") -> (" + pMaxX.x + "," + pMaxX.y + ")");*/
                // if three line is true return column y
                result2 = new Vector2Int[] { pMinX, new Vector2Int(x, pMinX.y), new Vector2Int(x, pMaxX.y), pMaxX };
                break;
            }
        }
        if (result1!=null && result2!=null)
        {
            int distance1 = Math.Abs(result1[1].x - pMinX.x);
            int distance2 = Math.Abs(result2[1].x - pMinX.x);
            if(distance1 > distance2) 
            {
                result1 = result2;
            }
        }


        // have a line in three line not true then return null
        if (result1 != null)
        {
            return result1;
        }
        return result2;
    }

    private Vector2Int[] CheckRectY(Vector2Int p1, Vector2Int p2)
    {
        Vector2Int[] result1 = null;
        Vector2Int[] result2 = null;
        // find point have y min
        Vector2Int pMinY = p1, pMaxY = p2;
        if (p1.y > p2.y)
        {
            pMinY = p2;
            pMaxY = p1;
        }
        // find line and y begin
        for (int y = pMinY.y; y < Matrix.GetLength(1); y++)
        {
            if (CheckLineY((int)pMinY.x, y, (int)pMinY.y)
                    && CheckLineX(y, (int)pMaxY.x, (int)pMinY.x)
                    && CheckLineY((int)pMaxY.x, y, (int)pMaxY.y))
            {

/*                print(" Rect Y:(" + pMinY.x + "," + pMinY.y + ") -> (" + pMinY.x
                        + "," + y + ") -> (" + pMaxY.x + "," + y
                        + ") -> (" + pMaxY.x + "," + pMaxY.y + ")");*/
                result1= new Vector2Int[] { pMinY, new Vector2Int(pMinY.x,y), new Vector2Int(pMaxY.x, y), pMaxY };
                break;
            }
        }
        for (int y = pMinY.y; y >=0; y--)
        {
            if (CheckLineY((int)pMinY.x, y, (int)pMinY.y)
                    && CheckLineX(y, (int)pMaxY.x, (int)pMinY.x)
                    && CheckLineY((int)pMaxY.x, y, (int)pMaxY.y))
            {

                /*                print(" Rect Y:(" + pMinY.x + "," + pMinY.y + ") -> (" + pMinY.x
                                        + "," + y + ") -> (" + pMaxY.x + "," + y
                                        + ") -> (" + pMaxY.x + "," + pMaxY.y + ")");*/
                result2 = new Vector2Int[] { pMinY, new Vector2Int(pMinY.x, y), new Vector2Int(pMaxY.x, y), pMaxY };
                break;
            }
        }


        if (result1 != null && result2 != null)
        {
            int distance1 = Math.Abs(result1[1].y - pMinY.y);
            int distance2 = Math.Abs(result2[1].y - pMinY.y);
            if (distance1 > distance2)
            {
                result1 = result2;
            }
        }


        // have a line in three line not true then return -1
        if (result1 != null)
        {
            return result1;
        }
        return result2;
    }
}
