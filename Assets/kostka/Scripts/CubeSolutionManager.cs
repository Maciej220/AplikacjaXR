using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSolutionManager : MonoBehaviour
{
    public LoadStartCube LoadStartCube; // Dostep do listy osi 
    public AxisMenager AxisMenager; // Klasa do obslugi osi

    private GameObject[,,] cubies = new GameObject[3, 3, 3];

    public GameObject parent;

    void LoadCubies()
    {
        // Sprawdz, czy parent nie jest null
        if (parent == null)
        {
            Debug.LogError("Parent object is null!");
            return;
        }

        // Znajdz wszystkie cubie'y z tagiem "colorBox" wewnatrz parenta
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("colorBox"))
            {
                // Pobierz lokalna pozycje cubie'a wzgledem parenta
                Vector3 localPos = child.localPosition;

                // Zaokraglij pozycje do najblizszej liczby calkowitej
                int x = Mathf.RoundToInt(localPos.x);
                int y = Mathf.RoundToInt(localPos.y);
                int z = Mathf.RoundToInt(localPos.z);

                // Upewnij sie, ze pozycje sa w zakresie 0, 1, 2
                x = Mathf.Clamp(x, 0, 2);
                y = Mathf.Clamp(y, 0, 2);
                z = Mathf.Clamp(z, 0, 2);

                // Przypisz cubie'a do odpowiedniej komorki w tablicy
                cubies[x, y, z] = child.gameObject;
            }
        }
    }
}