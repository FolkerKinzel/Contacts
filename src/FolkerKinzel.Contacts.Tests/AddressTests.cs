using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests;

[TestClass()]
public class AddressTests
{
    [TestMethod()]
    public void CloneTest()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "   Berghain",
            PostalCode = "   "
        };

        var addr2 = (Address)addr1.Clone();


        Assert.AreNotSame(addr1, addr2);
        Assert.IsTrue(addr1 == addr2);
    }


    [TestMethod()]
    public void AddressTest()
    {
        var adr = new Address();
        Assert.IsTrue(adr.IsEmpty);
    }


    [TestMethod()]
    public void CleanTest()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "   Berghain ",
            PostalCode = "   "
        };

        addr1.Clean();

        Assert.AreEqual("Berliner Str. 42", addr1.Street);
        Assert.AreEqual("Berghain", addr1.City);
        Assert.IsNull(addr1.PostalCode);

        Assert.IsFalse(addr1.IsEmpty);
    }


    [TestMethod()]
    public void EqualsTest01()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        object adr2 = new Address
        {
            PostalCode = "4711",
            City = "Berlin (Wilmersdorf)",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.IsFalse(adr1 == adr2);
        Assert.IsTrue(adr1 != adr2);

        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());

        Assert.IsTrue(Address.AreMergeable(adr1 as Address, adr2 as Address));
    }


    [TestMethod()]
    public void EqualsTest02()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        object adr2 = new Address
        {
            PostalCode = "4711",
            City = "Berlin (Wilmersdorf)",
            Street = "Gänseweg 5"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        Assert.IsFalse(Address.AreMergeable(adr1 as Address, adr2 as Address));
    }

    [TestMethod()]
    public void EqualsTest03()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        object adr2 = new Address
        {
            PostalCode = "4712",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        Assert.IsFalse(Address.AreMergeable(adr1 as Address, adr2 as Address));
    }

    [TestMethod()]
    public void EqualsTest04()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        object adr2 = new Address
        {
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        Assert.IsTrue(Address.AreMergeable(adr1 as Address, adr2 as Address));
    }

    [TestMethod()]
    public void EqualsTest05()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(null));
        Assert.IsTrue(Address.AreMergeable(adr1 as Address, null));
    }

    [TestMethod()]
    public void EqualsTest06()
    {
        var adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        var adr2 = new Address
        {
            PostalCode = "4711",
            City = "Berlin (Wilmersdorf)",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        Assert.IsTrue(Address.AreMergeable(adr1, adr2));
    }

    [TestMethod()]
    public void EqualsTest07()
    {
        var adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        var adr2 = new Address
        {
            PostalCode = "4711",
            City = "Berlin (Wilmersdorf)",
            Street = "Gänseweg 5"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        Assert.IsFalse(Address.AreMergeable(adr1, adr2));
    }

    [TestMethod()]
    public void EqualsTest08()
    {
        var adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        var adr2 = new Address
        {
            PostalCode = "4712",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        Assert.IsFalse(Address.AreMergeable(adr1, adr2));
    }

    [TestMethod()]
    public void EqualsTest09()
    {
        var adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        var adr2 = new Address
        {
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        Assert.IsTrue(Address.AreMergeable(adr1, adr2));
    }

    [TestMethod()]
    public void EqualsTest10()
    {
        var adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        Assert.IsFalse(adr1.Equals(null));
        Assert.IsFalse(adr1 == null);
        Assert.IsTrue(adr1 != null);
        Assert.IsFalse(null == adr1);
        Assert.IsTrue(null != adr1);
    }


    [TestMethod]
    public void EqualsTest11()
    {
        var adr1 = new Address();
        Address? adr2 = null;

        Assert.IsFalse(adr1.Equals(adr2));
        Assert.IsFalse(adr1 == adr2);
        Assert.IsTrue(adr1 != adr2);
        Assert.IsFalse(adr2 == adr1);
        Assert.IsTrue(adr2 != adr1);
    }

    [TestMethod]
    public void EqualsTest12()
    {
        var adr1 = new Address();
        Assert.IsTrue(adr1.Equals(adr1));

#pragma warning disable CS1718 // Vergleich erfolgte mit derselben Variable
        Assert.IsTrue(adr1 == adr1);
        Assert.IsFalse(adr1 != adr1);
#pragma warning restore CS1718 // Vergleich erfolgte mit derselben Variable
    }

    [TestMethod]
    public void EqualsTest13()
    {
        object adr1 = new Address();
        object? adr2 = null;

        Assert.IsFalse(adr1.Equals(adr2));
    }

    [TestMethod]
    public void EqualsTest14()
    {
        object adr1 = new Address();
        Assert.IsTrue(adr1.Equals(adr1));
    }

    [TestMethod()]
    public void GetHashCodeTest1()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        object adr2 = new Address
        {
            PostalCode = "4711",
            City = "Berlin (Wilmersdorf)",
            Street = "Gänseweg 4"
        };

        Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
    }


    [TestMethod()]
    public void GetHashCodeTest2()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        object adr2 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 5"
        };

        Assert.AreNotEqual(adr1, adr2);
    }


    [TestMethod()]
    public void GetHashCodeTest3()
    {
        object adr1 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        object adr2 = new Address
        {
            PostalCode = "4711",
            City = "Berlin",
            Street = "Gänseweg 4"
        };

        Assert.AreEqual(adr1.GetHashCode(), adr2.GetHashCode());
    }



    [TestMethod()]
    public void ToStringTest()
    {
        var adr = new Address();

        string? s = adr.ToString();

        Assert.IsNotNull(s);
        Assert.AreEqual(0, s.Length);
    }
}
