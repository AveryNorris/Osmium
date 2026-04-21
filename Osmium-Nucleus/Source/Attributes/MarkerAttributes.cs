using System;


namespace OsmiumNucleus;


/// <summary>
/// Provides many handy cosmetic attributes for methods to specify use cases or provide information.
/// </summary>
/// <author> Avery Norris </author>
public static class MarkerAttributes
{
 
    
    
    /// <summary> Shows that the given object is unsafe (ex. it doesn't check for null values and such, or it doesn't have guardrails based on cases).
    /// This is just for internal/private methods to remind myself how to call it :) The reasoning is case by case, but most of the time,
    /// it is because all the exposing public methods already check, and double checks would only slow me down </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class UnsafeInternal : Attribute { }



    /// <summary> Shows that the given object is calculated every time it is called! Good to know for performance heavy systems. </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CalculatedProperty : Attribute { }



    /// <summary> Just a way to write how expensive a calculated property or method can be. </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class Expense(Expense.ExpenseLevel __expense) : Attribute
    {
        public enum ExpenseLevel {
            None,
            VeryLow,
            Low,
            Medium,
            High,
            VeryHigh,
        }


        public ExpenseLevel expense = __expense;
    }
    
    
    
    /// <summary> Just a way to write the time complexity of a calculated property or method. </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class Complexity(Complexity.TimeComplexity __complexity) : Attribute
    {
        public enum TimeComplexity
        {
            O1,
            OLogN,
            ON,
            ONLogN,
            ON2,
            ON3,
            ONK,
        }
        
        public TimeComplexity complexity = __complexity;
    }



    /// <summary> Shows that the given method does not actually belong to the object, but instead just calls a lambda to another method. </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class MethodPointer : Attribute;



    /// <summary> Shows that the given variable does not actually belong to the object, but instead just calls a lambda to another method. </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class VariablePointer : Attribute;



    /// <summary> Shows that the given variable or function is required for the editor pipeline </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class EditorPipeline : Attribute;



}