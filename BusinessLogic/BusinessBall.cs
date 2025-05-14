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
    public Ball(Data.IBall ball)
    {
      ball.NewPositionNotification += RaisePositionChangeEvent;
      Diameter = ball.Diameter;
      Mass = ball.Mass;
      Velocity = ball.Velocity;
    }

    #region IBall

    public IVector Velocity { get; set; }
    public float Diameter { get; set; }
    public float Mass { get; set; }
    public event EventHandler<IPosition>? NewPositionNotification;
    #endregion IBall

    #region private

    private void RaisePositionChangeEvent(object? sender, Data.IVector e)
    {
      NewPositionNotification?.Invoke(this, new Position(e.x, e.y));
    }

    #endregion private
  }
}