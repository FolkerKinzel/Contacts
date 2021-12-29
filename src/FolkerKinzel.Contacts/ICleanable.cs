namespace FolkerKinzel.Contacts;

/// <summary>
/// Interface, das das implementierende Objekt befähigt, sich selbst von Datenmüll zu reinigen.
/// </summary>
public interface ICleanable
{
    /// <summary>
    /// Reinigt die im Objekt gespeicherten Daten: Entfernt z.B. leere Unterobjekte oder überflüssige Leerzeichen.
    /// </summary>
    void Clean();


    /// <summary>
    /// Gibt <c>true</c> zurück, wenn das Objekt keine verwertbaren Daten enthält, andernfalls <c>false</c>.
    /// </summary>
    bool IsEmpty { get; }
}
