using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject _key = null;

     public GameObject _target = null;

    [SerializeField] private Vector3 _targetPos;

    [SerializeField] private bool _opening = false;

    [SerializeField] private float _targetSpeed = 0.1f;
    // Use this for initialization
    void Start()
    {
        _targetPos = _target.transform.position;
        _targetPos.y -= 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (_opening) _target.transform.position = Vector3.MoveTowards(_target.transform.position, _targetPos, _targetSpeed);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == _key)
            OpenTarget(false);
    }

    private void OpenTarget(bool destroy)
    {
        if (destroy) Destroy(_target);
        else _opening = true;
    }
}
