using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private float delayTime;

    public GameObject panel;
    private LineRenderer lineRenderer;
    private int maxLengthX;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        delayTime = 0.15f;
        if (panel.GetComponent<ListCellController>())
        {
            maxLengthX = panel.GetComponent<ListCellController>().colPlay+2;
        }
        else if(panel.GetComponent<EndlessModeController>())
        {
            maxLengthX = panel.GetComponent<EndlessModeController>().colPlay + 2;
        }
        else if (panel.GetComponent<PuzzleModeController>())
        {
            maxLengthX = panel.GetComponent<PuzzleModeController>().colPlay + 2;

        }else if (panel.GetComponent<ShadowModeController>())
        {
            maxLengthX=panel.GetComponent<ShadowModeController>().colPlay + 2;
        }
        else if (panel.GetComponent<ListCellEscapeController>())
        {
            maxLengthX = panel.GetComponent<ListCellEscapeController>().colPlay + 2;
        }
    }

    public void CreateLine(GameObject g1, GameObject g2)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0,g1.transform.position);
        lineRenderer.SetPosition(1, g2.transform.position);
        Invoke("TurnOffLineRenderer", delayTime);
    }

    public void CreateLine(GameObject g1, GameObject g2,GameObject g3)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetVertexCount(3);
        lineRenderer.SetPosition(0, g1.transform.position);
        lineRenderer.SetPosition(1, g2.transform.position);
        lineRenderer.SetPosition(2, g3.transform.position);
        Invoke("TurnOffLineRenderer", delayTime);
    }
    public void CreateLine(GameObject g1, GameObject g2,GameObject g3,GameObject g4)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetVertexCount(4);
        lineRenderer.SetPosition(0, g1.transform.position);
        lineRenderer.SetPosition(1, g2.transform.position);
        lineRenderer.SetPosition(2, g3.transform.position);
        lineRenderer.SetPosition(3, g4.transform.position);
        Invoke("TurnOffLineRenderer", delayTime);
    }

    public void CreateLine(Vector2Int p1, Vector2Int p2)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, panel.transform.GetChild(p1.y * maxLengthX + p1.x).transform.position);
        lineRenderer.SetPosition(1, panel.transform.GetChild(p2.y * maxLengthX + p2.x).transform.position);
        Invoke("TurnOffLineRenderer", delayTime);

    }
    public void CreateLine(Vector2Int p1, Vector2Int p2,Vector2Int p3)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetVertexCount(3);

        lineRenderer.SetPosition(0, panel.transform.GetChild(p1.y * maxLengthX + p1.x).transform.position);
        lineRenderer.SetPosition(1, panel.transform.GetChild(p2.y * maxLengthX + p2.x).transform.position);
        lineRenderer.SetPosition(2, panel.transform.GetChild(p3.y * maxLengthX + p3.x).transform.position);
        Invoke("TurnOffLineRenderer", delayTime);
    }
    public void CreateLine(Vector2Int p1, Vector2Int p2, Vector2Int p3,Vector2Int p4)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetVertexCount(4);

        lineRenderer.SetPosition(0, panel.transform.GetChild(p1.y * maxLengthX + p1.x).transform.position);
        lineRenderer.SetPosition(1, panel.transform.GetChild(p2.y * maxLengthX + p2.x).transform.position);
        lineRenderer.SetPosition(2, panel.transform.GetChild(p3.y * maxLengthX + p3.x).transform.position);
        lineRenderer.SetPosition(3, panel.transform.GetChild(p4.y * maxLengthX + p4.x).transform.position);
        Invoke("TurnOffLineRenderer", delayTime);
    }

    private void TurnOffLineRenderer()
    {
        lineRenderer.enabled = false;
    }
}
