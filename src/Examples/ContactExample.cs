using FolkerKinzel.Contacts;

namespace Examples;

public static class ContactExample
{
    public static Contact[] InitializeContacts() =>
        [
            new()
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

                EmailAddresses = ["john.doe@internet.com"]
            },

            new()
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

                PhoneNumbers =
                [
                    new() {
                        Value = "0123-45678",
                        IsWork = true
                    },

                    new() {
                        Value = "876-54321",
                        IsMobile = true
                    }
                ]
            }
        ];
}
