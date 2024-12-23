namespace FolkerKinzel.Contacts;

    /// <summary>Interface, that enables the implementing class to clean itself of data
    /// garbage.</summary>
public interface ICleanable
{
    /// <summary>Cleans the data stored in the object: Removes e.g. empty sub-objects
    /// or superfluous white space.</summary>
    void Clean();


    /// <summary>Returns <c>true</c> if the object does not contain any usable data,
    /// otherwise <c>false</c>.</summary>
    bool IsEmpty { get; }
}
