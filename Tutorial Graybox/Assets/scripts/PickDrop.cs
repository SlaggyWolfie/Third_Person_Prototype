using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickDrop : MonoBehaviour
{
    public GameObject _target;
    private Collider _targetCollider = null;
    private Rigidbody _targetRigidbody = null;
    [SerializeField] private bool _holding = false;

    // Use this for initialization
    private void Start()
    {

        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (!_holding) return;
        _target.transform.position = transform.position+new Vector3(1,0.3f,1);
        _target.transform.rotation = transform.rotation;

        if (Input.GetKeyUp(KeyCode.F)) Hold(false);
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag != "Crate"|| !Input.GetKeyUp(KeyCode.E)) return;
        _target = collider.gameObject;
        _targetCollider = collider;
        _targetRigidbody = collider.attachedRigidbody;
        if (!_holding) Hold();
        
        
    }

    private void Hold(bool hold = true)
    {
        _targetCollider.enabled = true;
        _holding = hold;
        if (_targetRigidbody != null)
        {
            //_targetRigidbody.useGravity = false ;
            //_targetRigidbody.isKinematic = hold;
        }
    }
}
