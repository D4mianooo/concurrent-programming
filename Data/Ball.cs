//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.Data
{
  public class Ball : IBall
  {
    #region ctor
    internal Ball(Vector initialPosition, Vector initialVelocity)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
    }
    internal Ball(Vector initialPosition, Vector initialVelocity, float diameter, float mass, int id)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
      Diameter = diameter;
      Mass = mass;
    }
    #endregion ctor

    #region IBall

    public event EventHandler<IVector>? NewPositionNotification;
    public IVector Velocity { get; set; }
    public Vector Position { get; set; }
    public float Diameter { get; set; }
    public float Mass { get; set; }

    #endregion IBall
  }
}