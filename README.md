# FolkerKinzel.Contacts
[![NuGet](https://img.shields.io/nuget/v/FolkerKinzel.Contacts)](https://www.nuget.org/packages/FolkerKinzel.Contacts/)


Simple .NET data model to store contact informations about organizations or natural persons.

(If you need the ability to store this as vCard (*.vcf) or CSV, have a look at 
[FolkerKinzel.Contacts.IO](https://github.com/FolkerKinzel/Contacts.IO).)

```
nuget Package Manager:
PM> Install-Package FolkerKinzel.Contacts -Version 1.3.0

.NET CLI:
> dotnet add package FolkerKinzel.Contacts --version 1.3.0

PackageReference (Visual Studio Project File):
<PackageReference Include="FolkerKinzel.Contacts" Version="1.3.0" />

Paket CLI:
> paket add FolkerKinzel.Contacts --version 1.3.0
```

* [Download Reference (English)](https://github.com/FolkerKinzel/Contacts/blob/master/FolkerKinzel.Contacts.Reference.en/Help/FolkerKinzel.Contacts.en.chm)

* [Projektdokumentation (Deutsch) herunterladen](https://github.com/FolkerKinzel/Contacts/blob/master/FolkerKinzel.Contacts.Doku.de/Help/FolkerKinzel.Contacts.de.chm)

> IMPORTANT: On some systems, the content of the CHM file is blocked. Before extracting it,
>  right click on the file, select Properties, and check the "Allow" checkbox - if it 
> is present - in the lower right corner of the General tab in the Properties dialog.

## Example
```csharp
using FolkerKinzel.Contacts;
using System;

namespace Examples
{
    static class ContactExample
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
                        Anniversary = new DateTime(2001, 6, 15)
                    },

                    Work = new Work
                    {
                        JobTitle = "Facility Manager",
                        Company = "Does Company"
                    },

                    PhoneNumbers = new PhoneNumber[]
                    {
                        new PhoneNumber
                        {
                            Value = "0123-45678",
                            IsWork = true
                        }
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
                        Anniversary = new DateTime(2001, 6, 15)
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
                            Value = "876-54321",
                            IsMobile = true
                        }
                    }
                }//new Contact()
            };//new Contact[]
    }
}

```
