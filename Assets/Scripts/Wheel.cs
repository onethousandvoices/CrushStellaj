using System;
using UnityEngine;

namespace CrushStellaj
{
    [Serializable]
    public struct Wheel
    {
        public WheelCollider Collider;
        public MeshRenderer Mesh;
        public bool Steering;
    }
}