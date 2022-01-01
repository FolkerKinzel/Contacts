using FolkerKinzel.Strings.Polyfills;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests;

[TestClass()]
public class ContactTests
{
#nullable disable
    public TestContext TestContext { get; set; }
#nullable restore

    [TestMethod]
    public void CleanTest1()
    {
        var cont = new Contact()
        {
            DisplayName = $"Folker {Environment.NewLine}Kinzel",
            TimeStamp = new DateTime(1855, 1, 1),
            Comment = $"  Erste Zeile {Environment.NewLine}zweite Zeile "
        };

        cont.Clean();

        Assert.AreEqual(default, cont.TimeStamp);
        Assert.IsNotNull(cont.Comment);
        Assert.IsTrue(cont.Comment.StartsWith("Erste", StringComparison.Ordinal));
        Assert.IsTrue(cont.Comment.EndsWith("Zeile", StringComparison.Ordinal));
        Assert.AreEqual("Folker Kinzel", cont.DisplayName);

        Assert.IsTrue(cont.Comment.Contains(Environment.NewLine, StringComparison.Ordinal));
    }

    [TestMethod]
    public void CleanTest2()
    {
        var cont = new Contact
        {
            AddressHome = new Address(),
            Person = new Person
            {
                Name = new Name
                {
                    LastName = "   "
                }
            },
            EmailAddresses = new string?[]
            {
                    null,
                    "",
                    "    "
            },
            InstantMessengerHandles = new string?[]
            {
                    null,
                    "",
                    "    "
            },
            PhoneNumbers = new PhoneNumber?[]
            {
                    new  PhoneNumber(),
                    null,
                    new PhoneNumber("   ")
            },
            Work = new Work { Company = "  " }

        };

        Assert.IsNotNull(cont.AddressHome);
        Assert.IsNotNull(cont.Person);
        Assert.IsNotNull(cont.EmailAddresses);
        Assert.IsNotNull(cont.InstantMessengerHandles);
        Assert.IsNotNull(cont.PhoneNumbers);
        Assert.IsNotNull(cont.Work);

        cont.Clean();

        Assert.IsNull(cont.AddressHome);
        Assert.IsNull(cont.Person);
        Assert.IsNull(cont.EmailAddresses);
        Assert.IsNull(cont.InstantMessengerHandles);
        Assert.IsNull(cont.PhoneNumbers);
        Assert.IsNull(cont.Work);
    }

    [TestMethod]
    public void CleanTest3()
    {
        var cont = new Contact
        {
            PhoneNumbers = new PhoneNumber?[]
            {
                    new PhoneNumber(" 1 2 3 ", false, true),
                    new  PhoneNumber(),
                    null,
                    new PhoneNumber("   "),
                    new PhoneNumber("123", true),
                    new PhoneNumber(" 1 23 ", false, true, true),
                    new PhoneNumber("456", true),
                    new PhoneNumber("456", false, true)
            }

        };

        cont.Clean();

        Assert.IsNotNull(cont.PhoneNumbers);
        Assert.AreEqual(2, cont.PhoneNumbers.Count());
        Assert.IsTrue(cont.PhoneNumbers.Any(x => new PhoneNumber("1 2 3", true, true, true).Equals(x)));
        Assert.IsTrue(cont.PhoneNumbers.Any(x => new PhoneNumber("456", true, true, false).Equals(x)));
    }


    [TestMethod()]
    public void ToStringTest1()
    {
        //Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        var addr1 = new Address
        {
            Street = "Priorauer Str. 32",
            City = "   Raguhn",
            PostalCode = "   06779"
        };


        var ed1 = new Work
        {
            AddressWork = addr1,
            Company = "Folkers Firma",
            JobTitle = " Boss  "
        };

        var pers1 = new Person
        {
            BirthDay = new DateTime(1985, 6, 5),
            Name = new Name { FirstName = "Folker", LastName = "Kinzel" }
        };

        var contact = new Contact
        {
            Work = ed1,
            WebPagePersonal = "www.folker.de",
            DisplayName = "Folker  ",
            EmailAddresses = new string?[] { "folker@freenet.de", "info@folker.de", null },
            Person = pers1,
            Comment = "Dies ist ein Kommentar",
            AddressHome = addr1,
            WebPageWork = "info@work.de",
            TimeStamp = DateTime.Now,
            InstantMessengerHandles = new string[] { "folker@twitter.com" },
            PhoneNumbers = new PhoneNumber[]
            {
                    new PhoneNumber("123-456"),
                    new PhoneNumber("789-101112")
            }

        };

        contact.Clean();

        string? s = contact.ToString();

        Assert.IsNotNull(s);

        Assert.AreNotEqual(0, s.Length);

        TestContext.WriteLine(s);
    }

