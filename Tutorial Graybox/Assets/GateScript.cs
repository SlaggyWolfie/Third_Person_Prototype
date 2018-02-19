using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour {
    Vector3 _targetPos;
    Vector3 _idlePos;
    public List<GameObject> plates;
    public List<bool> platesActivated;
	[Range(0,2)]
	public int onlyEven = 0;
	public int numberOfGates;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < plates.Count; i++)
        {
            platesActivated.Add(false);
        }
            _idlePos = transform.position;
        _targetPos = transform.position;
        _targetPos.y -= 10;
    }
	
	// Update is called once per frame
	void Update () {

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
		if (onlyEven == 0) {
			for (int i = 0; i < platesActivated.Count; ++i) {
				if (platesActivated [i] == false) {
					return false;
				}
			}

			return true;
		} else if (onlyEven == 1) {
		
			int index = 0;
			for (int i = 0; i < platesActivated.Count; ++i) {
				if (platesActivated [i] == true) {
					index++;
				}
			}
			return index != 0 && index % numberOfGates == 0;

		} else if (onlyEven == 2) {

			int index = 0;
			for (int i = 0; i < platesActivated.Count; ++i) {
				if (platesActivated [i] == true) {
					index++;
				}
			}

			if (numberOfGates == 1)
				return index != 0 && index % 2 != 0;
			return index != 0 && index % numberOfGates != 0;

		} else
			return false;
    }
}
