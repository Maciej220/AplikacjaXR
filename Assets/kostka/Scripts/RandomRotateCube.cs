using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;

public class RandomRotateCube : MonoBehaviour
{
    public HandControlActive HandControlActive; // Referencja do klasy HandControlActive
    public StartCubePosition StartCubePosition; // Referencja do klasy StartCubePosition
    public AxisMenager AxisMenager; // Referencja do klasy StartCubePosition

    private static bool isRotating = false; // Static to ensure it is shared across all instances

    public void RandomRotate()
    {
        if (isRotating)
        {
            return; // Exit the function if it's already running
        }

        // Wylacza komponent HandGrabInteractable dla kazdego obiektu w tablicy AxisStartGameObject
        for (int i = 0; i < 9; i++)
        {
            StartCubePosition.AxisStartGameObject[i].GetComponent<HandGrabInteractable>().enabled = false;
        }

        StartCoroutine(RandomRotateCoroutine()); // Rozpoczyna korutyne do losowego obracania kostki

        // Wlacza komponent HandGrabInteractable dla kazdego obiektu w tablicy AxisStartGameObject
        for (int i = 0; i < 9; i++)
        {
            StartCubePosition.AxisStartGameObject[i].GetComponent<HandGrabInteractable>().enabled = true;
        }
    }

    char backAxis;

    private IEnumerator RandomRotateCoroutine()
    {
        isRotating = true; // Ustawia flage, aby wskazac, ze funkcja jest uruchomiona

        // StartCubePosition.RestartionPozition(); // Resetuje pozycje startowa kostki
        backAxis = 'q';
        // yield return new WaitForSeconds(0.1f); // Czeka 0.1 sekundy

        int numberOfMoves = Random.Range(10, 20); // Losuje liczbe ruchow miedzy 10 a 20
        // int numberOfMoves = 1; // Losuje liczbe ruchow miedzy 10 a 20
        for (int i = 0; i < numberOfMoves; i++)
        {
            char axis;
            short index;
            int valueRotate;

            // Losowanie osi obrotu
            do
            {
                switch (Random.Range(1, 4)) // Random.Range(1, 4) zwraca 1, 2 lub 3
                {
                    case 1:
                        axis = 'X';
                        break;
                    case 2:
                        axis = 'Y';
                        break;
                    case 3:
                        axis = 'Z';
                        break;
                    default:
                        axis = 'Z';
                        break;
                }
            } while (axis == backAxis);
            backAxis = axis;
            index = (short)Random.Range(0, 3); // Losuje indeks miedzy 0 a 2

            // Losowanie wartosci obrotu
            switch (Random.Range(2, 5))
            {
                case 1:
                    valueRotate = 0;
                    break;
                case 2:
                    valueRotate = 90;
                    break;
                case 3:
                    valueRotate = 180;
                    break;
                case 4:
                    valueRotate = 270;
                    break;
                default:
                    valueRotate = 90;
                    break;
            }

            // Rozpoczyna korutyne do obrotu osi
            yield return StartCoroutine(HandControlActive.RotateAxis(index, valueRotate, axis));
        }

        isRotating = false; // Resetuje flage po zakonczeniu
    }
}