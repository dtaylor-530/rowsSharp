﻿using rowsSharp.Model;
using rowsSharp.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace rowsSharp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RowsVM viewModel;

        public MainWindow()
        {
            viewModel = new RowsVM();
            DataContext = viewModel;
            InitializeComponent();
            viewModel.Logger.Info("Okay, it's happening! Everybody stay calm!");
        }

        // Sorry, no MVVM
        private void Grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (((DataGrid)sender).Columns.Count >= viewModel.Csv.Headers.Count)
            {
                e.Cancel = true;
                return;
            }

            string internalColumnName = e.Column.Header.ToString()!;
            int columnIndex = ((DataGrid)sender).Columns.Count;
            string columnName = viewModel.Csv.Headers[columnIndex];
            e.Column.Header = columnName;
            e.Column.CellStyle = new();

            // Column width
            if (viewModel.Config.Style.Width.ContainsKey(columnName))
            {
                e.Column.Width = viewModel.Config.Style.Width[columnName];
            }

            // Conditional formatting
            if (viewModel.Config.Style.Color.ContainsKey(columnName))
            {
                foreach (KeyValuePair<string, string> colorPair in viewModel.Config.Style.Color[columnName])
                {
                    DataTrigger trigger = new()
                    {
                        Binding = new Binding(internalColumnName),
                        Value = colorPair.Key,
                    };

                    trigger.Setters.Add(
                        new Setter()
                        {
                            Property = BackgroundProperty,
                            Value = new BrushConverter().ConvertFromString(colorPair.Value)
                        }
                    );
                    e.Column.CellStyle.Triggers.Add(trigger);
                }
            }
        }
    }
}
