# FolkerKinzel.Contacts
[![NuGet](https://img.shields.io/nuget/v/FolkerKinzel.Contacts)](https://www.nuget.org/packages/FolkerKinzel.Contacts/)
[![GitHub](https://img.shields.io/github/license/FolkerKinzel/Contacts)](https://github.com/FolkerKinzel/Contacts/blob/master/LICENSE)


.NET library, which provides an easy to use data model to store contact data of organizations and natural persons.

(If you need to persist this data model as vCard (*.vcf) or CSV, have a look at [FolkerKinzel.Contacts.IO](https://github.com/FolkerKinzel/Contacts.IO).)



* [Download Project Reference (English)](https://github.com/FolkerKinzel/Contacts/blob/master/ProjectReference/2.0.0-rc.2/FolkerKinzel.Contacts.en.chm)

* [Projektdokumentation (Deutsch) herunterladen](https://github.com/FolkerKinzel/Contacts/blob/master/ProjectReference/2.0.0-rc.2/FolkerKinzel.Contacts.de.chm)

> IMPORTANT: On some systems the content of the CHM file is blocked. Before opening the file right click on the file icon, select Properties, and check the "Allow" checkbox (if it is present) in the lower right corner of the General tab in the Properties dialog.


[Version History](https://github.com/FolkerKinzel/Contacts/releases)


## Example
```csharp
using FolkerKinzel.Contacts;

namespace Examples;

public static class ContactExample
{
    public static Contact[] InitializeContacts() => new Contact[]
        {
                new Contact
                {
                    DisplayName = "John Doe",
                    Person = new Person
                    {
                        Name = new Name
                        {
                            FirstName = "John",
                            MiddleName = "William",
                            LastName = "Doe",
                            Suffix = "jr."
                        },

                        BirthDay = new DateTime(1972, 1, 3),
                        Spouse = "Jane Doe",
                        Anniversary = new DateTime(2001, 6, 15),
                        Gender = Sex.Male,
                        NickName = "The Dude"
                    },

                    Work = new Work
                    {
                        JobTitle = "Facility Manager",
                        Company = "Does Company"
                    },

                    // PhoneNumber implements IEnumerable<PhoneNumber>. So you
                    // can assign a single instance without having to wrap it 
                    // into an Array or List:
                    PhoneNumbers = new PhoneNumber
                    {
                        Value = "0123-45678",
                        IsWork = true
                    },

                    EmailAddresses = new string[]
                    {
                        "john.doe@internet.com"
                    }
                },//new Contact()

                ///////////

                new Contact
                {
                    DisplayName = "Jane Doe",
                    Person = new Person
                    {
                        Name = new Name
                        {
                            FirstName = "Jane",
                            LastName = "Doe",
                            Prefix = "Dr."
                        },
                        BirthDay = new DateTime(1981, 5, 4),
                        Spouse = "John Doe",
                        Anniversary = new DateTime(2001, 6, 15),
                        Gender = Sex.Female
                    },

                    Work = new Work
                    {
                        JobTitle = "CEO",
                        Company = "Does Company"
                    },

                    PhoneNumbers = new PhoneNumber[]
                    {
                        new PhoneNumber
                        {
                            Value = "0123-45678",
                            IsWork = true
                        },

                        new PhoneNumber
                        {
                            Value = "876-54321",
                            IsMobile = true
                        }
                    }
                }//new Contact()
        };//new Contact[]
}
```
