using System.Collections;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : IEnumerable<Contact>
{
    /// <summary>
    /// Gibt einen Enumerator zurück, der die ausführende Instanz der <see cref="Contact"/>-Klasse
    /// zurückgibt.
    /// </summary>
    /// <returns>Ein Enumerator, der die ausführende Instanz der <see cref="Contact"/>-Klasse
    /// zurückgibt.</returns>
    /// <remarks>
    /// Diese Implementierung ermöglicht es, ein einzelnes <see cref="Contact"/>-Objekt als Argument an einen Methodenparameter vom Typ
    /// <see cref="IEnumerable{T}">IEnumerable&lt;Contact&gt;</see> zu übergeben.
    /// </remarks>
    IEnumerator<Contact> IEnumerable<Contact>.GetEnumerator()
    {
        yield return this;
    }


    /// <summary>
    /// Gibt einen Enumerator zurück, der die ausführende Instanz der <see cref="Contact"/>-Klasse
    /// zurückgibt.
    /// </summary>
    /// <returns>Ein Enumerator, der die ausführende Instanz der <see cref="Contact"/>-Klasse
    /// zurückgibt.</returns>
    /// <remarks>
    /// Diese Implementierung ermöglicht es, ein einzelnes <see cref="Contact"/>-Objekt als Argument an einen Methodenparameter vom Typ
    /// <see cref="IEnumerable"/> zu übergeben.
    /// </remarks>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Contact>)this).GetEnumerator();
}//class
