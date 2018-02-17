using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool index = true;
    public bool CloseGateIfPlateNotPushed;
    [SerializeField] private float _targetSpeed = 0.1f;
    // Use this for initialization
    private void Awake()
    {

    }
    void Start()
    {

        
    }

    // Update is called once per frame

    //private void OnTriggerStay(Collider collider)
    //{
    //    if (collider.gameObject.tag == "Crate")
    //        index = false;
    //}
    void OnTriggerExit(Collider collider)
    {
        if(CloseGateIfPlateNotPushed)
        if (collider.gameObject.tag == "Crate")
            index = true;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Crate")
        {
            index = false;
        }
    }
}
