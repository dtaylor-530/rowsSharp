﻿using rowsSharp.Model;
using rowsSharp.ViewModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace rowsSharp.Domain;

internal class Filter
{
    private readonly Status status;
    private readonly DataStore.Config config;
    private readonly DataStore.Csv csv;
    private readonly RecordsView recordsView;

    internal Filter(Status inputStatus, DataStore.Config inputConfig, DataStore.Csv inputCsv, RecordsView inputRecordsView)
    {
        status = inputStatus;
        config = inputConfig;
        csv = inputCsv;
        recordsView = inputRecordsView;
    }

    internal void FocusFilter()
    {
        // Force update ViewModel by refocusing
        status.IsFilterFocused = false;
        status.IsFilterFocused = true;
    }

    private List<KeyValuePair<string, string>> criteria = new();

    private List<KeyValuePair<string, string>> ParseInput()
    {
        List<KeyValuePair<string, string>> output = new();

        string[] splittedFilterText = Regex.Split(status.FilterText, "\\s+(?=(?:\"[^\"]*\"|[^\"])*$)");
        foreach (string criterion in splittedFilterText)
        {
            string[] keyvalue = Regex.Split(criterion, ":(?=(?:\"[^\"]*\"|[^\"])*$)");
            string header = keyvalue[0].Trim().Trim('"');
            string value = string.Empty;

            // Handle Header:Value
            if (keyvalue.Length == 2)
            {
                if (!csv.Headers.Contains(header)) { throw new InvalidFilterCriteriaException($"Invalid column {header}"); }

                value = keyvalue[1].Trim().Trim('"');

                Dictionary<string, string> thisAlias = config.UseInputAlias ? config.Style.Alias.GetValueOrDefault(header) ?? new() : new();
                foreach (KeyValuePair<string, string> aliasKeyValue in thisAlias)
                {
                    value = value.Replace(aliasKeyValue.Value, aliasKeyValue.Key);
                }

                // Convert user-provided header to internal ColumnX notation
                header = csv.Headers.IndexOf(header).ToString();
            }

            // Validate regular expression
            if (config.UseRegexFilter)
            {
                string regexToTest = value == string.Empty ? header : value;
                try
                {
                    Regex.IsMatch("", regexToTest);
                }
                catch
                {
                    throw new InvalidFilterCriteriaException($"Invalid regex {regexToTest}");
                }
            }
            output.Add(new(header, value));
        }
        return output;
    }

    private ICollectionView OutputAlias()
    {
        List<Record> tempRecords = new();
        foreach (Record record in recordsView.CollectionView)
        {
            Record thisRecord = record.DeepCopy(csv.Headers.Count);
            for (int i = 0; i < csv.Headers.Count; i++)
            {
                Dictionary<string, string> thisAlias = config.Style.Alias.GetValueOrDefault(csv.Headers[i]) ?? new();
                foreach (KeyValuePair<string, string> aliasKeyValue in thisAlias)
                {
                    thisRecord.SetField(
                        i,
                        thisRecord.GetField(i).Replace(aliasKeyValue.Key, aliasKeyValue.Value)
                    );
                }
            }
            tempRecords.Add(thisRecord);
        }
        return CollectionViewSource.GetDefaultView(tempRecords);
    }

    private bool RecordsViewFilter(object obj)
    {
        var row = (Record)obj;
        foreach (KeyValuePair<string, string> criterion in criteria)
        {
            string input = string.IsNullOrWhiteSpace(criterion.Value)
                ? row.ConcatenateFields(csv.Headers.Count)
                : row.GetField(int.Parse(criterion.Key));

            string pattern = string.IsNullOrWhiteSpace(criterion.Value)
                ? criterion.Key
                : criterion.Value;

            input = input.ToLower();
            pattern = pattern.ToLower();

            if (
                (config.UseRegexFilter && Regex.IsMatch(input, pattern)) ||
                (!config.UseRegexFilter && input.Contains(pattern))
            ) { continue; }
            return false;
        }
        return true;
    }

    internal void DoFilter()
    {
        App.Logger.Info("Filtering CSV, ({filter}, IOAlias: {IAlias}, {OAlias})",
            status.FilterText,
            config.UseInputAlias,
            config.UseOutputAlias
        );

        // Parse input
        try
        {
            criteria = ParseInput();
        }
        catch (InvalidFilterCriteriaException ex)
        {
            App.Logger.Warn(ex.Message);
            recordsView.CollectionView.Filter = (object obj) => false;
            return;
        }

        // Filtering
        recordsView.CollectionView = CollectionViewSource.GetDefaultView(csv.Records);
        recordsView.CollectionView.Filter = RecordsViewFilter;

        // Output alias
        if (config.UseOutputAlias)
        {
            recordsView.CollectionView = OutputAlias();
        }

        criteria = new();
        status.PreviewBitmap = new();
        status.SelectedIndex = -1;
        App.Logger.Debug("Filtering CSV completed");
    }
}
