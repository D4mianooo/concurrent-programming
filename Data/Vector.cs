//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//  by introducing yourself and telling us what you do with this community.
//_____________________________________________________________________________________________________________________________________

using System.Numerics;

namespace TP.ConcurrentProgramming.Data
{
  /// <summary>
  ///  Two dimensions immutable vector
  /// </summary>
  public record Vector : IVector
  {
    #region IVector

    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public double x { get; init; }
    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public double y { get; init; }

    #endregion IVector

    /// <summary>
    /// Creates new instance of <seealso cref="Vector"/> and initialize all properties
    /// </summary>
    /// 
    public Vector() {
      
    }
    public Vector(double XComponent, double YComponent)
    {
      x = XComponent;
      y = YComponent;
    }
    private Vector(Vector vector, double factor) {
      x = vector.x * factor;
      y = vector.y * factor;
    }
    private Vector(IVector vector, double factor) {
      x = vector.x * factor;
      y = vector.y * factor;
    }
    private Vector(IVector vector, IVector vector2) {
      x = vector.x + vector2.x;
      y = vector.y + vector2.y;
    }
    public double Length() => Math.Sqrt(x * x + y * y);
    public static double Magnitude(Vector vector) {
      return Math.Sqrt(vector.x * vector.x + vector.y * vector.y);
    }
    
    public static double Dot(Vector v1, Vector v2) {
      return v1.x * v2.x + v1.y * v2.y;
    }
    public static Vector Normalize(Vector vector) {
      if(Magnitude(vector) == 0) return new Vector(0, 0);
      return vector / Magnitude(vector);
    }
    public static Vector operator *(Vector vector, double factor) => new Vector(vector, factor);
    public static Vector operator /(Vector vector, double factor) {
      if (factor == 0) throw new DivideByZeroException();
      return new Vector(vector, 1 / factor);
    }
    public static Vector operator +(Vector v1, Vector v2) => new Vector(v1, v2);
    public static Vector operator -(Vector v1, Vector v2) => new Vector(v1, v2 * -1);
  }
}