using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Intls.Tests;

[TestClass]
public class StripTests
{
    [TestMethod]
    public void ItemStripperTest1() => _ = new Strip(null);

    [DataTestMethod]
    [DataRow(null, true)]
    [DataRow("", true)]
    [DataRow("    ", true)]
    [DataRow("  .-@+*  ", true)]
    [DataRow("   h ", false)]
    [DataRow("hello", false)]
    public void IsEmptyTest1(string? input, bool expected) => Assert.AreEqual(expected, Strip.IsEmpty(input));

    [DataTestMethod]
    [DataRow(null, true)]
    [DataRow("", true)]
    [DataRow("    ", true)]
    [DataRow("  .-@+*  ", true)]
    [DataRow("   h ", false)]
    [DataRow("hello", false)]
    public void IsEmptyTest2(string? input, bool expected) => Assert.AreEqual(expected, Strip.IsEmpty(input));

    [DataTestMethod]
    [DataRow(null, 0)]
    [DataRow("", 0)]
    [DataRow("    ", 0)]
    [DataRow("  .-@+*  ", 0)]
    [DataRow("   h ", 1)]
    [DataRow("hello", 5)]
    public void GetLengthTest1(string? input, int expected) => Assert.AreEqual(expected, Strip.GetLength(input));


    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EqualsTest1() => _ = new Strip("").Equals(null);


    [DataTestMethod]
    [DataRow(null, null, true)]
    [DataRow("", null, true)]
    [DataRow(null, "", true)]
    [DataRow("   ", null, true)]
    [DataRow(null, "   ", true)]
    [DataRow("   ", "", true)]
    [DataRow("", "   ", true)]
    [DataRow(" +-*@  ", "", true)]
    [DataRow("", " +-*@  ", true)]
    [DataRow(" +-*@  ", "%%", true)]
    [DataRow("%%", " +-*@  ", true)]
    [DataRow("hi", "   hi&&& ", true)]
    [DataRow( "   hi&&& ", "hi", true)]
    [DataRow("Hi", "hi", false)]
    [DataRow("high", "hi", false)]
    [DataRow("hi", "high", false)]
    public void EqualsTest2(string? inp1, string? inp2, bool expected)
    {
        var strip1 = new Strip(inp1);
        var strip2 = new Strip(inp2);

        Assert.AreEqual(expected, strip1.Equals(strip2));

        // Test it twice!!
        Assert.AreEqual(expected, strip1.Equals(strip2));

        Assert.AreEqual(expected, Strip.Equals(inp1, inp2));

        if (expected)
        {
            int hashCode = strip1.GetHashCode();

            Assert.AreEqual(hashCode, strip2.GetHashCode());
            Assert.AreEqual(hashCode, strip1.GetHashCode());
        }
    }


    [DataTestMethod]
    [DataRow(null, null, true)]
    [DataRow("", null, true)]
    [DataRow(null, "", true)]
    [DataRow("   ", null, true)]
    [DataRow(null, "   ", true)]
    [DataRow("   ", "", true)]
    [DataRow("", "   ", true)]
    [DataRow(" +-*@  ", "", true)]
    [DataRow("", " +-*@  ", true)]
    [DataRow(" +-*@  ", "%%", true)]
    [DataRow("%%", " +-*@  ", true)]
    [DataRow("hi", "   hi&&& ", true)]
    [DataRow("   hi&&& ", "hi", true)]
    [DataRow("Hi", "hi", true)]
    [DataRow("high", "hi", false)]
    [DataRow("hi", "high", false)]
    public void EqualsTest3(string? inp1, string? inp2, bool expected)
    {
        var strip1 = new Strip(inp1, false);
        var strip2 = new Strip(inp2, false);

        Assert.AreEqual(expected, strip1.Equals(strip2));

        // Test it twice!!
        Assert.AreEqual(expected, strip1.Equals(strip2));

        Assert.AreEqual(expected, Strip.Equals(inp1, inp2, true));

        if (expected)
        {
            int hashCode = strip1.GetHashCode();

            Assert.AreEqual(hashCode, strip2.GetHashCode());
            Assert.AreEqual(hashCode, strip1.GetHashCode());
        }
    }


    [TestMethod]
    [DataRow(null, null, true)]
    [DataRow("", null, true)]
    [DataRow(null, "", true)]
    [DataRow("   ", null, true)]
    [DataRow(null, "   ", true)]
    [DataRow("   ", "", true)]
    [DataRow("", "   ", true)]
    [DataRow(" +-*@  ", "", true)]
    [DataRow("", " +-*@  ", true)]
    [DataRow(" +-*@  ", "%%", true)]
    [DataRow("%%", " +-*@  ", true)]
    [DataRow("High", "hi", false)]
    [DataRow("high", "Hi", false)]
    [DataRow("high", "hi", true)]
    [DataRow("high", "    hi-***", true)]
    [DataRow("hi", "high", true)]
    public void StartsEqualTest1(string? inp1, string? inp2, bool expected)
        => Assert.AreEqual(expected, Strip.StartEqual(inp1, inp2));


    [TestMethod]
    [DataRow(null, null, true)]
    [DataRow("", null, true)]
    [DataRow(null, "", true)]
    [DataRow("   ", null, true)]
    [DataRow(null, "   ", true)]
    [DataRow("   ", "", true)]
    [DataRow("", "   ", true)]
    [DataRow(" +-*@  ", "", true)]
    [DataRow("", " +-*@  ", true)]
    [DataRow(" +-*@  ", "%%", true)]
    [DataRow("%%", " +-*@  ", true)]
    [DataRow("High", "hi", true)]
    [DataRow("high", "Hi", true)]
    [DataRow("high", "hi", true)]
    [DataRow("high", "    hi-***", true)]
    [DataRow("hi", "high", true)]
    public void StartEqualTest2(string? inp1, string? inp2, bool expected)
        => Assert.AreEqual(expected, Strip.StartEqual(inp1, inp2, true));
}
