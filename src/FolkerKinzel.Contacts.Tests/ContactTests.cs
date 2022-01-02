using FolkerKinzel.Strings.Polyfills;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests;

[TestClass()]
public class ContactTests
{
    [NotNull]
    public TestContext? TestContext { get; set; }

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
            Street = "Berliner Str. 42",
            City = "   Berghain",
            PostalCode = "   09876"
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
            EmailAddresses = new string?[] { "folker@internet.de", "info@folker.de", null },
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
            Street = "Berliner Str. 42",
            City = "   Berghain",
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
            EmailAddresses = new string?[] { "folker@internet.de", "info@folker.de", null },
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

        Assert.IsTrue(cont1.Equals(cont2));
        Assert.IsTrue(((Contact)cont1).IsEmpty);
        Assert.IsTrue(((Contact)cont2).IsEmpty);
        Assert.IsFalse(cont1.Equals(null));
        Assert.IsFalse(cont1 == null);
        Assert.IsFalse(null == cont1);
        Assert.IsTrue(cont1 != null);
        Assert.IsTrue(null != cont1);

        Assert.AreEqual(cont1!.GetHashCode(), cont2.GetHashCode());

    }

    [TestMethod()]
    public void EqualsTest2()
    {
        var addr = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain",
            PostalCode = "09876"
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
            EmailAddresses = new string?[] { "folker@internet.de", "info@folker.de", null },
            Person = pers,
            AddressHome = addr,
            InstantMessengerHandles = new string?[] { "Folker@twitter.com" },
            PhoneNumbers = new PhoneNumber[] { new PhoneNumber("123") }
        };

        var contact2 = (Contact)contact1.Clone();

        Assert.IsTrue(contact1.Equals(contact2));
        Assert.IsTrue(contact1 == contact2);
        Assert.IsFalse(contact1 != contact2);

        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.DisplayName = "Folker Kinzel";

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsFalse(contact1 == contact2);
        Assert.IsTrue(contact1 != contact2);

        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.Person!.Name!.LastName = "Müller";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.EmailAddresses = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.InstantMessengerHandles = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.Person = null;

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.Work!.Company = "Apple";

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.Work = null;

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.DisplayName = contact1.DisplayName;

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.DisplayName = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.PhoneNumbers = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.AddressHome!.PostalCode = "12345";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.WebPagePersonal = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.AddressHome = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.WebPagePersonal = "www.other.de";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.WebPagePersonal = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.WebPageWork = "other.com";

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsFalse(Contact.AreMergeable(contact1, contact2));

        contact2.WebPageWork = "  ";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        contact2.WebPageWork = null;

        Assert.IsTrue(contact2.IsEmpty);
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));
    }


    [TestMethod()]
    public void GetHashCodeTest()
    {
        var addr = new Address
        {
            Street = "Berliner Str. 42",
            City = "Berghain",
            PostalCode = "09876"
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
            EmailAddresses = new string?[] { "folker@internet.de", "info@folker.de", null },
            Person = pers,
            AddressHome = addr,
            InstantMessengerHandles = new string?[] { "Folker@twitter.com" },
            PhoneNumbers = new PhoneNumber[] { new PhoneNumber("123") }
        };

        var contact2 = (Contact)contact1.Clone();

        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreNotSame(contact1, contact2);
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.DisplayName = "Folker Kinzel";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.Person!.Name!.LastName = "Müller";

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.Person = null;

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.Person = null;
        contact1.DisplayName = contact2.DisplayName;
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.Work!.Company = "Apple";

        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.Work = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.Work = null;
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.EmailAddresses = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.EmailAddresses = null;
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.InstantMessengerHandles = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.InstantMessengerHandles = null;
        contact2.DisplayName = contact1.DisplayName;
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.DisplayName = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.DisplayName = null;
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.PhoneNumbers = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.PhoneNumbers = new PhoneNumber?[] { null };
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.AddressHome!.PostalCode = "12345";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.AddressHome = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.AddressHome = new Address();
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPagePersonal = "www.other.de";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPagePersonal = null;
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.WebPagePersonal = "   ";
        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPageWork = "other.com";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact2.WebPageWork = "  ";
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.AreNotEqual(contact1.GetHashCode(), contact2.GetHashCode());

        contact1.WebPageWork = null;
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());

        Assert.IsTrue(contact2.IsEmpty);
        Assert.IsTrue(contact1.IsEmpty);
    }



    [TestMethod]
    public void MergeTest1()
    {
        const string comment = "Dies ist ein Kommentar";

        var addr1 = new Address
        {
            Street = "Berliner Str. 42",
            City = "   Berghain",
            PostalCode = "   09876"
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

        var contact1 = new Contact
        {
            Work = ed1,
            WebPagePersonal = "www.folker.de",
            DisplayName = "Folker  ",
            EmailAddresses = new string?[] { "folker@internet.de", "info@folker.de", null },
            Person = pers1,
            Comment = comment,
            AddressHome = addr1,
            WebPageWork = "info@work.de",
            TimeStamp = DateTime.Now,
            InstantMessengerHandles = new string[] { "folker@twitter.com" },
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

        var contact2 = new Contact();
        Assert.IsTrue(contact2.IsEmpty);
        Assert.IsFalse(contact1.Equals(contact2));
        Assert.IsTrue(Contact.AreMergeable(contact1, contact2));

        _ = contact2.Merge(contact1);

        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreNotSame(contact1.AddressHome, contact2.AddressHome);
        Assert.AreNotSame(contact1.EmailAddresses, contact2.EmailAddresses);
        Assert.AreNotSame(contact1.InstantMessengerHandles, contact2.InstantMessengerHandles);
        Assert.AreNotSame(contact1.PhoneNumbers, contact2.PhoneNumbers);

        Assert.IsTrue(!contact1.PhoneNumbers.Where(x => x != null).Any(x => contact2.PhoneNumbers!.Any(y => object.ReferenceEquals(x, y))));


        var emails = new List<string?>(contact1.EmailAddresses);
        contact1.EmailAddresses = emails;
        emails.Add("INFO@folker.de");

        var imHandles = new List<string?>(contact1.InstantMessengerHandles);
        contact1.InstantMessengerHandles = imHandles;
        imHandles.Add("  ");
        imHandles.Add("");
        imHandles.Add(null);
        imHandles.Add("FOLKER@TWITTER.COM");

        contact1.AddressHome.Country = "Germany";
        contact1.Work.Office = "17";
        contact1.Person.Spouse = "Schatzi";

        var phoneNumbers = contact1.PhoneNumbers.ToList();
        contact1.PhoneNumbers = phoneNumbers;
        phoneNumbers.Add(new PhoneNumber(" 4 5 - 6", isFax: true));

        _ = contact2.Merge(contact1);

        Assert.AreNotEqual(contact1, contact2);
        Assert.IsTrue(!contact1.PhoneNumbers.Where(x => x != null).Any(x => contact2.PhoneNumbers!.Any(y => object.ReferenceEquals(x, y))));
        Assert.AreEqual(contact2.Comment, comment);

        contact1.Clean();
        contact2.Clean();

        Assert.IsTrue(contact1.Equals(contact2));
        Assert.AreEqual(contact1.GetHashCode(), contact2.GetHashCode());


    }


}