    [TestMethod()]
    public void ToStringTest2()
    {
        var contact = new Contact();

        string? s = contact.ToString();

        Assert.IsNotNull(s);

        Assert.AreEqual(0, s.Length);

        TestContext.WriteLine(s);
    }


    [TestMethod()]
    public void CloneTest()
    {
        var addr = new Address
        {
            Street = "Priorauer Str. 32",
            City = "   Raguhn",
            PostalCode = "   "
        };


        var work = new Work
        {
            AddressWork = addr,
            Company = "Folkers Firma",
            JobTitle = "   "
        };

        var pers = new Person
        {
            BirthDay = new DateTime(1885, 6, 5),
            Name = new Name { FirstName = "Folker", LastName = "Kinzel" }
        };


        var contact1 = new Contact
        {
            Work = work,
            WebPagePersonal = "www.folker.de",
            DisplayName = "Folker  ",
            EmailAddresses = new string?[] { "folker@freenet.de", "info@folker.de", null },
            Person = pers,
            AddressHome = addr,
            InstantMessengerHandles = new string?[] { "Folker@twitter.com" },
            PhoneNumbers = new PhoneNumber[] { new PhoneNumber("123") }
        };

        var contact2 = (Contact)contact1.Clone();
        

        Assert.AreNotSame(contact1.Work, contact2.Work);
        Assert.AreEqual(contact1.Work, contact2.Work);

        Assert.AreNotSame(contact1.Person, contact2.Person);
        Assert.AreEqual(contact1.Person, contact2.Person);

        Assert.AreNotSame(contact1.AddressHome, contact2.AddressHome);
        Assert.AreEqual(contact1.AddressHome, contact2.AddressHome);

        Assert.AreNotSame(contact1.EmailAddresses, contact2.EmailAddresses);
        Assert.IsNotNull(contact2.EmailAddresses);
        Assert.IsTrue(contact1.EmailAddresses.SequenceEqual(contact2.EmailAddresses!));

        Assert.AreNotSame(contact1.InstantMessengerHandles, contact2.InstantMessengerHandles);
        Assert.IsNotNull(contact2.InstantMessengerHandles);
        Assert.IsTrue(contact1.InstantMessengerHandles.SequenceEqual(contact2.InstantMessengerHandles!));

        Assert.AreNotSame(contact1.PhoneNumbers, contact2.PhoneNumbers);
        Assert.IsNotNull(contact2.PhoneNumbers);
        Assert.AreEqual(contact1.PhoneNumbers.Count(), contact2.PhoneNumbers!.Count());
        Assert.AreEqual(contact1.PhoneNumbers.First(), contact2.PhoneNumbers!.First());
        Assert.AreNotSame(contact1.PhoneNumbers.First(), contact2.PhoneNumbers!.First());

        Assert.AreNotSame(contact1, contact2);
        Assert.AreEqual(contact1, contact2);
    }

    [TestMethod()]
    public void EqualsTest1()
    {
        object cont1 = new Contact();
        object cont2 = new Contact();

        Assert.AreEqual(cont1, cont2);
        Assert.IsTrue(((Contact)cont1).IsEmpty);
        Assert.IsTrue(((Contact)cont2).IsEmpty);
        Assert.AreNotEqual(cont1, null);
        Assert.AreEqual(cont1.GetHashCode(), cont2.GetHashCode());

    }

    [TestMethod()]
    public void EqualsTest2()
    {
        var addr = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn",
            PostalCode = "06779"
        };

