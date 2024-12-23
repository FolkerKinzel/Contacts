namespace FolkerKinzel.Contacts;

    /// <summary>Data model for storing contact data.</summary>
    /// <example>
    /// <para>
    /// Initialize <see cref="Contact" /> objects:
    /// </para>
    /// <code language="cs" source="..\Examples\ContactExample.cs" />
    /// </example>
public sealed partial class Contact
{
    /// <summary>Initializes an empty instance of the <see cref="Contact" /> class.</summary>
    public Contact() { }


    /// <summary> Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts und aller
    /// seiner Unterobjekte. </summary>
    /// <param name="source">Quellobjekt, dessen Inhalt kopiert wird.</param>
    private Contact(Contact source)
    {
        foreach (KeyValuePair<Prop, object> kvp in source._propDic)
        {
            this._propDic[kvp.Key] = kvp.Value switch
            {
                IEnumerable<PhoneNumber?> phones => phones.Select(x => (PhoneNumber?)x?.Clone()).ToList(),
                ICloneable adr => adr.Clone(),
                IEnumerable<string?> strings => strings.ToList(),
                _ => kvp.Value,
            };
        }
    }

}//class
