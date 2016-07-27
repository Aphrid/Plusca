using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 *  Maintains the state of the board. This includes:
 *      - Creating and destroying the squares in the correct places
 *      - Tracking selected squares
 */
public class BoardManager : MonoBehaviour {

    private static float BASE_POSITION_X = 0.5f; // marks the x,y of (0,0) positions on the board
    private static float BASE_POSITION_Y = 3.3f;
    private static float SPAWN_DELAY = 0.5f;

    private GameController gameController;
    private GameObject[,] grid;
    private GameObject prefabTemp;
    private List<int[]> selected;   

    public GameObject square; // square prefab to be instantiated

    // Spawns the initial 16 squares on the board
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        selected = new List<int[]>();
        grid = new GameObject[4, 4];
        CreateSquares();
    }

    // Removes the appropriate squares if a correct sum was given
    void Update()
    {
        if (selected.Count > 1 && Input.GetKeyDown(KeyCode.Space) && !gameController.GetGameOver())
        {
            CheckSum();
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            RefreshBoard();
        }
    }

    // Compares the selected squares with the goal
    public void CheckSum()
    {
        int goal;
        if (gameController.GetCurSignIndex() == 0)
        {
            goal = 0;
        }
        else
        {
            goal = grid[selected[0][0], selected[0][1]].GetComponent<Square>().GetValue();
        }

        for (int i = 0; i < selected.Count; i++)
        {
            if (gameController.GetCurSignIndex() == 0)
            {
                goal += grid[selected[i][0], selected[i][1]].GetComponent<Square>().GetValue();
            } else if (i > 0)
            {
                goal -= grid[selected[i][0], selected[i][1]].GetComponent<Square>().GetValue();
            }
        }

        if (gameController.CheckGoal(goal))
        {
            for (int i = 0; i < selected.Count; i++)
            {
                Destroy(grid[selected[i][0], selected[i][1]].GetComponent<Square>().GetValueText().gameObject);
                Destroy(grid[selected[i][0], selected[i][1]].GetComponent<Square>().gameObject);
                StartCoroutine(SpawnNew(selected[i][0], selected[i][1]));
            }
            selected = new List<int[]>();
        }
        else
        {
            Debug.Log("Sum was incorrect!");
        }
    }

    // Creates a new number on every grid square after deleting the old squares
    public void RefreshBoard()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Destroy(grid[i,j].GetComponent<Square>().GetValueText().gameObject);
                Destroy(grid[i, j].gameObject);
            }
        }
        selected = new List<int[]>();
        CreateSquares();
    }

    // Instantiates a square gameObject at each grid location
    void CreateSquares()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                prefabTemp = Instantiate(square, new Vector3(BASE_POSITION_X + (j * 2.2f), BASE_POSITION_Y - (i * 2.2f), 0.0f), transform.rotation) as GameObject;
                prefabTemp.GetComponent<Square>().SetPos(j, i);
                grid[j, i] = prefabTemp;
            }
        }
    }

    // Remembers which squares were selected
    public void SelectSq(int x, int y)
    {
        int[] temp = { x, y };
        selected.Add(temp);
        Debug.Log("("+x.ToString()+","+y.ToString()+") was added to selected list");
    }

    // Remove the selected square
    public void DeselectSq(int x, int y)
    {
        for(int i = 0; i < selected.Count; i++)
        {
            if (selected[i][0] == x && selected[i][1] == y)
            {
                Debug.Log("(" + x.ToString() + "," + y.ToString() + ") was removed from selected list");
                selected.RemoveAt(i);
                break;
            }
        }        
    }

    // Spawns a new square after the old one is destroyed
    public IEnumerator SpawnNew(int x, int y)
    {
        yield return new WaitForSeconds(SPAWN_DELAY);
        prefabTemp = Instantiate(square, new Vector3(BASE_POSITION_X + (x * 2.2f), BASE_POSITION_Y - (y * 2.2f), 0.0f), transform.rotation) as GameObject;
        prefabTemp.GetComponent<Square>().SetPos(x, y);
        grid[x, y] = prefabTemp;
    }
}
