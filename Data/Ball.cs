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
    internal Ball(Vector initialPosition, Vector initialVelocity, int id)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
      ID = id;
    }
    internal Ball(Vector initialPosition, Vector initialVelocity, float diameter, float mass, int id)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
      this.Diameter = diameter;
      this.Mass = mass;
      ID = id;
    }
    #endregion ctor

    #region IBall

    public IReadOnlyList<Ball> Balls { get; set; }
    public event EventHandler<IVector>? NewPositionNotification;
    private int ID;
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
          CollisionDetection();

          if (Position.x + Velocity.x > 400 - 8 - Diameter || Position.x + Velocity.x < 0) {
            Velocity = new Vector(-Velocity.x ,Velocity.y);
          }
          if (Position.y + Velocity.y > 420 - 8 - Diameter || Position.y + Velocity.y < 0) {
            Velocity = new Vector(Velocity.x ,-Velocity.y);
          }
          Position = new Vector(Position.x + Velocity.x, Position.y + Velocity.y);
          RaiseNewPositionChangeNotification();
          Thread.Sleep(20);
      }
    }

    private void CollisionDetection() {
      foreach (Ball otherBall in Balls) {
        if(ID == otherBall.ID) return;
        Vector diff = new(Position.x - otherBall.Position.x, Position.y - otherBall.Position.y);
        double distance = Math.Sqrt(diff.x * diff.x + diff.y * diff.y);
        if (Diameter / 2 + otherBall.Diameter / 2 > distance) {
          Collison(this, otherBall);
        }
      }
    }

    internal void Collison(Ball a, Ball b) {
      Vector v1 = (Vector)a.Velocity, v2 = (Vector) b.Velocity;
      
      Vector normal = a.Position - b.Position;
      normal = Vector.Normalize(normal);
      
      Vector relativeVelocity = v1 - v2;
      double velocityAlongNormal = Vector.Dot(normal, relativeVelocity);

      if (velocityAlongNormal >= 0) {
        return;
      }
      
      velocityAlongNormal *= -2;
      velocityAlongNormal *= a.Mass * b.Mass / (a.Mass + b.Mass);
      lock (a) {
        lock (b) {
          a.Velocity = v1 + normal * velocityAlongNormal/a.Mass;
          b.Velocity = v2 - normal * velocityAlongNormal/b.Mass;
        }
      }
    }

    #endregion private

  }
}