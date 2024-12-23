namespace FolkerKinzel.Contacts;

public sealed partial class Contact
{
    /// <summary>Overloads the != operator.</summary>
    /// <remarks>Compares two <see cref="Contact" /> objects to determine whether they
    /// are equal.</remarks>
    /// <param name="contact1">Left operand.</param>
    /// <param name="contact2">Right operand.</param>
    /// <returns> <c>true</c> if <paramref name="contact1" /> and <paramref name="contact2"
    /// /> are equal, otherwise <c>false</c>.</returns>
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


    /// <summary>Overloads the != operator.</summary>
    /// <remarks>Compares two <see cref="Contact" /> objects to determine whether they
    /// are not equal.</remarks>
    /// <param name="contact1">Left operand.</param>
    /// <param name="contact2">Right operand.</param>
    /// <returns> <c>true</c> if <paramref name="contact1" /> and <paramref name="contact2"
    /// /> are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Contact? contact1, Contact? contact2) => !(contact1 == contact2);


}
