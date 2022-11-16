using System;
using UnityEngine;

namespace CrushStellaj
{
    public class PursuerCam : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _target.position, _speed * Time.deltaTime);
        }
    }
}