// Copyright Joseph W Donahue and Sharper Hacks LLC (US-WA)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// SharperHacks is a trademark of Sharper Hacks LLC (US-Wa), and may not be
// applied to distributions of derivative works, without the express written
// permission of a registered officer of Sharper Hacks LLC (US-WA).

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable once CheckNamespace

namespace SharperHacks.CoreLibs.Constraints;

/// <summary>
/// The Verify class provides static members that provide always-on run-time verification of expected invariants.
/// </summary>
public static class Verify
{
    // TODO: Add use-counters?

    // These string constants are primarily intended for unit testing purposes.

    /// <summary>
    /// All of the AreEqual methods throw a VerifyException that start with this string.
    /// </summary>
    public const string AreEqualExceptionPrefix = "Verify failed, value1 is not equal to value2 ";

    /// <summary>
    /// All of the AreNotEqual methods throw a VerifyException that start with this string.
    /// </summary>
    public const string AreNotEqualExceptionPrefix = "Verify failed, value1 is equal to value2 ";

    /// <summary>
    /// All of the *NotNull methods throw a VerifyException that starts with this string.
    /// </summary>
    public const string IsNotNullExceptionPrefix = "Verify failed, value is NULL";

    /// <summary>
    /// All of the *Empty(string) methods throw a VerifyException that starts with this string.
    /// </summary>
    public const string StringIsEmptyExceptionPrefix = "Verify failed, string length is zero";

    /// <summary>
    /// All of the IsFalse methods throw a VerifyException that starts with this string.
    /// </summary>
    public const string ExpressionNotFalseExceptionPrefix = "Verify failed, expression result not false";

    /// <summary>
    /// All of the IsTrue methods throw a VerifyException that starts with this string.
    /// </summary>
    public const string ExpressionNotTrueExceptionPrefix = "Verify failed, expression result not true";

    /// <summary>
    /// The FileExists methods throw VerifyException that contains this string.
    /// </summary>
    public const string FileNotFoundExceptionPrefix = "Verify failed, file not found";

    /// <summary>
    /// The <see cref="IsGreaterThan{T}"/> method throws VerifyException that contains this string.
    /// </summary>
    public const string NotGreaterThanSegment = "not greater than";

    /// <summary>
    /// The <see cref="IsGreaterThanOrEqual{T}"/> mehtod throws VerifyException that contains this string.
    /// </summary>
    public const string NotGreaterThanOrEqualSegment = "not greater than or equal";

    /// <summary>
    /// The <see cref="IsLessThan{T}"/> method throws VerifyException that contains this string.
    /// </summary>
    public const string NotLessThanSegment = "not less than";

    /// <summary>
    /// The <see cref="IsLessThan{T}"/> method throws VerifyException that contains this string.
    /// </summary>
    public const string NotLessThanOrEqualSegment = "not less than or equal";

    /// <summary>
    /// Verify that value1 and value2 are equal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value1">It's okay to use the bang post-fix for null: v!</param>
    /// <param name="value2">It's okay to use the bang post-fix for null: v!</param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Optional. CallerMemberName attribute will fill this in.</param>
    /// <param name="fileName">Optional. CallerFilePath attribute will fill this in.</param>
    /// <param name="lineNumber">Optional. CallerLineNumber attribute will fill this in.</param>
    /// <exception cref="VerifyException"></exception>
    /// <remarks>
    /// Note that the VerifyException will be prefixed with <see cref="AreEqualExceptionPrefix"/>.
    /// </remarks>
    public static void AreEqual<T>(
        in T? value1,
        in T? value2,
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        )
    {
        if (value1 is null && value2 is null) return;

        if ( value1 is null || !value1.Equals(value2) )
        {
            throw new VerifyException(
                BuildExceptionMessage( 
                        BuildAreEqualExceptionMessagePrefix(value1, value2), 
                        comment, 
                        memberName, 
                        fileName, 
                        lineNumber));
        }
    }

    /// <summary>
    /// Verify that value1 and value2 are not equal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Optional. CallerMemberName attribute will fill this in.</param>
    /// <param name="fileName">Optional. CallerFilePath attribute will fill this in.</param>
    /// <param name="lineNumber">Optional. CallerLineNumber attribute will fill this in.</param>
    /// <exception cref="VerifyException"></exception>
    /// <remarks>
    /// Note that the VerifyException will be prefixed with <see cref="AreNotEqualExceptionPrefix"/>.
    /// </remarks>
    public static void AreNotEqual<T>(
        in T? value1,
        in T? value2,
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        )
    {
        var v1IsNull = value1 is null;
        var v2IsNull = value2 is null;
        var bothAreNull = v1IsNull && v2IsNull;

        // They can't be equal if only one of them is null.
        if ((v1IsNull && !v2IsNull) || (v2IsNull && !v1IsNull)) return;

        var throwException = bothAreNull;

        if (value1 is not null && !bothAreNull)
        {
            throwException |= value1.Equals(value2);
        } // else, already covered in the previous if...return.
        
        if (throwException)
        {
            throw new VerifyException(
                    BuildExceptionMessage(
                        AreNotEqualExceptionPrefix,
                        comment,
                        memberName,
                        fileName,
                        lineNumber
                        ));
        }

        return;
    }

