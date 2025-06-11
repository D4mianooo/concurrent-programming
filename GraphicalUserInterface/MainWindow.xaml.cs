//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TP.ConcurrentProgramming.Presentation.ViewModel;

namespace TP.ConcurrentProgramming.PresentationView
{
  public partial class MainWindow : Window
  {
    private Timer timer = null;
    public MainWindow()
    {
      Random random = new Random();
      InitializeComponent();
      MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
      double screenWidth = SystemParameters.PrimaryScreenWidth;
      double screenHeight = SystemParameters.PrimaryScreenHeight;
      viewModel.Start(random.Next(5, 10));
      timer = new Timer(Add, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
    }
    public void Add(object? o) {
      this.Dispatcher.Invoke(() =>
      {
        MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
        viewModel.Start(1);
      });
    }
    /// <summary>
    /// Raises the <seealso cref="System.Windows.Window.Closed"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnClosed(EventArgs e)
    {
      if (DataContext is MainWindowViewModel viewModel)
        viewModel.Dispose();
        timer.Dispose();
      base.OnClosed(e);
    }
    private void Button_Click(object sender, RoutedEventArgs e) {
      MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
      int.TryParse(BallsQuantity.Text, out int quantity);
      if (quantity <= 0) return;
      viewModel.Start(quantity);
    }
  }
}