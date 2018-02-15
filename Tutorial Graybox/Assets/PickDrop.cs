using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickDrop : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;
    private Collider _targetCollider = null;
    private Rigidbody _targetRigidbody = null;
    [SerializeField] private bool _holding = false;

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(_target != null);
        _targetCollider = _target.GetComponent<Collider>();
        Debug.Assert(_targetCollider != null);
        _targetRigidbody = _target.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (!_holding) return;
        _target.transform.position = transform.position;
        _target.transform.rotation = transform.rotation;

        if (Input.GetKeyUp(KeyCode.F)) Hold(false);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider != _targetCollider || !Input.GetKeyUp(KeyCode.E)) return;

        if (!_holding) Hold();
    }

    private void Hold(bool hold = true)
    {
        _targetCollider.enabled = !hold;
        _holding = hold;
        if (_targetRigidbody != null)
        {
            _targetRigidbody.useGravity = !hold;
            _targetRigidbody.isKinematic = hold;
        }
    }
}
