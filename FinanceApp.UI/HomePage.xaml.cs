using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using FinanceApp.UI.ViewModels;

namespace FinanceApp.UI
{
    public partial class HomePage : Page
    {
        private readonly MainViewModel _viewModel;
        private readonly Frame _mainFrame;

        public HomePage(MainViewModel viewModel, Frame mainFrame)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _mainFrame = mainFrame;
            this.DataContext = _viewModel;
        }

        private void AddTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new AddTransactionPage(_viewModel, _mainFrame));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new SettingsPage(_viewModel));
        }
    }
}