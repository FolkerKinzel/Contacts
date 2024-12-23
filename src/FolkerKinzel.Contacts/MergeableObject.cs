namespace FolkerKinzel.Contacts;

    /// <summary>Abstract base class, which provides methods that enable instances of
    /// derived classes to merge their data with one another.</summary>
    /// <typeparam name="T">Generic type parameter that represents a derived class.</typeparam>
public abstract class MergeableObject<T> : ICleanable where T : MergeableObject<T>
{
    /// <inheritdoc />
    public abstract bool IsEmpty { get; }


    /// <inheritdoc />
    public abstract void Clean();


    /// <summary>Determines whether there is nothing to prevent the data from <paramref
    /// name="other" /> from being merged with those of the current instance.</summary>
    /// <param name="other">Another <see cref="MergeableObject{T}" /> or <c>null</c>.</param>
    /// <returns> <c>true</c> if there is nothing to prevent a merging with <paramref
    /// name="other" />, otherwise <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    /// If <paramref name="other" /> is <c>null</c> or <see cref="IsEmpty" />, the method
    /// returns <c>true</c>. If the property <see cref="IsEmpty" /> of the current instance
    /// returns <c>true</c>, there is also nothing that prevents a merging of the data
    /// from <paramref name="other" /> with those of the current instance. Therefore
    /// the method returns <c>true</c> in this case as well.
    /// </para>
    /// <para>
    /// The method can find suitable candidates for a merging of their data ("doublets");
    /// however, it cannot determine whether the merging of the data makes sense. It
    /// would be a good practice to let the users of the application decide that.
    /// </para>
    /// </remarks>
    public bool IsMergeableWith([NotNullWhen(false)] T? other) => other is null || IsEmpty || other.IsEmpty || !DescribesForeignIdentity(other);


    /// <summary>Examines whether there is nothing to prevent the merging of the data
    /// from <paramref name="mergeable1" /> and <paramref name="mergeable2" />.</summary>
    /// <param name="mergeable1">The first object to be examined.</param>
    /// <param name="mergeable2">The second object to be examined.</param>
    /// <returns> <c>true</c> if there is nothing to prevent a merging of <paramref
    /// name="mergeable1" /> and <paramref name="mergeable2" />, otherwise <c>false</c>.</returns>
    /// <remarks>
    /// <para>
    /// If one of the objects to be examined is <c>null</c> or <see cref="IsEmpty" />,
    /// the method returns <c>true</c>.
    /// </para>
    /// <para>
    /// The method can find suitable candidates for a merging of their data ("doublets");
    /// however, it cannot determine whether the merging of the data makes sense. It
    /// would be a good practice to let the users of the application decide that.
    /// </para>
    /// </remarks>
    public static bool AreMergeable([NotNullWhen(false)]T? mergeable1, [NotNullWhen(false)] T? mergeable2) => mergeable1?.IsMergeableWith(mergeable2) ?? true;


    /// <summary>Supplements the executing instance with the data from <paramref name="source"
    /// />. No existing data will be overwritten in the current instance.</summary>
    /// <param name="source">The source object whose data is used to supplement the
    /// executing instance.</param>
    /// <returns>A reference to the executing instance in order to be able to chain
    /// calls.</returns>
    /// <remarks>
    /// <para>
    /// The method does not check whether it makes sense to supplement the data of the
    /// current instance with those from <paramref name="source" />. Check this beforehand
    /// with <see cref="IsMergeableWith(T?)" /> and have the result of the check confirmed
    /// by the users of the application, if possible.
    /// </para>
    /// <para>
    /// If <paramref name="source" /> is <c>null</c> or <see cref="IsEmpty" /> no data
    /// will be copied.
    /// </para>
    /// <para>
    /// When merging two <see cref="MergeableObject{T}" /> instances the result depends
    /// on which of the two instances the method is called on. Preserving the data of
    /// the instance on which the method is called has priority. It is the responsibility
    /// of the executing application to call the method on the more appropriate of the
    /// two instances.
    /// </para>
    /// </remarks>
    public T Merge(T? source)
    {
        if(source is not null && !source.IsEmpty && !ReferenceEquals(this, source))
        {
            SupplementWith(source);
        }
        return (T)this;
    }


    /// <summary>Supplements the executing instance with the data from <paramref name="source"
    /// /> without overwriting existing data.</summary>
    /// <param name="source">The source object. (Never <c>null</c> or <see cref="IsEmpty"
    /// />.)</param>
    /// <remarks>
    /// <note type="inherit">
    /// When overwriting the method in inheriting classes, the promise must be kept
    /// that the method will not overwrite any existing data!
    /// </note>
    /// </remarks>
    protected abstract void SupplementWith(T source);


    /// <summary>Examines <paramref name="other" /> to see whether it describes a foreign
    /// identity and therefore cannot be merged with the current instance.</summary>
    /// <param name="other">The object to be examined. (Never <c>null</c> or <see cref="IsEmpty"
    /// />.)</param>
    /// <returns>The method may only return <c>true</c> if the values of the properties
    /// of <paramref name="other" /> make it impossible to merge <paramref name="other"
    /// /> with the current instance, otherwise <c>false</c>.</returns>
    protected abstract bool DescribesForeignIdentity(T other);

}
