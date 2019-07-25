using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PressA();
        }

        else if (Input.GetKeyDown(KeyCode.B))
        {
            PressB();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            PressC();
        }
    }

    public void PressA()
    {
        Debug.Log("Pressed A");
    }

    public void PressB()
    {
        Debug.Log("Pressed B");
    }

    public void PressC()
    {
        Debug.Log("Pressed C");
    }
}
