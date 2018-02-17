using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public List<GameObject> Plates;
    public bool PlateShouldBePressedAlways;
    Vector3 _targetPos;
    Vector3 idlePos;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < Plates.Count; i++)
        {
            bool index = Plates[i].GetComponent<PressurePlate>().CloseGateIfPlateNotPushed = PlateShouldBePressedAlways;
        }
        _targetPos = transform.position;
        idlePos = transform.position;
        _targetPos.y -= 10;
    }

    public void Update()
    {
        for (int i = 0; i < Plates.Count; i++)
        {
            bool index=Plates[i].GetComponent<PressurePlate>().index;
            if (index)
                transform.position = Vector3.MoveTowards(transform.position, idlePos, 0.1f);
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, 0.1f);
            }
        }
        
    }
}
