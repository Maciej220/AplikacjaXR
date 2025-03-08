using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Unity.VisualScripting;
using UnityEngine.UIElements;








public class Rotate : MonoBehaviour
{
    
    private GameObject[] axisX = new GameObject[3];
    private GameObject[] axisY = new GameObject[3];
    private GameObject[] axisZ = new GameObject[3];

    Quaternion localRotationStart;
    Quaternion localRotationStop;

    public GameObject cube;
  
    private bool isRotating = false;

    
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

    private void StartHaptics(Handedness handedness)
    {
        if (handedness == Handedness.Left && OVRInput.IsControllerConnected(leftController))
        {
            OVRInput.SetControllerVibration(0.9f, 0.5f, leftController);
        }
        if (handedness == Handedness.Right && OVRInput.IsControllerConnected(rightController))
        {
            OVRInput.SetControllerVibration(0.9f, 0.5f, rightController);
        }
    }

    public void HandlePointerEvent(PointerEvent pointerEvent)
    {
        HandRef handData = (HandRef)pointerEvent.Data;
        Handedness handedness = handData.Handedness;
        StartHaptics(handedness);
    }













    // Start is called before the first frame update
    void Start()
    {
        InitializeAxis("X", axisX); // Inicjalizacja dla osi X
        InitializeAxis("Y", axisY); // Inicjalizacja dla osi Y
        InitializeAxis("Z", axisZ); // Inicjalizacja dla osi Z
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating) return;

