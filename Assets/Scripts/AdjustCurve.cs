using System.IO;
using PathCreation;
using UnityEngine;

namespace CrushStellaj
{
    public class AdjustCurve : MonoBehaviour
    {
        private GameObject _target;
        private Vector3 _staticPos;
        private PathCreator _pathCreator;
        private RoadMeshCreator _roadMeshCreator;
        private CarController _car;
        
        public bool IsMoving { get; private set; }

        private void Start()
        {
            _car = FindObjectOfType<CarController>();
            _roadMeshCreator = FindObjectOfType<RoadMeshCreator>();
            
            _pathCreator.bezierPath.SetPoint(0, _car.transform.position);
            _pathCreator.bezierPath.SetPoint(1, _car.transform.position - new Vector3(0f,0f,1f));
            _pathCreator.bezierPath.SetPoint(2, _car.transform.position - new Vector3(0f,0f,3f));
            _pathCreator.bezierPath.SetPoint(3, _target.transform.position);
            
            _staticPos = _pathCreator.bezierPath.GetPoint(2);
        }

        private void Update()
        {
            if (IsMoving == false) return;
            
            _pathCreator.bezierPath.MovePoint(2, _staticPos);
            _pathCreator.bezierPath.MovePoint(3, _target.transform.position);
            
            _roadMeshCreator.PathUpdated();
        }

        public void SetTarget(GameObject target) => _target = target;

        public void SetPath(PathCreator path) => _pathCreator = path;

        public void SetMovingState(bool state)
        {
            _pathCreator.UpdatePath(state);
            IsMoving = state;
        }
    }
}