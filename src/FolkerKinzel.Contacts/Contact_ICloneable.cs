namespace FolkerKinzel.Contacts;

public sealed partial class Contact : ICloneable
{
    /// <summary>Creates a deep copy of the object instance.</summary>
    /// <returns>Deep copy of the object instance.</returns>
    public object Clone() => new Contact(this);
}
