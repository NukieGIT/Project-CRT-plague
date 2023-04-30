using Movement;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class GetTarget : MonoBehaviour
{
    //objectHolder variables
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject objectHolder;
    [SerializeField] private GameObject playerCamera;
    private Quaternion _cameraRotation;
    private Vector3 _F2 = Vector3.forward* 2;
    private IPlayerMovement _movement;
    //HoldedObject
    private GameObject _holdedObject;
    private Vector3 _forceApplyer;
    private bool _forceApplying = false;
    private Vector3 _forceDirection;
    private void Awake()
    {
        _movement = GetComponent<IPlayerMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _cameraRotation = playerCamera.transform.rotation;
        var pPosition = _movement.Position;
        objectHolder.transform.rotation = _cameraRotation;
        objectHolder.transform.position = _movement.Position;
        objectHolder.transform.position += objectHolder.transform.TransformDirection(Vector3.forward) * 2f; 
        //Raycast
        if(Physics.Raycast(pPosition, objectHolder.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 16f) && hit.collider.gameObject.layer == 8)
        {
            _holdedObject = hit.collider.gameObject;
            _holdedObject.GetComponent<Rigidbody>().useGravity = false;
            _forceApplying |= true;
        }
        if (_forceApplying)
        {
            Vector3 _forceDirection = (objectHolder.transform.position - _holdedObject.transform.position).normalized;
            _forceApplyer = _forceDirection * 100f;
            if(_holdedObject.transform.position == objectHolder.transform.position)
            {
                _holdedObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            }
            else
            {
                _holdedObject.GetComponent<Rigidbody>().AddForce(_forceApplyer);
            }
            Debug.DrawLine(_holdedObject.transform.position, objectHolder.transform.position, UnityEngine.Color.red);
        }
        Debug.DrawRay(pPosition, objectHolder.transform.TransformDirection(Vector3.forward), UnityEngine.Color.yellow);
    }
}