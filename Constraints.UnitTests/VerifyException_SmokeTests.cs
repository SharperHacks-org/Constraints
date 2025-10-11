// Copyright and trademark notices at the end of this file.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace SharperHacks.CoreLibs.Constraints.UnitTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class VerifyExceptionSmokeTests //: TestBase
{
    [TestMethod]
    public void VerifyException_DefaultConstructor_SmokeTest()
    {
        var ex = new VerifyException();
        Console.WriteLine(ex.Message);

        Assert.Contains("SharperHacks.CoreLibs.Constraints.VerifyException", ex.Message);

        try
        {
            throw ex;
        }
        catch (Exception caught)
        {
            Assert.AreEqual(typeof(VerifyException), caught.GetType());
            Assert.Contains("SharperHacks.CoreLibs.Constraints.VerifyException", caught.Message);
        }
    }

    [TestMethod]
    public void VerifyException_MessageConstructor_SmokeTest()
    {
        var msg = "Test message.";

        var ex = new VerifyException(msg);
        Console.WriteLine(ex.Message);

        Assert.Contains(msg, ex.Message);

        try
        {
            throw ex;
        }
        catch (VerifyException caught)
        {
            Assert.Contains(msg, caught.Message);
        }
    }

    [TestMethod]
    public void VerifyException_MessageAndInnerConstructor_SmokeTest()
    {
        var innerMsg = "Inner message.";
        var outerMsg = "Outer message.";

        var innerEx = new VerifyException(innerMsg);
        Console.WriteLine(innerEx.Message);

        Assert.Contains(innerMsg, innerEx.Message);

        var outerEx = new VerifyException(outerMsg, innerEx);
        Assert.IsNotNull(outerEx);
        Assert.IsTrue(outerEx.InnerException != null && outerEx.InnerException.Message.Contains(innerMsg));
        Assert.Contains(outerMsg, outerEx.Message);

        try
        {
            throw outerEx;
        }
        catch (Exception caught)
        {
            Assert.AreEqual(typeof(VerifyException), caught.GetType());
            Assert.IsNotNull(caught.InnerException);
            Assert.Contains(innerMsg, outerEx.InnerException.Message);
            Assert.AreEqual(typeof(VerifyException), caught.InnerException!.GetType());
            Assert.Contains(outerMsg, caught.Message);
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