        var work = new Work
        {
            AddressWork = addr,
            Company = "Folkers Firma",
            JobTitle = "Chef"
        };

        var pers = new Person
        {
            BirthDay = new DateTime(1985, 6, 5),
            Name = new Name { FirstName = "Folker", LastName = "Kinzel" }
        };


        var contact1 = new Contact
        {
            Work = work,
            WebPagePersonal = "www.folker.de",
            WebPageWork = "work.de",
            DisplayName = "Folker",
            EmailAddresses = new string?[] { "folker@freenet.de", "info@folker.de", null },
            Person = pers,
            AddressHome = addr,
            InstantMessengerHandles = new string?[] { "Folker@twitter.com" },
            PhoneNumbers = new PhoneNumber[] { new PhoneNumber("123") }
        };

        var contact2 = (Contact)contact1.Clone();

        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.DisplayName = "Folker Kinzel";

        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.Person!.Name!.LastName = "Müller";
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.EmailAddresses = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.InstantMessengerHandles = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));


        contact2.Person = null;

        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.Work!.Company = "Apple";

        Assert.AreNotEqual(contact1, contact2);
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.Work = null;

        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        

        contact2.DisplayName = contact1.DisplayName;

        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.DisplayName = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.PhoneNumbers = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.AddressHome!.PostalCode = "12345";
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.WebPagePersonal = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.AddressHome = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));


        contact2.WebPagePersonal = "www.other.de";
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));


        contact2.WebPagePersonal = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.WebPageWork = "other.com";

        Assert.AreNotEqual(contact1, contact2);
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.WebPageWork = "  ";
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));


        contact2.WebPageWork = null;

        Assert.IsTrue(contact2.IsEmpty);
        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));
    }


    [TestMethod()]
    public void GetHashCodeTest()
    {
        var addr = new Address
        {
            Street = "Priorauer Str. 32",
            City = "Raguhn",
            PostalCode = "06779"
        };


        var work = new Work
        {
            AddressWork = addr,
            Company = "Folkers Firma",
            JobTitle = "Chef"
        };

        var pers = new Person
        {
            BirthDay = new DateTime(1985, 6, 5),
            Name = new Name { FirstName = "Folker", LastName = "Kinzel" }
        };


        var contact1 = new Contact
        {
            Work = work,
            WebPagePersonal = "www.folker.de",
            WebPageWork = "work.de",
            DisplayName = "Folker",
            EmailAddresses = new string?[] { "folker@freenet.de", "info@folker.de", null },
            Person = pers,
            AddressHome = addr,
            InstantMessengerHandles = new string?[] { "Folker@twitter.com" },
            PhoneNumbers = new PhoneNumber[] { new PhoneNumber("123") }
        };

        var contact2 = (Contact)contact1.Clone();

        Assert.AreEqual(contact1, contact2);
        Assert.AreNotSame(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.DisplayName = "Folker Kinzel";
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.Person!.Name!.LastName = "Müller";

        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.Person = null;

        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.Person = null;
        contact1.DisplayName = contact2.DisplayName;
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());


        contact2.Work!.Company = "Apple";

        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.Work = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.Work = null;
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());


        contact2.EmailAddresses = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.EmailAddresses = null;
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.InstantMessengerHandles = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());


        contact1.InstantMessengerHandles = null;
        contact2.DisplayName = contact1.DisplayName;
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.DisplayName = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.DisplayName = null;
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());



        contact2.PhoneNumbers = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.PhoneNumbers = new PhoneNumber?[] {null};
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.AddressHome!.PostalCode = "12345";
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.AddressHome = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.AddressHome = new Address();
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPagePersonal = "www.other.de";
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPagePersonal = null;
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.WebPagePersonal = "   ";
        Assert.AreEqual(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPageWork = "other.com";
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPageWork = "  ";
        Assert.AreNotEqual(contact1, contact2);
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.WebPageWork = null;
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());


        Assert.IsTrue(contact2.IsEmpty);
        Assert.IsTrue(contact1.IsEmpty);


    }


}
