﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests;

[TestClass()]
public class WorkTests
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
            Street = " Berliner Str. 42 ",
            City = "   Berghain",
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
        Assert.AreEqual("Berghain", work1.AddressWork.City);
    }

    [TestMethod()]
    public void EqualsTest1()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain"
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



        Assert.IsFalse(work1.Equals(work2));
        Assert.IsFalse(work1 == work2);
        Assert.IsTrue(work1 != work2);
    }

    [TestMethod()]
    public void EqualsTest2()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain"
        };


        object work1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = "   "
        };

        var addr2 = new Address
        {
            Street = "Berliner Str. 42",
            City = "berghain"
        };


        object work2 = new Work
        {
            AddressWork = addr2,

            JobTitle = "Generaldirektor"
        };

        Assert.IsFalse(work1.Equals( work2));
        Assert.AreNotEqual(work1.GetHashCode(), work2.GetHashCode());
        Assert.IsTrue(Work.AreMergeable(work1 as Work, work2 as Work));
    }


    [TestMethod()]
    public void EqualsTest3()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain"
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

        Assert.IsFalse(work1.Equals( work2));
    }

    [TestMethod()]
    public void EqualsTest4()
    {
        object work1 = new Work
        {

            Company = "Ingrids Firma"

        };

        Assert.IsFalse(work1.Equals( null));
        Assert.IsFalse(work1 == null);
        Assert.IsTrue(work1 != null);
        Assert.IsFalse(null == work1);
        Assert.IsTrue(null != work1);
    }

    [TestMethod()]
    public void EqualsTest5()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain"
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

        Assert.IsFalse(work1.Equals( work2));
        Assert.AreNotEqual(work1.GetHashCode(), work2.GetHashCode());
        Assert.IsTrue(Work.AreMergeable(work1, work2));
    }

    [TestMethod()]
    public void EqualsTest6()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain"
        };


        var work1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = "   "
        };

        var addr2 = new Address
        {
            Street = "Berliner Str. 42",
            City = "berghain"
        };


        var work2 = new Work
        {
            AddressWork = addr2,

            JobTitle = "Generaldirektor"
        };

        Assert.IsFalse(work1.Equals( work2));
        Assert.AreNotEqual(work1.GetHashCode(), work2.GetHashCode());
        Assert.IsTrue(Work.AreMergeable(work1, work2));
    }

    [TestMethod()]
    public void EqualsTest7()
    {
        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain"
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

        Assert.IsFalse(work1.Equals( work2));
    }

    [TestMethod()]
    public void EqualsTest8()
    {
        var work1 = new Work
        {

            Company = "Ingrids Firma"

        };

        Assert.IsFalse(work1.Equals(null));
        Assert.IsFalse(work1 == null);
        Assert.IsTrue(work1 != null);
        Assert.IsFalse(null == work1);
        Assert.IsTrue(null != work1);
    }

    [TestMethod]
    public void EqualsTest9()
    {
        var w1 = new Work();
        Work w2 = w1;
        Assert.IsTrue(w1.Equals(w2));
        Assert.IsTrue(w1 == w2);
        Assert.IsFalse(w1 != w2);
    }

    [TestMethod]
    public void EqualsTest10()
    {
        var w1 = new Work();
        object w2 = w1;
        Assert.IsTrue(w1.Equals(w2));
#pragma warning disable CS0253 // Möglicher unbeabsichtigter Referenzvergleich; rechte Seite muss umgewandelt werden
        Assert.IsTrue(w1 == w2);
        Assert.IsFalse(w1 != w2);
#pragma warning restore CS0253 // Möglicher unbeabsichtigter Referenzvergleich; rechte Seite muss umgewandelt werden

    }

    [TestMethod]
    public void EqualsTest11()
    {
        var w1 = new Work();
        object w2 = new();
        Assert.IsFalse(w1.Equals(w2));
#pragma warning disable CS0253 // Möglicher unbeabsichtigter Referenzvergleich; rechte Seite muss umgewandelt werden
        Assert.IsTrue(w1 != w2);
        Assert.IsFalse(w1 == w2);
#pragma warning restore CS0253 // Möglicher unbeabsichtigter Referenzvergleich; rechte Seite muss umgewandelt werden
    }

    [DataTestMethod]
    [DataRow("dep", "dep", "1", "1", true)]
    [DataRow("dep", "dep2", "1", "1", false)]
    [DataRow("dep", "dep", "1", "2", false)]
    //[DataRow("dep", null, "1", "2", true)]
    public void EqualsTest12(string? department1, string? department2, string? office1, string? office2, bool expected)
    {
        const string companyName = "Contoso";
        var w1 = new Work()
        { 
            Company = companyName,
            Department = department1,
            Office = office1
        };

        var w2 = new Work()
        {
            Company = companyName,
            Department = department2,
            Office = office2
        };

        Assert.AreEqual(expected, w1.Equals(w2));

        if(expected)
        {
            Assert.AreEqual(expected, w1.GetHashCode() == w2.GetHashCode());
        }
    }

    [TestMethod()]
    public void GetHashCodeTest1()
    {
        var work1 = new Work();
        var work2 = new Work
        {
            JobTitle = "Chef"
        };

        Assert.IsFalse(work1.Equals( work2));
        Assert.AreNotEqual(work1.GetHashCode(), work2.GetHashCode());
        Assert.IsTrue(Work.AreMergeable(work1, work2));
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
            PostalCode = "09876"
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
                Street = "Berliner Str. 42",
                City = "Berghain-Taldorf",
                PostalCode = "09876",
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
