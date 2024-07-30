using Assets.Scripts.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class PlayerInteraction : MonoBehaviour
    {
        public GameObject player;
        public Transform holdPos;
        public float throwForce = 500f; // force at which the object is thrown at
        public float pickUpRange = 2f; // how far the player can pickup the object from
        private GameObject _heldObj; // object which we pick up
        private Rigidbody _heldObjRb; // rigidbody of object we pick up
        private bool _isHoldingObj;

        private bool _isEating = false;
        [SerializeField]
        private float _timeToEat = 1f;
        private float _timeToFinishFood = 0f;

        public Animator rightHandAnimator;

        void Start()
        {

        }

        void Update()
        {
            #region Object interaction

            if (_isEating) // animate eating food
            {
                if (Time.time < _timeToFinishFood)
                {
                    float timeLeft = _timeToFinishFood - Time.time;
                }
                else
                {
                    _isEating = false;
                    GameObject consumedFood = _heldObj;
                    // apply mutation to player if food has any
                    IMutator mutation = consumedFood.GetComponent<IMutator>();
                    if (mutation != null)
                    {
                        PlayerMutation playerMutation = player.GetComponent<PlayerMutation>();
                        playerMutation.applyMutation(mutation);
                    }
                    _isHoldingObj = false;
                    _heldObj = null;
                    Destroy(consumedFood);
                    this.player.GetComponent<Hunger>().hungerLevel -= 10f;
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) // try to pick up object
            {
                if (!_isHoldingObj) // if currently not holding anything
                {
                        // perform raycast to check if player is looking at object within pickuprange
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                        {
                            // make sure pickup tag is attached
                            if (hit.transform.gameObject.tag == "CanPickUp" || hit.transform.gameObject.tag == "Food")
                            {
                                // pass in object hit into the PickUpObject function
                                PickUpObject(hit.transform.gameObject);
                            }
                        }
                    //if (!_isHoldingObj)
                    //{
                    //}
                }
                else
                {
                    StopClipping(); // prevents object from clipping through walls
                    DropObject();
                }
            }

            if (_isHoldingObj) // player is holding food object
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

            #endregion
        }

        void EatFood()
        {
            rightHandAnimator.SetTrigger("EatFood");

            _isEating = true;
            _timeToFinishFood = Time.time + _timeToEat;
        }

        void PickUpObject(GameObject pickUpObj)
        {
            if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
            {
                _heldObj = pickUpObj; // assign object that was hit by the raycast to _heldObj (no longer == null)
                _heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody

                rightHandAnimator.SetTrigger("GrabObject");
            }
        }

        public void GrabObject()
        {
            if (_heldObj != null)
            {
                _heldObjRb.isKinematic = true;
                _heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
                _heldObjRb.transform.rotation = holdPos.rotation;
                //make sure object doesnt collide with player, it can cause weird bugs
                Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
                _isHoldingObj = true;
            }
        }

        void DropObject()
        {
            rightHandAnimator.SetTrigger("DropObject");

            // re-enable collision with player
            Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldObjRb.isKinematic = false;
            _heldObj.transform.parent = null; //unparent object
            _isHoldingObj = false;
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
            rightHandAnimator.SetTrigger("ThrowObject");

            //same as drop function, but add force to object before undefining it
            Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldObj.layer = 0;
            _heldObjRb.isKinematic = false;
            _heldObj.transform.parent = null;
            _heldObjRb.AddForce(transform.forward * throwForce);
            _isHoldingObj = false;
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
            if (hits.Length > 3)
            {
                //change object position to camera position 
                _heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
                                                                                               //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
            }
        }
    }
}