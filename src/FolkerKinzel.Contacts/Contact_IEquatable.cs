using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : IEquatable<Contact>
{
    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        // If parameter cannot be cast to Contact return false.
        if (obj is not Contact p)
        {
            return false;
        }

        if (object.ReferenceEquals(this, obj))
        {
            return true;
        }

        // Return true if the fields match:
        return CompareBoolean(p);
    }

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] Contact? other)
    {
        // If parameter is null return false:
        if (other is null)
        {
            return false;
        }

        if (object.ReferenceEquals(this, other))
        {
            return true;
        }

        // Return true if the fields match:
        return CompareBoolean(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(TimeStamp);
        hash.Add(StringCleaner.PrepareForComparison(DisplayName));
        HashStringCollection(EmailAddresses, ref hash);
        HashMergeable(Person, ref hash);
        HashPhoneNumbers(PhoneNumbers, ref hash);
        HashMergeable(Work, ref hash);
        HashStringCollection(InstantMessengerHandles, ref hash);
        hash.Add(StringCleaner.PrepareForComparison(WebPagePersonal));
        HashMergeable(AddressHome, ref hash);
        hash.Add(StringCleaner.PrepareForComparison(WebPageWork));
        return hash.ToHashCode();

        static void HashMergeable<T>(T? mergeable, ref HashCode hash) where T : MergeableObject<T>
            => hash.Add<T?>(mergeable?.IsEmpty ?? true ? null : mergeable);

        static void HashPhoneNumbers(IEnumerable<PhoneNumber?>? coll, ref HashCode hash)
        {
            if (coll is null)
            {
                return;
            }

            foreach (PhoneNumber? item in coll)
            {
                if (item is not null && !item.IsEmpty)
                {
                    hash.Add(item);
                }
            }
        }

        static void HashStringCollection(IEnumerable<string?>? coll, ref HashCode hash)
        {
            if (coll is null)
            {
                return;
            }

            foreach (string? item in coll)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    hash.Add(item);
                }
            }
        }
    }

    /// <summary> Vergleicht die Eigenschaften mit denen eines anderen <see cref="Contact"
    /// />-Objekts. </summary>
    /// <param name="other">Das <see cref="Contact" />-Objekt, mit dem verglichen wird.</param>
    /// <returns> <c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
    private bool CompareBoolean(Contact other)
    {
        StringComparer comp = StringComparer.Ordinal;

        return TimeStamp == other.TimeStamp
            && comp.Equals(StringCleaner.PrepareForComparison(DisplayName), StringCleaner.PrepareForComparison(other.DisplayName))
            && EqualsStringCollections(EmailAddresses, other.EmailAddresses, comp)
            && EqualsMergeables(Person, other.Person)
            && EqualsPhoneNumbers(PhoneNumbers, other.PhoneNumbers)
            && EqualsMergeables(Work, other.Work)
            && EqualsStringCollections(InstantMessengerHandles, other.InstantMessengerHandles, comp)
            && EqualsMergeables(AddressHome, other.AddressHome)
            && comp.Equals(StringCleaner.PrepareForComparison(WebPagePersonal), StringCleaner.PrepareForComparison(other.WebPagePersonal))
            && comp.Equals(StringCleaner.PrepareForComparison(Comment), StringCleaner.PrepareForComparison(other.Comment))
            && comp.Equals(StringCleaner.PrepareForComparison(WebPageWork), StringCleaner.PrepareForComparison(other.WebPageWork));


        ////////////////////////////////////////////////////////////////////////////////////////////

        static bool EqualsMergeables<T>(T? x, T? y) where T : MergeableObject<T>
        {
            if (x is null || y is null || x.IsEmpty || y.IsEmpty)
            {
                return (x is null || x.IsEmpty) && (y is null || y.IsEmpty);
            }

            return x.IsMergeableWith(y);
        }

        static bool EqualsStringCollections(IEnumerable<string?>? coll1, IEnumerable<string?>? coll2, StringComparer comp)
        {
            if (ReferenceEquals(coll1, coll2))
            {
                return true;
            }

            if (coll1 is null)
            {
                return !coll2!.Any(x => !string.IsNullOrWhiteSpace(x));
            }

            if (coll2 is null)
            {
                return !coll1!.Any(x => !string.IsNullOrWhiteSpace(x));
            }

            return coll1.Select(x => StringCleaner.PrepareForComparison(x))
                        .SequenceEqual(coll2.Select(x => StringCleaner.PrepareForComparison(x)), comp);
        }

        static bool EqualsPhoneNumbers(IEnumerable<PhoneNumber?>? coll1, IEnumerable<PhoneNumber?>? coll2)
        {
            if (ReferenceEquals(coll1, coll2))
            {
                return true;
            }

            if (coll1 is null)
            {
                return !coll2!.Any(x => x is not null && !x.IsEmpty);
            }

            if (coll2 is null)
            {
                return !coll1!.Any(x => x is not null && !x.IsEmpty);
            }

            return coll1.Select(x => x is null || x.IsEmpty ? null : x).SequenceEqual(coll2.Select(x => x is null || x.IsEmpty ? null : x));
        }
    }
}
