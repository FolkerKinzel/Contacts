using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests;

[TestClass]
public class PersonTests
{
#nullable disable
    public TestContext TestContext { get; set; }
#nullable restore

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
            BirthDay = new DateTime(1885, 6, 5),
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

        Assert.IsFalse(pers.IsEmpty);

        pers.BirthDay = DateTime.MinValue;
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
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }

        };

        Assert.IsTrue(p1.Equals(p2));
    }



    [TestMethod()]
    public void EqualsTest2()
    {
        object p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateTime(1972, 1, 31)
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateTime(1972, 1, 31)
        };

        Assert.IsFalse(p1.Equals(p2));
    }

    [TestMethod()]
    public void EqualsTest3()
    {
        object p1 = new Person()
        {
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }

        };

        object p2 = new Person()
        {
            BirthDay = new DateTime(1972, 1, 30),
            Name = new Name { LastName = "Kinzel" }

        };

        Assert.IsFalse(p1.Equals(p2));
    }

    [TestMethod()]
    public void EqualsTest4()
    {
        object p1 = new Person();
        Assert.IsFalse(p1.Equals(null));
    }

    [TestMethod()]
    public void EqualsTest5()
    {
        var p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }

        };

        var p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.IsTrue(p1.Equals(p2));
    }

    [TestMethod()]
    public void EqualsTest6()
    {
        var p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateTime(1972, 1, 31)
        };

        var p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateTime(1972, 1, 31)
        };

        Assert.IsFalse(p1.Equals(p2));
    }

    [TestMethod()]
    public void EqualsTest7()
    {
        var p1 = new Person()
        {
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        var p2 = new Person()
        {
            BirthDay = new DateTime(1972, 1, 30),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.IsFalse(p1.Equals(p2));
    }

    [TestMethod()]
    public void EqualsTest8()
    {
        var p1 = new Person();
        Assert.IsFalse(p1.Equals(null));
    }


    [TestMethod()]
    public void GetHashCodeTest1()
    {
        object p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
    }

    [TestMethod()]
    public void GetHashCodeTest2()
    {
        object p1 = new Person()
        {
            NickName = "Genie",
            BirthDay = new DateTime(1972, 1, 31)
        };

        object p2 = new Person()
        {
            NickName = "Trottel",
            BirthDay = new DateTime(1972, 1, 31)
        };

        Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
    }

    [TestMethod()]
    public void GetHashCodeTest3()
    {
        object p1 = new Person()
        {
            BirthDay = new DateTime(1972, 1, 31),
            Name = new Name { LastName = "Kinzel" }
        };

        object p2 = new Person()
        {
            BirthDay = new DateTime(1972, 1, 30),
            Name = new Name { LastName = "Kinzel" }
        };

        Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
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
        //var uiCulture = CultureInfo.CurrentUICulture;

        //var currentUiCulture = Thread.CurrentThread.CurrentUICulture;

        //var currentCulture = Thread.CurrentThread.CurrentCulture;

        //Res.Culture = uiCulture;

        var pers = new Person
        {
            Anniversary = DateTime.Now,
            BirthDay = DateTime.Now,
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
