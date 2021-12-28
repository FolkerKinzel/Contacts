namespace FolkerKinzel.Contacts;

/// <summary>
/// Schnittstelle, die implementierende Klassen befähigt, festzustellen, ob sie zur
/// Beschreibung derselben Identität dienen könnten.
/// </summary>
public abstract class Mergeable<T> : ICleanable where T : Mergeable<T>
{

    /// <inheritdoc/>
    public abstract bool IsEmpty { get; }


    /// <inheritdoc/>
    public abstract void Clean();


    /// <summary>
    /// Gibt <c>true</c> zurück, wenn einer Verschmelzung mit <paramref name="other"/> nichts entgegen steht,
    /// andernfalls <c>false</c>.
    /// </summary>
    /// <param name="other">Ein anderes <see cref="Mergeable{T}"/>-Objekt oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn einer Verschmelzung mit <paramref name="other"/> nichts entgegen steht,
    /// andernfalls <c>false</c>. Wenn <paramref name="other"/>&#160;<c>null</c> ist, wird <c>true</c> zurückgegeben.</returns>
    public bool CanBeMergedWith(T? other) => other is null || IsEmpty || other.IsEmpty || !BelongsToOtherIdentity(other);


    protected abstract bool BelongsToOtherIdentity(T other);
}
