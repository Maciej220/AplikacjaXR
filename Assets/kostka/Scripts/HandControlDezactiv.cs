using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControlDezactiv : MonoBehaviour
{
    // Zaladowanie elementow z innych klas
    public LoadStartCube LoadStartCube; // Dostep do listy osi 
    public AxisMenager AxisMenager; // Klasa do obslugi osi
    public BlokadeCubeRotate BlokadeCubeRotate; // Klasa do blokady innych ruchow w trakcie trwania aktywnego

    // Blok wywolan funkcji za pomoca akcji puszczenia dlonia
    public void RotateX1Dezactive() { StartCoroutine(Dezkative(0, 'X')); }
    public void RotateX2Dezactive() { StartCoroutine(Dezkative(1, 'X')); }
    public void RotateX3Dezactive() { StartCoroutine(Dezkative(2, 'X')); }
    public void RotateY1Dezactive() { StartCoroutine(Dezkative(0, 'Y')); }
    public void RotateY2Dezactive() { StartCoroutine(Dezkative(1, 'Y')); }
    public void RotateY3Dezactive() { StartCoroutine(Dezkative(2, 'Y')); }
    public void RotateZ1Dezactive() { StartCoroutine(Dezkative(0, 'Z')); }
    public void RotateZ2Dezactive() { StartCoroutine(Dezkative(1, 'Z')); }
    public void RotateZ3Dezactive() { StartCoroutine(Dezkative(2, 'Z')); }

    // Funkcja zaokraglajaca kat do najblizszej wielokrotnosci 90 stopni
    float SnapToNearest90(float angle)
    {
        // Oblicz reszte z dzielenia przez 90
        float remainder = angle % 90;

        if (remainder > 45)
        {
            // Jesli reszta jest wieksza niz 45, obroc do nastepnej wielokrotnosci 90
            //return (Mathf.Ceil(angle / 90) * 90) % 360;
            return (90 - remainder);
        }
        else
        {
            // Jesli reszta jest mniejsza lub rowna 45, obroc do poprzedniej wielokrotnosci 90
            //return (Mathf.Floor(angle / 90) * 90) % 360;
            return (0 - remainder);   
        }
    }

    // Funkcja dezaktywujaca obrot osi
    public IEnumerator Dezkative(short index, char axis)
    {
        // Znajdowanie obiektu osi wedlug nazwy
        GameObject axis1 = LoadStartCube.FindByName(axis, index);

        if (axis1 != null)
        {
            Vector3 localEulerAngles = axis1.transform.localEulerAngles; // Pobranie lokalnych katow obrotu
        
            float newRotation = 0;

            // Zaokraglij kazdy kat do najblizszej wielokrotnosci 90 stopni
            switch (axis)
            {
                case 'X':
                    newRotation = SnapToNearest90(localEulerAngles.x); // Zaokraglanie kata w osi X
                    break;
                case 'Y':
                    newRotation = SnapToNearest90(localEulerAngles.y); // Zaokraglanie kata w osi Y
                    break;
                case 'Z':
                    newRotation = SnapToNearest90(localEulerAngles.z); // Zaokraglanie kata w osi Z
                    break;
            }

            // Ustaw nowy obrot
            // axis1.transform.localRotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ); // Ustawianie nowego obrotu
            AxisMenager.RotateSpeed(90f);
            yield return StartCoroutine(AxisMenager.RotateObject(axis1, newRotation, axis));

            // Rozpocznij korutyne do przetwarzania dzieci z opoznieniem
            StartCoroutine(ProcessChildrenWithDelay(axis1));
        }

        BlokadeCubeRotate.unlock(); // Odblokowanie ruchow
    }

    IEnumerator ProcessChildrenWithDelay(GameObject axis1)
    {
        yield return new WaitForSeconds(0.1f); // Opoznienie 0,1s
        while (AxisMenager.rotate)
        {
            yield return new WaitForSeconds(0.1f); // Czeka 0.1 sekundy, mozna dostosowac
        }
        AxisMenager.ProcessChildren(axis1); // Przetwarzanie dzieci obiektu osi
    }
}