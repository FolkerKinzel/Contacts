using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests;

[TestClass]
public class PersonTests
{
    [NotNull]
    public TestContext? TestContext { get; set; }

    private const string FIRST_NAME = "FirstName";
    private const string LAST_NAME = "LastName";
    private const string MIDDLE_NAME = "MiddleName";
    private const string PREFIX = "Prefix";
    private const string SUFFIX = "Suffix";


    [TestMethod]
    public void CloneTest()
    {
        var pers1 = new Person()
        {
            BirthDay = new DateOnly(1885, 6, 5),
            Name = new Name { LastName = "Kinzel", FirstName = "Folker" }
        };


        var pers2 = (Person)pers1.Clone();

        Assert.AreNotSame(pers1, pers2);
        Assert.AreNotSame(pers1.Name, pers2.Name);

        Assert.AreEqual(pers1, pers2);
        Assert.AreEqual(pers1.Name, pers2.Name);
    }


    [TestMethod()]
    public void PersonTest()
    {
        var pers = new Person
        {
            Name = new Name
            {
                LastName = LAST_NAME,
                FirstName = FIRST_NAME,
                MiddleName = MIDDLE_NAME,
                Prefix = PREFIX,
                Suffix = SUFFIX
            }
        };

        Assert.AreEqual(LAST_NAME, pers.Name?.LastName);
        Assert.AreEqual(FIRST_NAME, pers.Name?.FirstName);
        Assert.AreEqual(MIDDLE_NAME, pers.Name?.MiddleName);
        Assert.AreEqual(PREFIX, pers.Name?.Prefix);
        Assert.AreEqual(SUFFIX, pers.Name?.Suffix);
    }



    [TestMethod()]
    public void CleanTest()
    {
        var pers = new Person { Name = new Name { LastName = "  " } };

        Assert.IsTrue(pers.IsEmpty);

        pers.BirthDay = DateOnly.MinValue;
        pers.NickName = "";

        Assert.IsNotNull(pers.BirthDay);
        Assert.IsNotNull(pers.NickName);

        pers.Clean();

        Assert.IsTrue(pers.IsEmpty);
        Assert.IsNull(pers.BirthDay);
        Assert.IsNull(pers.NickName);
    }


    [TestMethod()]
    public void EqualsTest1()
    {
        object p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.IsFalse(p1.Equals(p2));
        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
        Assert.IsTrue(Person.AreMergeable(p1 as Person, p2 as Person));
    }



    [TestMethod()]
    public void EqualsTest2()
    {
        object p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateOnly(1972, 1, 31)
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateOnly(1972, 1, 31)
        };

        Assert.IsFalse(p1.Equals(p2));
    }

    [TestMethod()]
    public void EqualsTest3()
    {
        object p1 = new Person()
        {
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }

        };

        object p2 = new Person()
        {
            BirthDay = new DateOnly(1972, 1, 30),
            Name = new Name { LastName = "Kinzel" }

        };

        Assert.IsFalse(p1.Equals(p2));
    }

    [TestMethod()]
    public void EqualsTest4()
    {
        object p1 = new Person();
        Assert.IsFalse(p1.Equals(null));
        Assert.IsFalse(p1 == null);
        Assert.IsFalse(null == p1);
        Assert.IsTrue(p1 != null);
        Assert.IsTrue(null != p1);

    }

    [TestMethod()]
    public void EqualsTest5()
    {
        var p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }

        };

        var p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.IsFalse(p1.Equals( p2));
        Assert.IsFalse(p1 == p2);
        Assert.IsTrue(p1 != p2);

        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
        Assert.IsTrue(Person.AreMergeable(p1, p2));
    }


    [TestMethod()]
    public void EqualsTest6()
    {
        var p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateOnly(1972, 1, 31)
        };

        var p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateOnly(1972, 1, 31)
        };

        Assert.IsFalse(p1.Equals( p2));
        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
        Assert.IsFalse(Person.AreMergeable(p1, p2));
    }

    [TestMethod()]
    public void EqualsTest7()
    {
        var p1 = new Person()
        {
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        var p2 = new Person()
        {
            BirthDay = new DateOnly(1972, 1, 30),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.AreNotEqual(p1, p2);
        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
        Assert.IsFalse(Person.AreMergeable(p1, p2));
    }

    [TestMethod()]
    public void EqualsTest8()
    {
        var p1 = new Person();
        Person? p2 = null;
        Assert.IsFalse(p1.Equals(p2));
        Assert.IsFalse(p1 == p2);
        Assert.IsFalse(p2 == p1);
        Assert.IsTrue(p1 != p2);
        Assert.IsTrue(p2 != p1);
    }

    [TestMethod()]
    public void EqualsTest9()
    {
        var p1 = new Person();
        Person p2 = p1;
        Assert.IsTrue(p1.Equals(p2));
        Assert.IsTrue(p1 == p2);
        Assert.IsTrue(p2 == p1);
        Assert.IsFalse(p1 != p2);
        Assert.IsFalse(p2 != p1);
    }


    [TestMethod()]
    public void GetHashCodeTest1()
    {
        object p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.AreNotEqual(p1, p2);
        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
    }

    [TestMethod()]
    public void GetHashCodeTest2()
    {
        object p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateOnly(1972, 1, 31)
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateOnly(1972, 1, 31)
        };

        Assert.AreNotEqual(p1, p2);
        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
    }

    [TestMethod()]
    public void GetHashCodeTest3()
    {
        object p1 = new Person()
        {
            BirthDay = new DateOnly(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        object p2 = new Person()
        {
            BirthDay = new DateOnly(1972, 1, 30),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.AreNotEqual(p1, p2);
        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
    }

    [TestMethod()]
    public void ToStringTest1()
    {
        string s = new Person().ToString();

        TestContext.WriteLine(s);

        Assert.IsNotNull(s);
        Assert.AreEqual(0, s.Length);
    }

    [TestMethod()]
    public void ToStringTest2()
    {
        var pers = new Person
        {
            Anniversary = new DateOnly(2014, 7, 15),
            BirthDay = new DateOnly(1980, 2, 3),
            Gender = Sex.Female,
            Name = new Name
            {
                LastName = "Berben",
                FirstName = "Iris"
            },
            NickName = "Schnuckelchen",
            Spouse = "Brad Pitt"
        };

        string s = pers.ToString();

        Assert.IsNotNull(s);

        TestContext.WriteLine(s);
    }
}
