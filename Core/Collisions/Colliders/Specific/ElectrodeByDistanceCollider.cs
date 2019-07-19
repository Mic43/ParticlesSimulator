using System;
using System.Numerics;
using Core.ElectricFieldSources;
using System.Windows;
using Core.Utils;

namespace Core.Collisions.Colliders.Specific
{
    public class ElectrodeByDistanceCollider : ICollider<float,Electrode,Particle>
    {
        private readonly float _distance;

        public ElectrodeByDistanceCollider(float distance)
        {
            if (distance <= 0) throw new ArgumentOutOfRangeException(nameof(distance));
            _distance = distance;
        }

        private float Distance(float x, float y, float x1, float y1, float x2, float y2)
        {

            var A = x - x1;
            var B = y - y1;
            var C = x2 - x1;
            var D = y2 - y1;

            var dot = A * C + B * D;
            var len_sq = C * C + D * D;
            float param = -1;
            if (len_sq != 0) //in case of 0 length line
                param = dot / len_sq;

            float xx, yy;

            if (param < 0)
            {
                xx = x1;
                yy = y1;
            }
            else if (param > 1)
            {
                xx = x2;
                yy = y2;
            }
            else
            {
                xx = x1 + param * C;
                yy = y1 + param * D;
            }

            var dx = x - xx;
            var dy = y - yy;
            return (float) Math.Sqrt( dx * dx + dy * dy);
        }

        public bool AreColliding(Electrode electrode, Particle particle)
        {
            //return polyCheck(particle.Position, new[]
            //{
            //    electrode.Position - Vector2D.Create(size),
            //    electrode.Position + Vector2D.Create(size),
            //    electrode.EndPosition + Vector2D.Create(size),
            //    electrode.EndPosition - Vector2D.Create(size)
            //});

            return Distance(particle.Position.X(), particle.Position.Y(), 
                       electrode.Position.X(), electrode.Position.Y(),
                       electrode.EndPosition.X(), electrode.EndPosition.Y()) < _distance;

            //Rect rect = new Rect(new Point(electrode.Position[0] - size, electrode.Position[1] - size),
            //    new Point(electrode.EndPosition[0] + size, electrode.EndPosition[1] + size));
            //return rect.Contains(new Point(particle.Position[0], particle.Position[1]));
        }
    }
}