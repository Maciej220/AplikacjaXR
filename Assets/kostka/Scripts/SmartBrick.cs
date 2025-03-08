using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using TMPro;

public class SmartBrick : MonoBehaviour
{
    public TextMeshProUGUI statueText; // Wyświetlanie wartości prędkości w interfejsie


    public AxisMenager axisManager;
    public HandControlActive HandControlActive;
    public BlokadeCubeRotate BlokadeCubeRotate;
    public DebugMenager DebugMenager;

    public GameObject Up;
    public GameObject Down;
    public GameObject Left;
    public GameObject Right;
    public GameObject Front;
    public GameObject Back;

    private GameObject[,,] SearchColorObject = new GameObject[6, 3, 3];
    private char[,,] ColorArray = new char[6, 3, 3];
    private char[,,] CopyColorArray = new char[6, 3, 3];

    private List<string> tags = new List<string> { "Blue", "Red", "White", "Yellow", "Green", "Orange" };

    private int status = 0;


    void Start()
    {
        
        AssignSearchColorObjects();
        DebugSearchColorObjects();
        FillColorArray();
        DebugColorArray();
        FinishTest();
        LBLStartCross();
    }

    public void FinishTest()
    {
        if (WallTest())
        {
            Debug.Log("win");
        }
    }

    public bool WallTest()
    {
        FillColorArray();
        for (int i = 0; i < 6; i++)
        {
            char referenceValue = ColorArray[i, 0, 0];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (ColorArray[i, x, y] != referenceValue)
                    {
                        return false;
                    }
                }
            }
        }
        DebugColorArray();
        return true;
    }

    void AssignSearchColorObjects()
    {
        GameObject[] parents = { Up, Down, Left, Right, Front, Back };
        for (int i = 0; i < parents.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    string childName = j.ToString() + k.ToString();
                    Transform childTransform = parents[i].transform.Find(childName);
                    if (childTransform != null)
                    {
                        SearchColorObject[i, j, k] = childTransform.gameObject;
                    }
                    else
                    {
                        Debug.LogWarning($"Child {childName} not found in parent {parents[i].name}");
                    }
                }
            }
        }
    }

    void DebugSearchColorObjects()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (SearchColorObject[i, j, k] != null)
                    {
                        Debug.Log($"Cell [{i}, {j}, {k}] - Object Name: {SearchColorObject[i, j, k].name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Cell [{i}, {j}, {k}] - Object not assigned");
                    }
                }
            }
        }
    }

    public char FindTagChar(GameObject gameObject)
    {
        string[] tags = { "Blue", "Orange", "White", "Yellow", "Red", "Green" };
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Bounds bounds = boxCollider.bounds;
            Collider[] hitColliders = Physics.OverlapBox(bounds.center, bounds.extents, boxCollider.transform.rotation);
            foreach (Collider hitCollider in hitColliders)
            {
                foreach (string tag in tags)
                {
                    if (hitCollider.CompareTag(tag))
                    {
                        return tag[0];
                    }
                }
            }
        }
        return '\0';
    }

    void FillColorArray()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (SearchColorObject[i, j, k] != null)
                    {
                        ColorArray[i, j, k] = FindTagChar(SearchColorObject[i, j, k]);
                    }
                    else
                    {
                        ColorArray[i, j, k] = 'N';
                    }
                }
            }
        }
    }

    public void DebugColorArray()
    {
        FillColorArray();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    Debug.Log($"ColorArray[{i}, {j}, {k}] = {ColorArray[i, j, k]}");
                }
            }
        }
    }
    /*
   * Q X1 90   L'
   * W X1 -90  L
   * E X1 180  LL
   * 
   * P  X2  90
   * O  X2  -90
   * I  x2  180
   * 
   * R X3 90   R
   * T X3 -90  R'
   * Y X3 180  RR
   * 
   * 
   * A Y1 90   D'
   * S Y1 -90  D
   * D Y1 180  DD
   * 
   * L  Y2 90
   * K  Y2 -90
   * J  Y2 180
   * 
   * F Y3 90   U
   * G Y3 -90  U'
   * H Y3 180  UU
   * 
   * Z Z1 90   F'
   * X Z1 -90  F
   * C Z1 180  FF
   * 
   * .  Z2 90
   * ,  Z2 -90
   * M  Z2 180
   * 
   * V Z3 90   B
   * B Z3 -90  B'
   * N Z3 180  BB
   * 
   * 
   */




    IEnumerator Move(char move)
    {
        
        switch (move)
        {
            case 'Q':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, 90, 'X'));
                break;
            case 'W':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, -90, 'X'));
                break;
            case 'E':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, 180, 'X'));
                break;
            

            case 'P':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, 90, 'X'));
                break;
            case 'O':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, -90, 'X'));
                break;
            case 'I':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, 180, 'X'));
                break;
            case 'U':



            case 'R':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, 90, 'X'));
                break;
            case 'T':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, -90, 'X'));
                break;
            case 'Y':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, 180, 'X'));
                break;
            case 'A':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, 90, 'Y'));
                break;
            case 'S':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, -90, 'Y'));
                break;
            case 'D':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, 180, 'Y'));
                break;

            case 'L':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, 90, 'Y'));
                break;
            case 'K':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, -90, 'Y'));
                break;
            case 'J':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, 180, 'Y'));
                break;

            case 'F':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, 90, 'Y'));
                break;
            case 'G':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, -90, 'Y'));
                break;
            case 'H':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, 180, 'Y'));
                break;


            case 'Z':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, 90, 'Z'));
                break;
            case 'X':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, -90, 'Z'));
                break;
            case 'C':
                yield return StartCoroutine(HandControlActive.RotateAxis(0, 180, 'Z'));
                break;

            case '.':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, 90, 'Z'));
                break;
            case ',':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, -90, 'Z'));
                break;
            case 'M':
                yield return StartCoroutine(HandControlActive.RotateAxis(1, 180, 'Z'));
                break;


            case 'V':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, 90, 'Z'));
                break;
            case 'B':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, -90, 'Z'));
                break;
            case 'N':
                yield return StartCoroutine(HandControlActive.RotateAxis(2, 180, 'Z'));
                break;
        }
       

    }
    public void lblStart()
    {
        StartCoroutine(LBLStartCross());
    }
    public void lblStepByStep()
    {
        StartCoroutine(LBLSBS());
    }
    private IEnumerator LBLStartPosition()
    {
        FillColorArray();
        //pole koloru srodka
        int x1 = 0;
        int y1 = 1;
        int z1 = 1;
       

        //ukladanie poprawnej pozycji

        if (ColorArray[x1, y1, z1] == 'W')
        {
            yield return StartCoroutine(Move('E'));//up
            yield return StartCoroutine(Move('I'));//up
            yield return StartCoroutine(Move('Y'));//up

        }
        x1 = 1;
        if (ColorArray[x1, y1, z1] == 'W')
        {
            //pole docelowe down
        }
        x1 = 2;
        if (ColorArray[x1, y1, z1] == 'W')
        {
            yield return StartCoroutine(Move('Z'));
            yield return StartCoroutine(Move('.'));
            yield return StartCoroutine(Move('V'));

        }
        x1 = 3;
        if (ColorArray[x1, y1, z1] == 'W')
        {
            yield return StartCoroutine(Move('X'));
            yield return StartCoroutine(Move(','));
            yield return StartCoroutine(Move('B'));

        }
        x1 = 4;
        if (ColorArray[x1, y1, z1] == 'W')
        {
            yield return StartCoroutine(Move('W'));
            yield return StartCoroutine(Move('O'));
            yield return StartCoroutine(Move('T'));

        }
        x1 = 5;
        if (ColorArray[x1, y1, z1] == 'W')
        {
            yield return StartCoroutine(Move('Q'));
            yield return StartCoroutine(Move('P'));
            yield return StartCoroutine(Move('R'));

        }
        FillColorArray();

        x1 = 2;
        if (ColorArray[x1, y1, z1] == 'R')
        {
            yield return StartCoroutine(Move('S'));
            yield return StartCoroutine(Move('K'));
            yield return StartCoroutine(Move('G'));

        }
        x1 = 3;
        if (ColorArray[x1, y1, z1] == 'R')
        {
            yield return StartCoroutine(Move('A'));
            yield return StartCoroutine(Move('L'));
            yield return StartCoroutine(Move('F'));

        }
        x1 = 4;
        if (ColorArray[x1, y1, z1] == 'R')
        {
            ////
        }
        x1 = 5;
        if (ColorArray[x1, y1, z1] == 'R')
        {
            yield return StartCoroutine(Move('D'));
            yield return StartCoroutine(Move('J'));
            yield return StartCoroutine(Move('H'));

        }
    }
   
    private IEnumerator LBLCross()
    {
        FillColorArray();
        //pole koloru srodka
        int x1 = 0;
        int y1 = 1;
        int z1 = 1;
        int x2 = 2;
        int y2 = 1;
        int z2 = 2;
        //blok testowany
        int x3 = 1;
        int y3 = 1;
        int z3 = 1;
        int x4 = 2;
        int y4 = 1;
        int z4 = 1;
        x1 = 1; y1 = 1; z1 = 1;
        x2 = 4; y2 = 1; z2 = 1;
        x3 = 1; y3 = 1; z3 = 2;
        x4 = 4; y4 = 1; z4 = 2;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //break;
        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //FFULF'L'
            yield return StartCoroutine(Move('C'));

            yield return StartCoroutine(Move('F'));

            yield return StartCoroutine(Move('W'));

            yield return StartCoroutine(Move('Z'));

            yield return StartCoroutine(Move('Q'));

        }//ok
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 3; y3 = 2; z3 = 1;
        x4 = 4; y4 = 2; z4 = 1;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //F
            yield return StartCoroutine(Move('X'));

           
        }//ok
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //F'
            yield return StartCoroutine(Move('Z'));

        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 0; y3 = 1; z3 = 2;
        x4 = 4; y4 = 1; z4 = 0;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //FF
            yield return StartCoroutine(Move('C'));

            //yield return StartCoroutine(Move('F'));
            //yield return StartCoroutine(Move('W'));
            //yield return StartCoroutine(Move('Z'));
            //yield return StartCoroutine(Move('Q'));
        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //U'R'FR
            yield return StartCoroutine(Move('G'));

            yield return StartCoroutine(Move('T'));

            yield return StartCoroutine(Move('X'));

            yield return StartCoroutine(Move('R'));

            //yield return StartCoroutine(Move('A'));
            //yield return StartCoroutine(Move('X'));
        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 2; y3 = 2; z3 = 1;
        x4 = 4; y4 = 0; z4 = 1;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //F'
            yield return StartCoroutine(Move('Z'));

            
        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //F
            yield return StartCoroutine(Move('X'));

          
        }

        //przeszukiwanie przodu zakonczone

        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 2; y3 = 1; z3 = 0;
        x4 = 0; y4 = 0; z4 = 1;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //U'
            yield return StartCoroutine(Move('G'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //D'LD
            yield return StartCoroutine(Move('A'));
            yield return StartCoroutine(Move('W'));
            yield return StartCoroutine(Move('S'));

        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 3; y3 = 1; z3 = 2;
        x4 = 1; y4 = 2; z4 = 1;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //R
            yield return StartCoroutine(Move('R'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //R
            yield return StartCoroutine(Move('R'));


        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 2; y3 = 1; z3 = 2;
        x4 = 1; y4 = 0; z4 = 1;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //L'
            yield return StartCoroutine(Move('Q'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //L'
            yield return StartCoroutine(Move('Q'));

            

        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 0; y3 = 2; z3 = 1;
        x4 = 3; y4 = 1; z4 = 0;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //U
            yield return StartCoroutine(Move('F'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //U
            yield return StartCoroutine(Move('F'));

            


        }

        ///BACK
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 0; y3 = 1; z3 = 0;
        x4 = 5; y4 = 1; z4 = 0;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //U'
            yield return StartCoroutine(Move('G'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //U'
            yield return StartCoroutine(Move('G'));


        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 3; y3 = 0; z3 = 1;
        x4 = 5; y4 = 2; z4 = 1;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //R'UR
            yield return StartCoroutine(Move('T'));

            yield return StartCoroutine(Move('F'));

            yield return StartCoroutine(Move('R'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //R'UR
            yield return StartCoroutine(Move('T'));

            yield return StartCoroutine(Move('F'));

            yield return StartCoroutine(Move('R'));


        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 2; y3 = 0; z3 = 1;
        x4 = 5; y4 = 0; z4 = 1;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //LU'L'
            yield return StartCoroutine(Move('W'));

            yield return StartCoroutine(Move('G'));

            yield return StartCoroutine(Move('Q'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //LU'L'
            yield return StartCoroutine(Move('W'));

            yield return StartCoroutine(Move('G'));

            yield return StartCoroutine(Move('Q'));


        }
        //x1 = 1; y1 = 1; z1 = 1;
        //x2 = 4; y2 = 1; z2 = 1;
        x3 = 1; y3 = 1; z3 = 0;
        x4 = 5; y4 = 1; z4 = 2;
        if ((ColorArray[x1, y1, z1] == ColorArray[x3, y3, z3]) && (ColorArray[x2, y2, z2] == ColorArray[x4, y4, z4]))
        {
            //BB
            yield return StartCoroutine(Move('N'));


        }
        if ((ColorArray[x1, y1, z1] == ColorArray[x4, y4, z4]) && (ColorArray[x2, y2, z2] == ColorArray[x3, y3, z3]))
        {
            //BB
            yield return StartCoroutine(Move('N'));



        }
    }

    private IEnumerator LBLWhiteWall()
    {

        FillColorArray();
        if (ColorArray[4, 2, 0] == ColorArray[4, 1, 1] && ColorArray[3, 2, 0] == ColorArray[1, 1, 1] && ColorArray[0, 2, 2] == ColorArray[3, 1, 1])
        {
            //RUR'
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('T'));

        }
        if (ColorArray[4, 2, 0] == ColorArray[3, 1, 1] && ColorArray[3, 2, 0] == ColorArray[4, 1, 1] && ColorArray[0, 2, 2] == ColorArray[1, 1, 1])
        {
            //RRURRU'RR
            yield return StartCoroutine(Move('Y'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('Y'));
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('Y'));

        }
        if (ColorArray[4, 2, 0] == ColorArray[1, 1, 1] && ColorArray[3, 2, 0] == ColorArray[3, 1, 1] && ColorArray[0, 2, 2] == ColorArray[4, 1, 1])
        {
            //URU'R'
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('T'));

        }
        FillColorArray();
        if (ColorArray[1, 1, 1] == ColorArray[0, 0, 0] || ColorArray[1, 1, 1] == ColorArray[0, 0, 2] || ColorArray[1, 1, 1] == ColorArray[0, 2, 0] || ColorArray[1, 1, 1] == ColorArray[0, 2, 2] || ColorArray[1, 1, 1] == ColorArray[4, 0, 0] || ColorArray[1, 1, 1] == ColorArray[4, 2, 0] || ColorArray[1, 1, 1] == ColorArray[3, 2, 0] || ColorArray[1, 1, 1] == ColorArray[3, 0, 0] || ColorArray[1, 1, 1] == ColorArray[5, 2, 0] || ColorArray[1, 1, 1] == ColorArray[5, 0, 0] || ColorArray[1, 1, 1] == ColorArray[2, 0, 0] || ColorArray[1, 1, 1] == ColorArray[2, 2, 0] )
        {
            if (ColorArray[1, 1, 1] == ColorArray[0, 0, 2] || ColorArray[1, 1, 1] == ColorArray[4, 0, 0] || ColorArray[1, 1, 1] == ColorArray[2, 2, 0])
            {
                //FRONT UP LEFT 
                if ((ColorArray[2, 2, 0] == ColorArray[2, 1, 1] || ColorArray[4, 0, 0] == ColorArray[2, 1, 1] || ColorArray[0, 0, 2] == ColorArray[2, 1, 1])&& (ColorArray[2, 2, 0] == ColorArray[4, 1, 1] || ColorArray[4, 0, 0] == ColorArray[4, 1, 1] || ColorArray[0, 0, 2] == ColorArray[4, 1, 1]))
                {
                    yield return StartCoroutine(Move('G'));
                    yield return StartCoroutine(Move('K'));
                    yield return StartCoroutine(Move('S'));

                }
                //FRONT UP RIGHT 
                else if ((ColorArray[2, 2, 0] == ColorArray[3, 1, 1] || ColorArray[4, 0, 0] == ColorArray[3, 1, 1] || ColorArray[0, 0, 2] == ColorArray[3, 1, 1]) && (ColorArray[2, 2, 0] == ColorArray[4, 1, 1] || ColorArray[4, 0, 0] == ColorArray[4, 1, 1] || ColorArray[0, 0, 2] == ColorArray[4, 1, 1]))
                {
                    yield return StartCoroutine(Move('G'));

                }
                //BACK UP RIGHT 
                else if ((ColorArray[2, 2, 0] == ColorArray[3, 1, 1] || ColorArray[4, 0, 0] == ColorArray[3, 1, 1] || ColorArray[0, 0, 2] == ColorArray[3, 1, 1]) && (ColorArray[2, 2, 0] == ColorArray[5, 1, 1] || ColorArray[4, 0, 0] == ColorArray[5, 1, 1] || ColorArray[0, 0, 2] == ColorArray[5, 1, 1]))
                {
                    yield return StartCoroutine(Move('G'));
                    yield return StartCoroutine(Move('L'));
                    yield return StartCoroutine(Move('A'));
                }
                //BACK UP LEFT 
                else if ((ColorArray[2, 2, 0] == ColorArray[2, 1, 1] || ColorArray[4, 0, 0] == ColorArray[2, 1, 1] || ColorArray[0, 0, 2] == ColorArray[2, 1, 1]) && (ColorArray[2, 2, 0] == ColorArray[5, 1, 1] || ColorArray[4, 0, 0] == ColorArray[5, 1, 1] || ColorArray[0, 0, 2] == ColorArray[5, 1, 1]))
                {

                    yield return StartCoroutine(Move('G'));
                    yield return StartCoroutine(Move('J'));
                    yield return StartCoroutine(Move('D'));

                }
                FillColorArray();
            }
            else if (ColorArray[1, 1, 1] == ColorArray[2, 0, 0] || ColorArray[1, 1, 1] == ColorArray[0, 0, 0] || ColorArray[1, 1, 1] == ColorArray[5, 0, 0])
            {
                //FRONT UP LEFT
                if ((ColorArray[2, 0, 0] == ColorArray[2, 1, 1] || ColorArray[0, 0, 0] == ColorArray[2, 1, 1] || ColorArray[5, 0, 0] == ColorArray[2, 1, 1]) && (ColorArray[2, 0, 0] == ColorArray[4, 1, 1] || ColorArray[0, 0, 0] == ColorArray[4, 1, 1] || ColorArray[5, 0, 0] == ColorArray[4, 1, 1]) )
                {
                    yield return StartCoroutine(Move('H'));
                    yield return StartCoroutine(Move('K'));
                    yield return StartCoroutine(Move('S'));

                }
                //FRONT UP RIGHT 
                else if ((ColorArray[2, 0, 0] == ColorArray[3, 1, 1] || ColorArray[0, 0, 0] == ColorArray[3, 1, 1] || ColorArray[5, 0, 0] == ColorArray[3, 1, 1]) && (ColorArray[2, 0, 0] == ColorArray[4, 1, 1] || ColorArray[0, 0, 0] == ColorArray[4, 1, 1] || ColorArray[5, 0, 0] == ColorArray[4, 1, 1]))
                {
                    yield return StartCoroutine(Move('H'));

                }
                //BACK UP RIGHT 
                else if ((ColorArray[2, 0, 0] == ColorArray[3, 1, 1] || ColorArray[0, 0, 0] == ColorArray[3, 1, 1] || ColorArray[5, 0, 0] == ColorArray[3, 1, 1]) && (ColorArray[2, 0, 0] == ColorArray[5, 1, 1] || ColorArray[0, 0, 0] == ColorArray[5, 1, 1] || ColorArray[5, 0, 0] == ColorArray[5, 1, 1]))
                {
                    yield return StartCoroutine(Move('H'));
                    yield return StartCoroutine(Move('L'));
                    yield return StartCoroutine(Move('A'));
                }
                //BACK UP LEFT 
                else if ((ColorArray[2, 0, 0] == ColorArray[2, 1, 1] || ColorArray[0, 0, 0] == ColorArray[2, 1, 1] || ColorArray[5, 0, 0] == ColorArray[2, 1, 1]) && (ColorArray[2, 0, 0] == ColorArray[5, 1, 1] || ColorArray[0, 0, 0] == ColorArray[5, 1, 1] || ColorArray[5, 0, 0] == ColorArray[5, 1, 1]))
                {

                    yield return StartCoroutine(Move('H'));
                    yield return StartCoroutine(Move('J'));
                    yield return StartCoroutine(Move('D'));

                }
                FillColorArray();
            }
            else if (ColorArray[1, 1, 1] == ColorArray[0, 2, 0] || ColorArray[1, 1, 1] == ColorArray[3, 0, 0] || ColorArray[1, 1, 1] == ColorArray[5, 2, 0])
            {
                //FRONT UP LEFT
                if ((ColorArray[0, 2, 0] == ColorArray[2, 1, 1] || ColorArray[3, 0, 0] == ColorArray[2, 1, 1] || ColorArray[5, 2, 0] == ColorArray[2, 1, 1])&& (ColorArray[0, 2, 0] == ColorArray[4, 1, 1] || ColorArray[3, 0, 0] == ColorArray[4, 1, 1] || ColorArray[5, 2, 0] == ColorArray[4, 1, 1]))
                {
                    yield return StartCoroutine(Move('F'));
                    yield return StartCoroutine(Move('K'));
                    yield return StartCoroutine(Move('S'));

                }
                //FRONT UP RIGHT 
                else if ((ColorArray[0, 2, 0] == ColorArray[3, 1, 1] || ColorArray[3, 0, 0] == ColorArray[3, 1, 1] || ColorArray[5, 2, 0] == ColorArray[3, 1, 1]) && (ColorArray[0, 2, 0] == ColorArray[4, 1, 1] || ColorArray[3, 0, 0] == ColorArray[4, 1, 1] || ColorArray[5, 2, 0] == ColorArray[4, 1, 1]))
                {
                    yield return StartCoroutine(Move('F'));

                }
                //BACK UP RIGHT 
                else if ((ColorArray[0, 2, 0] == ColorArray[3, 1, 1] || ColorArray[3, 0, 0] == ColorArray[3, 1, 1] || ColorArray[5, 2, 0] == ColorArray[3, 1, 1]) && (ColorArray[0, 2, 0] == ColorArray[5, 1, 1] || ColorArray[3, 0, 0] == ColorArray[5, 1, 1] || ColorArray[5, 2, 0] == ColorArray[5, 1, 1]))
                {
                    yield return StartCoroutine(Move('F'));
                    yield return StartCoroutine(Move('L'));
                    yield return StartCoroutine(Move('A'));
                }
                //BACK UP LEFT 
                else if ((ColorArray[0, 2, 0] == ColorArray[2, 1, 1] || ColorArray[3, 0, 0] == ColorArray[2, 1, 1] || ColorArray[5, 2, 0] == ColorArray[2, 1, 1]) && (ColorArray[0, 2, 0] == ColorArray[5, 1, 1] || ColorArray[3, 0, 0] == ColorArray[5, 1, 1] || ColorArray[5, 2, 0] == ColorArray[5, 1, 1]))
                {

                    yield return StartCoroutine(Move('F'));
                    yield return StartCoroutine(Move('J'));
                    yield return StartCoroutine(Move('D'));

                }
                FillColorArray();
            }
            else if (ColorArray[1, 1, 1] == ColorArray[0, 2, 2] || ColorArray[1, 1, 1] == ColorArray[4, 2, 0] || ColorArray[1, 1, 1] == ColorArray[3, 2, 0])
            {
                FillColorArray();
                if ((ColorArray[0, 2, 2] != ColorArray[4, 1, 1] || ColorArray[4, 2, 0] != ColorArray[4, 1, 1] || ColorArray[3, 2, 0] != ColorArray[4, 1, 1])&& (ColorArray[0, 2, 2] != ColorArray[3, 1, 1] || ColorArray[4, 2, 0] != ColorArray[3, 1, 1] || ColorArray[3, 2, 0] != ColorArray[3, 1, 1]))
                {
                    //FRONT UP LEFT
                    if ((ColorArray[0, 2, 2] == ColorArray[2, 1, 1] || ColorArray[4, 2, 0] == ColorArray[2, 1, 1] || ColorArray[3, 2, 0] == ColorArray[2, 1, 1])&& (ColorArray[0, 2, 2] == ColorArray[4, 1, 1] || ColorArray[4, 2, 0] == ColorArray[4, 1, 1] || ColorArray[3, 2, 0] == ColorArray[4, 1, 1]) )
                    {
                        yield return StartCoroutine(Move('K'));
                        yield return StartCoroutine(Move('S'));

                    }
                    //BACK UP LEFT
                    else if ((ColorArray[0, 2, 2] == ColorArray[2, 1, 1] || ColorArray[4, 2, 0] == ColorArray[2, 1, 1] || ColorArray[3, 2, 0] == ColorArray[2, 1, 1]) && (ColorArray[0, 2, 2] == ColorArray[5, 1, 1] || ColorArray[4, 2, 0] == ColorArray[5, 1, 1] || ColorArray[3, 2, 0] == ColorArray[5, 1, 1]))
                    {
                        yield return StartCoroutine(Move('J'));
                        yield return StartCoroutine(Move('D'));
                    }
                    //BACK UP RIGHT
                    else if ((ColorArray[0, 2, 2] == ColorArray[3, 1, 1] || ColorArray[4, 2, 0] == ColorArray[3, 1, 1] || ColorArray[3, 2, 0] == ColorArray[3, 1, 1]) && (ColorArray[0, 2, 2] == ColorArray[5, 1, 1] || ColorArray[4, 2, 0] == ColorArray[5, 1, 1] || ColorArray[3, 2, 0] == ColorArray[5, 1, 1]))
                    {
                        yield return StartCoroutine(Move('L'));
                        yield return StartCoroutine(Move('A'));
                    }
                    FillColorArray();
                }
                
               
            }
        }
        else
        {
            if(ColorArray[1, 1, 1] != ColorArray[1, 2, 2] || ColorArray[4, 1, 1] != ColorArray[4, 2, 2] || ColorArray[3, 1, 1] != ColorArray[3, 2, 2])
            {
                //RUR'
                yield return StartCoroutine(Move('R'));
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('T'));
            }
            if (ColorArray[1, 1, 1] != ColorArray[1, 0, 2] || ColorArray[4, 1, 1] != ColorArray[4, 0, 2] || ColorArray[2, 1, 1] != ColorArray[2, 2, 2])
            {
                //FUF'
                yield return StartCoroutine(Move('X'));
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('Z'));
            }
            if (ColorArray[1, 1, 1] != ColorArray[1, 0, 0] || ColorArray[2, 1, 1] != ColorArray[2, 0, 2] || ColorArray[5, 1, 1] != ColorArray[5, 0, 2])
            {
                //LUL'
                yield return StartCoroutine(Move('W'));
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('Q'));
            }
            if (ColorArray[1, 1, 1] != ColorArray[1, 2, 0] || ColorArray[5, 1, 1] != ColorArray[5, 2, 2] || ColorArray[3, 1, 1] != ColorArray[3, 0, 2])
            {
                //R'UR
                yield return StartCoroutine(Move('T'));
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('R'));
            }
        }

        
    }

    private IEnumerator LBLSecondLayer()
    {

        FillColorArray();
        if (ColorArray[4, 1, 1] == ColorArray[4, 1, 0] && ColorArray[3, 1, 1] == ColorArray[0, 1, 2])
        {
            //URU'R'U'F'UF
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('T'));
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('Z'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('X'));
        }
        if (ColorArray[4, 1, 1] == ColorArray[4, 1, 0] && ColorArray[2, 1, 1] == ColorArray[0, 1, 2])
        {
            //U'L'ULUFU'F'
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('Q'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('W'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('X'));
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('Z'));
        }
        FillColorArray();
        if ((ColorArray[4, 1, 1] != ColorArray[4, 1, 0] || ColorArray[3, 1, 1] != ColorArray[0, 1, 2]) || (ColorArray[3, 1, 1] != ColorArray[4, 1, 0] || ColorArray[4, 1, 1] != ColorArray[0, 1, 2]))
        {
            if ((ColorArray[3, 1, 1] == ColorArray[4, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 1, 2]))
            {
                 yield return StartCoroutine(Move('A'));
                 yield return StartCoroutine(Move('L'));
                FillColorArray();
            }
            else if ((ColorArray[5, 1, 1] == ColorArray[4, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 1, 2]))
            {
                yield return StartCoroutine(Move('J'));
                yield return StartCoroutine(Move('D'));
                FillColorArray();
            }
            else if ((ColorArray[2, 1, 1] == ColorArray[4, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 1, 2]))
            {
                yield return StartCoroutine(Move('K'));
                yield return StartCoroutine(Move('S'));
                FillColorArray();
            }



            else if ((ColorArray[4, 1, 1] == ColorArray[3, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 1]))
            {
                yield return StartCoroutine(Move('F'));
                
                FillColorArray();
            }
            else if ((ColorArray[3, 1, 1] == ColorArray[3, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 1]))
            {
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('A'));
                yield return StartCoroutine(Move('L'));
                FillColorArray();
            }
            else if ((ColorArray[5, 1, 1] == ColorArray[3, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 1]))
            {
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('J'));
                yield return StartCoroutine(Move('D'));
                FillColorArray();
            }
            else if ((ColorArray[2, 1, 1] == ColorArray[3, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 1]))
            {
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('K'));
                yield return StartCoroutine(Move('S'));
                FillColorArray();
            }



            else if ((ColorArray[4, 1, 1] == ColorArray[5, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 1, 0]))
            {
                yield return StartCoroutine(Move('H'));

                FillColorArray();
            }
            else if ((ColorArray[3, 1, 1] == ColorArray[5, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 1, 0]))
            {
                yield return StartCoroutine(Move('H'));
                yield return StartCoroutine(Move('A'));
                yield return StartCoroutine(Move('L'));
                FillColorArray();
            }
            else if ((ColorArray[5, 1, 1] == ColorArray[5, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 1, 0]))
            {
                yield return StartCoroutine(Move('H'));
                yield return StartCoroutine(Move('J'));
                yield return StartCoroutine(Move('D'));
                FillColorArray();
            }
            else if ((ColorArray[2, 1, 1] == ColorArray[5, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 1, 0]))
            {
                yield return StartCoroutine(Move('H'));
                yield return StartCoroutine(Move('K'));
                yield return StartCoroutine(Move('S'));
                FillColorArray();
            }

            else if ((ColorArray[4, 1, 1] == ColorArray[2, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 0, 1]))
            {
                yield return StartCoroutine(Move('G'));

                FillColorArray();
            }
            else if ((ColorArray[3, 1, 1] == ColorArray[2, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 0, 1]))
            {
                yield return StartCoroutine(Move('G'));
                yield return StartCoroutine(Move('A'));
                yield return StartCoroutine(Move('L'));
                FillColorArray();
            }
            else if ((ColorArray[5, 1, 1] == ColorArray[2, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 0, 1]))
            {
                yield return StartCoroutine(Move('G'));
                yield return StartCoroutine(Move('J'));
                yield return StartCoroutine(Move('D'));
                FillColorArray();
            }
            else if ((ColorArray[2, 1, 1] == ColorArray[2, 1, 0] && ColorArray[0, 1, 1] != ColorArray[0, 0, 1]))
            {
                yield return StartCoroutine(Move('G'));
                yield return StartCoroutine(Move('K'));
                yield return StartCoroutine(Move('S'));
                FillColorArray();
            }
            else
            {

                 if ((ColorArray[0, 1, 1] != ColorArray[4, 2, 1] && ColorArray[0, 1, 1] != ColorArray[3, 2, 1])&& (ColorArray[4, 1, 1] != ColorArray[4, 2, 1] || ColorArray[3, 1, 1] != ColorArray[3, 2, 1]) )
                {
                    //URU'R'U'F'UF
                    yield return StartCoroutine(Move('F'));
                    yield return StartCoroutine(Move('R'));
                    yield return StartCoroutine(Move('G'));
                    yield return StartCoroutine(Move('T'));
                    yield return StartCoroutine(Move('G'));
                    yield return StartCoroutine(Move('Z'));
                    yield return StartCoroutine(Move('F'));
                    yield return StartCoroutine(Move('X'));
                }
                 else
                {
                    yield return StartCoroutine(Move('S'));
                    yield return StartCoroutine(Move('K'));
                    yield return StartCoroutine(Move('G'));
                }
            }
        }
        

    }

    private IEnumerator LBLYellowCross()
    {
        FillColorArray();
        
        if (ColorArray[0, 1, 1] == ColorArray[0, 1, 0] && ColorArray[0, 1, 1] == ColorArray[0, 2, 1])
        {
            yield return StartCoroutine(Move('G'));
        }
        if (ColorArray[0, 1, 1] == ColorArray[0, 2, 1] && ColorArray[0, 1, 1] == ColorArray[0, 1, 2])
        {
            yield return StartCoroutine(Move('H'));
        }
        if (ColorArray[0, 1, 1] == ColorArray[0, 0, 1] && ColorArray[0, 1, 1] == ColorArray[0, 1, 2])
        {
            yield return StartCoroutine(Move('F'));
        }

       
        //FRUR'U'F'
        yield return StartCoroutine(Move('X'));
        yield return StartCoroutine(Move('R'));
        yield return StartCoroutine(Move('F'));
        yield return StartCoroutine(Move('T'));
        yield return StartCoroutine(Move('G'));
        yield return StartCoroutine(Move('Z'));

    }

    private IEnumerator LBLYellowWall()
    {
        FillColorArray();

        if (ColorArray[0, 1, 1] == ColorArray[0, 0, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 2] && ColorArray[0, 1, 1] != ColorArray[0, 0, 2])
        {
            yield return StartCoroutine(Move('G'));
        }
        if (ColorArray[0, 1, 1] != ColorArray[0, 0, 0] && ColorArray[0, 1, 1] == ColorArray[0, 2, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 2] && ColorArray[0, 1, 1] != ColorArray[0, 0, 2])
        {
            yield return StartCoroutine(Move('H'));
        }
        if (ColorArray[0, 1, 1] != ColorArray[0, 0, 0] && ColorArray[0, 1, 1] != ColorArray[0, 2, 0] && ColorArray[0, 1, 1] == ColorArray[0, 2, 2] && ColorArray[0, 1, 1] != ColorArray[0, 0, 2])
        {
            yield return StartCoroutine(Move('F'));
        }


        if (ColorArray[0, 1, 1] == ColorArray[0, 0, 2] && (ColorArray[0, 1, 1] == ColorArray[0, 2, 2] || ColorArray[0, 1, 1] == ColorArray[0, 2, 0] || ColorArray[0, 1, 1] == ColorArray[0, 0, 0]))
        {
            if(ColorArray[0, 1, 1] == ColorArray[0, 0, 0])
            {
                yield return StartCoroutine(Move('G'));
            }
            
        }
        if (ColorArray[0, 1, 1] == ColorArray[0, 0, 2] && (ColorArray[0, 1, 1] == ColorArray[0, 2, 2] || ColorArray[0, 1, 1] == ColorArray[0, 2, 0] || ColorArray[0, 1, 1] == ColorArray[0, 0, 0]))
        {
            if (ColorArray[0, 1, 1] == ColorArray[0, 2, 0])
            {
                yield return StartCoroutine(Move('H'));
            }

        }
        if (ColorArray[0, 1, 1] == ColorArray[0, 0, 2] && (ColorArray[0, 1, 1] == ColorArray[0, 2, 2] || ColorArray[0, 1, 1] == ColorArray[0, 2, 0] || ColorArray[0, 1, 1] == ColorArray[0, 0, 0]))
        {
            if (ColorArray[0, 1, 1] == ColorArray[0, 2, 2])
            {
                yield return StartCoroutine(Move('F'));
            }

        }
        //RUR'URUUR'
        yield return StartCoroutine(Move('R'));
        yield return StartCoroutine(Move('F'));
        yield return StartCoroutine(Move('T'));
        yield return StartCoroutine(Move('F'));
        yield return StartCoroutine(Move('R'));
        yield return StartCoroutine(Move('H'));
        yield return StartCoroutine(Move('T'));

    }

    private IEnumerator LBLYellowEdge()
    {
        FillColorArray();
        if (ColorArray[4, 0, 0] == ColorArray[4, 2, 0])
        {
            ///
        }
        else if (ColorArray[2, 0, 0] == ColorArray[2, 2, 0])
        {
            yield return StartCoroutine(Move('G'));
        }
        else if (ColorArray[3, 0, 0] == ColorArray[3, 2, 0])
        {
            yield return StartCoroutine(Move('F'));
        }
        else if (ColorArray[5, 0, 0] == ColorArray[5, 2, 0])
        {
            yield return StartCoroutine(Move('H'));
        }
        //RB'RFFR'BRFFRR
        yield return StartCoroutine(Move('R'));
        yield return StartCoroutine(Move('B'));
        yield return StartCoroutine(Move('R'));
        yield return StartCoroutine(Move('C'));
        yield return StartCoroutine(Move('T'));
        yield return StartCoroutine(Move('V'));
        yield return StartCoroutine(Move('R'));
        yield return StartCoroutine(Move('C'));
        yield return StartCoroutine(Move('Y'));
    }

    private IEnumerator LBLFinish()
    {
        FillColorArray();

        if (ColorArray[4, 0, 0] == ColorArray[4, 1, 0]&& ColorArray[4, 0, 0] == ColorArray[4, 2, 0])
        {
            yield return StartCoroutine(Move('H'));
        }
        if (ColorArray[3, 0, 0] == ColorArray[3, 1, 1] && (ColorArray[2, 0, 0] == ColorArray[2, 1, 1] || ColorArray[4, 0, 0] == ColorArray[4, 1, 1] || ColorArray[5, 0, 0] == ColorArray[5, 1, 1]))
        {
            yield return StartCoroutine(Move('G'));
        }
        if (ColorArray[2, 0, 0] == ColorArray[2, 1, 1] && (ColorArray[4, 0, 0] == ColorArray[4, 1, 1] || ColorArray[3, 0, 0] == ColorArray[3, 1, 1] || ColorArray[5, 0, 0] == ColorArray[5, 1, 1]))
        {
            yield return StartCoroutine(Move('F'));
        }
        FillColorArray();

        if (ColorArray[2, 1, 0] == ColorArray[3, 2, 0] || ColorArray[3, 1, 0] == ColorArray[4, 0, 0] || ColorArray[4, 1, 0] == ColorArray[3, 0, 0] )
        {
            //FFULR'FFL'RUFF
            yield return StartCoroutine(Move('C'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('W'));
            yield return StartCoroutine(Move('T'));
            yield return StartCoroutine(Move('C'));
            yield return StartCoroutine(Move('Q'));
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('C'));
        }

        if (ColorArray[2, 1, 0] == ColorArray[4, 2, 0] || ColorArray[4, 1, 0] != ColorArray[3, 0, 0] || ColorArray[3, 1, 0] == ColorArray[2, 0, 0])
        {
            //FFU'LR'FFL'RU'FF
            yield return StartCoroutine(Move('C'));
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('W'));
            yield return StartCoroutine(Move('T'));
            yield return StartCoroutine(Move('C'));
            yield return StartCoroutine(Move('Q'));
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('C'));
        }





        FillColorArray();
        if (ColorArray[4, 0, 0] == ColorArray[4, 1, 0]&&ColorArray[4, 1, 0] == ColorArray[4, 2, 0]&& ColorArray[2, 0, 0] == ColorArray[2, 1, 0] && ColorArray[2, 1, 0] == ColorArray[2, 2, 0] && ColorArray[3, 0, 0] == ColorArray[3, 1, 0]&& ColorArray[3, 1, 0] == ColorArray[3, 2, 0] && ColorArray[5, 0, 0] == ColorArray[5, 1, 0] && ColorArray[5, 1, 0] == ColorArray[5, 2, 0] )
        {
                if (ColorArray[4, 0, 0] == ColorArray[3, 1, 1])
                {
                    yield return StartCoroutine(Move('G'));
                }
                else if (ColorArray[4, 0, 0] == ColorArray[5, 1, 1])
                {
                    yield return StartCoroutine(Move('H'));
                }
                else if (ColorArray[4, 0, 0] == ColorArray[2, 1, 1])
                {
                    yield return StartCoroutine(Move('F'));
                }
        }
        


    }

    public IEnumerator LBLYellowCrossColorCheck()
    {
        FillColorArray();
        if ((ColorArray[4, 1, 0] == ColorArray[2, 1, 1]|| ColorArray[4, 1, 0] == ColorArray[3, 1, 1])&& (ColorArray[5, 1, 0] == ColorArray[2, 1, 1] || ColorArray[5, 1, 0] == ColorArray[3, 1, 1]))
        {
            if(ColorArray[4, 1, 0] == ColorArray[2, 1, 1])
            {
                yield return StartCoroutine(Move('S'));
                yield return StartCoroutine(Move('K'));
            }
            else
            {
                yield return StartCoroutine(Move('A'));
                yield return StartCoroutine(Move('L'));
            }
        }
        else if ((ColorArray[4, 1, 0] == ColorArray[4, 1, 1] || ColorArray[4, 1, 0] == ColorArray[5, 1, 1]) && (ColorArray[5, 1, 0] == ColorArray[4, 1, 1] || ColorArray[5, 1, 0] == ColorArray[5, 1, 1]))
        {
            if (ColorArray[4, 1, 0] == ColorArray[5, 1, 1])
            {
                yield return StartCoroutine(Move('H'));
                
            }
            else
            {//RUR'URUUR'
                yield return StartCoroutine(Move('R'));
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('T'));
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('R'));
                yield return StartCoroutine(Move('H'));
                yield return StartCoroutine(Move('T'));
                
            }
        }
        else if ((ColorArray[2, 1, 0] == ColorArray[4, 1, 1] || ColorArray[2, 1, 0] == ColorArray[5, 1, 1]) && (ColorArray[3, 1, 0] == ColorArray[4, 1, 1] || ColorArray[3, 1, 0] == ColorArray[5, 1, 1]))
        {
            if (ColorArray[2, 1, 0] == ColorArray[5, 1, 1])
            {
                yield return StartCoroutine(Move('F'));

            }
            else
            {
                yield return StartCoroutine(Move('G'));
                

            }
        }
        else if ((ColorArray[2, 1, 0] == ColorArray[2, 1, 1] || ColorArray[2, 1, 0] == ColorArray[3, 1, 1]) && (ColorArray[3, 1, 0] == ColorArray[2, 1, 1] || ColorArray[3, 1, 0] == ColorArray[3, 1, 1]))
        {
            if (ColorArray[2, 1, 0] == ColorArray[3, 1, 1])
            {
                yield return StartCoroutine(Move('G'));
                yield return StartCoroutine(Move('A'));
                yield return StartCoroutine(Move('L'));

            }
            else
            {
                yield return StartCoroutine(Move('F'));
                yield return StartCoroutine(Move('S'));
                yield return StartCoroutine(Move('K'));



            }
        }
        else if ((ColorArray[2, 1, 0] == ColorArray[4, 1, 1] && ColorArray[4, 1, 0] == ColorArray[3, 1, 1]) )
        {
            yield return StartCoroutine(Move('H'));
            yield return StartCoroutine(Move('K'));
            yield return StartCoroutine(Move('S'));
        }
        else if ((ColorArray[2, 1, 0] == ColorArray[3, 1, 1] && ColorArray[4, 1, 0] == ColorArray[5, 1, 1]))
        {
            yield return StartCoroutine(Move('H'));
        }
        else if ((ColorArray[2, 1, 0] == ColorArray[5, 1, 1] && ColorArray[4, 1, 0] == ColorArray[2, 1, 1]))
        {
            yield return StartCoroutine(Move('H'));
            yield return StartCoroutine(Move('A'));
            yield return StartCoroutine(Move('L'));
        }
        else if ((ColorArray[2, 1, 0] == ColorArray[2, 1, 1] && ColorArray[4, 1, 0] == ColorArray[4, 1, 1]))
        {
            yield return StartCoroutine(Move('H'));
            yield return StartCoroutine(Move('J'));
            yield return StartCoroutine(Move('D'));
        }


        else if ((ColorArray[4, 1, 0] == ColorArray[4, 1, 1] && ColorArray[3, 1, 0] == ColorArray[3, 1, 1]))
        {
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('K'));
            yield return StartCoroutine(Move('S'));
        }
        else if ((ColorArray[4, 1, 0] == ColorArray[3, 1, 1] && ColorArray[3, 1, 0] == ColorArray[5, 1, 1]))
        {
            yield return StartCoroutine(Move('G'));
        }
        else if ((ColorArray[4, 1, 0] == ColorArray[5, 1, 1] && ColorArray[3, 1, 0] == ColorArray[2, 1, 1]))
        {
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('A'));
            yield return StartCoroutine(Move('L'));
        }
        else if ((ColorArray[4, 1, 0] == ColorArray[2, 1, 1] && ColorArray[3, 1, 0] == ColorArray[4, 1, 1]))
        {
            yield return StartCoroutine(Move('G'));
            yield return StartCoroutine(Move('J'));
            yield return StartCoroutine(Move('D'));
        }




        else if ((ColorArray[3, 1, 0] == ColorArray[4, 1, 1] && ColorArray[5, 1, 0] == ColorArray[3, 1, 1]))
        {
            
            yield return StartCoroutine(Move('K'));
            yield return StartCoroutine(Move('S'));
        }
        else if ((ColorArray[3, 1, 0] == ColorArray[3, 1, 1] && ColorArray[5, 1, 0] == ColorArray[5, 1, 1]))
        {
            //RUR'URUUR'U
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('T'));
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('H'));
            yield return StartCoroutine(Move('T'));
            yield return StartCoroutine(Move('F'));
        }
        else if ((ColorArray[3, 1, 0] == ColorArray[5, 1, 1] && ColorArray[5, 1, 0] == ColorArray[2, 1, 1]))
        {
           
            yield return StartCoroutine(Move('A'));
            yield return StartCoroutine(Move('L'));
        }
        else if ((ColorArray[3, 1, 0] == ColorArray[2, 1, 1] && ColorArray[5, 1, 0] == ColorArray[4, 1, 1]))
        {
           
            yield return StartCoroutine(Move('J'));
            yield return StartCoroutine(Move('D'));
        }



        else if ((ColorArray[5, 1, 0] == ColorArray[4, 1, 1] && ColorArray[2, 1, 0] == ColorArray[3, 1, 1]))
        {
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('K'));
            yield return StartCoroutine(Move('S'));
        }
        else if ((ColorArray[5, 1, 0] == ColorArray[3, 1, 1] && ColorArray[2, 1, 0] == ColorArray[5, 1, 1]))
        {
            yield return StartCoroutine(Move('F'));
        }
        else if ((ColorArray[5, 1, 0] == ColorArray[5, 1, 1] && ColorArray[2, 1, 0] == ColorArray[2, 1, 1]))
        {
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('A'));
            yield return StartCoroutine(Move('L'));
        }
        else if ((ColorArray[5, 1, 0] == ColorArray[2, 1, 1] && ColorArray[2, 1, 0] == ColorArray[4, 1, 1]))
        {
            yield return StartCoroutine(Move('F'));
            yield return StartCoroutine(Move('J'));
            yield return StartCoroutine(Move('D'));
        }
    }
    bool EdgeTest(char X1, char X2, char X3, char testX1,char testX2,char testX3 )
    {       bool test=false;
            if(X1==testX1||X2==testX1||X3==testX1)
            {
                test=true;
            }
            if(X1==testX2||X2==testX2||X3==testX2)
            {
                test=true;
            }
            if(X1==testX3||X2==testX3||X3==testX3)
            {
                test=true;
            }
            return test;
    }
   

    private IEnumerator LBLYellowEdgeMix()//////error
    {
        FillColorArray();
         if (!((ColorArray[4, 2, 0] == ColorArray[4, 1, 1] || ColorArray[4, 2, 0] == ColorArray[0, 1, 1] || ColorArray[4, 2, 0] == ColorArray[3, 1, 1] )&& (ColorArray[0, 2, 2] == ColorArray[4, 1, 1] || ColorArray[0, 2, 2] == ColorArray[0, 1, 1] || ColorArray[0, 2, 2] == ColorArray[3, 1, 1])&& (ColorArray[3, 2, 0] == ColorArray[4, 1, 1] || ColorArray[3, 2, 0] == ColorArray[0, 1, 1] || ColorArray[3, 2, 0] == ColorArray[3, 1, 1])))//ok
        {
        
            if ((ColorArray[3, 0, 0] == ColorArray[3, 1, 1] || ColorArray[3, 0, 0] == ColorArray[0, 1, 1] || ColorArray[3, 0, 0] == ColorArray[5, 1, 1]) && (ColorArray[0, 2, 0] == ColorArray[3, 1, 1] || ColorArray[0, 2, 0] == ColorArray[0, 1, 1] || ColorArray[0, 2, 0] == ColorArray[5, 1, 1]) && (ColorArray[5, 2, 0] == ColorArray[3, 1, 1] || ColorArray[5, 2, 0] == ColorArray[0, 1, 1] || ColorArray[5, 2, 0] == ColorArray[5, 1, 1]))//ok
            {
                yield return StartCoroutine(Move('A'));
                yield return StartCoroutine(Move('L'));
                yield return StartCoroutine(Move('F'));
            }
            else if ((ColorArray[0, 0, 0] == ColorArray[2, 1, 1] || ColorArray[0, 0, 0] == ColorArray[0, 1, 1] || ColorArray[0, 0, 0] == ColorArray[5, 1, 1]) && (ColorArray[2, 0, 0] == ColorArray[2, 1, 1] || ColorArray[2, 0, 0] == ColorArray[0, 1, 1] || ColorArray[2, 0, 0] == ColorArray[5, 1, 1]) && (ColorArray[5, 0, 0] == ColorArray[2, 1, 1] || ColorArray[5, 0, 0] == ColorArray[0, 1, 1] || ColorArray[5, 0, 0] == ColorArray[5, 1, 1]))//ok
            {
                yield return StartCoroutine(Move('D'));
                yield return StartCoroutine(Move('J'));
                yield return StartCoroutine(Move('H'));
            }
            else if ((ColorArray[4, 0, 0] == ColorArray[4, 1, 1] || ColorArray[4, 0, 0] == ColorArray[0, 1, 1] || ColorArray[4, 0, 0] == ColorArray[2, 1, 1]) && (ColorArray[2, 2, 0] == ColorArray[2, 1, 1] || ColorArray[2, 2, 0] == ColorArray[0, 1, 1] || ColorArray[2, 2, 0] == ColorArray[4, 1, 1]) && (ColorArray[0, 0, 2] == ColorArray[2, 1, 1] || ColorArray[0, 0, 2] == ColorArray[0, 1, 1] || ColorArray[0, 0, 2] == ColorArray[4, 1, 1]))//ok
            {
             
                yield return StartCoroutine(Move('S'));
                yield return StartCoroutine(Move('K'));
                yield return StartCoroutine(Move('G'));
            }
        }
        

        //URU'L'UR'U'L
        //axisManager.RotateSpeed(0f);
        yield return StartCoroutine(Move('F'));
        yield return StartCoroutine(Move('R'));
        yield return StartCoroutine(Move('G'));
        yield return StartCoroutine(Move('Q'));
        yield return StartCoroutine(Move('F'));
        yield return StartCoroutine(Move('T'));
        yield return StartCoroutine(Move('G'));
        yield return StartCoroutine(Move('W'));
       

    }
    
    private IEnumerator LBLYellowWallFinish()
    {
       FillColorArray();
        if (!(ColorArray[4, 2, 0] == ColorArray[4, 1, 0] && ColorArray[3, 2, 0] == ColorArray[3, 1, 0] && ColorArray[0, 2, 2] == ColorArray[0, 1, 1] ))
        {
            //R'D'RD

            //RUR'U'
            yield return StartCoroutine(Move('T'));
            yield return StartCoroutine(Move('A'));
            yield return StartCoroutine(Move('R'));
            yield return StartCoroutine(Move('S'));
        }
        else
        {
            yield return StartCoroutine(HandControlActive.RotateAxis(2, 90, 'Y'));
            //yield return StartCoroutine(HandControlActive.RotateAxis(1, 90, 'Y'));
           // yield return StartCoroutine(HandControlActive.RotateAxis(2, 90, 'Y'));
        }
        FillColorArray();
    }

    private IEnumerator PauseLBL()
    {
        if(DebugMenager.actionLBL==2)
        {DebugMenager.actionLBL=1;}
        while(DebugMenager.actionLBL==1)
        {
            yield return null;
        }
        yield return null;
    }

    public IEnumerator LBLStartCross()
    {

        yield return StartCoroutine(LBLStartPosition());
       //axisManager.RotateSpeed(65f);
        do
        {
            while((ColorArray[1, 1, 1] != ColorArray[1, 1, 2]) || (ColorArray[4, 1, 1] != ColorArray[4, 1, 2]))
            {
                
                yield return StartCoroutine(LBLCross());
                //yield return StartCoroutine(PauseLBL());
            }
            
            
            yield return StartCoroutine(HandControlActive.RotateAxis(0, 90, 'Y'));
            yield return StartCoroutine(HandControlActive.RotateAxis(1, 90, 'Y'));
            yield return StartCoroutine(HandControlActive.RotateAxis(2, 90, 'Y'));
                yield return StartCoroutine(PauseLBL());
             FillColorArray();
             
        } while ((ColorArray[1, 1, 1] != ColorArray[1, 1, 2]) || (ColorArray[4, 1, 1] != ColorArray[4, 1, 2])|| (ColorArray[1, 1, 1] != ColorArray[1, 2, 1]) || (ColorArray[3, 1, 1] != ColorArray[3, 1, 2])|| (ColorArray[1, 1, 1] != ColorArray[1, 1, 0]) || (ColorArray[5, 1, 1] != ColorArray[5, 1, 2])|| (ColorArray[1, 1, 1] != ColorArray[1, 0, 1]) || (ColorArray[2, 1, 1] != ColorArray[2, 1, 2]));
        do
        {
            yield return StartCoroutine(LBLWhiteWall());
                 yield return StartCoroutine(PauseLBL());
             FillColorArray();
        } while (ColorArray[1, 1, 1] != ColorArray[1, 0, 2] || ColorArray[1, 1, 1] != ColorArray[1, 2, 2] || ColorArray[1, 1, 1] != ColorArray[1, 2, 0] || ColorArray[1, 1, 1] != ColorArray[1, 0, 0] || ColorArray[4, 1, 1] != ColorArray[4, 0, 2] || ColorArray[4, 1, 1] != ColorArray[4, 2, 2] || ColorArray[3, 1, 1] != ColorArray[3, 2, 2] || ColorArray[3, 1, 1] != ColorArray[3, 0, 2] || ColorArray[5, 1, 1] != ColorArray[5, 2, 2] || ColorArray[5, 1, 1] != ColorArray[5, 0, 2] || ColorArray[2, 1, 1] != ColorArray[2, 0, 2] || ColorArray[2, 1, 1] != ColorArray[2, 2, 2]);
        do
        {
           
            yield return StartCoroutine(LBLSecondLayer());
                yield return StartCoroutine(PauseLBL());
             FillColorArray();
        } while (ColorArray[4, 1, 1] != ColorArray[4, 2, 1] || ColorArray[4, 1, 1] != ColorArray[4, 0, 1] || ColorArray[3, 1, 1] != ColorArray[3, 2, 1] || ColorArray[3, 1, 1] != ColorArray[3, 0, 1] || ColorArray[5, 1, 1] != ColorArray[5, 2, 1] || ColorArray[5, 1, 1] != ColorArray[5, 0, 1] || ColorArray[2, 1, 1] != ColorArray[2, 2, 1] || ColorArray[2, 1, 1] != ColorArray[2, 0, 1]);
        //axisManager.RotateSpeed(60f);
        
        do
        {
            yield return StartCoroutine(LBLYellowCross());
                yield return StartCoroutine(PauseLBL());
             FillColorArray();
        } while (ColorArray[0, 1, 1] != ColorArray[0, 2, 1] || ColorArray[0, 1, 1] != ColorArray[0, 1, 0] || ColorArray[0, 1, 1] != ColorArray[0, 0, 1] || ColorArray[0, 1, 1] != ColorArray[0, 1, 2]);

        do
        {
            yield return StartCoroutine(LBLYellowCrossColorCheck());
                 yield return StartCoroutine(PauseLBL());
             FillColorArray();
        } while (ColorArray[4, 1, 1] != ColorArray[4, 1, 0] || ColorArray[3, 1, 1] != ColorArray[3, 1, 0] || ColorArray[5, 1, 1] != ColorArray[5, 1, 0] || ColorArray[2, 1, 1] != ColorArray[2, 1, 0]);
        // axisManager.RotateSpeed(0f);
        do
        {
             yield return StartCoroutine(LBLYellowEdgeMix());
                  yield return StartCoroutine(PauseLBL());
              FillColorArray();
        }while(!((ColorArray[4,2,0]==ColorArray[4,1,1]||ColorArray[4,2,0]==ColorArray[3,1,1]||ColorArray[4,2,0]==ColorArray[0,1,1])&&(ColorArray[3,2,0]==ColorArray[4,1,1]||ColorArray[3,2,0]==ColorArray[3,1,1]||ColorArray[3,2,0]==ColorArray[0,1,1])&&(ColorArray[0,2,2]==ColorArray[4,1,1]||ColorArray[0,2,2]==ColorArray[3,1,1]||ColorArray[0,2,2]==ColorArray[0,1,1])&&
        (ColorArray[4,0,0]==ColorArray[4,1,1]||ColorArray[4,0,0]==ColorArray[2,1,1]||ColorArray[4,0,0]==ColorArray[0,1,1])&&(ColorArray[2,2,0]==ColorArray[4,1,1]||ColorArray[2,2,0]==ColorArray[2,1,1]||ColorArray[2,2,0]==ColorArray[0,1,1])&&(ColorArray[0,0,2]==ColorArray[4,1,1]||ColorArray[0,0,2]==ColorArray[2,1,1]||ColorArray[0,0,2]==ColorArray[0,1,1])&&
        (ColorArray[5,0,0]==ColorArray[5,1,1]||ColorArray[5,0,0]==ColorArray[2,1,1]||ColorArray[5,0,0]==ColorArray[0,1,1])&&(ColorArray[2,0,0]==ColorArray[5,1,1]||ColorArray[2,0,0]==ColorArray[2,1,1]||ColorArray[2,0,0]==ColorArray[0,1,1])&&(ColorArray[0,0,0]==ColorArray[5,1,1]||ColorArray[0,0,0]==ColorArray[2,1,1]||ColorArray[0,0,0]==ColorArray[0,1,1])&&
        (ColorArray[5,2,0]==ColorArray[5,1,1]||ColorArray[5,2,0]==ColorArray[3,1,1]||ColorArray[5,2,0]==ColorArray[0,1,1])&&(ColorArray[3,0,0]==ColorArray[5,1,1]||ColorArray[3,0,0]==ColorArray[3,1,1]||ColorArray[3,0,0]==ColorArray[0,1,1])&&(ColorArray[0,2,0]==ColorArray[5,1,1]||ColorArray[0,2,0]==ColorArray[3,1,1]||ColorArray[0,2,0]==ColorArray[0,1,1])));
        //axisManager.RotateSpeed(30f);
        do
        {
             yield return StartCoroutine(LBLYellowWallFinish());
                 yield return StartCoroutine(PauseLBL());
              FillColorArray();
        }while(!(ColorArray[4,2,0]==ColorArray[4,1,1]&&ColorArray[3,2,0]==ColorArray[3,1,1]&&ColorArray[0,2,2]==ColorArray[0,1,1]&&
        ColorArray[4,0,0]==ColorArray[4,1,1]&&ColorArray[2,2,0]==ColorArray[2,1,1]&&ColorArray[0,0,2]==ColorArray[0,1,1]&&
        ColorArray[5,0,0]==ColorArray[5,1,1]&&ColorArray[2,0,0]==ColorArray[2,1,1]&&ColorArray[0,0,0]==ColorArray[0,1,1]&&
        ColorArray[5,2,0]==ColorArray[5,1,1]&&ColorArray[3,0,0]==ColorArray[3,1,1]&&ColorArray[0,2,0]==ColorArray[0,1,1]));
           
    

        /*do
        {
            yield return StartCoroutine(LBLYellowWall());
        } while (ColorArray[0, 1, 1] != ColorArray[0, 0, 0] || ColorArray[0, 1, 1] != ColorArray[0, 0, 2] || ColorArray[0, 1, 1] != ColorArray[0, 2, 0] || ColorArray[0, 1, 1] != ColorArray[0, 2, 2]  );
      2
        do
        {
            yield return StartCoroutine(LBLYellowEdge());
        } while (ColorArray[4, 0, 0] != ColorArray[4, 1, 1] || ColorArray[4, 2, 0] != ColorArray[4, 1, 1] || ColorArray[2, 0, 0] != ColorArray[2, 1, 1] || ColorArray[2, 2, 0] != ColorArray[2, 1, 1] || ColorArray[3, 0, 0] != ColorArray[3, 1, 1] || ColorArray[3, 2, 0] != ColorArray[3, 1, 1] || ColorArray[5, 0, 0] != ColorArray[5, 1, 1] || ColorArray[5, 2, 0] != ColorArray[5, 1, 1] );
        Debug.LogError("finished");
        do
        {
            yield return StartCoroutine(LBLFinish());
        } while (!WallTest());


        */






    }
   

    public IEnumerator LBLSBS()
    {
        
        yield return StartCoroutine(LBLStartPosition());
        switch(status)
        {
            case 0:
                statueText.text = "Układam biały krzyż.";
                break;
            case 1:
                statueText.text = "Układam białą podstawę.";
                break;
            case 2:
                statueText.text = "Układam drugą warstwę.";
                break;
            case 3:
                statueText.text = "Układam żółty krzyż.";
                break;
            case 4:
                statueText.text = "Układam biały żółte narożniki.";
                break;
            case 5:
                statueText.text = "Koryguję oriętację żółtych narożników.";
                break;

        }
        //axisManager.RotateSpeed(65f);
        do
        {
            while ((ColorArray[1, 1, 1] != ColorArray[1, 1, 2]) || (ColorArray[4, 1, 1] != ColorArray[4, 1, 2]))
            {

                yield return StartCoroutine(LBLCross());
                //yield return StartCoroutine(PauseLBL());
            }


            yield return StartCoroutine(HandControlActive.RotateAxis(0, 90, 'Y'));
            yield return StartCoroutine(HandControlActive.RotateAxis(1, 90, 'Y'));
            yield return StartCoroutine(HandControlActive.RotateAxis(2, 90, 'Y'));
            yield return StartCoroutine(PauseLBL());
            FillColorArray();

        } while ((ColorArray[1, 1, 1] != ColorArray[1, 1, 2]) || (ColorArray[4, 1, 1] != ColorArray[4, 1, 2]) || (ColorArray[1, 1, 1] != ColorArray[1, 2, 1]) || (ColorArray[3, 1, 1] != ColorArray[3, 1, 2]) || (ColorArray[1, 1, 1] != ColorArray[1, 1, 0]) || (ColorArray[5, 1, 1] != ColorArray[5, 1, 2]) || (ColorArray[1, 1, 1] != ColorArray[1, 0, 1]) || (ColorArray[2, 1, 1] != ColorArray[2, 1, 2]));
        if(status>0)
        {
            do
            {
                yield return StartCoroutine(LBLWhiteWall());
                yield return StartCoroutine(PauseLBL());
                FillColorArray();
            } while (ColorArray[1, 1, 1] != ColorArray[1, 0, 2] || ColorArray[1, 1, 1] != ColorArray[1, 2, 2] || ColorArray[1, 1, 1] != ColorArray[1, 2, 0] || ColorArray[1, 1, 1] != ColorArray[1, 0, 0] || ColorArray[4, 1, 1] != ColorArray[4, 0, 2] || ColorArray[4, 1, 1] != ColorArray[4, 2, 2] || ColorArray[3, 1, 1] != ColorArray[3, 2, 2] || ColorArray[3, 1, 1] != ColorArray[3, 0, 2] || ColorArray[5, 1, 1] != ColorArray[5, 2, 2] || ColorArray[5, 1, 1] != ColorArray[5, 0, 2] || ColorArray[2, 1, 1] != ColorArray[2, 0, 2] || ColorArray[2, 1, 1] != ColorArray[2, 2, 2]);
            if (status > 1)
            {
                do
                {

                    yield return StartCoroutine(LBLSecondLayer());
                    yield return StartCoroutine(PauseLBL());
                    FillColorArray();
                } while (ColorArray[4, 1, 1] != ColorArray[4, 2, 1] || ColorArray[4, 1, 1] != ColorArray[4, 0, 1] || ColorArray[3, 1, 1] != ColorArray[3, 2, 1] || ColorArray[3, 1, 1] != ColorArray[3, 0, 1] || ColorArray[5, 1, 1] != ColorArray[5, 2, 1] || ColorArray[5, 1, 1] != ColorArray[5, 0, 1] || ColorArray[2, 1, 1] != ColorArray[2, 2, 1] || ColorArray[2, 1, 1] != ColorArray[2, 0, 1]);
                //axisManager.RotateSpeed(60f);
                if (status > 2)
                {

                    do
                    {
                        yield return StartCoroutine(LBLYellowCross());
                        yield return StartCoroutine(PauseLBL());
                        FillColorArray();
                    } while (ColorArray[0, 1, 1] != ColorArray[0, 2, 1] || ColorArray[0, 1, 1] != ColorArray[0, 1, 0] || ColorArray[0, 1, 1] != ColorArray[0, 0, 1] || ColorArray[0, 1, 1] != ColorArray[0, 1, 2]);
                    if (status > 3)
                    {
                        do
                        {
                            yield return StartCoroutine(LBLYellowCrossColorCheck());
                            yield return StartCoroutine(PauseLBL());
                            FillColorArray();
                        } while (ColorArray[4, 1, 1] != ColorArray[4, 1, 0] || ColorArray[3, 1, 1] != ColorArray[3, 1, 0] || ColorArray[5, 1, 1] != ColorArray[5, 1, 0] || ColorArray[2, 1, 1] != ColorArray[2, 1, 0]);
                        // axisManager.RotateSpeed(0f);
                        if (status > 4)
                        {
                            do
                            {
                                yield return StartCoroutine(LBLYellowEdgeMix());
                                yield return StartCoroutine(PauseLBL());
                                FillColorArray();
                            } while (!((ColorArray[4, 2, 0] == ColorArray[4, 1, 1] || ColorArray[4, 2, 0] == ColorArray[3, 1, 1] || ColorArray[4, 2, 0] == ColorArray[0, 1, 1]) && (ColorArray[3, 2, 0] == ColorArray[4, 1, 1] || ColorArray[3, 2, 0] == ColorArray[3, 1, 1] || ColorArray[3, 2, 0] == ColorArray[0, 1, 1]) && (ColorArray[0, 2, 2] == ColorArray[4, 1, 1] || ColorArray[0, 2, 2] == ColorArray[3, 1, 1] || ColorArray[0, 2, 2] == ColorArray[0, 1, 1]) &&
                             (ColorArray[4, 0, 0] == ColorArray[4, 1, 1] || ColorArray[4, 0, 0] == ColorArray[2, 1, 1] || ColorArray[4, 0, 0] == ColorArray[0, 1, 1]) && (ColorArray[2, 2, 0] == ColorArray[4, 1, 1] || ColorArray[2, 2, 0] == ColorArray[2, 1, 1] || ColorArray[2, 2, 0] == ColorArray[0, 1, 1]) && (ColorArray[0, 0, 2] == ColorArray[4, 1, 1] || ColorArray[0, 0, 2] == ColorArray[2, 1, 1] || ColorArray[0, 0, 2] == ColorArray[0, 1, 1]) &&
                             (ColorArray[5, 0, 0] == ColorArray[5, 1, 1] || ColorArray[5, 0, 0] == ColorArray[2, 1, 1] || ColorArray[5, 0, 0] == ColorArray[0, 1, 1]) && (ColorArray[2, 0, 0] == ColorArray[5, 1, 1] || ColorArray[2, 0, 0] == ColorArray[2, 1, 1] || ColorArray[2, 0, 0] == ColorArray[0, 1, 1]) && (ColorArray[0, 0, 0] == ColorArray[5, 1, 1] || ColorArray[0, 0, 0] == ColorArray[2, 1, 1] || ColorArray[0, 0, 0] == ColorArray[0, 1, 1]) &&
                             (ColorArray[5, 2, 0] == ColorArray[5, 1, 1] || ColorArray[5, 2, 0] == ColorArray[3, 1, 1] || ColorArray[5, 2, 0] == ColorArray[0, 1, 1]) && (ColorArray[3, 0, 0] == ColorArray[5, 1, 1] || ColorArray[3, 0, 0] == ColorArray[3, 1, 1] || ColorArray[3, 0, 0] == ColorArray[0, 1, 1]) && (ColorArray[0, 2, 0] == ColorArray[5, 1, 1] || ColorArray[0, 2, 0] == ColorArray[3, 1, 1] || ColorArray[0, 2, 0] == ColorArray[0, 1, 1])));
                            //axisManager.RotateSpeed(30f);

                            if (status > 5)
                            {
                                status = 0;
                                yield return StartCoroutine(LBLStartCross());

                            }
                            
                           
                        }
                        else
                        {
                            status = 5;
                        }


                    }
                    else
                    {
                        status = 4;
                    }


                }
                else
                {
                    status = 3;
                }


            }
            else
            {
                status = 2;
            }

        }
        else
        {
            status = 1;
        }


    }


}