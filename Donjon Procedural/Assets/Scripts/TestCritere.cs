using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCritere : MonoBehaviour
{
    int critere1 = 0x1000;
    int critere2 = 0x0100;
    int critere3 = 0x0040;
    int critere4 = 0x0004;

    int roomFlag = 0x0000;
    List<int> criteres;
    // Start is called before the first frame update
    void Start()
    {
        criteres = new List<int>() { critere1, critere2, critere3, critere4 };

        bool result = true;
        foreach (int critere in criteres)
        {
            if ((roomFlag | critere) == roomFlag)
            {
                result = false;
            }
        }


        Debug.Log(result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
