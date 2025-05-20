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

namespace TP.ConcurrentProgramming.BusinessLogic
{
  internal class Ball : IBall
  {
    public Ball(Data.IBall ball, List<Ball> balls)
    {
      Position = ball.Position;
      Diameter = ball.Diameter;
      Mass = ball.Mass;
      Velocity = ball.Velocity;
      this.balls = balls;
    }

    #region IBall

    private IReadOnlyList<Ball> balls;
    private Vector Position { get; set; }
    public IVector Velocity { get; set; }
    public float Diameter { get; set; }
    public float Mass { get; set; }
    public event EventHandler<IPosition>? NewPositionNotification;
    #endregion IBall

    #region private

    public void Move() {
      while (true) {
        CollisionDetection();
        if (Position.x + Velocity.x > 400 - 8 - Diameter || Position.x + Velocity.x < 0) {
          lock (this) {
            Velocity = new Vector(-Velocity.x ,Velocity.y);
          }
        }
        if (Position.y + Velocity.y > 420 - 8 - Diameter || Position.y + Velocity.y < 0) {
          lock (this) {
            Velocity = new Vector(Velocity.x ,-Velocity.y);
          }
        }
        lock (this) {
          Position = new Vector(Position.x + Velocity.x, Position.y + Velocity.y);
        }
        RaiseNewPositionChangeNotification();
        Thread.Sleep(10);
      }
    }
    private void RaiseNewPositionChangeNotification() {
      NewPositionNotification?.Invoke(this, new Position(Position.x, Position.y));
    }

    private void CollisionDetection() {
      foreach (Ball otherBall in balls) {
        Vector diff = new Vector(this.Position.x - otherBall.Position.x, this.Position.y - otherBall.Position.y);
        double distance = Math.Sqrt(diff.x * diff.x + diff.y * diff.y);
        double halfDiameters = (Diameter + otherBall.Diameter) / 2;
        if (halfDiameters > distance) {
          Collison(this, otherBall);
        }
      }
    }

    internal void Collison(Ball a, Ball b) {
      Vector v1 = (Vector)a.Velocity, v2 = (Vector) b.Velocity;
      
      Vector normal = GetNormal(a, b);

      Vector relativeVelocity = v1 - v2;
      double velocityAlongNormal = Vector.Dot(normal, relativeVelocity);

      if (velocityAlongNormal >= 0) {
        return;
      }
      velocityAlongNormal *= -2;
      velocityAlongNormal *= a.Mass * b.Mass / (a.Mass + b.Mass);
      lock (a) {
        a.Velocity = v1 + normal * velocityAlongNormal/a.Mass;
      }
      lock (b) {
        b.Velocity = v2 - normal * velocityAlongNormal/b.Mass;
      }

    }
    private static Vector GetNormal(Ball a, Ball b) {

      Vector normal = a.Position - b.Position;
      normal = Vector.Normalize(normal);


      return normal;
    }

    #endregion private
  }
}