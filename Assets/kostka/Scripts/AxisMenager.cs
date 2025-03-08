using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMenager : MonoBehaviour
{
    public SmartBrick SmartBrick;

    private Vector3 rotationAxis; // Deklaracja zmiennej do przechowywania osi rotacji
    public float rotationSpeed; // degrees per second
    private GameObject axisRotate;
    
    private float targetAngle;
    private float currentAngle;
    private char axisChar;
    private bool addAxisSize;
    private float startScal;


    public bool rotate;
    public GameObject cube; // Parent nadrz�dny dla wszystkich obiekt�w kostki 
    


   
    public void RotateSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    public float scale()
    {
        return startScal;
    }
    public List<GameObject> FindObjectsInRange(GameObject parentObject, string tag)
    {
        List<GameObject> objectsInRange = new List<GameObject>(); // Tworzenie listy do przechowywania znalezionych obiekt�w

        if (parentObject == null)
        {
            Debug.LogError("Parent object is null."); // Wy�wietlanie b��du, je�li obiekt nadrz�dny jest null
            return objectsInRange; // Zwr�cenie pustej listy
        }

        // Pobranie BoxCollider obiektu przeszukiwanego
        BoxCollider parentCollider = parentObject.GetComponent<BoxCollider>();
        if (parentCollider == null)
        {
            Debug.LogError("Parent object does not have a BoxCollider component."); // Wy�wietlanie b��du, je�li obiekt nadrz�dny nie ma komponentu BoxCollider
            return objectsInRange; // Zwr�cenie pustej listy
        }

        // Pobranie skali obiektu cube
        Vector3 cubeScale = cube.transform.localScale;
        float averageScale = (cubeScale.x + cubeScale.y + cubeScale.z) / 3.0f;

        // Korygowanie rozmiaru BoxCollider przy u�yciu skali
        Vector3 parentSize = parentCollider.size / startScal * averageScale;
        Vector3 searchPosition = parentObject.transform.position + parentCollider.center; // Obliczenie pozycji wyszukiwania (globalna pozycja centrum BoxCollider)

        // Wyszukiwanie obiekt�w w zadanym zakresie przy u�yciu box overlap
        Collider[] hitColliders = Physics.OverlapBox(searchPosition, parentSize / 2, parentObject.transform.rotation);

        // Iteracja przez znalezione obiekty i sprawdzanie ich tagu oraz czy maj� BoxCollider
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(tag) && hitCollider is BoxCollider)
            {
                objectsInRange.Add(hitCollider.gameObject); // Dodanie obiektu do listy, je�li ma odpowiedni tag i jest BoxColliderem
            }
        }

        return objectsInRange; // Zwr�cenie listy znalezionych obiekt�w
    }


    public void AssignObjectsToParent(GameObject parentObject, List<GameObject> objectsToAssign)
    {
        foreach (GameObject obj in objectsToAssign)
        {
            obj.transform.SetParent(parentObject.transform); // Ustawienie nowego parenta dla ka�dego obiektu w li�cie
        }
    }

    public IEnumerator RotateObject(GameObject obj, float value, char axis)
    {
        
        if (obj == null)
        {
            Debug.LogError("Provided object is null."); // Wy�wietlanie b��du, je�li obiekt jest null
            yield break;
        }


        axisChar = axis;
        addAxisSize = true;
        switch (axis)
        {
            case 'X':
                rotationAxis = Vector3.right; // Ustawienie osi rotacji na 
                break;
            case 'Y':
                rotationAxis = Vector3.up; // Ustawienie osi rotacji na Y
                break;
            case 'Z':
                rotationAxis = Vector3.forward; // Ustawienie osi rotacji na Z
                break;
            default:
                Debug.LogError("Invalid axis. Please use 'X', 'Y', or 'Z'."); // Wy�wietlanie b��du, je�li podana o� jest niepoprawna
                yield break;
        }
        if (value < 0f)
        {
            rotationAxis = -rotationAxis;
            value = -value;
            addAxisSize = !addAxisSize;
        }
        axisRotate = obj;
        targetAngle = value;
        rotate = true;

        while (rotate)
        {
            float angleToRotate = rotationSpeed * Time.fixedDeltaTime;
            currentAngle += angleToRotate;

            // Sprawd�, czy po nast�pnym obrocie nie przekroczymy docelowego k�ta
            if (currentAngle >= targetAngle)
            {

                angleToRotate -= (currentAngle - targetAngle);
                float rotateAxis = 180;
                switch (axisChar)
                {
                    case 'X':
                        rotateAxis = axisRotate.transform.localEulerAngles.x;
                        break;
                    case 'Y':
                        rotateAxis = axisRotate.transform.localEulerAngles.y;
                        break;
                    case 'Z':
                        rotateAxis = axisRotate.transform.localEulerAngles.y;
                        break;
                }
                if (addAxisSize)
                {
                    if (angleToRotate + rotateAxis > 360)
                    {
                        angleToRotate -= 360;
                    }
                }
                else
                {
                    if (angleToRotate - rotateAxis < 0)
                    {
                        angleToRotate += 360;
                    }
                }


            }

            axisRotate.transform.Rotate(rotationAxis, angleToRotate, Space.Self);
            if (currentAngle >= targetAngle)
            {
                currentAngle = 0;
                rotate = false;
                SmartBrick.WallTest();
                yield break;
            }
            yield return null;
        }
    }
    public void ProcessChildren(GameObject parent)
    {
        // Stw�rz list� do przechowywania dzieci
        List<GameObject> children = new List<GameObject>();

        // Iteracja przez wszystkie dzieci obiektu
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.tag == "colorBox")
            {
                children.Add(child.gameObject); // Dodanie obiektu do listy, je�li jego tag to "colorBox"
            }
        }

        // Wywo�aj funkcj� pomocnicz�
        AssignObjectsToParent(cube, children); // Przepi�cie listy obiekt�w do nowego parenta
    } // Przepi�cie listy obiekt�w do nowego parenta
    private void Start()
    {
        Vector3 cubeScale = cube.transform.localScale;
        startScal = (cubeScale.x + cubeScale.y + cubeScale.z) / 3.0f;
        
         // 90 stopni na sekund�
        rotate = false;
        currentAngle = 0f;
        Debug.Log("Start test");
        rotationSpeed = 65f;
    }
     /*   
    void FixedUpdate()
    {
        if (rotate)
        {
            float angleToRotate = rotationSpeed * Time.fixedDeltaTime;
            currentAngle += angleToRotate;

            // Sprawd�, czy po nast�pnym obrocie nie przekroczymy docelowego k�ta
            if (currentAngle >= targetAngle)
            {
                
                angleToRotate -= (currentAngle - targetAngle);
                float rotateAxis=180;
                switch(axisChar)
                {
                    case 'X':
                        rotateAxis= axisRotate.transform.localEulerAngles.x;
                        break;
                    case 'Y':
                        rotateAxis = axisRotate.transform.localEulerAngles.y;
                        break;
                    case 'Z':
                        rotateAxis = axisRotate.transform.localEulerAngles.y;
                        break;
                }
                if (addAxisSize)
                {
                    if (angleToRotate+rotateAxis>360)
                    {
                        angleToRotate -= 360;
                    }
                }
                else
                {
                    if (angleToRotate - rotateAxis < 0)
                    {
                        angleToRotate += 360;
                    }
                }
               

            }

            axisRotate.transform.Rotate(rotationAxis, angleToRotate, Space.Self);
            if (currentAngle >= targetAngle)
            {
                currentAngle = 0;
                rotate = false;
                SmartBrick.WallTest();
            }
        }
    }*/

}
