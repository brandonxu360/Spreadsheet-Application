// <auto-generated />
#pragma warning disable CS8669 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context. Auto-generated code requires an explicit '#nullable' directive in source.

using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Spreadsheet_Brandon_Xu.ViewModels;

namespace Spreadsheet_Brandon_Xu;

/// <inheritdoc />
public class ViewLocator : IDataTemplate
{
    /// <summary>Creates the control.</summary>
    /// <returns>The created control.</returns>
    public Control? Build(object? data)
    {
        if (data is null)
            return null;
        
        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data;
            return control;
        }
        
        return new TextBlock { Text = "Not Found: " + name };
    }

    /// <summary>
    /// Checks to see if this data template matches the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>
    /// True if the data template can build a control for the data, otherwise false.
    /// </returns>
    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
