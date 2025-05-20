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

    private void CollisionDetection() {
      foreach (Ball otherBall in Balls) {
        if(this.ID == otherBall.ID) continue;
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