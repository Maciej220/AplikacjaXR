using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Dodajemy bibliotekę TextMesh Pro

public class DebugMenager : MonoBehaviour
{
    public SmartBrick SmartBrick;
    public AxisMenager AxisMenager;
    public RandomRotateCube RandomRotateCube;
    public HandControlActive HandControlActive;
    public HandControlDezactiv HandControlDezactiv;
    public StartCubePosition StartCubePosition;
    public BlokadeCubeRotate BlokadeCubeRotate;
    public LoadStartCube LoadStartCube;

    public float speed = 65f;
    public float operate = 1f;

    public int actionLBL=0;

    public TextMeshProUGUI speedText; // Wyświetlanie wartości prędkości w interfejsie
    public TextMeshProUGUI operateText; // Wyświetlanie wartości operacji w interfejsie

    public void ReloadSene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void RandomRotate()
    {
        RandomRotateCube.RandomRotate();
    }

    public void LBLStartCross()
    {
        
        if(actionLBL==0)
        {
            actionLBL=0;
           StartCoroutine(SmartBrick.LBLStartCross()); 
        }
        else
        {
             actionLBL=0;
        }
        
    }

    public void StepLBL()
    {
        if(actionLBL==0)
        {
            actionLBL=2;
            StartCoroutine(SmartBrick.LBLStartCross());
        }
        else
        {
            actionLBL=2;
        }
    }

    public void speedUp()
    {
        if(speed+operate>1000f)
        {
            speed=1000f;
        }
        else{
           speed += operate; 
        }
        
        AxisMenager.RotateSpeed(speed);
    }

    public void speedDown()
    {
        if(speed-operate<1f)
        {
            speed=1f;
        }
        else{
           speed -= operate; 
        }
        speed -= operate;
        AxisMenager.RotateSpeed(speed);
    }

    public void operateUp()
    {
        switch (operate)
        {
            case 1f:
                operate = 5f;
                break;
            case 5f:
                operate = 10f;
                break;
            case 10f:
                operate = 50f;
                break;

        }
    }

    public void operateDown()
    {
        switch (operate)
        {
            case 5f:
                operate = 1f;
                break;
            case 10f:
                operate = 5f;
                break;
            case 50f:
                operate = 10f;
                break;
        }
    }

    // Update jest wywoływany raz na każdą klatkę
    void Update()
    {
        // Konwersja wartości float na string i przypisanie jej do pola tekstowego TextMeshProUGUI
        operateText.text = operate.ToString();
        speedText.text = speed.ToString();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RandomRotateCube.RandomRotate();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(SmartBrick.LBLStartCross());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(HandControlActive.RotateAxis(0, -90, 'X'));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(HandControlActive.RotateAxis(3, 90, 'X'));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(HandControlActive.RotateAxis(3, -90, 'X'));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(HandControlActive.RotateAxis(3, 90, 'Y'));
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(HandControlActive.RotateAxis(3, -90, 'Y'));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(HandControlActive.RotateAxis(3, 90, 'Z'));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(HandControlActive.RotateAxis(3, -90, 'Z'));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            LBLStartCross();
        }
         if (Input.GetKeyDown(KeyCode.X))
        {
            StepLBL();
        }
    }
}