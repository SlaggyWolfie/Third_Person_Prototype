using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

    public GameObject Player;
    public GameObject Gate;
    public GameObject plate;
    public GameObject Crate;
    PickDrop dropScript;
    PressurePlate plateScript;
	// Use this for initialization
	void Start () {
        dropScript = Player.GetComponent<PickDrop>();
        plateScript = plate.GetComponent<PressurePlate>();

        dropScript._target = Crate;
        plateScript._target = Gate;
        plateScript._key = Crate;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
