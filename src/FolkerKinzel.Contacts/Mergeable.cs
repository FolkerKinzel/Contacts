namespace FolkerKinzel.Contacts;

/// <summary>
/// Schnittstelle, die implementierende Klassen befähigt, festzustellen, ob sie zur
/// Beschreibung derselben Identität dienen könnten.
/// </summary>
/// <typeparam name="T">Generischer Typparameter.</typeparam>
public abstract class Mergeable<T> : ICleanable where T : Mergeable<T>
{
    /// <inheritdoc/>
    public abstract bool IsEmpty { get; }


    /// <inheritdoc/>
    public abstract void Clean();


    /// <summary>
    /// Untersucht, ob einer Verschmelzung der Daten aus <paramref name="source"/> mit denen der aktuellen
    /// Instanz nichts entgegen steht.
    /// </summary>
    /// <param name="source">Ein anderes <see cref="Mergeable{T}"/>-Objekt oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn einer Verschmelzung mit <paramref name="source"/> nichts entgegen steht,
    /// andernfalls <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    ///  Wenn <paramref name="source"/>&#160;<c>null</c> oder <see cref="IsEmpty"/> ist,
    ///  gibt die Methode <c>true</c> zurück. Wenn die Eigenschaft <see cref="IsEmpty"/> der aktuellen Instanz
    ///  <c>true</c> zurückgibt, steht einer Verschmelzung der Daten aus <paramref name="source"/> mit denen der aktuellen
    /// Instanz ebenfalls nichts entgegen. Deshalb gibt die Methode auch in diesem Fall <c>true</c> zurück.
    /// </para>
    /// <para>
    /// Die Methode ist geeignet, Kandidaten für die Zusammenführung ihrer Daten zu finden ("Doubletten"). Die Methode
    /// kann nicht feststellen, ob die Zusammenführung der Daten sinnvoll ist. Es wäre eine gute Praxis, die Benutzer der
    /// Anwendung darüber entscheiden zu lassen.
    /// </para>
    /// </remarks>
    public bool CanBeMerged(T? source) => source is null || IsEmpty || source.IsEmpty || !DescribesForeignIdentity(source);


    /// <summary>
    /// Untersucht, ob einer Verschmelzung der Daten von <paramref name="obj1"/> und <paramref name="obj2"/> nichts entgegen steht.
    /// </summary>
    /// <param name="obj1">Das erste zu untersuchende Objekt.</param>
    /// <param name="obj2">Das zweite zu untersuchende Objekt.</param>
    /// <returns><c>true</c>, wenn einer Verschmelzung von <paramref name="obj1"/> und <paramref name="obj2"/> nichts entgegen steht,
    /// andernfalls <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    ///  Wenn eines der zu untersuchenden Objekte <c>null</c> oder <see cref="IsEmpty"/> ist,
    ///  gibt die Methode <c>true</c> zurück.
    /// </para>
    /// <para>
    /// Die Methode ist geeignet, Kandidaten für die Zusammenführung ihrer Daten zu finden ("Doubletten"). Die Methode
    /// kann nicht feststellen, ob die Zusammenführung der Daten sinnvoll ist. Es wäre eine gute Praxis, die Benutzer der
    /// Anwendung darüber entscheiden zu lassen.
    /// </para>
    /// </remarks>
    public static bool CanBeMerged(T? obj1, T? obj2) => obj1?.CanBeMerged(obj2) ?? true;


    /// <summary>
    /// Ergänzt die ausführende Instanz mit den Daten von <paramref name="source"/>. Es werden dabei keine vorhandenen Daten
    /// in der ausführenden Instanz überschrieben.
    /// </summary>
    /// <param name="source">Das Quellobjekt, dessen Daten zu den Daten der ausführenden Instanz hinzugefügt werden, ohne vorhandene 
    /// Daten zu überschreiben - soweit dies möglich ist.</param>
    /// <returns>Eine Referenz auf das ausführende Objekt, um Aufrufe verketten zu können.</returns>
    /// <remarks>
    /// <para>
    /// Die Methode führt keinerlei Überprüfung durch, ob eine Zusammenführung der in der Instanz gespeicherten Daten mit denen 
    /// von <paramref name="source"/> sinnvoll ist. Prüfen Sie dies vorher mit <see cref="CanBeMerged(T?)"/> und lassen Sie sich
    /// das Ergebnis der Prüfung von den Benutzern der Anwendung bestätigen.
    /// </para>
    /// <para>
    /// Wenn <paramref name="source"/>&#160;<c>null</c> oder <see cref="IsEmpty"/> ist, werden keine Daten kopiert.
    /// </para>
    /// <para>
    /// Bei der Verschmelzung zweier <see cref="Mergeable{T}"/>-Instanzen ist das Ergebnis davon abhängig, auf welcher der beiden 
    /// Instanzen die Methode aufgerufen wird. Die Erhaltung der Daten der Instanz, auf der die Methode aufgerufen wird, hat Priorität.
    /// Es liegt in der Verantwortung der ausführenden Anwendung, die Methode auf der geeigneteren der beiden Instanzen 
    /// aufzurufen.
    /// </para>
    /// </remarks>
    public T Merge(T? source)
    {
        if(source is not null && !source.IsEmpty)
        {
            CompleteDataWith(source);
        }
        return (T)this;
    }


    /// <summary>
    /// Führt die Daten von <paramref name="source"/> mit denen der aktuellen Instanz zusammen, ohne vorhandene Daten zu überschreiben.
    /// </summary>
    /// <param name="source">Das Quellobjekt. (Nie <c>null</c> oder <see cref="IsEmpty"/>.)</param>
    /// <remarks>
    /// <note type="inherit">
    /// Beim Überschreiben der Methode in erbenden Klassen muss das Versprechen eingehalten werden, dass die Methode keine vorhandenen
    /// Daten überschreibt!
    /// </note>
    /// </remarks>
    protected abstract void CompleteDataWith(T source);


    /// <summary>
    /// Untersucht <paramref name="other"/> daraufhin, ob es eine fremde Identität beschreibt, und
    /// deshalb nicht mit der aktuellen Instanz verschmolzen werden kann.
    /// </summary>
    /// <param name="other">Das zu untersuchende Objekt. (Nie <c>null</c> oder <see cref="IsEmpty"/>.)</param>
    /// <returns>Die Methode darf nur dann <c>true</c> zurückgeben, wenn die Werte der Eigenschaften von <paramref name="other"/>
    /// eine Verschmelzung mit der aktuellen Instanz unmöglich machen, andernfalls <c>false</c>.</returns>
    protected abstract bool DescribesForeignIdentity(T other);

}
