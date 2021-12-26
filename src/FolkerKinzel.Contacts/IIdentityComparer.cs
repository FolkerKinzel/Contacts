﻿namespace FolkerKinzel.Contacts;

/// <summary>
/// Schnittstelle, die implementierende Klassen befähigt, festzustellen, ob sie zur
/// Beschreibung derselben Identität dienen könnten.
/// </summary>
public interface IIdentityComparer
{
    /// <summary>
    /// Gibt <c>true</c> zurück, wenn <paramref name="other"/> zur Beschreibung derselben
    /// Identität dienen könnte, anderenfalls <c>false</c>.
    /// </summary>
    /// <param name="other">Ein anderes <see cref="IIdentityComparer"/>-Objekt oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn <paramref name="other"/> zur Beschreibung derselben
    /// Identität dienen könnte, anderenfalls <c>false</c>.</returns>
    bool IsProbablyTheSame(IIdentityComparer? other);
}
