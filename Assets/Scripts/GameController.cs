using PathCreation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CrushStellaj
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private GameObject _pathMeshHolder;

        private Camera _cam;
        private CarController _car;
        private PathCreator _path;
        private AdjustCurve _curve;
        private bool _isDrag;

        private Vector2 _startDragPos;
        private Vector2 _endDragPos;

        private void Start()
        {
            _cam = FindObjectOfType<Camera>();
            _car = FindObjectOfType<CarController>();
            _path = FindObjectOfType<PathCreator>();
            _curve = FindObjectOfType<AdjustCurve>();
            
            _curve.SetTarget(_target);
            _curve.SetPath(_path);
            
            _car.SetPath(_path);
            _car.BeginDragEvent += CarBeginDrag;
            _car.EndDragEvent += CarEndDrag;
            
            _pathMeshHolder.SetActive(false);
        }
        
        private void CarBeginDrag(PointerEventData obj)
        {
            _startDragPos = obj.position;
            _isDrag = true;
            _pathMeshHolder.SetActive(true);
        }

        private void CarEndDrag(PointerEventData obj)
        {
            _endDragPos = obj.position;
            _isDrag = false;
            _pathMeshHolder.SetActive(false);
            
            _car.Acceleration(_startDragPos.y - _endDragPos.y);
        }

        private void Update()
        {
            ChangeTargetPosition();
        }

        private void ChangeTargetPosition()
        {
            if (_isDrag == false)
            {
                if (_curve.IsMoving) 
                    _curve.SetMovingState(false);
                return;
            }
            
            _curve.SetMovingState(true);
            
            Vector3 mouse = Input.mousePosition;
            mouse.z = 10f;
            Vector3 mouseWorld = _cam.ScreenToWorldPoint(mouse);

            _target.transform.position = new Vector3(-mouseWorld.x, 0f, mouseWorld.y + mouseWorld.z / 2f);
        }

        private void OnDestroy()
        {
            _car.BeginDragEvent -= CarBeginDrag;
            _car.EndDragEvent -= CarEndDrag;
        }
    }
}