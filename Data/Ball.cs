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
  public class Ball : IBall
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
    public Vector Position { get; set; }
    public float Diameter { get; set; }
    public float Mass { get; set; }

    #endregion IBall

    #region private

    private void RaiseNewPositionChangeNotification()
    {
      NewPositionNotification?.Invoke(this, Position);
    }

    public void Move() {
      while (true) {
        Position = new Vector(Position.x + Velocity.x, Position.y + Velocity.y);

        if (Position.x + Velocity.x > 400 - 8 - Diameter || Position.x + Velocity.x < 0) {
          Velocity = new Vector(-Velocity.x ,Velocity.y);
        }
        if (Position.y + Velocity.y > 420 - 8 - Diameter || Position.y + Velocity.y < 0) {
          Velocity= new Vector(Velocity.x ,-Velocity.y);
        }

        RaiseNewPositionChangeNotification();
        Thread.Sleep(20);
      }
    }

    #endregion private
  }
}