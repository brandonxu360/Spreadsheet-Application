using System.Diagnostics;

namespace SpreadsheetEngine;

/// <summary>
/// Node class to represent and apply the subtraction operator.
/// </summary>
public class SubtractionNode : OperatorNode
{
    /// <summary>
    /// Character symbol to identify/represent subtraction.
    /// </summary>
    public static char OperatorSymbol = '-';

    /// <summary>
    /// Returns the difference of the left and right children node values.
    /// </summary>
    /// <returns>The double difference of the left and right children node values.</returns>
    public override double Evaluate()
    {
        Debug.Assert(this.LeftChild != null, nameof(this.LeftChild) + " != null");
        Debug.Assert(this.RightChild != null, nameof(this.RightChild) + " != null");

        var leftValue = this.LeftChild.Evaluate();
        var rightValue = this.RightChild.Evaluate();

        return leftValue - rightValue;
    }
}