using Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetTarget : MonoBehaviour
{
    //objectHolder variables
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject objectHolder;
    [SerializeField] private GameObject playerCamera;
    private bool _IsPickedUp;
    private Quaternion _cameraRotation;
    private Vector3 _F2 = Vector3.forward* 2;
    private IPlayerMovement _movement;
    //HoldedObject
    [SerializeField]private float _baseDrag;
    [SerializeField] private float _ThrowPower;
    private GameObject _holdedObject;
    private bool _forceApplying = false;
    private Vector3 _forceDirection;
    private CharacterInput _characterInput;
    private void Awake()
    {
        _movement = GetComponent<IPlayerMovement>();
        _characterInput = new CharacterInput();
    }
    private void OnEnable()
    {
        _characterInput.Humanoid.Enable();
        _characterInput.Humanoid.PickUp.performed += PickUp;
        _characterInput.Humanoid.PutDown.performed += PutDown;
    }
    private void OnDisable()
    {
        _characterInput.Humanoid.Disable();
        _characterInput.Humanoid.PickUp.performed -= PickUp;
        _characterInput.Humanoid.PutDown.performed -= PutDown;
    }
    private void PutDown(InputAction.CallbackContext obj)
    {
        if (_IsPickedUp)
        {
            _IsPickedUp = false;
            _holdedObject.GetComponent<Rigidbody>().useGravity = true;
            _forceApplying = false;
            _holdedObject = null;
        }
    }

    private void PickUp(InputAction.CallbackContext obj)
    {
        var pPosition = _movement.Position;
        if (Physics.Raycast(pPosition, objectHolder.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 16f) && hit.collider.gameObject.layer == 8 && !_IsPickedUp)
        {
            _IsPickedUp = true;
            _holdedObject = hit.collider.gameObject;
            _holdedObject.GetComponent<Rigidbody>().useGravity = false;
            _holdedObject.GetComponent<Rigidbody>().drag = 5;
            _forceApplying = true;
        }
        else if(_IsPickedUp)
        {
            _IsPickedUp = false;
            _holdedObject.GetComponent<Rigidbody>().useGravity = true;
            _forceApplying = false;
            _holdedObject.GetComponent<Rigidbody>().drag = _baseDrag;
            _holdedObject.GetComponent<Rigidbody>().AddForce(playerCamera.transform.TransformDirection(Vector3.forward) * _ThrowPower, ForceMode.Impulse);
            _holdedObject = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _cameraRotation = playerCamera.transform.rotation;
        var pPosition = _movement.Position;
        objectHolder.transform.rotation = _cameraRotation;
        objectHolder.transform.position = _movement.Position;
        objectHolder.transform.position += objectHolder.transform.TransformDirection(Vector3.forward) * 3f; 
        //Raycast
        if (_forceApplying)
        {
            Vector3 _forceDirection = (objectHolder.transform.position - _holdedObject.transform.position);
            _holdedObject.GetComponent<Rigidbody>().AddForce(_forceDirection, ForceMode.Impulse);
            Debug.DrawLine(_holdedObject.transform.position, objectHolder.transform.position, UnityEngine.Color.red);
        }
        
        Debug.DrawRay(pPosition, objectHolder.transform.TransformDirection(Vector3.forward), UnityEngine.Color.yellow);
    }
}