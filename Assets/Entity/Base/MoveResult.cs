using System.Collections.Generic;
using UnityEngine;

namespace Entity.Base
{
    public class MoveResult
    {
        public bool HitRight = false;
        public bool HitLeft = false;
        public bool HitUp = false;
        public bool HitDown = false;
        public Vector3 NewPos;
        public bool HitOneWay = false;
        public readonly List<RaycastHit2D> HorizontalHits = new List<RaycastHit2D>();
        public readonly List<RaycastHit2D> VerticalHits = new List<RaycastHit2D>();
        public Vector3 StartPos;
        public bool HitAny => HitLeft || HitUp || HitDown || HitRight;

        public override string ToString()
        {
            return $"l:{(HitLeft ? 1:0)} u:{(HitUp ? 1:0)} r:{(HitRight ? 1:0)} d:{(HitDown ? 1:0)}";
        }
    }
}
