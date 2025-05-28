using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using FinanceApp.UI.ViewModels;

namespace FinanceApp.UI
{
    public partial class SettingsPage : Page
    {
        private readonly MainViewModel _viewModel;

        public SettingsPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json"
            };
            if (dialog.ShowDialog() == true)
            {
                _viewModel.SaveToJson(dialog.FileName);
            }
        }

        private void ImportJson_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json"
            };
            if (dialog.ShowDialog() == true)
            {
                _viewModel.LoadFromJson(dialog.FileName);
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}

 