    /// <summary>
    /// Verify that the array and its members are not null.
    /// </summary>
    /// <param name="array">Array to check.</param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException">
    /// Thrown when <paramref name="array"/> or any of the array elements are null.
    /// </exception>
    /// <exception cref="VerifyException"></exception>
    /// <remarks>
    /// Note that the VerifyException will be prefixed with <see cref="IsNotNullExceptionPrefix"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void AreNotNull(
        [NotNull] in object?[]? array,
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        )
    {
        IsNotNull(array, comment, memberName, fileName, lineNumber);

        foreach (var item in array)
        {
            IsNotNull(item, comment, memberName, fileName, lineNumber);
        }
    }

    /// <summary>
    /// Verify the array of strings is not null and its members are not null or empty.
    /// </summary>
    /// <param name="array">Array to check.</param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException">
    /// Thrown when <paramref name="array"/> or if any of the <paramref name="array"/> elements are null or empty.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void AreNotNullOrEmpty([NotNull] in string?[]? array,
                                         in string? comment = null,
                                         [CallerMemberName] in string memberName = "",
                                         [CallerFilePath] in string fileName = "",
                                         [CallerLineNumber] in int lineNumber = 0
        ) 
    {
        IsNotNull(array, comment, memberName, fileName, lineNumber);

        // Arrays must have at least one element, so we don't check array.Length here.

        foreach ( var item in array! )
        {
            IsNotNullOrEmpty(item, comment, memberName, fileName, lineNumber);
        }
    }

    /// <summary>
    /// Verify that an object reference is not null.
    /// </summary>
    /// <param name="value">String to check.</param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException">
    /// Thrown when <paramref name="value"/> is null.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void IsNotNull([NotNull] in object? value,
                                 in string? comment = null,
                                 [CallerMemberName] in string memberName = "",
                                 [CallerFilePath] in string fileName = "",
                                 [CallerLineNumber] in int lineNumber = 0
        )
    {
        if (value is null)
        {
            _ = ThrowValueIsNullException<object>(
                    comment,
                    memberName,
                    fileName,
                    lineNumber
                    );
        } // Code coverage sometimes erroneously marks this uncovered.
    }

    /// <summary>
    /// Verify <paramref name="value"/> string is not null or empty.
    /// </summary>
    /// <param name="value">String to check.</param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException">
    /// Thrown when <paramref name="value"/> is null or empty.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void IsNotNullOrEmpty([NotNull] in string? value,
                                        in string? comment = null,
                                        [CallerMemberName] in string memberName = "",
                                        [CallerFilePath] in string fileName = "",
                                        [CallerLineNumber] in int lineNumber = 0
        ) 
    {
        IsNotNull(value, comment, memberName, fileName, lineNumber);

        if ( value!.Length == 0 )
        {
            throw new VerifyException(
                BuildExceptionMessage(
                    StringIsEmptyExceptionPrefix,
                    comment,
                    memberName,
                    fileName,
                    lineNumber
                    ));
        }
    }

    /// <summary>
    /// Verify <paramref name="left"/> is greater than <paramref name="right"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left">Left side of comparison.</param>
    /// <param name="right">Right side of comparion.</param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException"></exception>
    public static void IsGreaterThan<T>(
        in T left, 
        in T right, 
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        ) where T : IComparable<T>
    {
        if (left.CompareTo(right) < 0)
        {
            throw new VerifyException(
                BuildExceptionMessage(
                    $"{left} {NotGreaterThanSegment} {right}",
                    comment,
                    memberName,
                    fileName,
                    lineNumber
                    ));
        }
    }

    /// <summary>
    /// Verify <paramref name="left"/> is greater than or equal <paramref name="right"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left">Left side of comparison.</param>
    /// <param name="right">Right side of comparion.</param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException"></exception>
    public static void IsGreaterThanOrEqual<T>(
        in T left,
        in T right,
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        ) where T : IComparable<T>
    {
        if (left.CompareTo(right) < 0)
        {
            throw new VerifyException(
                BuildExceptionMessage(
                    $"{left} {NotGreaterThanOrEqualSegment} {right}",
                    comment,
                    memberName,
                    fileName,
                    lineNumber
                    ));
        }
    }

    /// <summary>
    /// Verify <paramref name="left"/> is less than <paramref name="right"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="comment"></param>
    /// <param name="memberName"></param>
    /// <param name="fileName"></param>
    /// <param name="lineNumber"></param>
    /// <exception cref="VerifyException"></exception>
    public static void IsLessThan<T>(
            in T left,
            in T right,
            in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
            ) where T : IComparable<T>
        {
            if (left.CompareTo(right) > 0)
            {
                throw new VerifyException(
                    BuildExceptionMessage(
                        $"{left} {NotLessThanSegment} {right}",
                        comment,
                        memberName,
                        fileName,
                        lineNumber
                        ));
            }
        }

    /// <summary>
    /// Verify <paramref name="left"/> is less than or Equal to <paramref name="right"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="comment"></param>
    /// <param name="memberName"></param>
    /// <param name="fileName"></param>
    /// <param name="lineNumber"></param>
    /// <exception cref="VerifyException"></exception>
    public static void IsLessThanOrEqual<T>(
        in T left,
        in T right,
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        ) where T : IComparable<T>
    {
        if (left.CompareTo(right) > 0)
        {
            throw new VerifyException(
                BuildExceptionMessage(
                    $"{left} {NotLessThanOrEqualSegment} {right}",
                    comment,
                    memberName,
                    fileName,
                    lineNumber
                    ));
        }
    }

    /// <summary>
    /// Verify that <paramref name="expressionResult"/> is true.
    /// </summary>
    /// <param name="expressionResult"></param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException">
    /// Thrown when <paramref name="expressionResult"/> is not true.
    /// </exception>
    public static void IsTrue(
        [DoesNotReturnIf(false)] bool expressionResult,
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        )
    {
        if ( !expressionResult )
        {
            throw new VerifyException(
                BuildExceptionMessage(
                    ExpressionNotTrueExceptionPrefix,
                    comment,
                    memberName,
                    fileName,
                    lineNumber));
        }
    }

    /// <summary>
    /// Verify that <paramref name="expressionResult"/> is false.
    /// </summary>
    /// <param name="expressionResult"></param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException">
    /// Thrown when <paramref name="expressionResult"/> is not false.
    /// </exception>
    public static void IsFalse(
        [DoesNotReturnIf(true)] bool expressionResult,
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        )
    {
        if ( expressionResult )
        {
            throw new VerifyException(
                BuildExceptionMessage(
                    ExpressionNotFalseExceptionPrefix,
                    comment,
                    memberName,
                    fileName,
                    lineNumber
                    ));
        }
    }

    /// <summary>
    /// Throw the value is null exception.
    /// </summary>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <returns>Throws exception, return type is syntactic candy for "??" operator.</returns>
    /// <exception cref="VerifyException"></exception>
    [DoesNotReturn]
    public static T ThrowValueIsNullException<T>(
            in string? comment,
            in string memberName = "",
            in string fileName = "",
            in int lineNumber = 0
            ) =>
            throw new VerifyException(
                    BuildExceptionMessage(
                            IsNotNullExceptionPrefix,
                            comment,
                            memberName,
                            fileName,
                            lineNumber
                            ));

    /// <summary>
    /// Throw VerifyException if file does not exist.
    /// </summary>
    /// <param name="pathFileName"></param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException"></exception>
    public static void FileExists(
        string pathFileName, 
        in string? comment = null,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        )
    {
        if (!File.Exists(pathFileName))
        {
            throw new VerifyException(
                    BuildExceptionMessage(
                            FileNotFoundExceptionPrefix,
                            comment,
                            memberName,
                            fileName,
                            lineNumber
                            ));
        }
    }

    /// <summary>
    /// Throw FileNotFoundException if file does not exist.
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="comment">
    /// Any additional information to tag onto the exception message. May be null or empty.
    /// </param>
    /// <param name="memberName">Automatically filled in by CallerMemberName.</param>
    /// <param name="fileName">Automatically filled in by CallerFilePath.</param>
    /// <param name="lineNumber">Automatically filled in by CallerLineNumber.</param>
    /// <exception cref="VerifyException"></exception>
    public static void FileExists(
        FileInfo fileInfo,
        in string? comment,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "",
        [CallerLineNumber] in int lineNumber = 0
        )
    {
        if (!fileInfo.Exists)
        {
            throw new VerifyException(
                    BuildExceptionMessage(
                            FileNotFoundExceptionPrefix,
                            comment,
                            memberName,
                            fileName,
                            lineNumber
                            ));
        }
    }

    #region Private

    private static string BuildExceptionMessage(
        in string prefix,
        in string? postfix,
        in string memberName,
        in string fileName,
        in int lineNumber) => $"{prefix} {postfix ?? string.Empty} @ {memberName} [{fileName}({lineNumber})]";

    private static string BuildAreEqualExceptionMessagePrefix<T>(T value1, T value2) =>
            BuildComparisonExceptionMessagePrefix(value1, " != ", value2);

    private static string BuildComparisonExceptionMessagePrefix<T>(
        in T value1, 
        in string opStr, 
        in T value2) =>
            AreEqualExceptionPrefix
          + '('
          + BuildExceptionMessageValueString(value1)
          + opStr
          + BuildExceptionMessageValueString(value2)
          + ").";

    private static string BuildExceptionMessageValueString<T>(in T value)
    {
        const string nul = "null";

        string result = value is null ? nul : value.ToString() ?? nul;

        if (result == string.Empty)
        {
            result = "string.Empty";
        }

        return result;
    }
    #endregion Private
}
