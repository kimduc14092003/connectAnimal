using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    public GameObject tutorialFirstCell, tutorialSecondCell;
    public LineController lineController;
    public GameObject cursorPoint;
    public TutorialController tutorialController;
    public GameObject effectClick1,bubuEffect1,bubuEffect2;

    private Vector3 targetPos;
    private bool isMovingToSecondCell;
    private void Start()
    {
        tutorialController = transform.parent.GetComponent<TutorialController>();
        lineController = transform.parent.GetComponent<TutorialController>().lineController;
        targetPos = tutorialSecondCell.transform.position;
        Invoke("Tutorial1ClickFirstCell", 0.2f);
    }


    private void FixedUpdate()
    {
        if (isMovingToSecondCell)
        {
            MoveToSecondCell();
        }
    }


    private void Tutorial1ClickFirstCell()
    {
        Animator animator = cursorPoint.GetComponent<Animator>();
        //animator.Play("cursorPointAnim", 0, 0f);
        StartCoroutine(SelectedCell(tutorialFirstCell));
        Invoke("MoveToSecondCell", 1.1f);
    }

    IEnumerator SelectedCell(GameObject cell)
    {
        yield return new WaitForSeconds(0.4f);
        effectClick1.transform.position= cell.transform.position;
        effectClick1.SetActive(true);
        cell.GetComponent<CellController>().isSelected = true;
    }

    private void MoveToSecondCell()
    {
        isMovingToSecondCell = true;
        cursorPoint.transform.parent.position = Vector3.MoveTowards(cursorPoint.transform.parent.position, targetPos, Time.deltaTime * 3f);
        if (cursorPoint.transform.parent.position == targetPos)
        {
            isMovingToSecondCell = false;
            Invoke("Tutorial1ClickSecondCell", 0.1f);
        }
    }
    private void Tutorial1ClickSecondCell()
    {
        Animator animator = cursorPoint.GetComponent<Animator>();
        animator.Play("cursorPointAnim", 0, 0f);
        StartCoroutine(SelectedCell(tutorialSecondCell));
        StartCoroutine(CreateLine(tutorialFirstCell, tutorialSecondCell));
    }

     IEnumerator CreateLine(GameObject g1, GameObject g2)
    {
        yield return new WaitForSeconds(0.7f);
        lineController.CreateLine(g1, g2);
        StartCoroutine(TurnOffGameObject(tutorialFirstCell, tutorialSecondCell));
    }

    IEnumerator TurnOffGameObject(GameObject g1, GameObject g2)
    {
        bubuEffect1.SetActive(true); bubuEffect2.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        g1.SetActive(false);
        g2.SetActive(false);
        Invoke("CloseTutorial1", 0.5f);
    }

    private void CloseTutorial1()
    {
        //listPanelTutorial[1].SetActive(true);
        tutorialController.OpenPanelTutorial2();
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
