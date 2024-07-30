using Assets.Scripts.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject player;
    public Transform holdPosFood;
    public Transform holdPosObject;
    public float throwForce = 500f; // force at which the object is thrown at
    public float pickUpRange = 2f; // how far the player can pickup the object from
    private GameObject _heldFoodObj; // food object which we pick up
    private Rigidbody _heldFoodObjRb; // rigidbody of food object we pick up
    private GameObject _heldThrowObj; // throwable object which we pick up
    private Rigidbody _heldThrowObjRb; // rigidbody of throwable object we pick up

    private bool _isEating = false;
    [SerializeField]
    private float _timeToEat = 1f;
    private float _timeToFinishFood = 0f;

    void Start()
    {

    }

    void Update()
    {
        #region Food interaction

        if (_isEating) // animate eating food
        {
            if (Time.time < _timeToFinishFood)
            {
                float timeLeft = _timeToFinishFood - Time.time;
            }
            else
            {
                _isEating = false;
                GameObject consumedFood = _heldFoodObj;
                // apply mutation to player if food has any
                IMutator mutation = consumedFood.GetComponent<IMutator>();
                if (mutation != null)
                {
                    PlayerMutation playerMutation = player.GetComponent<PlayerMutation>();
                    playerMutation.applyMutation(mutation);
                }
                _heldFoodObj = null;
                Destroy(consumedFood);
                this.player.GetComponent<Hunger>().hungerLevel -= 10f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // try to pick up object
        {
            if (_heldFoodObj == null) // if currently not holding anything
            {
                // perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    // make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "Food")
                    {
                        // pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                StopClipping(_heldFoodObj); // prevents object from clipping through walls
                DropObject(_heldFoodObj);
            }
        }

        if (_heldFoodObj != null) // player is holding food object
        {
            MoveObject(_heldFoodObj); // keep object position at holdPos

            if (!Input.GetKey(KeyCode.Mouse1))
            {
                // eat the food
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (_heldFoodObj.tag == "Food")
                    {
                        EatFood();
                    }
                }
            }
        }

        #endregion

        #region Throwable object interaction

        if (Input.GetKeyDown(KeyCode.E)) // try to pick up object
        {
            if (_heldThrowObj == null) // if currently not holding anything
            {
                // perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    // make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "CanPickUp")
                    {
                        // pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                StopClipping(_heldThrowObj); // prevents object from clipping through walls
                DropObject(_heldThrowObj);
            }
        }

        if (_heldThrowObj != null) // player is holding throwable object
        {
            MoveObject(_heldThrowObj); // keep object position at holdPos

            if (Input.GetKey(KeyCode.Mouse1))
            {
                // throw held object
                if (Input.GetKeyDown(KeyCode.Mouse0)) // Mous0 (leftclick) is used to throw, change this if you want another button to be used)
                {
                    StopClipping(_heldThrowObj);
                    ThrowObject(_heldThrowObj);
                }
            }
        }

        #endregion
    }

    void EatFood()
    {
        _isEating = true;
        _timeToFinishFood = Time.time + _timeToEat;
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            if (pickUpObj.tag == "Food")
            {
                _heldFoodObj = pickUpObj; // assign object that was hit by the raycast to _heldObj (no longer == null)
                _heldFoodObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
                _heldFoodObjRb.isKinematic = true;
                _heldFoodObjRb.transform.parent = holdPosFood.transform; //parent object to holdposition
                _heldFoodObjRb.transform.rotation = holdPosFood.rotation;
                //make sure object doesnt collide with player, it can cause weird bugs
                Physics.IgnoreCollision(_heldFoodObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            }
            else if (pickUpObj.tag == "CanPickUp")
            {
                _heldThrowObj = pickUpObj; // assign object that was hit by the raycast to _heldObj (no longer == null)
                _heldThrowObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
                _heldThrowObjRb.isKinematic = true;
                _heldThrowObjRb.transform.parent = holdPosObject.transform; //parent object to holdposition
                _heldThrowObjRb.transform.rotation = holdPosObject.rotation;
                //make sure object doesnt collide with player, it can cause weird bugs
                Physics.IgnoreCollision(_heldThrowObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            }
        }
    }

    void DropObject(GameObject heldObj)
    {
        if (heldObj.tag == "Food")
        {
            // re-enable collision with player
            Physics.IgnoreCollision(_heldFoodObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldFoodObjRb.isKinematic = false;
            _heldFoodObj.transform.parent = null; //unparent object
            _heldFoodObj = null; //undefine game object
        }
        else if (heldObj.tag == "CanPickUp")
        {
            // re-enable collision with player
            Physics.IgnoreCollision(_heldThrowObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldThrowObjRb.isKinematic = false;
            _heldThrowObj.transform.parent = null; //unparent object
            _heldThrowObj = null; //undefine game object
        }
    }

    void MoveObject(GameObject heldObj)
    {
        if (heldObj.tag == "Food")
        {
            if (!_isEating)
            {
                // keep object position the same as the holdPosition position
                _heldFoodObj.transform.position = holdPosFood.transform.position;
            }
        }
        else if (heldObj.tag == "CanPickUp")
        {
            // keep object position the same as the holdPosition position
            _heldThrowObj.transform.position = holdPosObject.transform.position;
        }
    }

    void ThrowObject(GameObject heldObj)
    {
        if (heldObj.tag == "Food")
        {
            //same as drop function, but add force to object before undefining it
            Physics.IgnoreCollision(_heldFoodObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldFoodObj.layer = 0;
            _heldFoodObjRb.isKinematic = false;
            _heldFoodObj.transform.parent = null;
            _heldFoodObjRb.AddForce(transform.forward * throwForce);
            _heldFoodObj = null;
        }
        else if (heldObj.tag == "CanPickUp")
        {
            //same as drop function, but add force to object before undefining it
            Physics.IgnoreCollision(_heldThrowObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldThrowObj.layer = 0;
            _heldThrowObjRb.isKinematic = false;
            _heldThrowObj.transform.parent = null;
            _heldThrowObjRb.AddForce(transform.forward * throwForce);
            _heldThrowObj = null;
        }
    }

    void StopClipping(GameObject heldObj) // function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); // distance from holdPos to the camera
        // have to use RaycastAll as object blocks raycast in center screen
        // RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 3)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}