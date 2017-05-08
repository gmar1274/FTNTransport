using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MyStates;
using System.Windows.Media;
using System.Windows.Threading;
using System.Globalization;
using FTNTransport.Windows;
using FTNTransport.Interfaces;
using System.Threading.Tasks;

namespace FTNTransport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            ViewModels.VMMainWindow vm_window = new ViewModels.VMMainWindow(this);
        }

    }
}
