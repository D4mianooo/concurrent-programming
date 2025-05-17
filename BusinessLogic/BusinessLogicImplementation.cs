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
      IReadOnlyList<Data.Ball> balls = layerBellow.GetBalls();
      
      foreach (var ball in balls) {
        ball.Balls = balls;
        Thread thread = new Thread(ball.Move);
        thread.IsBackground = true;
        thread.Start();
      }
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