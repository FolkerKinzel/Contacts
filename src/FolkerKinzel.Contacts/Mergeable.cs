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
    /// Gibt <c>true</c> zurück, wenn einer Verschmelzung der Daten aus <paramref name="source"/> mit denen der aktuellen
    /// Instanz nichts entgegen steht, andernfalls <c>false</c>.
    /// </summary>
    /// <param name="source">Ein anderes <see cref="Mergeable{T}"/>-Objekt oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn einer Verschmelzung mit <paramref name="source"/> nichts entgegen steht,
    /// andernfalls <c>false</c>.</returns>
    /// <remarks>
    ///  Wenn <paramref name="source"/>&#160;<c>null</c> oder <see cref="ICleanable.IsEmpty"/> ist,
    ///  gibt die Methode <c>true</c> zurück. Wenn die Eigenschaft <see cref="ICleanable.IsEmpty"/> der aktuellen Instanz
    ///  <c>true</c> zurückgibt, steht einer Verschmelzung der Daten aus <paramref name="source"/> mit denen der aktuellen
    /// Instanz ebenfalls nichts entgegen. Deshalb gibt die Methode auch in diesem Fall <c>true</c> zurück.
    /// </remarks>
    public bool CanBeMerged(T? source) => source is null || IsEmpty || source.IsEmpty || !DescribesForeignIdentity(source);


    //public abstract bool Merge(T? source);


    /// <summary>
    /// Untersucht <paramref name="other"/> daraufhin, ob es eine fremde Identität beschreibt, und
    /// deshalb nicht mit der aktuellen Instanz verschmolzen werden kann.
    /// </summary>
    /// <param name="other">Das zu untersuchende Objekt. (Nie <c>null</c> oder <see cref="ICleanable.IsEmpty"/>.)</param>
    /// <returns>Die Methode darf nur dann <c>true</c> zurückgeben, wenn die Werte der Eigenschaften von <paramref name="other"/>
    /// eine Verschmelzung mit der aktuellen Instanz unmöglich machen, andernfalls <c>false</c>.</returns>
    protected abstract bool DescribesForeignIdentity(T other);

}
