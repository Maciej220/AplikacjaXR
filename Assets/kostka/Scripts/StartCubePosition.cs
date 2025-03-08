using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartCubePosition : MonoBehaviour
{
   
    public LoadStartCube LoadStartCube; // Dostep do listy osi 
    public AxisMenager AxisMenager; // Klasa do obslugi osi

    // Tablice do przechowywania poczatkowych danych Transform dla scianek kostki
    private GameObject[] ColorBoxStartGameObject = new GameObject[27];
    private Vector3[] ColorBoxStartPosition = new Vector3[27];
    private Vector3[] ColorBoxStartRotate = new Vector3[27];
    private Vector3[] ColorBoxStartScale = new Vector3[27];

    // Tablice do przechowywania poczatkowych danych Transform dla osi
    public GameObject[] AxisStartGameObject = new GameObject[9];
    private Vector3[] AxisStartPosition = new Vector3[9];
    private Vector3[] AxisStartRotate = new Vector3[9];
    private Vector3[] AxisStartScale = new Vector3[9];

    // Lista do przechowywania wszystkich GameObjectow
    private List<GameObject> ListGameObject = new List<GameObject>();

    public GameObject Cube; // Glowny obiekt kostki

    // Wczytanie pozycji startowej wszystkich danych Transform dla scianek kostki
    public void InitializeColorBoxStartTransform(string name)
    {
        int cubeNumber = 1; // Numeracja scianek
        for (int i = 0; i < 27; i++)
        {
            string cubeName = name + cubeNumber; // Tworzenie nazwy cuba
            GameObject cube = LoadStartCube.FindChildByName(cubeName); // Szukanie dziecka po nazwie

            if (cube != null)
            {
                ColorBoxStartGameObject[i] = cube; // Przypisywanie cuba do tablicy
                ColorBoxStartScale[i] = cube.transform.localScale;
                ColorBoxStartPosition[i] = cube.transform.localPosition;
                ColorBoxStartRotate[i] = cube.transform.localEulerAngles;
                ListGameObject.Add(cube); // Dodawanie do listy wszystkich obiektow
            }
            else
            {
                Debug.LogError("Cube not found: " + cubeName); // Logowanie bledu, jesli cub nie istnieje
            }
            cubeNumber++; // Inkrementacja numeru cuba
        }
    }

    // Wczytanie pozycji startowej wszystkich danych Transform dla osi
    public void InitializeAxisStartTransform(string name, int start, int stop)
    {
        int cubeNumber = 1; // Numeracja osi
        for (int i = start; i < stop; i++)
        {
            string cubeName = name + cubeNumber; // Tworzenie nazwy cuba
            GameObject cube = LoadStartCube.FindChildByName(cubeName); // Szukanie dziecka po nazwie

            if (cube != null)
            {
                AxisStartGameObject[i] = cube; // Przypisywanie cuba do tablicy
                AxisStartScale[i] = cube.transform.localScale;
                AxisStartPosition[i] = cube.transform.localPosition;
                AxisStartRotate[i] = cube.transform.localEulerAngles;
                ListGameObject.Add(cube); // Dodawanie do listy wszystkich obiektow
            }
            else
            {
                Debug.LogError("Cube not found: " + cubeName); // Logowanie bledu, jesli cub nie istnieje
            }
            cubeNumber++; // Inkrementacja numeru cuba
        }
    }

    // Resetowanie pozycji wszystkich obiektow do poczatkowych wartosci
    public void RestartionPozition()
    {
        AxisMenager.AssignObjectsToParent(Cube, ListGameObject); // Przypisanie wszystkich obiektow do glownego obiektu kostki
        
        // Resetowanie pozycji, skali i rotacji dla scianek kostki
        for (int i = 0; i < 27; i++)
        {
            ColorBoxStartGameObject[i].transform.localScale = ColorBoxStartScale[i];
            ColorBoxStartGameObject[i].transform.localPosition = ColorBoxStartPosition[i];
            ColorBoxStartGameObject[i].transform.localEulerAngles = ColorBoxStartRotate[i];
        }

        // Resetowanie pozycji, skali i rotacji dla osi
        for (int i = 0; i < 9; i++)
        {
            AxisStartGameObject[i].transform.localScale = AxisStartScale[i];
            AxisStartGameObject[i].transform.localPosition = AxisStartPosition[i];
            AxisStartGameObject[i].transform.localEulerAngles = AxisStartRotate[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeColorBoxStartTransform("Cube"); // Inicjalizacja scianek kostki
        InitializeAxisStartTransform("X", 0, 3); // Inicjalizacja osi X
        InitializeAxisStartTransform("Y", 3, 6); // Inicjalizacja osi Y
        InitializeAxisStartTransform("Z", 6, 9); // Inicjalizacja osi Z
    }
}