        CheckForKey(KeyCode.Q, 0, 90, 'X');
        CheckForKey(KeyCode.W, 1, 90, 'X');
        CheckForKey(KeyCode.E, 2, 90, 'X');
        CheckForKey(KeyCode.A, 0, 90, 'Y');
        CheckForKey(KeyCode.S, 1, 90, 'Y');
        CheckForKey(KeyCode.D, 2, 90, 'Y');
        CheckForKey(KeyCode.Z, 0, 90, 'Z');
        CheckForKey(KeyCode.X, 1, 90, 'Z');
        CheckForKey(KeyCode.C, 2, 90, 'Z');
    }

    private void CheckForKey(KeyCode key, int rotate, int value, char axis)
    {
        if (Input.GetKeyDown(key))
        {
            StartCoroutine(RotateAxis(rotate, value, axis));
        }
    }

    public void Rotate90X1() => StartCoroutine(RotateAxis(0, 90, 'X'));
    public void Rotate90X2() => StartCoroutine(RotateAxis(1, 90, 'X'));
    public void Rotate90X3() => StartCoroutine(RotateAxis(2, 90, 'X'));
    public void Rotate270X1() => StartCoroutine(RotateAxis(0, 270, 'X'));
    public void Rotate270X2() => StartCoroutine(RotateAxis(1, 270, 'X'));
    public void Rotate270X3() => StartCoroutine(RotateAxis(2, 270, 'X'));
    public void Rotate90Y1() => StartCoroutine(RotateAxis(0, 90, 'Y'));
    public void Rotate90Y2() => StartCoroutine(RotateAxis(1, 90, 'Y'));
    public void Rotate90Y3() => StartCoroutine(RotateAxis(2, 90, 'Y'));
    public void Rotate270Y1() => StartCoroutine(RotateAxis(0, 270, 'Y'));
    public void Rotate270Y2() => StartCoroutine(RotateAxis(1, 270, 'Y'));
    public void Rotate270Y3() => StartCoroutine(RotateAxis(2, 270, 'Y'));

    public void RotateX1Active() => StartCoroutine(RotateAxis(0, 0, 'X'));
    public void RotateX2Active() => StartCoroutine(RotateAxis(1, 0, 'X'));
    public void RotateX3Active() => StartCoroutine(RotateAxis(2, 0, 'X'));

    public void RotateY1Active() => StartCoroutine(RotateAxis(0, 0, 'Y'));
    public void RotateY2Active() => StartCoroutine(RotateAxis(1, 0, 'Y'));
    public void RotateY3Active() => StartCoroutine(RotateAxis(2, 0, 'Y'));


    public void RotateZ1Active() => StartCoroutine(RotateAxis(0, 0, 'Z'));
    public void RotateZ2Active() => StartCoroutine(RotateAxis(1, 0, 'Z'));
    public void RotateZ3Active() => StartCoroutine(RotateAxis(2, 0, 'Z'));

    public void RotateX1Dezactive()
    { Dezkative(0, 'X'); }
    public void RotateX2Dezactive()
    { Dezkative(1, 'X'); }
    public void RotateX3Dezactive()
    { Dezkative(2, 'X'); }
    public void RotateY1Dezactive()
    { Dezkative(0, 'Y'); }
    public void RotateY2Dezactive()
    { Dezkative(1, 'Y'); }
    public void RotateY3Dezactive()
    { Dezkative(2, 'Y'); }
    public void RotateZ1Dezactive()
    { Dezkative(0, 'Z'); }
    public void RotateZ2Dezactive()
    { Dezkative(1, 'Z'); }
    public void RotateZ3Dezactive()
    { Dezkative(2, 'Z'); }










    // Funkcja do wyszukiwania obiektów w zakresie
    public List<GameObject> FindObjectsInRange(GameObject parentObject, string tag)
    {
        List<GameObject> objectsInRange = new List<GameObject>();

        if (parentObject == null)
        {
            Debug.LogError("Parent object is null.");
            return objectsInRange;
        }

        // Pobranie wymiarów obiektu przeszukiwanego
        Renderer parentRenderer = parentObject.GetComponent<Renderer>();
        if (parentRenderer == null)
        {
            Debug.LogError("Parent object does not have a Renderer component.");
            return objectsInRange;
        }

        Vector3 parentSize = parentRenderer.bounds.size;
        Vector3 searchPosition = parentObject.transform.position;

        // Dodanie niewielkiej wartości odsunięcia
        float offset = 0.01f;
        Vector3 adjustedSize = parentSize / 2 - new Vector3(offset, offset, offset);

        // Wyszukiwanie obiektów w zadanym zakresie przy użyciu box overlap
        Collider[] hitColliders = Physics.OverlapBox(searchPosition, adjustedSize, parentObject.transform.rotation);

        // Iteracja przez znalezione obiekty i sprawdzanie ich tagu
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(tag))
            {
                objectsInRange.Add(hitCollider.gameObject);
            }
        }

        return objectsInRange;
    }

    public void AssignObjectsToParent(GameObject parentObject, List<GameObject> objectsToAssign)
    {
        foreach (GameObject obj in objectsToAssign)
        {
            obj.transform.SetParent(parentObject.transform);
        }
    }

    public void LogObjectSize(GameObject obj)
    {
        if (obj != null)
        {
            Vector3 size = obj.GetComponent<Renderer>().bounds.size;
            Debug.Log("Size of the object " + obj.name + ": " + size);
        }
        else
        {
            Debug.LogError("Provided object is null.");
        }
    }

    public void RotateObject(GameObject obj, float angle, char axis)
    {
        if (obj == null)
        {
            Debug.LogError("Provided object is null.");
            return;
        }

        Vector3 rotationAxis;

        switch (axis)
        {
            case 'X':
                rotationAxis = Vector3.right;
                break;
            case 'Y':
                rotationAxis = Vector3.up;
                break;
            case 'Z':
                rotationAxis = Vector3.forward;
                break;
            default:
                Debug.LogError("Invalid axis. Please use 'X', 'Y', or 'Z'.");
                return;
        }

        obj.transform.Rotate(rotationAxis, angle, Space.Self);
    }

    private IEnumerator RotateAxis(int rotate, int value, char axis)
    {
        
        if (isRotating&&value!=0) yield break;


        isRotating = true;
        
        GameObject axisObject = GetAxisObject(rotate, axis);
        localRotationStart = axisObject.transform.localRotation;
        
        if (axisObject != null)
        {
            // Wywołanie funkcji FindObjectsInRange
            List<GameObject> foundObjects = FindObjectsInRange(axisObject, "colorBox");
            LogObjectSize(axisObject);
            AssignObjectsToParent(axisObject, foundObjects);
            if (value != 0) { 
                RotateObject(axisObject, value, axis); }
            
            yield return new WaitForSeconds(0.05f); // Czas trwania obrotu, można dostosować
            //AssignObjectsToParent(cube, foundObjects);
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu dla indeksu: " + rotate);
        }

        isRotating = false;
    }

    private GameObject GetAxisObject(int rotate, char axis)
    {
        switch (axis)
        {
            case 'X':
                return axisX[rotate];
            case 'Y':
                return axisY[rotate];
            case 'Z':
                return axisZ[rotate];
            default:
                Debug.LogError("Nieznana oś obrotu: " + axis);
                return null;
        }
    }

    //wczytaj osie obrotu
    void InitializeAxis(string name, GameObject[] axisArray)
    {
        int cubeNumber = 1;
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
                Debug.LogError("Cube not found: " + cubeName); // Logowanie błędu, jeśli cub nie istnieje
            }
            cubeNumber++; // Inkrementacja numeru cuba
        }
    }

    public void ProcessChildren(GameObject parent)
    {
        // Stwórz listę do przechowywania dzieci
        List<GameObject> children = new List<GameObject>();

        // Iteracja przez wszystkie dzieci obiektu
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }

        // Wywołaj funkcję pomocniczą
        AssignObjectsToParent(cube, children);
    }

    GameObject FindChildByName(string name)
    {
        // Sprawdzenie wszystkich dzieci danego GameObject
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;  // Zwraca null, jeśli nie znajdzie odpowiedniego dziecka
    }

    float SnapToNearest90(float angle)
    {
        // Oblicz resztę z dzielenia przez 90
        float remainder = angle % 90;

        if (remainder > 45)
        {
            // Jeśli reszta jest większa niż 45, obróć do następnej wielokrotności 90
            return Mathf.Ceil(angle / 90) * 90;
        }
        else
        {
            // Jeśli reszta jest mniejsza lub równa 45, obróć do poprzedniej wielokrotności 90
            return Mathf.Floor(angle / 90) * 90;
        }
    }

    void Dezkative(int rotate, char axis)
    {

        GameObject axis1 = axisX[rotate];
       

        
        if (axis=='X') axis1 = axisX[rotate];
        if (axis == 'Y') axis1 = axisY[rotate];
        if (axis == 'Z') axis1 = axisZ[rotate];

        // localRotationStop = axis1.transform.localRotation;
        Vector3 localEulerAngles = axis1.transform.localEulerAngles;

        // Zaokrąglij każdy kąt do najbliższej wielokrotności 90 stopni
        float newRotationX = SnapToNearest90(localEulerAngles.x);
        float newRotationY = SnapToNearest90(localEulerAngles.y);
        float newRotationZ = SnapToNearest90(localEulerAngles.z);
        // Ustaw nowy obrót
        axis1.transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);

        if (axis1 != null)
        {

            ProcessChildren(axis1);
        }

    }

}
