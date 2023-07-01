using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GKYU.CoreLibrary.Expressions
{
    public class ExpressionTreeHelpers
    {
        // Visual Basic encodes string comparisons as a method call to 
        // Microsoft.VisualBasic.CompilerServices.Operators.CompareString. 
        // This method will convert the method call into a binary operation instead. 
        // Note that this makes the string comparison case sensitive. 
        public static BinaryExpression ConvertVBStringCompare(BinaryExpression exp)
        {
            MethodCallExpression compareStringCall = null;
            if (exp.Left.NodeType == ExpressionType.Call)
            {
                compareStringCall = (MethodCallExpression)exp.Left;
                if ((compareStringCall.Method.DeclaringType.FullName == "Microsoft.VisualBasic.CompilerServices.Operators") && (compareStringCall.Method.Name == "CompareString"))
                {

                    var arg1 = compareStringCall.Arguments[0];
                    var arg2 = compareStringCall.Arguments[1];

                    switch (exp.NodeType)
                    {
                        case ExpressionType.LessThan:
                            return Expression.LessThan(arg1, arg2);
                        case ExpressionType.LessThanOrEqual:
                            return Expression.GreaterThan(arg1, arg2);
                        case ExpressionType.GreaterThan:
                            return Expression.GreaterThan(arg1, arg2);
                        case ExpressionType.GreaterThanOrEqual:
                            return Expression.GreaterThanOrEqual(arg1, arg2);
                        default:
                            return Expression.Equal(arg1, arg2);
                    }
                }
            }
            return exp;
        }

        public static bool IsMemberEqualsValueExpression(Expression exp, Type declaringType, string memberName)
        {
            if (exp.NodeType != ExpressionType.Equal)
                return false;

            BinaryExpression be = (BinaryExpression)exp;

            // Assert. 
            if ((IsSpecificMemberExpression(be.Left, declaringType, memberName)) && (IsSpecificMemberExpression(be.Right, declaringType, memberName)))
                throw new Exception("Cannot have 'member' = 'member' in an expression!");
 
            return IsSpecificMemberExpression(be.Left, declaringType, memberName) || IsSpecificMemberExpression(be.Right, declaringType, memberName);
        }


        public static Boolean IsSpecificMemberExpression(Expression exp, Type declaringType, String memberName)
        {
            return ((exp.Type ==  typeof(MemberExpression)) && (((MemberExpression)exp).Member.DeclaringType == declaringType) && (((MemberExpression)exp).Member.Name == memberName));
        }


        public static String GetValueFromEqualsExpression(BinaryExpression be, Type memberDeclaringType, String memberName)
        {
            if (be.NodeType != ExpressionType.Equal)
                throw new Exception("There is a bug in this program.");


            if (be.Left.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression mEx = (MemberExpression)be.Left;

                if ((mEx.Member.DeclaringType == memberDeclaringType) && (mEx.Member.Name == memberName))
                    return GetValueFromExpression(be.Right);
            }
            else if (be.Right.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression mEx = (MemberExpression)be.Right;

                if ((mEx.Member.DeclaringType == memberDeclaringType) && (mEx.Member.Name == memberName))
                    return GetValueFromExpression(be.Left);
            }

            // We should have returned by now. 
            throw new Exception("There is a bug in this program.");
        }

        public static String GetValueFromExpression(Expression expr)
        {
            if (expr.NodeType == ExpressionType.Constant)
                return ((ConstantExpression)expr).Value.ToString();
            else
            {
                string s = "The expression type {0} is not supported to obtain a value.";
                throw new InvalidQueryException(String.Format(s, expr.NodeType));
            }
        }
    }
}
