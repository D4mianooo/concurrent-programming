//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{
  internal class Ball : IBall
  {
    #region ctor
    internal Ball(Vector initialPosition, Vector initialVelocity)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
    }
    internal Ball(Vector initialPosition, Vector initialVelocity, float diameter, float mass)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
      this.Diameter = diameter;
      this.Mass = mass;
    }
    #endregion ctor

    #region IBall

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }

    #endregion IBall

    #region private

    public Vector Position;
    public float Diameter;
    public float Mass;
    private void RaiseNewPositionChangeNotification()
    {
      NewPositionNotification?.Invoke(this, Position);
    }

    internal void Move(Vector delta) {
      if (Position.x + delta.x <= 372 && Position.x + delta.x >= 0 && Position.y + delta.y <= 392 && Position.y + delta.y >= 0) {
        Position = new Vector(Position.x + delta.x, Position.y + delta.y);
      }
      else if(Position.x + delta.x >  372 || Position.x + delta.x < 0) {
        Velocity = new Vector(-Velocity.x ,Velocity.y);
      }
      else if (Position.y + delta.y > 392 || Position.y + delta.y < 0) {
        Velocity = new Vector(Velocity.x ,-Velocity.y);
      }
      else {
        Velocity = new Vector(-Velocity.x,-Velocity.y);
      }
      RaiseNewPositionChangeNotification();
    }
    internal static Vector Collison(Ball b1, Ball b2) {
      double factor1 = b1.Mass - b2.Mass / b1.Mass + b2.Mass;
      double factor2 = 2 * b2.Mass - b1.Mass / b1.Mass + b2.Mass;
          
      Vector v1 = (Vector) b1.Velocity * factor1;
      Vector v2 = (Vector) b2.Velocity * factor2;

      return v1 + v2;
    }
    #endregion private
  }
}