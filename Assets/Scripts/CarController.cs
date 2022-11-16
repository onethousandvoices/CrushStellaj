using System;
using PathCreation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CrushStellaj
{
    public class CarController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Wheel[] _wheels;
        [SerializeField] private float _steerAngleMax;
        [SerializeField] private Vector3 _centerOfMass;
        [SerializeField] private GameObject _pointer;
        [SerializeField] private float _pointerSpeed;

        private PathCreator _path;
        private Rigidbody _rb;
        private float _angle;
        private float _torque;
        private float _pointerPosition;

        public event Action<PointerEventData> BeginDragEvent;
        public event Action<PointerEventData> EndDragEvent;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.centerOfMass = _centerOfMass;
        }

        private void Update()
        {
            for (int i = 0; i < _wheels.Length; i++)
                _wheels[i].Collider.motorTorque = Mathf.MoveTowards(_wheels[i].Collider.motorTorque, _torque, 1000f * Time.deltaTime);

            if (Vector3.Distance(transform.position, _pointer.transform.position) < 5f)
            {
                _pointerPosition += _pointerSpeed * Time.deltaTime;
                _pointer.transform.position = _path.path.GetPointAtDistance(_pointerPosition, EndOfPathInstruction.Stop);
            }
            
            _angle = Vector3.SignedAngle(transform.forward, _pointer.transform.position - transform.position, Vector3.up);
            
            UpdateVisual(_angle);
        }

        public void Acceleration(float torque)
        {
            torque = Mathf.Clamp(torque, 100f, 1500f);
            _torque = torque;
        }

        private void UpdateVisual(float angle)
        {
            if (Vector3.Distance(transform.position, _pointer.transform.position) < 1.5f) return;
            
            for (int i = 0; i < _wheels.Length; i++)
            {
                if (_wheels[i].Steering)
                {
                    angle = Mathf.Clamp(angle, -_steerAngleMax, _steerAngleMax);
                    _wheels[i].Collider.steerAngle = Mathf.MoveTowards(_wheels[i].Collider.steerAngle, angle, 100f * Time.deltaTime);
                }

                _wheels[i].Collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
                _wheels[i].Mesh.transform.position = pos;
                _wheels[i].Mesh.transform.rotation = rot;
            }
        }

        public void SetPath(PathCreator path) => _path = path;
        
        public void OnPointerDown(PointerEventData eventData) => BeginDragEvent?.Invoke(eventData);

        public void OnPointerUp(PointerEventData eventData) => EndDragEvent?.Invoke(eventData);
    }
}