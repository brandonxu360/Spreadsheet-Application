// <copyright file="ValueNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTree;

/// <summary>
/// Node class to represent a constant value.
/// </summary>
public class ValueNode : ExpTreeNode
{
    /// <summary>
    /// Represents the value of the ValueNode.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private readonly double value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueNode"/> class.
    /// </summary>
    /// <param name="value">The value of the node.</param>
    public ValueNode(double value)
    {
        this.value = value;
    }

    /// <summary>
    /// Returns the value field - ValueNodes evaluate to themselves.
    /// </summary>
    /// <returns>The double value of the ValueNode.</returns>
    /// <exception cref="NotImplementedException">The method is not implemented yet.</exception>
    public override double Evaluate()
    {
        return this.value;
    }
}