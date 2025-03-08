using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControlActive : MonoBehaviour
{
    // Za�adowanie element�w z innych klas
    public LoadStartCube LoadStartCube; // Dost�p do listy osi
    public AxisMenager AxisMenager; // Klasa do obs�ugi osi
    public BlokadeCubeRotate BlokadeCubeRotate; // Klasa do blokady innych ruch�w w trakcie trwania aktywnego
    public SmartBrick SmartBrick;
    
    // Metody wywo�uj�ce rotacj� dla r�nych osi
    public void RotateX1Active() => StartCoroutine(RotateAxis(0, 0, 'X'));
    public void RotateX2Active() => StartCoroutine(RotateAxis(1, 0, 'X'));
    public void RotateX3Active() => StartCoroutine(RotateAxis(2, 0, 'X'));

    public void RotateY1Active() => StartCoroutine(RotateAxis(0, 0, 'Y'));
    public void RotateY2Active() => StartCoroutine(RotateAxis(1, 0, 'Y'));
    public void RotateY3Active() => StartCoroutine(RotateAxis(2, 0, 'Y'));

    public void RotateZ1Active() => StartCoroutine(RotateAxis(0, 0, 'Z'));
    public void RotateZ2Active() => StartCoroutine(RotateAxis(1, 0, 'Z'));
    public void RotateZ3Active() => StartCoroutine(RotateAxis(2, 0, 'Z'));

    

    public GameObject cube; // Obiekt kostki

    // Korutyna do rotacji osi
    public IEnumerator RotateAxis(short index, int value, char axis)
    {
        // Je�li inny ruch jest aktywny, zablokuje nowe
        if (BlokadeCubeRotate.testRotate()) yield break;// przerwanie procesu
        BlokadeCubeRotate.block(); // Blokowanie innych ruch�w


        // Znajdowanie obiektu osi wed�ug nazwy
        GameObject axisObject = LoadStartCube.FindByName(axis, index);

        if (axisObject != null)
        {
            // Wywo�anie funkcji FindObjectsInRange
            List<GameObject> foundObjects = AxisMenager.FindObjectsInRange(axisObject, "colorBox");
            AxisMenager.AssignObjectsToParent(axisObject, foundObjects); // Przypisanie znalezionych obiekt�w do osi

            if (value != 0)
            {
               yield return  StartCoroutine(AxisMenager.RotateObject(axisObject, value, axis)); // Rotacja obiektu osi
            }

            if (value != 0)
            {
                AxisMenager.AssignObjectsToParent(cube, foundObjects); // Przypisanie obiekt�w z powrotem do kostki
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu dla indeksu: " + index); // Wy�wietlenie b��du, je�li obiekt osi nie zosta� znaleziony
        }

        BlokadeCubeRotate.unlock(); // Odblokowanie ruch�w
        
    }
}
