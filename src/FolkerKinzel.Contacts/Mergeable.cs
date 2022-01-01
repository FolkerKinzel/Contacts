namespace FolkerKinzel.Contacts;

/// <summary>
/// Abstrakte Basisklasse, die Methoden bereitstellt, die Instanzen abgeleiteter Klassen dazu befähigt, ihre Daten miteinander
/// zu verschmelzen.
/// </summary>
/// <typeparam name="T">Generischer Typparameter, der stellvertretend für eine abgeleitete Klasse steht.</typeparam>
public abstract class Mergeable<T> : ICleanable where T : Mergeable<T>
{
    /// <inheritdoc/>
    public abstract bool IsEmpty { get; }


    /// <inheritdoc/>
    public abstract void Clean();


    /// <summary>
    /// Untersucht, ob einer Verschmelzung der Daten aus <paramref name="other"/> mit denen der aktuellen
    /// Instanz nichts entgegen steht.
    /// </summary>
    /// <param name="other">Ein anderes <see cref="Mergeable{T}"/>-Objekt oder <c>null</c>.</param>
    /// <returns><c>true</c>, wenn einer Verschmelzung mit <paramref name="other"/> nichts entgegen steht,
    /// andernfalls <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    ///  Wenn <paramref name="other"/>&#160;<c>null</c> oder <see cref="IsEmpty"/> ist,
    ///  gibt die Methode <c>true</c> zurück. Wenn die Eigenschaft <see cref="IsEmpty"/> der aktuellen Instanz
    ///  <c>true</c> zurückgibt, steht einer Verschmelzung der Daten aus <paramref name="other"/> mit denen der aktuellen
    /// Instanz ebenfalls nichts entgegen. Deshalb gibt die Methode auch in diesem Fall <c>true</c> zurück.
    /// </para>
    /// <para>
    /// Die Methode kann geeignete Kandidaten für eine Verschmelzung ihrer Daten finden ("Doubletten"); sie
    /// kann aber nicht feststellen, ob die Zusammenführung der Daten sinnvoll ist. Es wäre eine gute Praxis, die Benutzer der
    /// Anwendung darüber entscheiden zu lassen.
    /// </para>
    /// </remarks>
    public bool IsMergeableWith(T? other) => other is null || IsEmpty || other.IsEmpty || !DescribesForeignIdentity(other);


    /// <summary>
    /// Untersucht, ob einer Verschmelzung der Daten von <paramref name="mergeable1"/> und <paramref name="mergeable2"/> nichts entgegen steht.
    /// </summary>
    /// <param name="mergeable1">Das erste zu untersuchende Objekt.</param>
    /// <param name="mergeable2">Das zweite zu untersuchende Objekt.</param>
    /// <returns><c>true</c>, wenn einer Verschmelzung von <paramref name="mergeable1"/> und <paramref name="mergeable2"/> nichts entgegen steht,
    /// andernfalls <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    ///  Wenn eines der zu untersuchenden Objekte <c>null</c> oder <see cref="IsEmpty"/> ist,
    ///  gibt die Methode <c>true</c> zurück.
    /// </para>
    /// <para>
    /// Die Methode kann geeignete Kandidaten für eine Verschmelzung ihrer Daten finden ("Doubletten"); sie
    /// kann aber nicht feststellen, ob die Zusammenführung der Daten sinnvoll ist. Es wäre eine gute Praxis, die Benutzer der
    /// Anwendung darüber entscheiden zu lassen.
    /// </para>
    /// </remarks>
    public static bool AreMergeable(T? mergeable1, T? mergeable2) => mergeable1?.IsMergeableWith(mergeable2) ?? true;


    /// <summary>
    /// Ergänzt die ausführende Instanz mit den Daten von <paramref name="source"/>. Es werden dabei in der aktuellen Instanz keine vorhandenen Daten
    /// überschrieben.
    /// </summary>
    /// <param name="source">Das Quellobjekt, mit dessen Daten die ausführenden Instanz vervollständigt wird.</param>
    /// <returns>Eine Referenz auf die ausführende Instanz, um Aufrufe verketten zu können.</returns>
    /// <remarks>
    /// <para>
    /// Die Methode führt keinerlei Überprüfung durch, ob die Ergänzung der Daten der aktuellen Instanz mit denen 
    /// von <paramref name="source"/> sinnvoll ist. Prüfen Sie dies vorher mit der Instanzmethode <see cref="IsMergeableWith(T?)"/> 
    /// oder der statischen Methode <see cref="AreMergeable(T?, T?)"/> und lassen Sie sich
    /// das Ergebnis der Prüfung möglichst von den Benutzern der Anwendung bestätigen.
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
    /// Ergänzt die ausführende Instanz mit den Daten von <paramref name="source"/>, ohne vorhandene Daten dabei zu überschreiben.
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
