using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlokadeCubeRotate : MonoBehaviour
{
    private bool isRotating = false; // Flaga wskazujaca, czy obrot jest w trakcie

    // Sprawdza, czy obrot jest w trakcie
    public bool testRotate()
    {
        return isRotating; // Zwraca wartosc flagi isRotating
    }

    // Ustawia flage isRotating na true, aby zablokowac inne obroty
    public void block()
    {
        isRotating = true; // Ustawienie flagi na true
    }

    // Ustawia flage isRotating na false, aby odblokowac inne obroty
    public void unlock()
    {
        isRotating = false; // Ustawienie flagi na false
    }
}