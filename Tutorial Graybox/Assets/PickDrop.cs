using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickDrop : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;
    [SerializeField] private bool _holding = false;

    // Use this for initialization
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (!_holding) return;
        _target.transform.position = transform.position+new Vector3(0,0.4f,2);
        //_target.transform.rotation = transform.rotation;

        if (Input.GetKeyUp(KeyCode.F)) Hold(false);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag!="Crate"|| !Input.GetKeyUp(KeyCode.E)) return;
        _target = collider.gameObject;
        if (!_holding) Hold();
    }

    private void Hold(bool hold = true)
    {
        _holding = hold;
		_target.GetComponent<Rigidbody> ().isKinematic = hold;
    }
}
