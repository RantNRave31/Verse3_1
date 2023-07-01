using GKYU.MathLibrary.Tensors.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;


namespace GKYU.MathLibrary.Geometry.Primatives
{
    public interface IAABB
    {
        Vector3D Min { get; }
        Vector3D Max { get; }

        Vector3D Center { get; }
        Vector3D Extent { get; }
        Vector3D Size { get; }

    }
    /// <summary>
    /// An axis-aligned bounding box, or AABB for short, is a box aligned with coordinate axes and fully enclosing some object. 
    /// Because the box is never rotated with respect to the axes, it can be defined by just its center and extents, 
    /// or alternatively by min and max points.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AABB : IAABB
    {
        protected Vector3D _center;
        public Vector3D Center { get { return _center; } private set { _center = value; } }
        protected Vector3D _extent;
        public Vector3D Extent { get { return _extent; } private set { _extent = value; } }
        public Vector3D Size { get { return _extent * 2.0F; } set { _extent = value * 0.5F; } }
        public Vector3D Min { get { return _center - _extent; } set { SetMinMax(value, Max); } }
        public Vector3D Max { get { return _center + _extent; } set { SetMinMax(Min, value); } }

        public AABB(Vector3D center, double extent)
        {
            _center = center;
            _extent = new Vector3D(extent, extent, extent);
        }

        public AABB(Vector3D center, Vector3D extent)
        {
            _center = center;
            _extent = extent;
        }
        public override int GetHashCode()
        {
            return Center.GetHashCode() ^ (Extent.GetHashCode() << 2);
        }
        public override bool Equals(object other)
        {
            if (!(other is AABB)) return false;

            return Equals((AABB)other);
        }

        public bool Equals(AABB other)
        {
            return Center.Equals(other.Center) && Extent.Equals(other.Extent);
        }
        public static bool operator ==(AABB lhs, IAABB rhs)
        {
            // Returns false in the presence of NaN values.
            return (lhs.Center == rhs.Center && lhs.Extent == rhs.Extent);
        }

        //*undoc*
        public static bool operator !=(AABB lhs, IAABB rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }
        public void SetMinMax(Vector3D min, Vector3D max)
        {
            _extent = (max - min) * 0.5F;
            _center = min + _extent;
        }
        public bool Contains(double x, double y, double z)
        {
            return (x >= _center.X - _extent.X)
                   && (x < _center.X + _extent.X)
                   && (y >= _center.Y - _extent.Y)
                   && (y < _center.Y + _extent.Y)
                   && (z >= _center.Z - _extent.Z)
                   && (z < _center.Z + _extent.Z)
                   ;
        }
        public bool Contains(Vector3D point)
        {
            return (point.X >= _center.X - _extent.X)
                   && (point.X < _center.X + _extent.X)
                   && (point.Y >= _center.Y - _extent.Y)
                   && (point.Y < _center.Y + _extent.Y)
                   && (point.Z >= _center.Z - _extent.Z)
                   && (point.Z < _center.Z + _extent.Z)
                   ;
        }
        public bool Contains(AABB bounds)
        {
            return Contains(bounds.Center.X - bounds.Extent.X, bounds.Center.Y - bounds.Extent.Y, bounds.Center.Z - bounds.Extent.Z)
                && Contains(bounds.Center.X + bounds.Extent.X, bounds.Center.Y + bounds.Extent.Y, bounds.Center.Z + bounds.Extent.Z)
                ;
        }
        // Grows the Bounds to include the /point/.
        public void Encapsulate(Vector3D point)
        {
            SetMinMax(Vector3D.Min(Min, point), Vector3D.Max(Max, point));
        }

        // Grows the Bounds to include the /Bounds/.
        public void Encapsulate(AABB bounds)
        {
            Encapsulate(bounds.Center - bounds.Extent);
            Encapsulate(bounds.Center + bounds.Extent);
        }
        // Expand the bounds by increasing its /size/ by /amount/ along each side.
        public void Expand(float amount)
        {
            amount *= .5f;
            _extent += new Vector3D(amount, amount, amount);
        }

        // Expand the bounds by increasing its /size/ by /amount/ along each side.
        public void Expand(Vector3D amount)
        {
            _extent += amount * .5f;
        }
        // Does another bounding box intersect with this bounding box?
        public bool Intersects(IAABB bounds)
        {
            return (Min.X <= bounds.Max.X) && (Max.X >= bounds.Min.X) &&
                (Min.Y <= bounds.Max.Y) && (Max.Y >= bounds.Min.Y) &&
                (Min.Z <= bounds.Max.Z) && (Max.Z >= bounds.Min.Z);
        }
    }
}
