using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject playerHands;
    private GameObject _moveableObject;
    private bool _canInteract;
    private bool _hasItem;

    // Start is called before the first frame update
    void Start()
    {
        _canInteract = false;
        _hasItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Can interact {_canInteract}");
        if (_canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _moveableObject.GetComponent<Rigidbody>().isKinematic = true;
                _moveableObject.transform.position = playerHands.transform.position;
                _moveableObject.transform.parent = playerHands.transform;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.G) && _hasItem)
        {
            _moveableObject.GetComponent<Rigidbody>().isKinematic = false;
            _moveableObject.transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            _canInteract = true;
            _moveableObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _canInteract = false;
    }
}
