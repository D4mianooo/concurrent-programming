//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System.Diagnostics;
using System.Numerics;
using UnderneathLayerAPI = TP.ConcurrentProgramming.Data.DataAbstractAPI;
using Vector = TP.ConcurrentProgramming.Data.Vector;

namespace TP.ConcurrentProgramming.BusinessLogic
{
  internal class BusinessLogicImplementation : BusinessLogicAbstractAPI
  {
    #region ctor

    public BusinessLogicImplementation() : this(null)
    { }

    internal BusinessLogicImplementation(UnderneathLayerAPI? underneathLayer)
    {
      layerBellow = underneathLayer == null ? UnderneathLayerAPI.GetDataLayer() : underneathLayer;
    }
    #endregion ctor

    #region BusinessLogicAbstractAPI
    private List<Ball> ballList = new List<Ball>();
    public override void Dispose()
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
      layerBellow.Dispose();
      Disposed = true;
    }

    public override void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler)
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));
      layerBellow.Start(numberOfBalls, (startingPosition, databall) => upperLayerHandler(new Position(startingPosition.x, startingPosition.x), new Ball(databall)));
      Thread thread = new Thread(CollisionDetection);
      thread.Start();
    }
    
    private void CollisionDetection(object? o) {
      while (true) {
        IReadOnlyList<Data.Ball> balls = layerBellow.GetBalls();
        for (int i = 0; i < balls.Count; i++) {
          Data.Ball currentBall = balls[i];
          for (int j = i + 1; j < balls.Count; j++) {
              Data.Ball otherBall = balls[j];
              Vector diff = new Vector(currentBall.Position.x - otherBall.Position.x, currentBall.Position.y - otherBall.Position.y);
              double distance = Math.Sqrt(diff.x * diff.x + diff.y * diff.y);
              if (currentBall.Diameter / 2 + otherBall.Diameter / 2 >= distance) {
                Collison(currentBall,  otherBall);  
              }
          }
        }
        Thread.Sleep(10);
      }
    }
    
    internal void Collison(Data.Ball a, Data.Ball b) {
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

      a.Velocity = v1 + normal * velocityAlongNormal/a.Mass;
      b.Velocity = v2 - normal * velocityAlongNormal/b.Mass;
    }
 
    #endregion BusinessLogicAbstractAPI

    #region private

    private bool Disposed = false;

    private readonly UnderneathLayerAPI layerBellow;

    #endregion private

    #region TestingInfrastructure

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }

    #endregion TestingInfrastructure
  }
}