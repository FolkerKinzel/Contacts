namespace FolkerKinzel.Contacts;

public sealed partial class Contact
{
    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Contact"/>-Objekte, um zu überprüfen, ob sie gleich sind.
    /// </remarks>
    /// <param name="contact1">Linker Operand.</param>
    /// <param name="contact2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="contact1"/> und <paramref name="contact2"/> gleich sind.</returns>
    public static bool operator ==(Contact? contact1, Contact? contact2)
    {
        // If both are null, or both are same instance, return true.
        if (object.ReferenceEquals(contact1, contact2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (contact1 is null)
        {
            return false; // auf Referenzgleichheit wurde oben geprüft
        }
        else
        {
            return contact2 is not null && contact1.CompareBoolean(contact2);
        }
    }


    /// <summary>
    /// Überladung des != Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Contact"/>-Objekte, um zu überprüfen, ob sie ungleich sind.
    /// </remarks>
    /// <param name="contact1">Linker Operand.</param>
    /// <param name="contact2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="contact1"/> und <paramref name="contact2"/> ungleich sind.</returns>
    public static bool operator !=(Contact? contact1, Contact? contact2) => !(contact1 == contact2);


}
