using System.Collections;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : IEnumerable<Contact>
{
    /// <summary>Returns an enumerator, which returns the executing instance of the
    /// <see cref="Contact" /> class.</summary>
    /// <returns>An enumerator, which returns the executing instance of the <see cref="Contact"
    /// /> class.</returns>
    /// <remarks>This implementation allows to pass a single <see cref="Contact" />
    /// object as an argument to a method parameter of type <see cref="IEnumerable{T}">IEnumerable&lt;Contact&gt;</see>.</remarks>
    IEnumerator<Contact> IEnumerable<Contact>.GetEnumerator()
    {
        yield return this;
    }


    /// <summary>Returns an enumerator, which returns the executing instance of the
    /// <see cref="Contact" /> class.</summary>
    /// <returns>An enumerator, which returns the executing instance of the <see cref="Contact"
    /// /> class.</returns>
    /// <remarks>This implementation allows to pass a single <see cref="Contact" />
    /// object as an argument to a method parameter of type <see cref="IEnumerable"
    /// />.</remarks>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Contact>)this).GetEnumerator();
}//class
