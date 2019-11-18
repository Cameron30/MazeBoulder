using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mapper : MonoBehaviour
{

    private bool mapped = false;
    private TextMeshPro textMesh;
    private string text;
    static System.Random rnd = new System.Random();

    private float scale;
    //private int k = 0;
    private GameObject drone;
    private bool update;

    private bool first = true;

    private char[,] grid = new char[33, 33];
    private bool[,] boolGrid = new bool[33, 33];

    // Use this for initialization
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        //get drone if possible
        var possibleDrone = GameObject.Find("Drone");
        if (possibleDrone != null)
        {
            drone = possibleDrone;
        }

        if (drone != null)
        {
            update = drone.GetComponent<DroneFly>().update;
            boolGrid = drone.GetComponent<DroneFly>().grid;
        }

        if (update)
        {
            mapped = false;
            text = "";
            update = false;
            drone.GetComponent<DroneFly>().update = false;
        }

        //map the maze
        if (!mapped)
        {
            var maze = GameObject.Find("Base(Clone)");

            if (maze != null)
            {
                if (maze.GetComponent<MazeGen>().done)
                {
                    //goes through and creates Os only once (not every update)
                    if (first == true)
                    {
                        var realGrid = maze.GetComponent<MazeGen>().grid;
                        for (int i = realGrid.GetLength(0) - 1; i > -1; i--)
                        {
                            for (int j = realGrid.GetLength(0) - 1; j > -1; j--)
                            {
                                var num = rnd.Next(1, 11);
                                if (num == 3 && realGrid[i, j] != '!')
                                {
                                    grid[i, j] = 'O';
                                }
                                else
                                {
                                    if (realGrid[i, j] == '-')
                                    {
                                        grid[i, j] = ' ';
                                    }
                                    else if (realGrid[i, j] != '\0')
                                    {
                                        grid[i, j] = realGrid[i, j];
                                    }

                                }
                            }


                        }
                        first = false;
                    }
                }

                for (int i = grid.GetLength(0) - 1; i > -1; i--)
                {
                    for (int j = grid.GetLength(0) - 1; j > -1; j--)
                    {
                        if (grid[i, j] == '!' || grid[i, j] == '+')
                        {
                            if (boolGrid[i, j] == false)
                            {
                                text += ' ';
                            }
                            else
                            {
                                if (grid[i, j] == '!')
                                {
                                    text += "<color=red>!<color=black>";
                                }
                                else
                                {
                                    text += "<color=green>+<color=black>";
                                }

                            }
                        }
                        else if (grid[i, j] == 'O')
                        {
                            text += "<color=blue>O<color=black>";
                        }
                        else
                        {
                            text += grid[i, j];
                        }
                    }
                    text += '\n';
                }
                textMesh.text = text;
                mapped = true;
            }
        }
    }
}
