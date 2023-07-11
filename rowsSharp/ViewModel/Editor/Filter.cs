﻿using ObservableTable.Core;
using rowsSharp.Model;
using rowsSharp.View;
using System.ComponentModel;
using System.Windows.Controls;

namespace rowsSharp.ViewModel.Editor;

public class Filter : NotifyPropertyChanged
{
    private readonly RootVM rootVM;
    private Preferences Preferences => rootVM.Preferences;
    private ObservableTable<string> Table => rootVM.Table;

    private readonly Domain.Filter filter;

    public Filter(RootVM rootViewModel, ICollectionView collectionView)
    {
        rootVM = rootViewModel;
        filter = new(collectionView);
    }

    private string filterText = "";
    public string FilterText
    {
        get => filterText;
        set => SetField(ref filterText, value);
    }

    public DelegateCommand InvokeFilter => new(() =>
    {
        filter.UseRegex = Preferences.Filter.IsRegexEnabled;
        filter.Headers = Table.Headers;
        filter.FilterText = FilterText;
        rootVM.EditorVM!.CollectionView = filter.Invoke();
    });

    private bool? isFilterFocused;
    public bool? IsFilterFocused
    {
        get => isFilterFocused;
        set => SetField(ref isFilterFocused, value);
    }

    public DelegateCommand FocusFilter => new(
        () =>
        {
            IsFilterFocused = false;
            IsFilterFocused = true;
        }
    );

    public DelegateCommand OpenPreferences => new(() =>
    {
        rootVM.SettingsVM = new(rootVM, rootVM.CurrentUserControl);
        rootVM.CurrentUserControl = new Settings();
    });

    public DelegateCommand<DataGrid> ResetSorting => new(
        (datagrid) =>
        {
            rootVM.EditorVM?.CollectionView.SortDescriptions.Clear();
            foreach (var column in datagrid.Columns)
            {
                column.SortDirection = null;
            }
        }
    );
}
