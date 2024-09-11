using Assets.Scripts.Core;
using Assets.Scripts.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class PlayerInteraction : MonoBehaviour
    {
        public GameObject player;
        private int LayerNumber; //layer index
        public Animator rightHandAnimator;

        public Transform holdPos;
        public float pickUpRange = 2f; // how far the player can pickup the object from
        //public float throwForce = 500f; // force at which the object is thrown at
        GameObject _heldObj; // object which we pick up
        Rigidbody _heldObjRb; // rigidbody of object we pick up

        bool _isWindingUp;
        float _totalWindUpTime = 0;

        public bool playerCanInteract;
        public bool isHoldingObj;
        public bool isEating;

        void Start()
        {
            LayerNumber = LayerMask.NameToLayer("Always on top");
            playerCanInteract = true;
        }

        void Update()
        {
            #region Object interaction

            if (playerCanInteract)
            {
                if (Input.GetKeyDown(KeyCode.E)) // try to pick up object
                {
                    if (!isHoldingObj) // if currently not holding anything
                    {
                        // perform raycast to check if player is looking at object within pickuprange
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickUpRange))
                        {
                            // make sure player is looking at an interactable object
                            Interactable pickUpObj;
                            if (hit.transform.gameObject.TryGetComponent<Interactable>(out pickUpObj))
                            {
                                // pass in object hit into the PickUpObject function
                                PickUpObject(pickUpObj.gameObject);
                            }
                        }
                    }
                    else
                    {
                        if (_isWindingUp)
                        {
                            _totalWindUpTime = 0;
                            _isWindingUp = false;
                        }

                        StopClipping(); // prevents object from clipping through walls
                        DropObject();
                    }
                }

                if (isHoldingObj) // player is holding food object
                {
                    MoveObject(); // keep object position at holdPos
                }
            }

            if (_isWindingUp)
            {
                _totalWindUpTime += Time.deltaTime;

                _totalWindUpTime = Mathf.Clamp(_totalWindUpTime, 0, 1);

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    _totalWindUpTime = 0;
                    _isWindingUp = false;

                    rightHandAnimator.SetTrigger("CancelWindUp");
                }
            }

            #endregion
        }

        #region Events

        public void OnShortClickEvent()
        {
            if (isHoldingObj)
            {
                Interactable interactable;
                if (_heldObj.TryGetComponent<Interactable>(out interactable) && interactable.isEdible)
                {
                    EatFood();
                }
            }
        }

        public void OnLongClickEvent()
        {
            if (isHoldingObj)
            {
                WindUpObject();
            }
        }

        public void OnLongClickReleasedEvent()
        {
            if (isHoldingObj && _isWindingUp)
            {
                StopClipping();
                ThrowObject();
            }
        }

        #endregion

        #region Food Interactions

        void EatFood()
        {
            playerCanInteract = false;
            rightHandAnimator.SetTrigger("EatFood");

            isEating = true;
        }

        public void ConsumeFood()
        {
            isEating = false;
            GameObject consumedFood = _heldObj;
            // apply mutation to player if food has any
            IMutator mutation = consumedFood.GetComponent<IMutator>();
            if (mutation != null)
            {
                PlayerMutation playerMutation = player.GetComponent<PlayerMutation>();
                playerMutation.ApplyMutation(mutation);
            }
            isHoldingObj = false;
            _heldObj = null;
            Destroy(consumedFood);
            this.player.GetComponent<Hunger>().hungerStatus.IncreaseStatusMeter(10f);
            playerCanInteract = true;
        }

        #endregion

        #region Object Interactions

        void PickUpObject(GameObject pickUpObj)
        {
            if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
            {
                playerCanInteract = false;
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
                _heldObj.layer = LayerNumber;
                //make sure object doesnt collide with player, it can cause weird bugs
                Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);

                for (int i = 0; i < _heldObj.transform.childCount; i++)
                {
                    var childTransform = _heldObj.transform.GetChild(i);
                    var child = childTransform.gameObject;

                    child.TryGetComponent(out Rigidbody childRb);
                    if (childRb != null)
                    {
                        childRb.isKinematic = true;
                    }
                    child.layer = LayerNumber;

                    //make sure object doesnt collide with player, it can cause weird bugs
                    child.TryGetComponent(out Collider childCollider);
                    if (childCollider != null)
                    {
                        Physics.IgnoreCollision(childCollider, player.GetComponent<Collider>(), true);
                    }
                }

                isHoldingObj = true;
                playerCanInteract = true;
            }
        }

        void DropObject()
        {
            rightHandAnimator.SetTrigger("DropObject");

            // re-enable collision with player
            Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldObj.layer = 0;
            _heldObjRb.isKinematic = false;

            for (int i = 0; i < _heldObj.transform.childCount; i++)
            {
                var childTransform = _heldObj.transform.GetChild(i);
                var child = childTransform.gameObject;

                // re-enable collision with player
                child.TryGetComponent(out Collider childCollider);
                if (childCollider != null)
                {
                    Physics.IgnoreCollision(childCollider, player.GetComponent<Collider>(), false);
                }

                child.layer = 0;

                child.TryGetComponent(out Rigidbody childRb);
                if (childRb != null)
                {
                    childRb.isKinematic = false;
                }
            }

            _heldObj.transform.parent = null; //unparent object
            isHoldingObj = false;
            _heldObj = null; //undefine game object
        }

        void MoveObject()
        {
            if (!isEating)
            {
                // keep object position the same as the holdPosition position
                _heldObj.transform.position = holdPos.transform.position;
            }
        }

        void WindUpObject()
        {
            _isWindingUp = true;
            rightHandAnimator.SetTrigger("WindUpObject");
        }

        void ThrowObject()
        {
            playerCanInteract = false;
            _isWindingUp = false;
            rightHandAnimator.SetTrigger("ThrowObject");
        }

        public void ReleaseThrownObject()
        {
            //same as drop function, but add force to object before undefining it
            Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
            _heldObj.layer = 0;
            _heldObjRb.isKinematic = false;

            for (int i = 0; i < _heldObj.transform.childCount; i++)
            {
                var childTransform = _heldObj.transform.GetChild(i);
                var child = childTransform.gameObject;

                // re-enable collision with player
                child.TryGetComponent(out Collider childCollider);
                if (childCollider != null)
                {
                    Physics.IgnoreCollision(childCollider, player.GetComponent<Collider>(), false);
                }

                child.layer = 0;

                child.TryGetComponent(out Rigidbody childRb);
                if (childRb != null)
                {
                    childRb.isKinematic = false;
                }
            }

            _heldObj.transform.parent = null;
            FirstPersonController firstPersonController;
            if (player.TryGetComponent<FirstPersonController>(out firstPersonController))
            {
                _heldObjRb.AddForce(_totalWindUpTime * firstPersonController.throwForce * transform.forward);
            } else
            {
                _heldObjRb.AddForce(_totalWindUpTime * 500f * transform.forward);
            }
            _totalWindUpTime = 0;
            isHoldingObj = false;
            _heldObj = null;
            playerCanInteract = true;
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

        #endregion
    }
}