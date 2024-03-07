// <copyright file="ExpressionTreeTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace SpreadsheetEngineTests;

using System.Reflection;
using SpreadsheetEngine;

/// <summary>
/// Class to unit test the ExpressionTree functionality.
/// </summary>
[TestFixture]
internal class ExpressionTreeTests
{
    /// <summary>
    /// Tests the evaluate method of ValueNode in a normal case.
    /// </summary>
    [Test]
    public void ValueNodeEvaluateTest()
    {
        // Arrange
        const double value = 10.5;
        var valueNode = new ValueNode(value);

        // Act
        var result = valueNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests the evaluate method of VariableNode in a normal case.
    /// </summary>
    [Test]
    public void VariableNodeEvaluateTest()
    {
        // Arrange
        const string name = "x";
        const double expectedValue = 20.7;
        var varDict = new Dictionary<string, double>
        {
            { "x", expectedValue },
        };
        var variableNode = new VariableNode(varDict, name);

        // Act
        var result = variableNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    /// <summary>
    /// Tests that the evaluate method of VariableNode returns the default value of 0 when a value is not found
    /// in the dictionary.
    ///
    /// * If variables are not set by the user, they can be default to 0 for this HW. In later HWs, once we
    ///   learn how to deal with exceptions, we will change this.
    /// </summary>
    [Test]
    public void VariableNodeEvaluateReturnsDefaultWhenVariableNotInDictionary()
    {
        // Arrange
        const string name = "y"; // Variable name not present in dictionary
        var varDict = new Dictionary<string, double>
        {
            { "x", 20.7 },
        };
        var variableNode = new VariableNode(varDict, name);

        // Act
        var result = variableNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests the evaluate method of AdditionNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double sum of the right and left values.</returns>
    [Test]
    [TestCase(5, 3, ExpectedResult = 8)] // Two positive inputs
    [TestCase(0, 0, ExpectedResult = 0)] // Both zero inputs
    [TestCase(-3, -4, ExpectedResult = -7)] // Two negative inputs
    [TestCase(5.5, 2.5, ExpectedResult = 8)] // Fractional inputs
    public double AdditionNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var additionNode = new AdditionNode
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = additionNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the evaluate method of SubtractionNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double difference of the left and right values.</returns>
    [Test]
    [TestCase(5, 3, ExpectedResult = 2)] // Two positive inputs
    [TestCase(0, 0, ExpectedResult = 0)] // Both zero inputs
    [TestCase(-3, -4, ExpectedResult = 1)] // Two negative inputs
    [TestCase(5.5, 2.5, ExpectedResult = 3)] // Fractional inputs
    public double SubtractionNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var subtractionNode = new SubtractionNode()
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = subtractionNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the evaluate method of MultiplicationNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double result of multiplying the left and right values.</returns>
    [Test]
    [TestCase(2, 3, ExpectedResult = 6)] // Two positive inputs
    [TestCase(0, 0, ExpectedResult = 0)] // Both zero inputs
    [TestCase(-3, -4, ExpectedResult = 12)] // Two negative inputs
    [TestCase(0.5, 4, ExpectedResult = 2)] // Fractional inputs
    public double MultiplicationNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var multiplicationNode = new MultiplicationNode()
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = multiplicationNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the evaluate method of DivisionNode in a normal case.
    /// </summary>
    /// <param name="leftValue">The double value of the left child node.</param>
    /// <param name="rightValue">The double value of the right child node.</param>
    /// <returns>The double result of dividing the left value by the right value.</returns>
    [Test]
    [TestCase(6, 3, ExpectedResult = 2)] // Two positive inputs
    [TestCase(0, 1, ExpectedResult = 0)] // Left input zero
    [TestCase(-12, -4, ExpectedResult = 3)] // Two negative inputs
    [TestCase(4, 0.5, ExpectedResult = 8)] // Fractional inputs
    public double DivisionNodeEvaluateTest(double leftValue, double rightValue)
    {
        // Arrange
        var divisionNode = new DivisionNode()
        {
            LeftChild = new ValueNode(leftValue),
            RightChild = new ValueNode(rightValue),
        };

        // Act
        var result = divisionNode.Evaluate();

        // Assert
        return result;
    }

    /// <summary>
    /// Tests the construction of ExpressionTree in exceptional cases.
    /// </summary>
    /// <param name="expression">The string expression used to build the expression tree.</param>
    [Test]
    [TestCase("+")] // Expression of only a single add operator missing both operands
    [TestCase("2+")] // Expression with an add operator missing the second operand
    public void ExpressionTreeConstructionExceptional(string expression)
    {
        Assert.That(
            () => new ExpressionTree(expression),
            Throws.TypeOf<System.Exception>());
    }

    /// <summary>
    /// Tests the evaluate method of ExpressionTree in normal cases.
    /// </summary>
    /// <param name="expression">The string expression used to build the expression tree.</param>
    /// <returns>The double evaluated value of the expression.</returns>
    [Test]
    [TestCase("3+7", ExpectedResult = 10)] // Expression with a single add operator
    [TestCase("3+7+2+1", ExpectedResult = 13)] // Expression with multiple add operators
    [TestCase("3/7", ExpectedResult = 3.0 / 7.0)] // Expression with a single division operator
    [TestCase("3/7/2/1", ExpectedResult = 3.0 / 7.0 / 2.0 / 1.0)] // Expression with multiple division operators
    public double ExpressionTreeEvaluateTestNormal(string expression)
    {
        var exp = new ExpressionTree(expression);
        return exp.Evaluate();
    }

    /// <summary>
    /// Tests the private tokenize method of ExpressionTree in a normal case with operators and values.
    /// </summary>
    [Test]
    public void TokenizePrivateMethodTestNormal()
    {
        // Arrange
        const string expression = "3+7+1";
        var expectedTokens = new List<string> { "3", "+", "7", "+", "1" };

        var expressionTree = new ExpressionTree(); // Object instance to call the private method
        var tokenizeMethod =
            typeof(ExpressionTree).GetMethod("Tokenize", BindingFlags.NonPublic | BindingFlags.Instance);

        if (tokenizeMethod == null)
        {
            Assert.Fail("Tokenize method not found");
            return;
        }

        // Act
        var result = (List<string>)tokenizeMethod.Invoke(expressionTree, new object[] { expression })!;

        // Assert
        Assert.That(result, Is.EqualTo(expectedTokens));
    }

    /// <summary>
    /// Tests the private tokenize method of ExpressionTree in a normal case with operators, values, and multi character variable names.
    /// </summary>
    [Test]
    public void TokenizePrivateMethodTestNormalWithVariables()
    {
        // Arrange
        const string expression = "3+7+hello+2+b";
        var expectedTokens = new List<string> { "3", "+", "7", "+", "hello", "+", "2", "+", "b" };

        var expressionTree = new ExpressionTree(); // Object instance to call the private method
        var tokenizeMethod =
            typeof(ExpressionTree).GetMethod("Tokenize", BindingFlags.NonPublic | BindingFlags.Instance);

        if (tokenizeMethod == null)
        {
            Assert.Fail("Tokenize method not found");
            return;
        }

        // Act
        var result = (List<string>)tokenizeMethod.Invoke(expressionTree, new object[] { expression })!;

        // Assert
        Assert.That(result, Is.EqualTo(expectedTokens));
    }
}