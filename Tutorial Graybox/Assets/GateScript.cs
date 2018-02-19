using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    Vector3 _targetPos;
    Vector3 _idlePos;
    public List<GameObject> plates;
    public List<bool> platesActivated;
    public bool onlyOneThreeFive = false;
    public int numOfPlates = 0;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < plates.Count; i++)
        {
            platesActivated.Add(false);
        }
        _idlePos = transform.position;
        _targetPos = transform.position;
        _targetPos.y -= 10;
    }

    // Update is called once per frame
    void Update()
    {

        if (AllTrue())
        {
            print("getting");
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, 0.1f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _idlePos, 0.1f);
        }
        for (int i = 0; i < plates.Count; i++)
        {
            platesActivated[i] = plates[i].GetComponent<PressurePlate>()._opening;
        }
    }
    private bool AllTrue()
    {
        if (!onlyOneThreeFive)
        {
            for (int i = 0; i < platesActivated.Count; ++i)
            {
                if (platesActivated[i] == false)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            int counter = 0;
            for (int i = 0; i < platesActivated.Count; ++i)
            {
                if (platesActivated[i]) counter++;
            }
            return counter != 0 && counter % 2 == 0;
        }
    }
}
