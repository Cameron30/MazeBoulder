using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{

    /**
     * Created by Jordan Nguyen on May/2018.
     * This time, we are going to make the random line at length/2
     */
    static System.Random rnd = new System.Random();
    public char[,] grid = new char[33, 33];
    //static int stop = 0;
    static int[] firstGap = new int[2];
    public bool done = false;

    void Start()
    {
        // outer edges are walls
        // indexes start at like, zero
        for (int z = 0; z < grid.GetLength(0); z++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[z, y] = '-';
            }
        }
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            grid[i, 0] = 'X';
            grid[i, grid.GetLength(0) - 1] = 'X';
            grid[0, i] = 'X';
            grid[grid.GetLength(0) - 1, i] = 'X';
        }

        sanzuRiver(0, 0, grid.GetLength(0) - 1, grid.GetLength(0) - 1, grid.GetLength(0) - 1, grid.GetLength(0) - 1, 0);
        //drawUp();
        // draw a line through the middle of every room
        //grid[firstGap[0]][firstGap[1]] = 'A';
        drawUp();
    }
    // grid[row][column]

    // this method outputs the 2D array in a visual format.
    //TODO CHANGE FOR UNITY
    public void drawUp()
    {
        //create holes
        grid[0, 1] = '-';
        grid[32, 31] = '-';

        bool closed = true;
        bool fixedWalls = false;
        int i = 0;
        //these methods fix the middle line, and puts a random hole in it if needed
        for (i = 1; i < grid.GetLength(0) - 1; i++)
        {
            if (grid[i, 16] == '-')
            {
                closed = false;
            }
        }
        if (closed)
        {
            while (fixedWalls == false)
            {
                int spot = rnd.Next(1, grid.GetLength(0));
                if (grid[spot, 15] == '-' && grid[spot, 17] == '-')
                {
                    grid[spot, 16] = '-';
                    fixedWalls = true;
                }
            }

        }


        //creates walls
        for (int c = 0; c < grid.GetLength(0); c++)
        {
            for (int d = 0; d < grid.GetLength(0); d++)
            {
                string wall;
                bool rotateTrap = false;
                // 1/10 are traps
                var isTrap = rnd.Next(1, 11);
                //var trapNum = rnd.Next(1, 2);

                //if actual wall, randomize
                if (grid[c, d] == 'X')
                {
                    var num = rnd.Next(1, 6);
                    wall = "Forest_" + num;
                }
                else  //else decide if trap or not
                {
                    if (c != 0 && c != 35)
                    {
                        if ((isTrap == 2) && grid[c - 1, d] == '-' && grid[c + 1, d] == '-' && grid[c, d + 1] != '-' && grid[c, d - 1] != '-')
                        {
                            wall = "Pit_" + 1;
                            grid[c, d] = '!';
                            rotateTrap = true;
                        }
                        else if ((isTrap == 2) && grid[c, d - 1] == '-' && grid[c, d + 1] == '-' && grid[c - 1, d] != '-' && grid[c + 1, d] != '-')
                        {
                            wall = "Pit_" + 1;
                            grid[c, d] = '!';
                        }
                        else if (isTrap == 3)
                        {
                            wall = "Pit";
                            grid[c, d] = '!';
                        }
                        else if (isTrap == 4)
                        {
                            wall = "Pit_2";
                            grid[c, d] = '!';
                        }
                        else
                        {
                            //get floorNum
                            var num = rnd.Next(1, 11);

                            if (num == 2)
                            {
                                wall = "Floor_" + 2;
                                grid[c, d] = '+';
                            }
                            else
                            {
                                wall = "Floor_" + 1;
                            }
                        }
                    }
                    else
                    {
                        //get floorNum
                        var num = rnd.Next(1, 11);

                        if (num == 2)
                        {
                            wall = "Floor_" + 2;
                            grid[c, d] = '+';
                        }
                        else
                        {
                            wall = "Floor_" + 1;
                        }
                    }


                }
                
                if (c == 1 && d == 0)
                {
                    wall = "Pit_2";
                }
                
                var instance = (GameObject)Instantiate(Resources.Load(wall));
                

                if (rotateTrap == true)
                {
                    instance.transform.rotation = Quaternion.Euler(0, 90, 0);
                    rotateTrap = false;
                }
                else if (isTrap != 2)
                {
                    var rot = rnd.Next(0, 4);
                    instance.transform.rotation = Quaternion.Euler(0, rot * 90, 0);
                }

                instance.transform.position = new Vector3(transform.position.x + c * 5 - 80f, transform.position.y, transform.position.z + d * 5 - 80f);
                instance.transform.parent = transform;

                
            }
        }
        var newRoom = (GameObject)Instantiate(Resources.Load("Buffer Room"));
        newRoom.transform.position = new Vector3(transform.position.x + 92.5f, transform.position.y + 2.5f, transform.position.z + 75);

        done = true;
    }

    // Which way to draw the line
    // cut parallel to the shortest part
    public int seijaChoosesDirection(int myBase, int height)
    {
        int orientation = 0;
        if (height > myBase)
        {
            orientation = 1; // cut parallel to the base
        }
        else if (height < myBase)
        {
            orientation = 0; // cut parallel to the height
        }
        else
        {
            orientation = rnd.Next(0, 1); // cut random if they equal
        }
        return orientation;
    }

    /*
     * rand		random number generator object
     * startR	top left YCoord of grid /row
     * startC	top left XCoord of grid /column
     * endR		bottom left YCoord of grid /row
     * endC		top right XCoord of grid /column
     * sectionHeight	length of height of section
     * sectionBase		length of base of section
     * rP       random point; to ensure that no random line blocks a gap
     */
    public void theMinistryOfRightAndWrong(int startR, int startC, int endR, int endC,
                                                  int sectionHeight, int sectionBase, int rP)
    {
        int orient = seijaChoosesDirection(sectionBase, sectionHeight);

        // FOR CHECKING STEP BY STEP (remember read.close() at the end of main()
        //System.out.print("Well? ");
        //int step = read.nextInt();
        //drawUp();
        //System.out.println("Height "+sectionHeight);
        //System.out.println("Base "+sectionBase);
        // variables for subfields later on
        int newR;
        int newC;
        int newER;
        int newEC;
        int newBase;
        int newHeight;

        // what happens depends on the orientation
        if (orient == 1)
        {
            // cut parallel to base
            // pick a LINE that is basically just length/2
            int randomLine = (endR + startR) / 2;
            // draw in the line in the section
            for (int i = startC; i <= endC; i++)
            {
                grid[randomLine, i] = 'X';
            } // start at the row chosen and go along the length by iterating column
              // pick a random spot on that line that's not in the middle lol
            int randomPoint = 0;
            while (randomPoint == 0 || randomPoint % 2 == 0)
            {
                randomPoint = rnd.Next(startC + 1, endC - 1);

                if (randomPoint % 3 == 0 && (startR + 1 - (endR - 1) > 4))
                {
                    randomPoint = rnd.Next(startC + 1, endC - 1);
                }
            }
            grid[randomLine, randomPoint] = '-';
            // Okay, great. Now we have a line with a hole in it.

            // Let's determine subfields
            // you got topfield
            newR = startR;
            newC = startC;
            newER = randomLine;
            newEC = endC;
            newBase = sectionBase;
            newHeight = randomLine - startR; // :0
            if (newER - newR > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
            // you got bottom field
            newR = randomLine;
            newC = startC;
            newER = endR;
            newEC = endC;
            newBase = sectionBase;
            //newHeight = sectionHeight - (randomLine - startY);
            newHeight = endR - randomLine;
            if (newER - newR > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
        }
        else
        {
            // cut parallel to height
            // pick a random line parallel to the height, range is base
            int randomLine = (endC + startC) / 2;
            // draw in the line in the section
            for (int i = startR; i <= endR; i++)
            {
                grid[i, randomLine] = 'X';
            } // start at ycoord and go along the length
              // pick a random spot on that line
            int randomPoint = 0;
            while (randomPoint == 0 || randomPoint % 2 == 0)
            {
                randomPoint = rnd.Next(startR + 1, endR - 1);
                if (randomPoint % 3 == 0 && (startR + 1 - (endR - 1) > 4))
                {
                    randomPoint = rnd.Next(startR + 1, endR - 1);
                }
            }
            grid[randomPoint, randomLine] = '-';
            // Okay, great. Now we have a vertical (whoa) line with a hole in it.

            // Subfields! of two.
            // LEFT FIELD
            newR = startR;
            newC = startC;
            newEC = randomLine;
            newER = endR;
            newBase = randomLine;
            newHeight = endR - startR;

            if (newEC - newC > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
            // RIGHT FIELD
            newC = randomLine;
            newR = startR;
            newEC = endC;
            newER = endR;
            newBase = endC - randomLine;
            newHeight = endR - startR;

            if (newEC - newC > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
        }
    }
    // first iteration to save the gap
    public void sanzuRiver(int startR, int startC, int endR, int endC,
            int sectionHeight, int sectionBase, int rP)
    {

        int orient = seijaChoosesDirection(sectionBase, sectionHeight);

        // variables for subfields later on
        int newR;
        int newC;
        int newER;
        int newEC;
        int newBase;
        int newHeight;

        // what happens depends on the orientation
        if (orient == 1)
        {
            // cut parallel to base
            // pick a LINE that is basically just length/2
            int randomLine = (endR + startR) / 2;
            // draw in the line in the section
            for (int i = startC; i <= endC; i++)
            {
                grid[randomLine, i] = 'X';
            } // start at the row chosen and go along the length by iterating column
              // pick a random spot on that line that's not in the middle lol
            int randomPoint = 0;
            while (randomPoint == 0 || randomPoint == (endC + startC) / 2)
            {
                randomPoint = rnd.Next(startC + 1, endC - 1);
            }
            grid[randomLine, randomPoint] = '-';
            firstGap[0] = randomLine;
            firstGap[1] = randomPoint;
            // Okay, great. Now we have a line with a hole in it.

            // Let's determine subfields
            // you got topfield
            newR = startR;
            newC = startC;
            newER = randomLine;
            newEC = endC;
            newBase = sectionBase;
            newHeight = randomLine - startR; // :0
            if (newER - newR > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
            // you got bottom field
            newR = randomLine;
            newC = startC;
            newER = endR;
            newEC = endC;
            newBase = sectionBase;
            //newHeight = sectionHeight - (randomLine - startY);
            newHeight = endR - randomLine;
            if (newER - newR > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
        }
        else
        {
            // cut parallel to height
            // pick a random line parallel to the height, range is base
            int randomLine = (endC + startC) / 2;
            // draw in the line in the section
            for (int i = startR; i <= endR; i++)
            {
                grid[i, randomLine] = 'X';
            } // start at ycoord and go along the length
              // pick a random spot on that line
            int randomPoint = 0;
            while (randomPoint == 0 || randomPoint == (startR + endR) / 2)
            {
                randomPoint = rnd.Next(startR + 1, endR - 1);
            }
            grid[randomPoint, randomLine] = '-';
            firstGap[0] = randomLine;
            firstGap[1] = randomPoint;
            // Okay, great. Now we have a vertical (whoa) line with a hole in it.

            // Subfields! of two.
            // LEFT FIELD
            newR = startR;
            newC = startC;
            newEC = randomLine;
            newER = endR;
            newBase = randomLine;
            newHeight = endR - startR;

            if (newEC - newC > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
            // RIGHT FIELD
            newC = randomLine;
            newR = startR;
            newEC = endC;
            newER = endR;
            newBase = endC - randomLine;
            newHeight = endR - startR;

            if (newEC - newC > 2)
            {
                theMinistryOfRightAndWrong(newR, newC, newER, newEC, newHeight, newBase, randomPoint);
            }
        }
    }
}
