using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace BarrazaParticlesWpf
{
    static class Utils { public static Random random; }

    class Dot
    {
        Canvas canvas;

        List<Ellipse> ellipses = new List<Ellipse>();

        double velocityX = 0.0;
        double velocityY = 0.0;

        public Dot(Canvas c)
        {
            canvas = c;

            var e = new Ellipse();
            e.Width = 10;
            e.Height = 10;
            Canvas.SetLeft(e, Utils.random.NextDouble() * 500);
            Canvas.SetTop(e, Utils.random.NextDouble() * 500);
            c.Children.Add(e);

            ellipses.Add(e);
        }

        public void Update()
        {
            velocityX += -1.0 + Utils.random.NextDouble() * 2.0;
            velocityX *= 0.96;

            velocityY += -1.0 + Utils.random.NextDouble() * 2.0;
            velocityY *= 0.96;

            var e = new Ellipse();
            e.Width = 10;
            e.Height = 10;
            Canvas.SetLeft(e, Canvas.GetLeft(ellipses.Last()) + velocityX);
            Canvas.SetTop(e, Canvas.GetTop(ellipses.Last()) + velocityY);
            canvas.Children.Add(e);
            ellipses.Add(e);

            if (Canvas.GetLeft(e) < 0)
            {
                Canvas.SetLeft(e, 0.0);
                velocityX *= -1.0;
            }

            if (Canvas.GetLeft(e) > canvas.ActualWidth)
            {
                Canvas.SetLeft(e, canvas.ActualWidth);
                velocityX *= -1.0;
            }

            if (Canvas.GetTop(e) < 0)
            {
                Canvas.SetTop(e, 0.0);
                velocityY *= -1.0;
            }

            if (Canvas.GetTop(e) > canvas.ActualHeight)
            {
                Canvas.SetTop(e, canvas.ActualHeight);
                velocityY *= -1.0;
            }

            byte alpha = 255;

            for (var i = ellipses.Count - 1; i >= 0; i--)
            {
                var red = (byte)((Canvas.GetLeft(ellipses[i]) / (double)canvas.ActualWidth) * 255);

                var green = (byte)((Canvas.GetTop(ellipses[i]) / (double)canvas.ActualHeight) * 255);

                ellipses[i].Fill = new SolidColorBrush(new Color() { R = red, G = green, B = 255, A = alpha });

                alpha = (byte)(alpha * 0.95);
            }

            if (ellipses.Count > 15)
            {
                canvas.Children.Remove(ellipses[0]);
                ellipses.RemoveAt(0);
            }


        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Utils.random = new Random();

            var dots = new List<Dot>();

            for (var i = 0; i < 100; i++)
                dots.Add(new Dot(canvas));

            var timer = new DispatcherTimer();

            timer.Tick += (s, e) => dots.ForEach(dot => dot.Update());

            timer.Start();
        }
    }
}
