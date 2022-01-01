using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Intls.Tests;

[TestClass]
public class PhoneNumberComparerTests
{
    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow(null, " ")]
    [DataRow(" ", null)]
    public void EqualsTrueTest(string? s1, string? s2)
    {
        var p1 = new PhoneNumber(s1);
        var p2 = new PhoneNumber(s2);
        Assert.IsTrue(PhoneNumberComparer.Instance.Equals(p1, p2));
        Assert.IsTrue(PhoneNumberComparer.Instance.Equals(p1, null));
        Assert.IsTrue(PhoneNumberComparer.Instance.Equals(null, p2));
    }

    [DataTestMethod]
    [DataRow(null, "hi")]
    [DataRow("hi", null)]
    public void EqualsFalseTest1(string? s1, string? s2)
    {
        var p1 = new PhoneNumber(s1);
        var p2 = new PhoneNumber(s2);
        Assert.IsFalse(PhoneNumberComparer.Instance.Equals(p1, p2));
    }

    [TestMethod]
    public void EqualsFalseTest2()
    {
        var p1 = new PhoneNumber("Hi");
        Assert.IsFalse(PhoneNumberComparer.Instance.Equals(p1, null));
        Assert.IsFalse(PhoneNumberComparer.Instance.Equals(null, p1));
    }

    [TestMethod]
    public void GetHashCodeTest1()
    {
        var p1 = new PhoneNumber("hi");

        Assert.AreEqual(PhoneNumberComparer.Instance.GetHashCode(p1), Strip.GetHashCode(p1.Value));
    }

    [TestMethod]
    public void GetHashCodeTest2() => Assert.AreEqual(PhoneNumberComparer.Instance.GetHashCode(null), Strip.GetHashCode(null));

}
