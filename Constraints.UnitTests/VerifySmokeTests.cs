// Copyright and trademark notices at the end of this file.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharperHacks.CoreLibs.IO;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace SharperHacks.CoreLibs.Constraints.UnitTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class VerifySmokeTests
{
    #region private

    private static void RunStandardAssertions(
        in string exceptionMessage,
        in int expectedLineNumber,
        in string[]? otherExpectedMessageParts,
        [CallerMemberName] in string memberName = "",
        [CallerFilePath] in string fileName = "")
    {
        Assert.IsTrue(exceptionMessage.Contains($"@ {memberName}"));
        Assert.IsTrue(exceptionMessage.Contains(fileName));
        Assert.IsTrue(exceptionMessage.Contains($"({expectedLineNumber})"));

        if (otherExpectedMessageParts is null) return;

        foreach(var segment in  otherExpectedMessageParts)
        {
            if (otherExpectedMessageParts.Contains(segment)) continue;
            Assert.Fail($"Missing expected message segment: {segment}");
        }
    }

    private void AreEqualTest<T>(T? left, T? right, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new string[]
        {
            Verify.AreEqualExceptionPrefix,
            guid,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.AreEqual(left, right, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreEqual)} failed to throw for values: {left}, {right}.");
        }
    }

    private void AreNotEqualTest<T>(in T? left, in T? right, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new string[]
        {
            Verify.AreNotEqualExceptionPrefix,
            nameof(Verify.AreNotEqual),
            guid,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.AreNotEqual(left, right, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreNotNull)} failed to throw {nameof(VerifyException)}");
        }
    }

    private void AreNotNullTest(in object?[]? array, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new string[]
        {
            Verify.IsNotNullExceptionPrefix,
            nameof(Verify.AreNotNull),
            nameof(Verify.IsNotNull),
            guid,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.AreNotNull(array, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreNotNull)} failed to throw {nameof(VerifyException)}");
        }
    }

    private void AreNotNullOrEmptyTest(in string?[]? array, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new string[]
        {
                Verify.IsNotNullExceptionPrefix,
                nameof(Verify.IsNotNull),
                nameof(Verify.AreNotNull),
                nameof(Verify.AreNotNullOrEmpty),
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.AreNotNullOrEmpty(array, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreNotNullOrEmpty)} failed to throw {nameof(VerifyException)}");
        }
    }

    private void IsNotNullTest(in object? datum, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new string[]
        {
            Verify.IsNotNullExceptionPrefix,
            nameof(Verify.IsNotNull),
            guid,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsNotNull(datum, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreNotNull)} failed to throw {nameof(VerifyException)}");
        }
    }

    private void IsNotNullOrEmptyTest(in string? datum, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new[]
        {
            Verify.StringIsEmptyExceptionPrefix,
            nameof(Verify.IsNotNull),
            nameof(Verify.IsNotNullOrEmpty),
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsNotNullOrEmpty(datum, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreNotNull)} failed to throw {nameof(VerifyException)}");
        }
    }

    private void IsTrueTest(bool testValue, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new string[]
        {
                Verify.ExpressionNotTrueExceptionPrefix,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsTrue(testValue, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreNotNull)} failed to throw {nameof(VerifyException)}");
        }
    }

    private static void IsGreaterThanTest<T>(T left, T right, bool expectException)
        where T : IComparable<T>, IComparable
    {
        var exceptionCaught = false;
        var lineNumber = -1;
        var expectedSegments = new string[]
        {
                Verify.NotGreaterThanSegment,
        };

        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsGreaterThan<T>(left, right);
        }
        catch (VerifyException ex)
        {
            exceptionCaught = true;
            string opSegment = expectException ? "expected" : "unexpected";
            string prefix = $"Caught {opSegment} exception: ";
            var exceptionMessage = $"{prefix} {ex.ToString()}";
            Console.WriteLine(exceptionMessage);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
        }
        Verify.IsTrue(expectException ? exceptionCaught : !exceptionCaught);
    }

    private static void IsGreaterThanOrEqualTest<T>(T left, T right, bool expectException)
        where T : IComparable<T>
    {
        var exceptionCaught = false;
        var lineNumber = -1;
        var expectedSegments = new string[]
        {
            Verify.NotGreaterThanOrEqualSegment,
        };

        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsGreaterThanOrEqual<T>(left, right);
        }
        catch (VerifyException ex)
        {
            exceptionCaught = true;
            string opSegment = expectException ? "expected" : "unexpected";
            string prefix = $"Caught {opSegment} exception: ";
            var exceptionMessage = $"{prefix} {ex.ToString()}";
            Console.WriteLine(exceptionMessage);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
        }
        Verify.IsTrue(expectException ? exceptionCaught : !exceptionCaught);
    }

    private static void IsLessThanOrEqualTest<T>(T left, T right, bool expectException)
        where T : IComparable<T>
    {
        var exceptionCaught = false;
        var lineNumber = -1;
        var expectedSegments = new string[]
        {
            Verify.NotLessThanOrEqualSegment,
        };

        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsLessThanOrEqual<T>(left, right);
        }
        catch (VerifyException ex)
        {
            exceptionCaught = true;
            string opSegment = expectException ? "expected" : "unexpected";
            string prefix = $"Caught {opSegment} exception: ";
            var exceptionMessage = $"{prefix} {ex.ToString()}";
            Console.WriteLine(exceptionMessage);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
        }
        Verify.IsTrue(expectException ? exceptionCaught : !exceptionCaught);
    }

    private void IsFalseTest(bool testValue, bool expectException)
    {
        var guid = Guid.NewGuid().ToString();
        var expectedSegments = new string[]
        {
            Verify.ExpressionNotFalseExceptionPrefix,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsFalse(testValue, guid);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.AreNotNull)} failed to throw {nameof(VerifyException)}");
        }
    }

    private static void IsLessThanTest<T>(T left, T right, bool expectException)
        where T : IComparable<T>
    {
        var exceptionCaught = false;
        var lineNumber = -1;
        var expectedSegments = new string[]
        {
            Verify.NotLessThanSegment,
        };

        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.IsLessThan<T>(left, right);
        }
        catch (VerifyException ex)
        {
            exceptionCaught = true;
            string opSegment = expectException ? "expected" : "unexpected";
            string prefix = $"Caught {opSegment} exception: ";
            var exceptionMessage = $"{prefix} {ex.ToString()}";
            Console.WriteLine(exceptionMessage);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
        }
        Verify.IsTrue(expectException ? exceptionCaught : !exceptionCaught);
    }

    private void AreNotEqualTest_ValuesAreEqual<T>(T? left, T? right, string comment)
    {
        var lineNumber = -1;

        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.AreNotEqual(left, right, comment);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(exceptionMessage.Contains(Verify.AreNotEqualExceptionPrefix));
            Assert.IsTrue(exceptionMessage.Contains($"@ {nameof(AreNotEqualTest_ValuesAreEqual)}"));
            Assert.IsTrue(exceptionMessage.Contains("VerifySmokeTests.cs"));
            Assert.IsTrue(exceptionMessage.Contains($"({lineNumber})"));
            Assert.IsTrue(exceptionMessage.Contains(comment));
            return;
        }

        Assert.Fail($"Failed throw when {left} equals {right} ({comment})");
    }

    #endregion private

    [TestMethod]
    public void AreEqual_SmokeTest()
    {
        AreEqualTest(null, "notnull", true);
        AreEqualTest<string>(null, null, false);
        AreEqualTest(string.Empty, "Not Empty", true);
        AreEqualTest("one", "one", false);
        AreEqualTest("one", "two", true);
        AreEqualTest(1, 1, false);
        AreEqualTest(4.2, 6.7, true);
    }

    [TestMethod]
    public void AreNotEqual_SmokeTest()
    {
        AreNotEqualTest(3.3, 3.5, false);
        AreNotEqualTest(null, "not null", false);
        AreNotEqualTest("not null", null, false);
        AreNotEqualTest(42, 6 * 7, true);
        AreNotEqualTest((string)null!, null, true);
    }

    [TestMethod]
    public void AreNotNull_SmokeTest()
    {
        object? nullObj = null;
        object[] array = {
                new(),
                new(),
                new()
            };
        string[] strings =
        {
            "This",
            "that",
            "something else",
        };

        object[] arrayOfNull = new[] { nullObj!, nullObj!, nullObj! };

        AreNotNullTest(null!, true);
        AreNotNullTest(arrayOfNull, true);
        AreNotNullTest(array, false);
        AreNotNullTest(strings, false);
    }

    [TestMethod]
    public void AreNotNullOrEmpty_SmokeTest()
    {
        string[] notNullElementsArray = {
                new("Not empty string."),
                new("Not empty string."),
                new("Not empty string."),
            };
        string?[] oneNullElementArray = {
                new("Not empty string."),
                new("Not empty string."),
                null,
            };

        AreNotNullOrEmptyTest(notNullElementsArray, false);
        AreNotNullOrEmptyTest(null, true);
        AreNotNullTest(oneNullElementArray, true);
    }


    [TestMethod]
    public void IsNotNull_SmokeTest()
    {
        IsNotNullTest(null, true);
        IsNotNullTest("Test string.", false);
        IsNotNullTest(42, false);
    }

    [TestMethod]
    public void IsNotNullOrEmpty_SmokeTest()
    {
        IsNotNullOrEmptyTest(null, true);
        IsNotNullOrEmptyTest("", true);
        IsNotNullOrEmptyTest("Test string", false);
    }

    [TestMethod]
    public void IsTrue_SmokeTest()
    {
        IsTrueTest(true, false);
        IsTrueTest(false, true);
    }

    [TestMethod]
    public void IsFalse_SmokeTest()
    {
        IsFalseTest(false, false);
        IsFalseTest(true, true);
    }

    [TestMethod]
    public void IsGreaterThan_SmokeTest()
    {
        IsGreaterThanTest(2, 1, false);
        IsGreaterThanTest(1, 2, true);
        IsGreaterThanTest("2", "1", false);
        IsGreaterThanTest("1", "2", true);
    }

    [TestMethod]
    public void IsGreaterThanOrEqual_SmokeTest()
    {
        IsGreaterThanOrEqualTest(2, 1, false);
        IsGreaterThanOrEqualTest(1, 2, true);
        IsGreaterThanOrEqualTest(2, 2, false);
        IsGreaterThanOrEqualTest("2", "1", false);
        IsGreaterThanOrEqualTest("1", "2", true);
        IsGreaterThanOrEqualTest("2", "2", false);
    }

    [TestMethod]
    public void IsLessThan_SmokeTest()
    {
        IsLessThanTest(1, 2, false);
        IsLessThanTest(2, 1, true);
        IsLessThanTest("1", "2", false);
        IsLessThanTest("2", "1", true);
    }

    [TestMethod]
    public void IsLessThanOrEqual_SmokeTest()
    {
        IsLessThanOrEqualTest(1, 2, false);
        IsLessThanOrEqualTest(2, 1, true);
        IsLessThanOrEqualTest(1, 1, false);
        IsLessThanOrEqualTest("1", "2", false);
        IsLessThanOrEqualTest("2", "1", true);
        IsLessThanOrEqualTest("1", "1", false);
    }

    [TestMethod]
    public void FileExists_SmokeTests()
    {
        using var tmpDir = new TempDirectory(nameof(FileExists_SmokeTests));
        using var tmpFile = new TempFile(tmpDir.DirectoryInfo);

        var fileName = tmpFile.FileInfo.FullName;
        var nonExistantFileName = Path.Combine(tmpDir.DirectoryInfo.FullName, "dmy.dmy.dmy.dmy");

        try
        {
            Verify.FileExists(nonExistantFileName);
            Assert.Fail($"{nameof(Verify.FileExists)} failed to throw exception FileNotFoundException.");
        }
        catch (VerifyException) { }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Assert.Fail($"{nameof(Verify.FileExists)} threw Exception instaead of FileNotFoundException.");
        }

        try
        {
            Verify.FileExists(string.Empty);
            Assert.Fail($"{nameof(Verify.FileExists)} failed to throw exception VerifyException.");
        }
        catch (VerifyException) { }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Assert.Fail($"{nameof(Verify.FileExists)} threw Exception instaead of VerifyException.");
        }

        try
        {
            Verify.FileExists(nonExistantFileName);
            Assert.Fail($"{nameof(Verify.FileExists)} failed to throw exception VerifyException.");
        }
        catch (VerifyException) { }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Assert.Fail($"{nameof(Verify.FileExists)} threw Exception instaead of VerifyException.");

        }
    }

    private void FileExistsTest(
        string filename, 
        TempDirectory tmpDir, 
        bool expectException)
    {
        var expectedSegments = new[]
        {
            Verify.FileNotFoundExceptionPrefix,
            nameof(Verify.FileExists),
            filename,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.FileExists(filename);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.FileExists)} failed to throw {nameof(VerifyException)}");
        }
    }

    private void FileExistsTest(
        FileInfo fileInfo,
        TempDirectory tmpDir,
        bool expectException)
    {
        var expectedSegments = new[]
        {
            Verify.FileNotFoundExceptionPrefix,
            nameof(Verify.FileExists),
            fileInfo.FullName,
        };

        var lineNumber = -1;
        try
        {
            lineNumber = Code.LineNumber() + 1;
            Verify.FileExists(fileInfo.FullName);
        }
        catch (VerifyException ex)
        {
            var exceptionMessage = ex.ToString();
            Console.WriteLine(exceptionMessage);
            Assert.IsTrue(expectException);
            RunStandardAssertions(exceptionMessage, lineNumber, expectedSegments);
            return;
        }

        if (expectException)
        {
            Assert.Fail($"{nameof(Verify.FileExists)} failed to throw {nameof(VerifyException)}");
        }
    }
}

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
