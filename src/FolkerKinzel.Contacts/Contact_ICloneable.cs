﻿namespace FolkerKinzel.Contacts;

public sealed partial class Contact : ICloneable
{
    /// <summary>
    /// Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <returns>Eine tiefe Kopie des Objekts.</returns>
    public object Clone() => new Contact(this);
}
