using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerThreeMatch : MonoBehaviour
{
    public GameObject listChooseCellGameObject;
    public List<GameObject> listChooseCellID;
    private Vector3[] listChooseCellPos;
    public GameObject nothingCell;

    public EndlessModeController endlessModeController;
    public Slider musicSlider, sfxSlider;
    public TMP_Text
         currentScoreTxt, highScoreTxt, loseTitleLevel;
    public Image bg;
    public Sprite[] listSpriteBg;
    public Slider timeSlider;
    public float limitTimeOfLevel;
    public GameObject PausePanel, TimePanel;
    public GameObject loseGamePanel;
    public GameObject[] listMatrix;
    public LineController lineController;

    public float timeRemaining;

    public ThreeMatchSpawnCellInLayer[] listLayer;

    public List<GameObject> listCell;

    private bool isPauseGame;
    public int remainingCell;
    public List<GameObject> cells;

    private List<GameObject> listTypeOfCell;
    public int numberOfDifferentCell;
    public GameObject[] listMap;
    public GameObject currentMap;
    private int currentLevel;
    public GameObject winGamePanel;
    public GameObject helpPanel;
    private float countStar;
    public GameObject[] listStars;
    public GameObject[] listStarEndGame;
    public GameObject pointGameObject;

    private int bombRemaining;
    private int findRemaining;
    private int shuffleRemaining;

    public TMP_Text txtBomb;
    public TMP_Text txtFind;
    public TMP_Text txtShuffle;

    private bool isShuffle;
    private bool isBomb=false;
    private bool isFind=false;

    private List<Vector3> listPos;

    public GameObject StopPanel;

    public Slider comboSlider;
    private float timeOfCombo;
    private float currentTimeOfCombo;

    private bool isRunningCombo;
    public int countCombo;

    public Slider starSlider;
    public TMP_Text starText;

    public GameObject comboEffect;
    private bool isDoneTutorial;
    public GameObject treasurePanel;
    private bool isHandleStar;
    private void Awake()
    {
        starSlider.value = PlayerPrefs.GetInt("currentStarThreeMatchMode", 0);
        countCombo = 0;
        isRunningCombo = false;
        isHandleStar = false;
        timeOfCombo = 3;
        currentLevel = PlayerPrefs.GetInt("currentLevelThreeMatchMode", 1);

        Application.targetFrameRate = 60;
        /* float coefficient= Mathf.Pow(0.667f, currentLevel/9);
         limitTimeOfLevel = StaticData.limitTimeInEndless*coefficient ;
 */
        listPos = new List<Vector3>();
        HandleNumOfDifferentCell();
        GetInforOfGame();
        GetListLayer();
        GetListChooseCell();
        GetListCellNeedToSpawn();
        GetListCell();
    }
    private void HandleNumOfDifferentCell()
    {
        switch (currentLevel)
        {
            case 1:
                {
                    numberOfDifferentCell = 4;
                    break;
                }
            case 2:
                {
                    numberOfDifferentCell = 8;
                    break;
                }
            case 3:
                {
                    numberOfDifferentCell = 6;
                    break;
                }
            case 4:
                {
                    numberOfDifferentCell = 8;
                    break;
                }
            case 5:
                {
                    numberOfDifferentCell = 14;
                    break;
                }

            case 6:
                {
                    numberOfDifferentCell = 10;
                    break;
                }
            default:
                {
                    numberOfDifferentCell = 10;
                    break;
                }
        }
    }
    private void GetInforOfGame()
    {
        if (currentLevel < listMap.Length)
        {
            currentMap = Instantiate(listMap[currentLevel - 1], transform);
        }
        else
        {
            currentMap = Instantiate(listMap[listMap.Length-1], transform);
        }
        currentMap.transform.SetSiblingIndex(7);

        switch (currentLevel)
        {
            case 2:
                {
                    pointGameObject.transform.position=txtBomb.transform.position+Vector3.right*1;
                    pointGameObject.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                    break;
                }
            case 3:
                {
                    pointGameObject.transform.position = txtFind.transform.position + Vector3.right * 1;
                    pointGameObject.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                    break;
                }
            case 4:
                {
                    pointGameObject.transform.position = txtShuffle.transform.position + Vector3.right * 1;
                    pointGameObject.transform.DOScale(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                    break;
                }
        }

        LoadDataCountHelp();
    }

    private void LoadDataCountHelp()
    {
        bombRemaining = PlayerPrefs.GetInt("bombRemainingThreeMatchMode", 5);
        findRemaining = PlayerPrefs.GetInt("findRemainingThreeMatchMode", 5);
        shuffleRemaining = PlayerPrefs.GetInt("shuffleRemainingThreeMatchMode", 5);

        txtBomb.text = bombRemaining + "";
        txtFind.text = findRemaining + "";
        txtShuffle.text = shuffleRemaining + "";
        CheckHelpRemaining();
    }

    public void GetListChooseCell()
    {
        listChooseCellID= new List<GameObject>();
        for(int i = 0; i < listChooseCellGameObject.transform.childCount; i++)
        {
            listChooseCellID.Add(listChooseCellGameObject.transform.GetChild(i).gameObject);
        }
        listChooseCellPos=new Vector3[7];
        for(int i=0;i<7; i++)
        {
            listChooseCellPos[i]= listChooseCellGameObject.transform.GetChild(i).transform.position;
        }
    }

    private void GetListCellNeedToSpawn()
    {
        //Lấy danh sách các ô khác nhau cần sinh ra
        listTypeOfCell = new List<GameObject>();
        for (int i = 0; i < numberOfDifferentCell; i++)
        {
            int randomNumber = Random.Range(0, cells.Count);
            listTypeOfCell.Add(cells[randomNumber]);
            cells.RemoveAt(randomNumber);
        }
    }
    private void GetListCell()
    {
        listCell = new List<GameObject>();
        remainingCell = 0 ;
        for(int i = 0; i < currentMap.transform.childCount; i++)
        {
            if(currentMap.transform.GetChild(i).name!= "PanelTutorial")
            {
                remainingCell += currentMap.transform.GetChild(i).childCount;
            }
        }

        //Tạo một danh sách tạm để lấy random từ danh sách này ra 
        List<GameObject> tempListCell = new List<GameObject>();
        tempListCell.AddRange(listTypeOfCell);

        if (currentLevel == 1)
        {
            remainingCell -= 3;
        }
        for (int i = 0; i < remainingCell; i+=3)
        {
            int randomNumber = Random.Range(0, tempListCell.Count);
            GameObject cell = Instantiate(tempListCell[randomNumber], transform);

            //Thay đổi component cellcontroller cho nó
            CellControllerThreeMatch cellControllerThreeMatch = cell.GetComponent<CellControllerThreeMatch>();
            cellControllerThreeMatch.levelManager =GetComponent<LevelManagerThreeMatch>();
            cellControllerThreeMatch.listChooseCell = GetComponent<LevelManagerThreeMatch>().listChooseCellGameObject;
            
            GameObject cell2 = Instantiate(cell, transform);
            GameObject cell3 = Instantiate(cell, transform);
            listCell.Add(cell);
            listCell.Add(cell2);
            listCell.Add(cell3);


            tempListCell.RemoveAt(randomNumber);
            if (tempListCell.Count == 0)
            {
                tempListCell.AddRange(listTypeOfCell);
            }
        }

        if (currentLevel == 1)
        {
            remainingCell += 3;
        }
    }
    private void GetListLayer()
    {
        if (currentLevel == 1|| currentLevel == 2|| currentLevel == 3|| currentLevel == 4)
        {
            listLayer = new ThreeMatchSpawnCellInLayer[currentMap.transform.childCount - 1];
        }
        else listLayer = new ThreeMatchSpawnCellInLayer[currentMap.transform.childCount];
        for (int i = 0; i < currentMap.transform.childCount; i++)
        {
            if (currentMap.transform.GetChild(i).GetComponent<ThreeMatchSpawnCellInLayer>())
            {
                listLayer[i]=currentMap.transform.GetChild(i).GetComponent<ThreeMatchSpawnCellInLayer>();
            }
        }
    }

    private void Start()
    {
        timeSlider.maxValue = limitTimeOfLevel;
        timeRemaining = limitTimeOfLevel;
        timeSlider.value = timeRemaining;
        if (currentLevel == 1)
        {
            helpPanel.SetActive(false);
        }
        AudioManager.Instance.PlayRandomMusic();
    }

    //Lấy vị trí của được chọn sẽ đến 
    public Vector3 GetPosCellToMove(string cellID)
    {
        // quy ước Vector3.z = i = sibilingIndex
        Vector3 lastChooseCellPos = listChooseCellPos[0];
        lastChooseCellPos.z = 0;

        bool isGetLastPos =false;
        for (int i = listChooseCellID.Count-1; i>= 0; i--)
        {

            if (cellID == listChooseCellID[i].GetComponent<CellControllerThreeMatch>().CellID)
            {
                Vector3 result = listChooseCellPos[i];
                result.z = i;
                return result;
            }
            else if (listChooseCellID[i].GetComponent<CellControllerThreeMatch>().CellID != "0"&&!isGetLastPos)
            {
                
                isGetLastPos = true;
                lastChooseCellPos= listChooseCellPos[i+1];
                lastChooseCellPos.z = i+1; 
            }
        }
        return lastChooseCellPos;
    }

    public void ResetPosOfChosenCell()
    {
        for(int i= 0; i < listChooseCellID.Count; i++)
        {
            CellControllerThreeMatch cellControllerThreeMatch = listChooseCellID[i].GetComponent<CellControllerThreeMatch>();
            listChooseCellID[i].transform.SetSiblingIndex(i);

            if (i != cellControllerThreeMatch.target.z)
            {
                cellControllerThreeMatch.target = listChooseCellPos[i];
                cellControllerThreeMatch.target.z = i;
                cellControllerThreeMatch.MoveToListChooseCell();
            }
        }
        CheckGetScore();

    }

    public void CheckGetScore()
    {
        string currentID= listChooseCellID[0].GetComponent<CellControllerThreeMatch>().CellID;
        int count = 0;
        for (int i = 0; i < listChooseCellID.Count; i++)
        {
            if (listChooseCellID[i].GetComponent<CellControllerThreeMatch>().CellID == "0")
            {
                continue;
            }
            if (currentID == listChooseCellID[i].GetComponent<CellControllerThreeMatch>().CellID)
            {
                count++;
            }
            else
            {
                currentID = listChooseCellID[i].GetComponent<CellControllerThreeMatch>().CellID;
                count = 1;
            }
            if (count == 3)
            {
                AudioManager.Instance.PlaySFX("connect");
                remainingCell -= 3;   
                CheckWinLevel();
                for(int j = i; j > i - 3; j--)
                {
                    StartAnimationScore(listChooseCellID[j]);
                    listChooseCellID.RemoveAt(j);
                    GameObject cell = Instantiate(nothingCell, listChooseCellGameObject.transform);
                    listChooseCellID.Add(cell);
                }

                timeRemaining += 5;
                if (timeRemaining >= limitTimeOfLevel)
                {
                    timeRemaining = limitTimeOfLevel;
                }
                StartCombo();
                ResetPosOfChosenCell();
            }
        }

        int countLength = 0;
        for(int i = 0;i < listChooseCellID.Count; i++)
        {
            if (listChooseCellID[i].GetComponent<CellControllerThreeMatch>().CellID != "0")
            {
                countLength++;
            }
        }
        if (countLength >= 7)
        {
            LoseGame();
        }
    }

    private void StartAnimationScore(GameObject cell)
    {
        CellControllerThreeMatch cellControllerThreeMatch = cell.GetComponent<CellControllerThreeMatch>();
        if (cellControllerThreeMatch)
        {
            cellControllerThreeMatch.DoAnim();
        }
    }

    public void LoseGame()
    {
        print("Ban thua roi!");
        AudioManager.Instance.StopMusic();

        AudioManager.Instance.PlaySFX("lose");
        loseTitleLevel.text = "Level " + currentLevel;
        PlayerPrefs.SetInt("currentLevelThreeMatchMode", 1);
        PlayerPrefs.SetInt("currentStarThreeMatchMode", 0);
        TurnOffToNotifyEndGame();
        loseGamePanel.SetActive(true);
        pointGameObject.SetActive(false);
    }

    private void TurnOffToNotifyEndGame()
    {
        isPauseGame = true;
        TimePanel.SetActive(false);
        listChooseCellGameObject.SetActive(false);
        helpPanel.SetActive(false);
        currentMap.SetActive(false);
        if (pointGameObject.active)
        {
            pointGameObject.SetActive(false);
        }
    }
    private void TurnOnAgain()
    {
        TimePanel.SetActive(true);
        listChooseCellGameObject.SetActive(true);
        helpPanel.SetActive(true);
        currentMap.SetActive(true);
    }

    private void CheckWinLevel()
    {
        if (remainingCell <= 0)
        {
            WinLevelNotification();
        }
    }

    public void WinLevelNotification()
    {
        AudioManager.Instance.PlaySFX("win");
        AudioManager.Instance.StopMusic();

        PlayerPrefs.SetInt("currentLevelThreeMatchMode", ++currentLevel);
        int currentStar = PlayerPrefs.GetInt("currentStarThreeMatchMode", 0);

        currentStar += (int)countStar;
        PlayerPrefs.SetInt("currentStarThreeMatchMode", currentStar);
        starText.text = currentStar + "/15";
        winGamePanel.SetActive(true);
        pointGameObject.SetActive(false);
        TurnOffToNotifyEndGame();
        for(int i=0;i<countStar; i++)
        {
            listStarEndGame[i].SetActive(true);
        }
        isHandleStar = true;
    }


    public void ReCheckAllCell()
    {
        foreach (ThreeMatchSpawnCellInLayer item in listLayer)
        {
            item.ReCheckCell();
        }
    }

    private void BombDone()
    {
        isBomb = false;
        ReCheckAllCell();

    }
    public void BombFunction()
    {
        if (bombRemaining > 0&&!isBomb)
        {
            AudioManager.Instance.PlaySFX("bomb");

            isBomb = true;
            Invoke("BombDone", 0.75f);
            bombRemaining--;
            PlayerPrefs.SetInt("bombRemainingThreeMatchMode", bombRemaining);
            LoadDataCountHelp();

            bool doneBombFunction=false;

            List<string> listIDCantBomb = new List<string>();

            do
            {
                GameObject cellWillDestroy = null;
                //Tìm ô cần xóa
                for (int i = listLayer.Length - 1; i >= 0; i--)
                {
                    if (cellWillDestroy)
                    {
                        break;
                    }
                    for (int j = 0; j < listLayer[i].transform.childCount; j++)
                    {
                        if (listLayer[i].transform.GetChild(j))
                        {
                            if (!listIDCantBomb.Contains(listLayer[i].transform.GetChild(j).GetComponent<CellControllerThreeMatch>().CellID))
                            {
                                cellWillDestroy = listLayer[i].transform.GetChild(j).gameObject;
                                break;
                            }
                        }
                    }
                }
                if (!cellWillDestroy)
                {
                    print("Hết ô rồi!");
                    bombRemaining++;
                    PlayerPrefs.SetInt("bombRemainingThreeMatchMode", bombRemaining);
                    LoadDataCountHelp();
                    break;
                }
                //Xóa ô
                List<GameObject> listCellWillDestroy = new List<GameObject>();
                for (int i = listLayer.Length - 1; i >= 0; i--)
                {
                    for (int j = 0; j < listLayer[i].transform.childCount; j++)
                    {
                        if (listLayer[i].transform.GetChild(j))
                        {
                            try
                            {
                                if (listLayer[i].transform.GetChild(j).GetComponent<CellControllerThreeMatch>().CellID
                                                            == cellWillDestroy.GetComponent<CellControllerThreeMatch>().CellID)
                                {
                                    listCellWillDestroy.Add(listLayer[i].transform.GetChild(j).gameObject);

                                    if (listCellWillDestroy.Count >= 6)
                                    {
                                        for (int k = 0; k < listCellWillDestroy.Count; k++)
                                        {
                                            listCellWillDestroy[k].GetComponent<CellControllerThreeMatch>().DoAnim();
                                            remainingCell--;
                                        }
                                        doneBombFunction = true;
                                        break;
                                    }
                                }
                            }
                            catch (System.Exception e)
                            {
                                //print(listLayer[i].listCellNeedToSpawn[j]);

                            }
                        }
                    }

                    if (doneBombFunction)
                    {
                        break;
                    }
                }

                if (listCellWillDestroy.Count < 6)
                {
                    listIDCantBomb.Add(cellWillDestroy.GetComponent<CellControllerThreeMatch>().CellID);
                }
            } 
            while (!doneBombFunction);


        }
    }


    private void FindDone()
    {
        isFind = false;
    }
    public void FindFunction()
    {
        if (findRemaining > 0&&!isFind)
        {
            isFind = true;
            Invoke("FindDone", 0.5f);
            findRemaining--;
            PlayerPrefs.SetInt("findRemainingThreeMatchMode", findRemaining);
            LoadDataCountHelp();

            bool doneFindFunction = false;

            List<string> listIDCantFind = new List<string>();

            do
            {
                GameObject cellWillDestroy = null;
                //Tìm ô cần xóa
                for (int i = listLayer.Length - 1; i >= 0; i--)
                {
                    if (cellWillDestroy)
                    {
                        break;
                    }
                    for (int j = 0; j < listLayer[i].transform.childCount; j++)
                    {
                        if (listLayer[i].transform.GetChild(j))
                        {
                            if (!listIDCantFind.Contains(listLayer[i].transform.GetChild(j).GetComponent<CellControllerThreeMatch>().CellID))
                            {
                                cellWillDestroy = listLayer[i].transform.GetChild(j).gameObject;
                                break;
                            }
                        }
                    }
                }
                if (!cellWillDestroy)
                {
                    print("Hết ô rồi!");
                    findRemaining++;
                    PlayerPrefs.SetInt("findRemainingThreeMatchMode", findRemaining);
                    LoadDataCountHelp();
                    break;
                }
                //Xóa ô
                List<GameObject> listCellWillDestroy = new List<GameObject>();
                for (int i = listLayer.Length - 1; i >= 0; i--)
                {
                    for (int j = 0; j < listLayer[i].transform.childCount; j++)
                    {
                        if (listLayer[i].transform.GetChild(j))
                        {
                            try
                            {
                                if (listLayer[i].transform.GetChild(j).GetComponent<CellControllerThreeMatch>().CellID
                                                            == cellWillDestroy.GetComponent<CellControllerThreeMatch>().CellID)
                                {
                                    listCellWillDestroy.Add(listLayer[i].transform.GetChild(j).gameObject);

                                    if (listCellWillDestroy.Count >= 3)
                                    {
                                        for (int k = 0; k < listCellWillDestroy.Count; k++)
                                        {
                                            listCellWillDestroy[k].GetComponent<CellControllerThreeMatch>().GetPosOfCell();
                                            listCellWillDestroy[k].GetComponent<CellControllerThreeMatch>().isDisable = false;
                                        }
                                        doneFindFunction = true;
                                        break;
                                    }
                                }
                            }
                            catch (System.Exception e)
                            {
                                //print(listLayer[i].listCellNeedToSpawn[j]);

                            }
                        }
                    }

                    if (doneFindFunction)
                    {
                        break;
                    }
                }

                if (listCellWillDestroy.Count < 3)
                {
                    listIDCantFind.Add(cellWillDestroy.GetComponent<CellControllerThreeMatch>().CellID);
                }
            }
            while (!doneFindFunction);
        }
    }

    public void ShuffleFunction()
    {
        if (isShuffle)
        {
            return;
        }
        if (shuffleRemaining <= 0)
        {
            return;
        }
        AudioManager.Instance.PlaySFX("shuffle");

        shuffleRemaining--;
        PlayerPrefs.SetInt("shuffleRemainingThreeMatchMode", shuffleRemaining);
        LoadDataCountHelp();

        listPos = new List<Vector3>();
        for (int i = 0; i < listLayer.Length; i++) 
        {
            int lengthOfLayer = listLayer[i].transform.childCount;
            for (int j = 0; j < lengthOfLayer; j++)
            {
                listPos.Add(new Vector3(listLayer[i].transform.GetChild(0).position.x, listLayer[i].transform.GetChild(0).position.y,i));
                listCell.Add(listLayer[i].transform.GetChild(0).gameObject);
                listLayer[i].transform.GetChild(0).SetParent(TimePanel.transform);
            }
        }

        for (int i = 0; i < listPos.Count; i++)
        {
            int ramdomNumber = Random.Range(0, listCell.Count);
            listCell[ramdomNumber].transform.DOMove(listPos[i], 1f).OnComplete(() =>
            {
                isShuffle = false;
                ReCheckAllCell();
                StopPanel.SetActive(false);

            });
            listCell[ramdomNumber].transform.SetParent(currentMap.transform.GetChild((int)listPos[i].z));
            listCell[ramdomNumber].transform.SetAsLastSibling();
            listCell.RemoveAt(ramdomNumber);
        }
        isShuffle = true;
        StopPanel.SetActive(true);
        ReCheckAllCell();

    }

    private void StartCombo()
    {
        countCombo++;
        if (countCombo > 1&&remainingCell>0)
        {
            comboSlider.gameObject.SetActive(true);
            comboEffect.SetActive(true);
        }
        comboSlider.maxValue = timeOfCombo;
        currentTimeOfCombo = timeOfCombo;
        comboSlider.value = currentTimeOfCombo;
        isRunningCombo = true;
    }


    private void CheckHelpRemaining()
    {
        if (bombRemaining <= 0)
        {
            GameObject gb = txtBomb.transform.parent.gameObject;
            CanvasGroup canvasGroup = gb.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0.4f;
            canvasGroup.interactable = false;
        }
        if (findRemaining <= 0)
        {
            GameObject gb = txtFind.transform.parent.gameObject;
            CanvasGroup canvasGroup = gb.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0.4f;
            canvasGroup.interactable = false;
        }
        if (shuffleRemaining <= 0)
        {
            GameObject gb = txtShuffle.transform.parent.gameObject;
            CanvasGroup canvasGroup = gb.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0.4f;
            canvasGroup.interactable = false;
        }
    }

    public void ReturnHomeScene()
    {
        //AudioManager.Instance.PlaySFX("click_button");
        SceneManager.LoadScene("HomeScene");
    }

    public void NextLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReplayGame()
    {
        PlayerPrefs.SetInt("bombRemainingThreeMatchMode", 5);
        PlayerPrefs.SetInt("findRemainingThreeMatchMode", 5);
        PlayerPrefs.SetInt("shuffleRemainingThreeMatchMode", 5);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }

    private void FixedUpdate()
    {
        if (!isDoneTutorial)
        {
            pointGameObject.SetActive(false);
        }
        else if(!isPauseGame)
        {
            pointGameObject.SetActive(true);
        }

        switch (currentLevel)
        {
            case 2:
                {
                    if (currentMap.GetComponent<ThreeMatchTutorial2>())
                    {
                        isDoneTutorial = currentMap.GetComponent<ThreeMatchTutorial2>().enabled;
                    }
                    break;
                }
            case 3:
                {
                    if (currentMap.GetComponent<ThreeMatchTutorial3>())
                    {
                        isDoneTutorial = currentMap.GetComponent<ThreeMatchTutorial3>().enabled;
                    }
                    break;
                }
            case 4:
                {
                    if (currentMap.GetComponent<ThreeMatchTutorial4>())
                    {
                        isDoneTutorial = currentMap.GetComponent<ThreeMatchTutorial4>().enabled;
                    }
                    break;
                }
        }
        TimeRemainingController();
        LoadDataCountHelp();
        if (isRunningCombo)
        {
            currentTimeOfCombo -= Time.deltaTime;
            comboSlider.value = currentTimeOfCombo;
            if (currentTimeOfCombo <= 0)
            {
                isRunningCombo = false;
                countCombo = 0;
                comboSlider.gameObject.SetActive(false);
            }
            if(remainingCell<=0)
            {
                comboSlider.gameObject.SetActive(false);
            }
        }

        if (isHandleStar)
        {
            if(starSlider.value < PlayerPrefs.GetInt("currentStarThreeMatchMode", 0))
            {
                starSlider.value += 0.05f;
                if (starSlider.value >= 15)
                {
                    int currentStar= PlayerPrefs.GetInt("currentStarThreeMatchMode", 0) ;
                    currentStar -= 15;
                    PlayerPrefs.SetInt("currentStarThreeMatchMode", currentStar);
                    starSlider.value = currentStar;
                    HandleStarFull();
                }
            }
        }
    }

    private void HandleStarFull()
    {
        treasurePanel.SetActive(true);
        starText.text = PlayerPrefs.GetInt("currentStarThreeMatchMode", 0) + "/15";
    }

    public void TurnOffTreasurePanel()
    {
        treasurePanel.SetActive(false);
    }

    private void TimeRemainingController()
    {
        if (isPauseGame)
        {
            TimePanel.SetActive(false);
            //endlessModeController.gameObject.SetActive(false);
            return;
        }
        else
        {
            TimePanel.SetActive(true);
           // endlessModeController.gameObject.SetActive(true);
        }

        if (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            timeSlider.value = timeRemaining;
            StarController();
        }
        else
        {
            LoseGame();
        }
    }

    private void StarController()
    {
        if (timeRemaining >= 0.9f * limitTimeOfLevel)
        {
            countStar = 3;
            for(int i = 0; i < listStars.Length; i++)
            {
                listStars[i].SetActive(true);
            }
        }
        else 
        if(timeRemaining >= 0.5f * limitTimeOfLevel)
        {
            countStar = 2;
            for (int i = 0; i < listStars.Length; i++)
            {
                if (i < countStar)
                {
                    listStars[i].SetActive(true);
                }
                else
                {
                    listStars[i].SetActive(false);
                }
            }
        }
        else if(timeRemaining >= 0.15f * limitTimeOfLevel)
        {
            countStar = 1;
            for (int i = 0; i < listStars.Length; i++)
            {
                if (i < countStar)
                {
                    listStars[i].SetActive(true);
                }
                else
                {
                    listStars[i].SetActive(false);
                }
            }
        }
        else 
        { 
            countStar = 0;
            for (int i = 0; i < listStars.Length; i++)
            {
                if (i < countStar)
                {
                    listStars[i].SetActive(true);
                }
                else
                {
                    listStars[i].SetActive(false);
                }
            }
        }

    }


    public void PauseGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        TurnOffToNotifyEndGame();
        PausePanel.SetActive(true);
        SetDefaultSlider();
        isPauseGame = true;
    }


    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PausePanel.SetActive(false);
        isPauseGame = false;
        TurnOnAgain();
    }

    private void SetDefaultSlider()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

}
