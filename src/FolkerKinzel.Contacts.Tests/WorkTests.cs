using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests;

[TestClass()]
public class WorkTests
{
    [TestMethod()]
    public void CloneTest()
    {
        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "   Raguhn",
            PostalCode = "   "
        };


        var work1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = "   "
        };

        var work2 = (Work)work1.Clone();

        Assert.AreNotSame(work1, work2);
        Assert.AreNotSame(work1.AddressWork, work2.AddressWork);

        Assert.AreEqual(work1, work2);
        Assert.AreEqual(work1.AddressWork, work2.AddressWork);
    }

    [TestMethod()]
    public void WorkTest()
    {
        var work = new Work();

        Assert.IsTrue(work.IsEmpty);
    }


    [TestMethod()]
    public void CleanTest()
    {
        var addr1 = new Address
        {
            Street = " Priorauer Str. 32 ",
            City = "   Raguhn",
            PostalCode = "   "
        };


        var work1 = new Work
        {
            AddressWork = addr1,
            Company = " Folkers Firma ",
            JobTitle = "   "
        };

        work1.Clean();

        Assert.AreEqual("Folkers Firma", work1.Company);
        Assert.IsNull(work1.JobTitle);
        Assert.AreEqual("Raguhn", work1.AddressWork.City);
    }

    [TestMethod()]
    public void EqualsTest1()
    {
        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn"
        };


        object work1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = "   "
        };

        var addr2 = new Address
        {
            Street = "Andere Str. 33",
            City = "Wolfen"
        };


        object work2 = new Work
        {
            AddressWork = addr2,
            Company = "folkers firma",
            JobTitle = "   "
        };



        Assert.AreNotEqual(work1, work2);
    }

    [TestMethod()]
    public void EqualsTest2()
    {
        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn"
        };


        object work1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = "   "
        };

        var addr2 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "raguhn"
        };


        object work2 = new Work
        {
            AddressWork = addr2,

            JobTitle = "Generaldirektor"
        };

        Assert.AreEqual(work1, work2);
    }

    [TestMethod()]
    public void EqualsTest3()
    {
        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn"
        };


        object work1 = new Work
        {
            AddressWork = addr1,
            Company = "Ingrids Firma",

        };


        object work2 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma"
        };

        Assert.AreNotEqual(work1, work2);
    }

    [TestMethod()]
    public void EqualsTest4()
    {


        object work1 = new Work
        {

            Company = "Ingrids Firma"

        };

        Assert.AreNotEqual(work1, null);
    }

    [TestMethod()]
    public void EqualsTest5()
    {
        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn"
        };


        var work1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = "   "
        };


        var work2 = new Work
        {
            AddressWork = addr1,
            Company = "folkers firma",
            JobTitle = " "
        };

        Assert.AreEqual(work1, work2);
    }

    [TestMethod()]
    public void EqualsTest6()
    {
        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn"
        };


        var work1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = "   "
        };

        var addr2 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "raguhn"
        };


        var work2 = new Work
        {
            AddressWork = addr2,

            JobTitle = "Generaldirektor"
        };

        Assert.AreEqual(work1, work2);
    }

    [TestMethod()]
    public void EqualsTest7()
    {
        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn"
        };


        var work1 = new Work
        {
            AddressWork = addr1,
            Company = "Ingrids Firma",

        };


        var work2 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma"
        };

        Assert.AreNotEqual(work1, work2);
    }

    [TestMethod()]
    public void EqualsTest8()
    {


        var work1 = new Work
        {

            Company = "Ingrids Firma"

        };

        Assert.IsFalse(work1.Equals(null));
    }

    [TestMethod()]
    public void GetHashCodeTest1()
    {
        var work1 = new Work();
        var work2 = new Work
        {
            JobTitle = "Chef"
        };

        Assert.AreEqual(work1.GetHashCode(), work2.GetHashCode());
    }

    [TestMethod()]
    public void GetHashCodeTest2()
    {
        var work1 = new Work
        {
            Company = "Folkers Firma",
            JobTitle = "Butler"
        };
        var work2 = new Work
        {
            Company = "folkers firma",
            JobTitle = "Chef"
        };

        Assert.AreNotEqual(work1.GetHashCode(), work2.GetHashCode());
    }


    [TestMethod()]
    public void GetHashCodeTest3()
    {
        var work1 = new Work
        {
            Company = "Folkers Firma",
            JobTitle = "Chef"
        };
        var work2 = new Work
        {
            Company = "Ingrids Firma",
            JobTitle = "Chef"
        };

        Assert.AreNotEqual(work1.GetHashCode(), work2.GetHashCode());
    }

    [TestMethod()]
    public void GetHashCodeTest4()
    {
        var adr1 = new Address
        {
            PostalCode = "06779"
        };

        var adr2 = new Address
        {
            PostalCode = "06778"
        };

        var work1 = new Work
        {
            Company = "Folkers Firma",
            JobTitle = "Chef",
            AddressWork = adr1
        };
        var work2 = new Work
        {
            AddressWork = adr2,
            JobTitle = "Chef"
        };

        Assert.AreNotEqual(work1.GetHashCode(), work2.GetHashCode());
    }


    [TestMethod()]
    public void ToStringTest1()
    {
        var work = new Work();

        string? s = work.ToString();

        Assert.IsNotNull(s);
        Assert.AreEqual(0, s.Length);
    }


    [TestMethod()]
    public void ToStringTest2()
    {
        var work = new Work
        {
            AddressWork = new Address
            {
                Street = "Priorauer Str. 32",
                City = "Raguhn-Jeßnitz",
                PostalCode = "06779",
                State = "Sachsen-Anhalt",
                Country = "Germany"
            },
            Company = "Folkers Firma",
            Department = "Chefetage",
            Office = "1",
            JobTitle = "Chef"

        };

        string? s = work.ToString();

        Assert.IsNotNull(s);
        Assert.AreNotEqual(0, s.Length);

    }
}
