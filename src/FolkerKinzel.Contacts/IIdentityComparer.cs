namespace FolkerKinzel.Contacts;

/// <summary>
/// Schnittstelle, die implementierende Klassen befähigt, festzustellen, ob sie zur
/// Beschreibung derselben Identität dienen könnten.
/// </summary>
public interface IIdentityComparer<T> where T : IIdentityComparer<T>
{
    /// <summary>
    /// Gibt <c>true</c> zurück, wenn einer Verschmelzung mit <paramref name="other"/> nichts entgegen steht,
    /// andernfalls <c>false</c>.
    /// </summary>
    /// <param name="other">Ein anderes <see cref="IIdentityComparer{T}"/>-Objekt oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn einer Verschmelzung mit <paramref name="other"/> nichts entgegen steht,
    /// andernfalls <c>false</c>. Wenn <see cref="other"/>&#160;<c>null</c> ist, wird <c>true</c> zurückgegeben.</returns>
    bool MayBeMerged(T? other);
}
