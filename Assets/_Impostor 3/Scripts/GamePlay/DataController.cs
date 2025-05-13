using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lean.Pool;
using UnityEditor;
using Zitga.CsvTools.Tutorials;
using Random = System.Random;

public class DataController : MonoBehaviour
{
    [SerializeField] private List<Stack<int>> listCols;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject testCubesHolder;
    [SerializeField] private float cubeSize = 0.35f;

    private List<int> randomList;

    private GamePlayController gpc;

    private void SetBackground(int totalBox)
    {
        if (totalBox <= 6)
        {
            GameController.Ins.gamePlayController.GraphicController.Set1RowBackground();
        }
        else
        {
            GameController.Ins.gamePlayController.GraphicController.Set2RowBackground();
        }
    }
    
    private void Start()
    {
        gpc = GameController.Ins.gamePlayController;

        
    }

    public void LoadMap()
    {
        
        if (GameController.Ins.isTutorialLevel)
        {
            GameController.Ins.SetupTutorial();
        }
        else
        {
            if (GameController.Ins.tutorialHand != null)
            {
                Destroy(GameController.Ins.tutorialHand.transform.parent.gameObject);
            }
        }
        
        if (GameManager.Instance.currentLevel < 30)
        {
            GenarateDataMap();
        }
        else if (GameManager.Instance.currentLevel < 50)
        {
            GenarateRandomDataMap();
        }
        else
        {
            GenarateRandomMap();
        }
    }
    
    private bool availableRandomList()
    {
        for (int i = 0; i < randomList.Count; i += gpc.maxValueCols)
        {
            List<int> tempList = randomList.GetRange(i, gpc.maxValueCols);
            if (tempList.Distinct().Count() == 1)
            {
                return false;
            }
        }
        return true;
    }
    
    private void setUpRandomList()
    {
        int idxColor=0;
        randomList = new List<int>();
        for (int i = 0; i < gpc.maxCols;i++)
        {
            for (int j = 0; j < gpc.maxValueCols;j++)
            {
                randomList.Add(idxColor);
            }

            if (idxColor < gpc.numberColors-1)
            {
                idxColor++;
            }
            else
            {
                idxColor = 0;
            }
        }
        
        
        int cnt = gpc.numberColors*gpc.maxCols;
        while (cnt>0)
        {
            int idx = randomList.ElementAt(UnityEngine.Random.Range(0, randomList.Count));
            randomList.Add(randomList.ElementAt(idx));
            randomList.RemoveAt(idx);
            cnt--;
        }
        
        
        while (!availableRandomList())
        {
            int idx = randomList.ElementAt(UnityEngine.Random.Range(0, randomList.Count));
            randomList.Add(randomList.ElementAt(idx));
            randomList.RemoveAt(idx);
        }
    }    

    public void GenarateDataMap()
    {
        FirstLevels fl = GameManager.Instance.firstLevels;
        FirstLevels.FirstLevelsData currentLevel = fl.firstLevelsDatas[GameManager.Instance.currentLevel];

        if (GameManager.Instance.currentLevel == 0)
        {
            gpc.maxCols = 1;
        }
        else
        {
            gpc.maxCols = currentLevel.fillBox;
        }
        
        SetBackground(currentLevel.totalBox);

        gpc.GraphicController.Boxes = new List<Box>();
        
        gpc.numberCols = currentLevel.totalBox;
        gpc.numberRows = (currentLevel.totalBox-1) / gpc.maxBoxPerRow + 1;
        
        gpc.numberColors = currentLevel.fillBox;
        
        int boxPerRow = (gpc.numberCols/gpc.numberRows) + (gpc.numberCols>6 && gpc.numberCols%2==1 ? 1 : 0);
        int boxPerCol = gpc.numberRows;
        int spaceRowNum =  boxPerRow + 1;
        int spaceColNum =  boxPerCol + 1;
  
        Vector3 widthDistance = new Vector3(Math.Abs(GameHelper.bottomLeft.x - GameHelper.topRight.x)/spaceRowNum,0,0);
        Vector3 heightDistance = new Vector3(0,Math.Abs(GameHelper.bottomLeft.y - GameHelper.topRight.y)/spaceColNum,0);
        
        listCols = new List<Stack<int>>();
        for (int i = 0; i < gpc.numberCols; i++)
        {
            
            Stack<int> s = new Stack<int>();
            Vector3 pos;
            if (currentLevel.totalBox <= 6)
            {
                pos = GameHelper.bottomLeft + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,GameController.Ins.gamePlayController.GraphicController.posRow1.position.y,pos.z);
            }
            else
            {
                pos = GameHelper.bottomLeft  + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,i<=gpc.numberCols/2 ?GameController.Ins.gamePlayController.GraphicController.posRow1_2.position.y : GameController.Ins.gamePlayController.GraphicController.posRow2_2.position.y,pos.z);
            }
            
