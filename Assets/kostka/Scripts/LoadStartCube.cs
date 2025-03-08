using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStartCube : MonoBehaviour
{
    // Tablice przechowujace obiekty dla osi X, Y i Z
    private GameObject[] axisX = new GameObject[3];
    private GameObject[] axisY = new GameObject[3];
    private GameObject[] axisZ = new GameObject[3];
    public GameObject AxisX;
    public GameObject AxisY;
    public GameObject AxisZ;
    
    void Start()
    {
        InitializeAxis("X", axisX); // Inicjalizacja dla osi X
        InitializeAxis("Y", axisY); // Inicjalizacja dla osi Y
        InitializeAxis("Z", axisZ); // Inicjalizacja dla osi Z
    }

    // Inicjalizuje tablice obiektow dla danej osi
    private void InitializeAxis(string name, GameObject[] axisArray)
    {
        int cubeNumber = 1; // Numer dla osi np. X1, X2, X3, Y1, Y2, Z1, Z2, Z3...
        for (int i = 0; i < 3; i++)
        {
            string cubeName = name + cubeNumber; // Tworzenie nazwy cuba
            GameObject cube = FindChildByName(cubeName); // Szukanie dziecka po nazwie

            if (cube != null)
            {
                axisArray[i] = cube; // Przypisywanie cuba do tablicy
            }
            else
            {
                Debug.LogError("Cube not found: " + cubeName); // Logowanie bledu, jesli cub nie istnieje
            }
            cubeNumber++; // Inkrementacja numeru cuba
        }
    }

    // Znajduje dziecko obiektu o podanej nazwie
    public GameObject FindChildByName(string name)
    {
        // Sprawdzenie wszystkich dzieci danego GameObject
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.gameObject; // Zwraca dziecko, jesli nazwa sie zgadza
            }
        }
        return null; // Zwraca null, jesli nie znajdzie odpowiedniego dziecka
    }

    // Znajduje obiekt osi po nazwie i indeksie
    public GameObject FindByName(char axis, short index)
    {  
        if (axis == 'X') return axisX[index]; // Zwraca obiekt osi X o danym indeksie
        if (axis == 'Y') return axisY[index]; // Zwraca obiekt osi Y o danym indeksie
        if (axis == 'Z') return axisZ[index]; // Zwraca obiekt osi Z o danym indeksie
        return null; // Zwraca null, jesli nie znajdzie osi o danej nazwie
    }
}