// <copyright file="Spreadsheet.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

using System.ComponentModel;

/// <summary>
/// The spreadsheet class that will serve as a container for a 2D array of cells. It will also serve
/// as a factory for spreadsheet cells.
/// </summary>
public class Spreadsheet
{
    /// <summary>
    /// The 2D array of cells to represent the cells of the spreadsheet.
    /// </summary>
    // ReSharper disable once InconsistentNaming (conflicts with stylecop)
    private Cell?[,] cells;

    /// <summary>
    /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
    /// </summary>
    /// <param name="rowCount">The number of rows in the spreadsheet.</param>
    /// <param name="columnCount">The number of columns in the spreadsheet.</param>
    public Spreadsheet(int rowCount, int columnCount)
    {
        this.RowCount = rowCount;
        this.ColumnCount = columnCount;

        // Initialize the 2D array of cells according to the provided dimensions
        this.cells = new Cell[rowCount, columnCount];

        // Create a spreadsheet cell and assign it to each position in the cell array
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                this.cells[i, j] = new SpreadsheetCell(i, j);
                if (this.cells[i, j] != null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    this.cells[i, j].PropertyChanged += this.OnCellPropertyChanged;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }
    }

    /// <summary>
    /// Gets the number of columns in the spreadsheet.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// Gets the number of rows in the spreadsheet.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Returns the cell of at the specified column and row index.
    /// </summary>
    /// <param name="rowIndex">The row index of the cell.</param>
    /// <param name="columnIndex">The column index of the cell.</param>
    /// <returns>The Cell object at the column and cell index.</returns>
    public Cell? GetCell(int rowIndex, int columnIndex)
    {
        return this.cells[rowIndex, columnIndex] ?? null;
    }

    /// <summary>
    /// Executed when a cell property is changed, will call the cell Value setter which has
    /// its own implementation of setting the cell value.
    /// </summary>
    /// <param name="sender">Cell that had its property changed.</param>
    /// <param name="e">PropertyChanged event arguments.</param>
    private void OnCellPropertyChanged(object? sender, PropertyChangedEventArgs? e)
    {
        if (sender is Cell cell && e?.PropertyName == nameof(Cell.Text))
        {
            // Expression
            if (cell.Text.StartsWith('='))
            {
                cell.Value = this.EvaluateExpression(cell.Text);
            }

            // Plaintext
            else
            {
                cell.Value = cell.Text;
            }
        }
    }

    private string EvaluateExpression(string expression)
    {
        // Extract the cell reference from the text (e.g., "=A5" -> "A5")
        string cellReference = expression.Substring(1); // Remove the '='

        // Assuming the cell reference is in the format "A5" where 'A' is the column letter and '5' is the row number
        int columnIndex = cellReference[0] - 'A'; // Convert the column letter to a zero-based index
        int rowIndex = int.Parse(cellReference.Substring(1)) - 1; // Parse the row number

        // Check that reference is valid
        if (rowIndex >= 0 || rowIndex < this.RowCount || columnIndex >= 0 || columnIndex < this.ColumnCount)
        {
            // Get the referenced cell from the spreadsheet
            Cell? referencedCell = this.GetCell(rowIndex, columnIndex);

            // If the referenced cell is not null, return the value of this cell to the value of the referenced cell
            if (referencedCell != null)
            {
                return referencedCell.Value;
            }

            // Return the original expression if the referenced cell cannot be found
            return expression;
        }

        // Return original expression if reference is invalid
        return expression;
    }

    private class SpreadsheetCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="rowIndex">The row index of the cell.</param>
        /// <param name="columnIndex">The column index of the cell.</param>
        public SpreadsheetCell(int rowIndex, int columnIndex)
            : base(rowIndex, columnIndex)
        {
        }

        protected override void SetValue(string newValue)
        {
            // Placeholder implementation
            this.value = newValue;
        }
    }
}