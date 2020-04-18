using UnityEngine;

namespace Entity.Base
{
    public class Line
    {
        public Vector2 Pos1 { get; }
        public  Vector2 Pos2 { get; }
        public float Distance { get; }

        public Line(Vector2 pos1, Vector2 pos2)
        {
            this.Pos1 = pos1;
            this.Pos2 = pos2;
            Distance = (pos1 - pos2).magnitude;
        }
            
        public bool Intersects(Line otherLine)
        {
            return ((otherLine.Pos2.y - Pos1.y) * (otherLine.Pos1.x - Pos1.x) >
                    (otherLine.Pos1.y - Pos1.y) * (otherLine.Pos2.x - Pos1.x)) !=
                   ((otherLine.Pos2.y - Pos2.y) * (otherLine.Pos1.x - Pos2.x) >
                    (otherLine.Pos1.y - Pos2.y) * (otherLine.Pos2.x - Pos2.x)) &&
                   ((otherLine.Pos1.y - Pos1.y) * (Pos2.x - Pos1.x) >
                    (Pos2.y - Pos1.y) * (otherLine.Pos1.x - Pos1.x)) !=
                   ((otherLine.Pos2.y - Pos1.y) * (Pos2.x - Pos1.x) >
                    (Pos2.y - Pos1.y) * (otherLine.Pos2.x - Pos1.x));
        }
    }
}