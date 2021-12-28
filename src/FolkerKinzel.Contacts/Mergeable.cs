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


    /// <summary>
    /// Untersucht <paramref name="other"/> daraufhin, ob es eine andere Identität beschreibt, und
    /// deshalb nicht mit der aktuellen Instanz verschmolzen werden kann.
    /// </summary>
    /// <param name="other">Das zu untersuchende Objekt. (Nie <c>null</c> oder <see cref="ICleanable.IsEmpty"/>.)</param>
    /// <returns>Die Methode darf nur dann <c>true</c> zurückgeben, wenn die Eigenschaften von <paramref name="other"/>
    /// eine Verschmelzung mit der aktuellen Instanz unmöglich machen, andernfalls <c>false</c>.</returns>
    protected abstract bool BelongsToOtherIdentity(T other);
}
