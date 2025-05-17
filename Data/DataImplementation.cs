//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using System.Diagnostics;
using System.Numerics;

namespace TP.ConcurrentProgramming.Data
{
  public class BallManager
  {
    private readonly List<Ball> BallsList = [];
    public IReadOnlyList<Ball> Balls => BallsList;
  }
  
  internal class DataImplementation : DataAbstractAPI
  {
    #region ctor

    public DataImplementation() {
    }

    #endregion ctor
    
    #region DataAbstractAPI

    public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(DataImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));
      Random random = new Random();
      for (int i = 0; i < numberOfBalls; i++) {
        Vector startingPosition = new(random.Next(100, 400 - 100), random.Next(100, 400 - 100));
        
        Vector dir = new(random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 1);
        double velocity = random.NextDouble() * 4 + 1;
        
        Vector startingVelocity = new Vector(dir.x * velocity, dir.y * velocity);
        double diameter = random.NextDouble() * 30 + 20;
        float mass = random.NextDouble() > 0.5 ? 1 : 2;
        Ball newBall = new(startingPosition, startingVelocity, (float) diameter, mass);
        upperLayerHandler(startingPosition, newBall);
        BallsList.Add(newBall);
      }
      BallsList.Add(new Ball(new Vector(0, 0), new Vector(5, 0), 20, 2));
    }
    public override IReadOnlyList<Ball> GetBalls() {
      return BallsList;
    }

    #endregion DataAbstractAPI

    #region IDisposable

    protected virtual void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          BallsList.Clear();
        }
        Disposed = true;
      }
      else
        throw new ObjectDisposedException(nameof(DataImplementation));
    }

    public override void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    #endregion IDisposable

    #region private

    //private bool disposedValue;
    private bool Disposed = false;

    private Random RandomGenerator = new();
    private readonly List<Ball> BallsList = [];
    

    #endregion private

    #region TestingInfrastructure

    [Conditional("DEBUG")]
    internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
    {
      returnBallsList(BallsList);
    }

    [Conditional("DEBUG")]
    internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
    {
      returnNumberOfBalls(BallsList.Count);
    }

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }

    #endregion TestingInfrastructure
  }
}