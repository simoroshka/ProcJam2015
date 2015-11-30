using UnityEngine;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
   
    public class Cell
    {
        public bool visited;
        public GameObject top;
        public GameObject left;
        public GameObject right;
        public GameObject down;
    }


    public GameObject wall;
    public GameObject floor;
    public float wallLength = 1.0f;
    private int xSize = 3;
    private int ySize = 3;

    private Vector3 initialPos;
    private GameObject wallHolder;
    public Cell[] cells;
    public int currentCell;
    private int totalCells;
    private int visitedCells;
    private bool startedBuilding = false;
    private int currentNeighbour;
    private List<int> lastCells;
    private int backingUp;
    private int wallToBreak;

    // Use this for initialization
    void Start () {
        createLevel();
    }

    public void createLevel()
    {
        currentCell = 0;
        visitedCells = 0;
        startedBuilding = false;
        currentNeighbour = 0;
        backingUp = 0;
        wallToBreak = 0;

        CreateWalls();
        CreateFloor();
        SetStartEndPoints();

        xSize += 2;
        ySize += 2;
    }

    void SetStartEndPoints()
    {
        GameObject startPoint = GameObject.Find("start");
        startPoint.transform.position = cells[0].left.transform.position + new Vector3(0.5f, 0.5f);

        GameObject endPoint = GameObject.Find("exit");
        endPoint.transform.position = cells[cells.Length - 1].right.transform.position + new Vector3(-0.5f, 0.0f, 0.0f) ;

        GameObject player = GameObject.Find("player");
        player.transform.position = startPoint.transform.position + new Vector3(0.0f, 0.0f, 0.0f);
    }
    void CreateFloor()
    {
        GameObject temp = Instantiate(floor, new Vector3(0.0f, 0.0f), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(xSize, 0.1f, ySize);
        temp.transform.position = new Vector3(0.0f, 0.0f, ySize / 2 - 0.5f);
        temp.transform.parent = wallHolder.transform;

    }

    void CreateWalls ()
    {
        wallHolder = new GameObject();
        wallHolder.name = "maze";

        initialPos = new Vector3((wallLength - xSize) / 2, 
                                  0.0f, 
                                  (wallLength - ySize) / 2);
        Vector3 myPos;
        GameObject temp;

        //for x axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, 
                                    0.0f, 
                                    initialPos.y + (i * wallLength) - wallLength / 2);
                temp = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                temp.transform.parent = wallHolder.transform;
            }
        }

        //for y axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength),
                                    0.0f,
                                    initialPos.y + (i * wallLength) - wallLength);
                temp = Instantiate(wall, myPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                temp.transform.parent = wallHolder.transform;
            }
        }

        
        CreateCells();
    }
    void CreateCells ()
    {
        totalCells = xSize * ySize;
        lastCells = new List<int>();
        lastCells.Clear();
        cells = new Cell[totalCells];

        int children = wallHolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];

        //get all children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //assign walls to the cells

        int leftRight = 0;
        int child = 0;
        int termCount = 0; 

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = new Cell();
            cells[i].left = allWalls[leftRight];
            cells[i].down = allWalls[child + (xSize + 1) * ySize];

            if (termCount ==xSize)
            {
                leftRight += 2;
                termCount = 0;
            }
            else
            {
                leftRight++;
            }

            termCount++;
            child++;

            cells[i].right = allWalls[leftRight];
            cells[i].top = allWalls[child + (xSize + 1) * ySize + xSize - 1];

        }
        CreateMaze();
    }
    void BreakWall()
    {
        switch(wallToBreak)
        {
            case 1: Destroy(cells[currentCell].top); break;
            case 2: Destroy(cells[currentCell].left); break;
            case 3: Destroy(cells[currentCell].right); break;
            case 4: Destroy(cells[currentCell].down); break;
        }
    }

    void CreateMaze()
    {
        while (visitedCells < totalCells)
        {
            Debug.Log(currentCell);
            if (startedBuilding)
            {
                FindNeighbours();
                if (!cells[currentNeighbour].visited && cells[currentCell].visited)
                {
                    BreakWall();
                    cells[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            //start building with (0.0) cell
            else
            {
                currentCell = 0;
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }

          
        }

        Debug.Log("finished");
    }
    void FindNeighbours()
    {
        int length = 0;
        int[] neighbours = new int[4];
        int[] connectingWall = new int[4];
        int check = (currentCell + 1) / xSize;
        check -= 1;
        check *= xSize;
        check += xSize;

        //right
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                neighbours[length] = currentCell + 1;
                connectingWall[length] = 3;
                length++;
            }
        }

        //left
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbours[length] = currentCell - 1;
                connectingWall[length] = 2;
                length++;
            }
        }

        //top
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbours[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }

        //bottom
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbours[length] = currentCell - xSize;
                connectingWall[length] = 4;
                length++;
            }
        }

        if (length != 0)
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbour = neighbours[theChosenOne];
            wallToBreak = connectingWall[theChosenOne];
        }
        else
        {
            if (backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
