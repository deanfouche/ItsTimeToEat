using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f; // force at which the object is thrown at
    public float pickUpRange = 2f; // how far the player can pickup the object from
    private GameObject _heldObj; // object which we pick up
    private Rigidbody _heldObjRb; // rigidbody of object we pick up

    private bool _isEating = false;
    [SerializeField]
    private float _timeToEat = 1f;
    private float _timeToFinishFood = 0f;

    public AnimationCurve myCurve;

    ////adjust this to change speed
    //float speed = 5f;
    ////adjust this to change how high it goes
    //float height = 0.5f;
    //Vector3 pos;

    void Start()
    {

    }

    void Update()
    {
        if (_isEating) // animate eating food
        {
            if (Time.time < _timeToFinishFood)
            {
                float timeLeft = _timeToFinishFood - Time.time;
                Debug.Log($"You are eating the {_heldObj.name} (Time to finish food: ${timeLeft})");
                //_heldObj.transform.position = new Vector3(_heldObj.transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), _heldObj.transform.position.z);

                ////get the objects current position and put it in a variable so we can access it later with less code
                //Vector3 pos = _heldObj.transform.position;
                ////calculate what the new Y position will be
                //float newY = Mathf.Sin(Time.time * speed);
                ////set the object's Y to the new calculated Y
                //_heldObj.transform.position = new Vector3(pos.x, newY, pos.z) * height;

                ////calculate what the new Y position will be
                //float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
                ////set the object's Y to the new calculated Y
                //_heldObj.transform.position = new Vector3(_heldObj.transform.position.x, newY, _heldObj.transform.position.z);
            }
            else
            {
                Debug.Log($"You ate the {_heldObj.name}");
                _isEating = false;
                GameObject consumedFood = _heldObj;
                _heldObj = null;
                Destroy(consumedFood);
                this.player.GetComponent<Hunger>().hungerLevel -= 10f;
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) // try to pick up object
        {
            if (_heldObj == null) // if currently not holding anything
            {
                // perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    // make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "Food" || hit.transform.gameObject.tag == "CanPickUp")
                    {
                        // pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                StopClipping(); // prevents object from clipping through walls
                DropObject();
            }
        }

        if (_heldObj != null) // player is holding object
        {
            MoveObject(); // keep object position at holdPos

            if (Input.GetKey(KeyCode.Mouse1))
            {
                // throw held object
                if (Input.GetKeyDown(KeyCode.Mouse0)) // Mous0 (leftclick) is used to throw, change this if you want another button to be used)
                {
                    StopClipping();
                    ThrowObject();
                }
            }
            else
            {
                // eat the food
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (_heldObj.tag == "Food")
                    {
                        EatFood();
                    }
                }
            }
        }
        else
        {

        }
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
            _heldObj = pickUpObj; // assign object that was hit by the raycast to _heldObj (no longer == null)
            _heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            _heldObjRb.isKinematic = true;
            _heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    void DropObject()
    {
        // re-enable collision with player
        Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        _heldObjRb.isKinematic = false;
        _heldObj.transform.parent = null; //unparent object
        _heldObj = null; //undefine game object
    }

    void MoveObject()
    {
        if (!_isEating)
        {
            // keep object position the same as the holdPosition position
            _heldObj.transform.position = holdPos.transform.position;
        }
    }

    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        _heldObj.layer = 0;
        _heldObjRb.isKinematic = false;
        _heldObj.transform.parent = null;
        _heldObjRb.AddForce(transform.forward * throwForce);
        _heldObj = null;
    }

    void StopClipping() // function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(_heldObj.transform.position, transform.position); // distance from holdPos to the camera
        // have to use RaycastAll as object blocks raycast in center screen
        // RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            _heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}