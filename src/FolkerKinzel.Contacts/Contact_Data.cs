namespace FolkerKinzel.Contacts;

public sealed partial class Contact
{
    private readonly Dictionary<Prop, object> _propDic = [];

    private enum Prop
    {
        DisplayName,
        Person,
        AddressHome,
        EmailAdresses,
        PhoneNumbers,
        InstantMessengerHandles,
        WebPagePersonal,
        WebPageWork,
        Work,
        Comment,
        TimeStamp
    }

    #region Accessor Methods

    [return: MaybeNull]
    private T Get<T>(Prop prop) => _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;


    private void Set(Prop prop, object? value)
    {
        if (value is null)
        {
            _ = _propDic.Remove(prop);
        }
        else
        {
            _propDic[prop] = value;
        }
    }

    #endregion


    /// <summary>Display name</summary>
    public string? DisplayName
    {
        get => Get<string?>(Prop.DisplayName);
        set => Set(Prop.DisplayName, value);
    }

    /// <summary>Personal data</summary>
    public Person? Person
    {
        get => Get<Person?>(Prop.Person);
        set => Set(Prop.Person, value);
    }

    /// <summary>Email addresses</summary>
    public IEnumerable<string?>? EmailAddresses
    {
        get => Get<IEnumerable<string?>?>(Prop.EmailAdresses);
        set => Set(Prop.EmailAdresses, value);
    }

    /// <summary>Instant messenger handles</summary>
    public IEnumerable<string?>? InstantMessengerHandles
    {
        get => Get<IEnumerable<string?>?>(Prop.InstantMessengerHandles);
        set => Set(Prop.InstantMessengerHandles, value);
    }

    /// <summary>Phone numbers</summary>
    public IEnumerable<PhoneNumber?>? PhoneNumbers
    {
        get => Get<IEnumerable<PhoneNumber?>?>(Prop.PhoneNumbers);
        set => Set(Prop.PhoneNumbers, value);
    }

    /// <summary>Postal address (personal)</summary>
    public Address? AddressHome
    {
        get => Get<Address?>(Prop.AddressHome);
        set => Set(Prop.AddressHome, value);
    }

    /// <summary>Organizational data</summary>
    public Work? Work
    {
        get => Get<Work?>(Prop.Work);
        set => Set(Prop.Work, value);
    }

    /// <summary>Personal homepage</summary>
    public string? WebPagePersonal
    {
        get => Get<string?>(Prop.WebPagePersonal);
        set => Set(Prop.WebPagePersonal, value);
    }

    /// <summary>Business homepage</summary>
    public string? WebPageWork
    {
        get => Get<string?>(Prop.WebPageWork);
        set => Set(Prop.WebPageWork, value);
    }

    /// <summary>Notes</summary>
    public string? Comment
    {
        get => Get<string?>(Prop.Comment);
        set => Set(Prop.Comment, value);
    }

    /// <summary>Last Changed</summary>
    public DateTimeOffset TimeStamp
    {
        get => Get<DateTimeOffset>(Prop.TimeStamp);
        set => Set(Prop.TimeStamp, value == default ? null : (object)value);
    }
}
