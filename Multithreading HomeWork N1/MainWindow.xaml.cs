using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Multithreading_HomeWork_N1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int PrimeFrom;
        private int PrimeTo;
        private Thread PrimeThread;
        private Thread FibonacciThread;
        private bool PrimeToPause = false;
        private bool FibonacciToPause = false;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonPrimeOnClick(object sender, RoutedEventArgs e)
        {
            int buffer;
            PrimeFrom = int.TryParse(FromTextBox.Text, out buffer) ? buffer : 2;
            PrimeTo = int.TryParse(ToTextBox.Text, out buffer) ? buffer : -1;

            PrimeThread = new Thread(CountPrime) {IsBackground = true};
            PrimeThread.Start();

            ButtonPrime.IsEnabled = false;
            FromTextBox.IsEnabled = false;
            ToTextBox.IsEnabled = false;

            PrimeResume.IsEnabled = false;
            PrimeStop.IsEnabled = true;
            PrimePause.IsEnabled = true;
        }

        private void CountPrime()
        {
            for (int i = PrimeFrom; i < PrimeTo || PrimeTo == -1; i++)
            {
                if (isPrime(i))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        PrimeTextBox.AppendText($"\n{i}");
                        PrimeTextBox.ScrollToEnd();
                    });
                    Thread.Sleep(10);
                    if (PrimeToPause)
                    {
                        PrimeToPause = false;
                        try
                        {
                            Thread.Sleep(Timeout.Infinite);
                        }
                        catch (ThreadInterruptedException)
                        {
                        }
                    }
                }
            }
        }

        private bool isPrime(int num)
        {
            if (num % 2 == 0) return false;
            
            for (int i = 3; i < num; i+= 2)
            {
                if (num % i == 0) return false;
            }
            
            return true;
        }

        private void ButtonFibonacciOnClick(object sender, RoutedEventArgs e)
        {
            FibonacciThread = new Thread(CountFibonacci) { IsBackground = true };
            FibonacciThread.Start();

            FibonacciButton.IsEnabled = false;
            FibonacciResume.IsEnabled = false;
            FibonacciStop.IsEnabled = true;
            FibonacciPause.IsEnabled = true;
        }

        private void CountFibonacci()
        {
            long a = 1, b = 0, c = 0;

            while (true)
            {
                c = a + b;
                b = a;
                a = c;

                if (c < 0) Dispatcher.Invoke(() => FibonacciStop_OnClick(null,null));

                this.Dispatcher.Invoke(() =>
                {
                    FibonacciTextBox.AppendText($"\n{c}");
                    FibonacciTextBox.ScrollToEnd();
                });
                Thread.Sleep(100);
                
                if (FibonacciToPause)
                {
                    FibonacciToPause = false;
                    try
                    {
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (ThreadInterruptedException)
                    {
                    }
                }
            }
        }

        private void PrimePause_OnClick(object sender, RoutedEventArgs e)
        {
            PrimeResume.IsEnabled = true;
            PrimePause.IsEnabled = false;
            PrimeStop.IsEnabled = false;
            PrimeToPause = true;
        }

        private void PrimeStop_OnClick(object sender, RoutedEventArgs e)
        {
            PrimeResume.IsEnabled = false;
            PrimePause.IsEnabled = false;
            PrimeStop.IsEnabled = false;
            ButtonPrime.IsEnabled = true;
            FromTextBox.IsEnabled = true;
            ToTextBox.IsEnabled = true;
            PrimeThread.Abort();
        }

        private void PrimeResume_OnClick(object sender, RoutedEventArgs e)
        {
            PrimeResume.IsEnabled = false;
            PrimePause.IsEnabled = true;
            PrimeStop.IsEnabled = true;
            PrimeThread.Interrupt();
        }

        
        private void FibonacciPause_OnClick(object sender, RoutedEventArgs e)
        {
            FibonacciResume.IsEnabled = true;
            FibonacciPause.IsEnabled = false;
            FibonacciStop.IsEnabled = false;
            FibonacciToPause = true;
        }

        private void FibonacciStop_OnClick(object sender, RoutedEventArgs e)
        {
            FibonacciResume.IsEnabled = false;
            FibonacciPause.IsEnabled = false;
            FibonacciStop.IsEnabled = false;
            FibonacciButton.IsEnabled = true;
            FibonacciThread.Abort();
        }

        private void FibonacciResume_OnClick(object sender, RoutedEventArgs e)
        {
            FibonacciResume.IsEnabled = false;
            FibonacciPause.IsEnabled = true;
            FibonacciStop.IsEnabled = true;
            FibonacciThread.Interrupt();
        }
    }
}