            var box = LeanPool.Spawn(boxPrefab, pos , Quaternion.identity);
            box.transform.localScale = new Vector3(cubeSize,cubeSize,1f);
            box.transform.SetParent(testCubesHolder.transform);
            gpc.GraphicController.AddBox(box.GetComponent<Box>());
            if (i < currentLevel.fillBox)
            {
                for (int j = 0; j < currentLevel.boxes[i].box1.Length; j++)
                {
                    s.Push(currentLevel.boxes[i].box1[j]);
                    //Debug.Log(currentLevel.boxes[i].box1[j]);
                }
            }
            listCols.Add(s);
        }
        
        gpc.GraphicController.SetUp(listCols);
    }

    public void GenarateRandomDataMap()
    {
        RandomLevels rl = GameManager.Instance.randomLevels;
        RandomLevels.RandomLevelsData currentLevel = rl.randomlevelsDatas[GameManager.Instance.currentLevel-30];

        gpc.maxCols = currentLevel.fillBox;
        gpc.numberCols = currentLevel.totalBox;
        gpc.numberColors = currentLevel.numberColors;
        
        setUpRandomList();
        var myStack = new Stack<int>(randomList);
        
        listCols = new List<Stack<int>>();
        
        SetBackground(gpc.numberCols);

        gpc.GraphicController.Boxes = new List<Box>();
        
        gpc.numberRows = (gpc.numberCols-1) / gpc.maxBoxPerRow + 1;

        int boxPerRow = (gpc.numberCols/gpc.numberRows) + (gpc.numberCols>6 && gpc.numberCols%2==1 ? 1 : 0);
        int boxPerCol = gpc.numberRows;
            
        int spaceRowNum =  boxPerRow + 1;
        int spaceColNum =  boxPerCol + 1;
        
        Vector3 widthDistance = new Vector3(Math.Abs(GameHelper.bottomLeft.x - GameHelper.topRight.x)/spaceRowNum,0,0);
        Vector3 heightDistance = new Vector3(0,Math.Abs(GameHelper.bottomLeft.y - GameHelper.topRight.y)/spaceColNum,0);

        for (int i = 0; i < gpc.numberCols; i++)
        {
            Stack<int> s = new Stack<int>();
            
            
            Vector3 pos;
            if (gpc.numberCols <= 6)
            {
                pos = GameHelper.bottomLeft + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,GameController.Ins.gamePlayController.GraphicController.posRow1.position.y,pos.z);
            }
            else
            {
                pos = GameHelper.bottomLeft  + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,i<=gpc.numberCols/2 ?GameController.Ins.gamePlayController.GraphicController.posRow1_2.position.y: GameController.Ins.gamePlayController.GraphicController.posRow2_2.position.y ,pos.z);
            }
            
            var box = LeanPool.Spawn(boxPrefab, pos , Quaternion.identity);
            box.transform.localScale = new Vector3(cubeSize,cubeSize,1f);
            box.transform.SetParent(testCubesHolder.transform);

            gpc.GraphicController.AddBox(box.GetComponent<Box>());
            if (i < gpc.maxCols)
            {
                for (int j = 0; j < gpc.maxValueCols; j++)
                {
                    s.Push(myStack.Peek());
                    myStack.Pop();
                }
            }
            listCols.Add(s);
        }
        
        gpc.GraphicController.SetUp(listCols);
    }
    
    public void GenarateRandomMap()
    {
        gpc.maxBoxPerRow = 6;
        gpc.maxValueCols = 4;
        gpc.numberCols = 11;
        gpc.maxCols = 9;
        gpc.numberColors = 9;
        
        
        
        setUpRandomList();
        var myStack = new Stack<int>(randomList);
        
        listCols = new List<Stack<int>>();
        
        SetBackground(gpc.numberCols);

        gpc.GraphicController.Boxes = new List<Box>();
        
        gpc.numberRows = (gpc.numberCols-1) / gpc.maxBoxPerRow + 1;

        int boxPerRow = (gpc.numberCols/gpc.numberRows) + (gpc.numberCols>6 && gpc.numberCols%2==1 ? 1 : 0);
        int boxPerCol = gpc.numberRows;
            
        int spaceRowNum =  boxPerRow + 1;
        int spaceColNum =  boxPerCol + 1;
        
        Vector3 widthDistance = new Vector3(Math.Abs(GameHelper.bottomLeft.x - GameHelper.topRight.x)/spaceRowNum,0,0);
        Vector3 heightDistance = new Vector3(0,Math.Abs(GameHelper.bottomLeft.y - GameHelper.topRight.y)/spaceColNum,0);

        for (int i = 0; i < gpc.numberCols; i++)
        {
            Stack<int> s = new Stack<int>();
            
            
            Vector3 pos;
            if (gpc.numberCols <= 6)
            {
                pos = GameHelper.bottomLeft + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,GameController.Ins.gamePlayController.GraphicController.posRow1.position.y,pos.z);
            }
            else
            {
                pos = GameHelper.bottomLeft  + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,i<=gpc.numberCols/2 ?GameController.Ins.gamePlayController.GraphicController.posRow1_2.position.y : GameController.Ins.gamePlayController.GraphicController.posRow2_2.position.y ,pos.z);
            }
            
            var box = LeanPool.Spawn(boxPrefab, pos , Quaternion.identity);
            box.transform.localScale = new Vector3(cubeSize,cubeSize,1f);
            box.transform.SetParent(testCubesHolder.transform);

            gpc.GraphicController.AddBox(box.GetComponent<Box>());
            if (i < gpc.maxCols)
            {
                for (int j = 0; j < gpc.maxValueCols; j++)
                {
                    s.Push(myStack.Peek());
                    myStack.Pop();
                }
            }
            listCols.Add(s);
        }
        
        gpc.GraphicController.SetUp(listCols);
    }

    public void AddOneBox()
    {
        gpc.numberCols++;
        
        int boxPerRow = (gpc.numberCols/gpc.numberRows) + (gpc.numberCols>6 && gpc.numberCols%2==1 ? 1 : 0);
        int boxPerCol = gpc.numberRows;
        int spaceRowNum =  boxPerRow + 1;
        int spaceColNum =  boxPerCol + 1;
  
        Vector3 widthDistance = new Vector3(Math.Abs(GameHelper.bottomLeft.x - GameHelper.topRight.x)/spaceRowNum,0,0);
        Vector3 heightDistance = new Vector3(0,Math.Abs(GameHelper.bottomLeft.y - GameHelper.topRight.y)/spaceColNum,0);
   
        var box = LeanPool.Spawn(boxPrefab, Vector3.zero , Quaternion.identity);
        box.transform.localScale = new Vector3(cubeSize,cubeSize,1f);
        box.transform.SetParent(testCubesHolder.transform);
        box.GetComponent<Box>().SetUp(new Stack<int>());
        gpc.GraphicController.AddBox(box.GetComponent<Box>());
        
        listCols = new List<Stack<int>>();
        for (int i = 0; i < gpc.GraphicController.Boxes.Count; i++)
        {
            Vector3 pos;
            if (gpc.numberCols <= 6)
            {
                pos = GameHelper.bottomLeft + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,GameController.Ins.gamePlayController.GraphicController.posRow1.position.y,pos.z);
            }
            else
            {
                pos = GameHelper.bottomLeft  + new Vector3(0,0,1) + widthDistance*(i % boxPerRow + 1);
                pos = new Vector3(pos.x,i<gpc.numberCols/2 ?GameController.Ins.gamePlayController.GraphicController.posRow1_2.position.y : GameController.Ins.gamePlayController.GraphicController.posRow2_2.position.y,pos.z);
            }

            gpc.GraphicController.Boxes[i].transform.position = pos;
            foreach (Imposter imposter in gpc.GraphicController.Boxes[i].Imposters)
            {
                imposter.Toptarget = gpc.GraphicController.Boxes[i].TopBox.position;
            }
        }
    }
}
