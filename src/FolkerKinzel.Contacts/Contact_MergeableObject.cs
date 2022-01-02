using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : MergeableObject<Contact>
{
    /// <inheritdoc/>
    protected override bool DescribesForeignIdentity(Contact other)
    {
        StringComparer comp = StringComparer.Ordinal;

        if(UrlCollectionHasEvidence(EmailAddresses, other.EmailAddresses, comp, out bool isDifferentIdentity) && !isDifferentIdentity)
        {
            return false;
        }

        if (UrlCollectionHasEvidence(InstantMessengerHandles, other.InstantMessengerHandles, comp, out isDifferentIdentity) && !isDifferentIdentity)
        {
            return false;
        }

        if (HasEvidence(Person, other.Person, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if (HasEvidence(Work, other.Work, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if (DisplayNameHasEvidence(DisplayName, other.DisplayName, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if(PhoneNumberCollectionHasEvidence(PhoneNumbers, other.PhoneNumbers, out isDifferentIdentity) && !isDifferentIdentity)
        {
            return false;
        }

        if (UrlHasEvidence(WebPagePersonal, other.WebPagePersonal, comp, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if (HasEvidence(AddressHome, other.AddressHome, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }


        if (UrlHasEvidence(WebPageWork, other.WebPageWork, comp, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        return false;

        /////////////////////////////////////

        static bool UrlCollectionHasEvidence(IEnumerable<string?>? urls1, IEnumerable<string?>? urls2, StringComparer comp, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (urls1 == null || urls2 == null)
            {
                return false;
            }

            var arr1 = urls1.Where(x => !Strip.IsEmpty(x)).ToArray();
            var arr2 = urls2.Where(x => !Strip.IsEmpty(x)).ToArray();

            if (arr1.Length != 0 && arr2.Length != 0)
            {
                isDifferentIdentity = !arr1.Intersect(arr2, comp).Any();
                return true;
            }

            return false;
        }

        static bool PhoneNumberCollectionHasEvidence(IEnumerable<PhoneNumber?>? urls1, IEnumerable<PhoneNumber?>? urls2, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (urls1 == null || urls2 == null)
            {
                return false;
            }

            PhoneNumber[] arr1 = urls1.Where(x => x != null && !x.IsEmpty).ToArray()!;
            PhoneNumber[] arr2 = urls2.Where(x => x != null && !x.IsEmpty).ToArray()!;

            if (arr1.Length != 0 && arr2.Length != 0)
            {
                isDifferentIdentity = !arr1.Intersect(arr2, PhoneNumberComparer.Instance).Any();
                return true;
            }

            return false;
        }


        static bool HasEvidence<T>(T? p1, T? p2, out bool isDifferentIdentity) where T : MergeableObject<T>
        {
            isDifferentIdentity = true;

            if (p1 is null || p2 is null || p1.IsEmpty || p2.IsEmpty)
            {
                return false;
            }

            isDifferentIdentity = !p1.IsMergeableWith(p2);
            return true;
        }


        static bool DisplayNameHasEvidence(string? dp1, string? dp2, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (string.IsNullOrWhiteSpace(dp1) || string.IsNullOrWhiteSpace(dp2))
            {
                return false;
            }

            isDifferentIdentity = !Strip.StartEqual(dp1, dp2, true);
            return true;
        }

        static bool UrlHasEvidence(string? dp1, string? dp2, StringComparer comp, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (string.IsNullOrWhiteSpace(dp1) || string.IsNullOrWhiteSpace(dp2))
            {
                return false;
            }

            isDifferentIdentity = !comp.Equals(dp1, dp2);
            return true;

        }

    }

    /// <inheritdoc/>
    protected override void CompleteDataWith(Contact source)
    {
        if (TimeStamp < source.TimeStamp)
        {
            TimeStamp = source.TimeStamp;
        }

        Person = MergeMergeableObjects(Person, source.Person);
        Work = MergeMergeableObjects(Work, source.Work);
        AddressHome = MergeMergeableObjects(AddressHome, source.AddressHome);
       
        if (Strip.IsEmpty(DisplayName))
        {
            DisplayName = source.DisplayName;
        }

        if (Strip.IsEmpty(WebPagePersonal))
        {
            WebPagePersonal = source.WebPagePersonal;
        }

        if (Strip.IsEmpty(WebPageWork))
        {
            WebPageWork = source.WebPageWork;
        }

        string? comment = Comment;

        if (string.IsNullOrWhiteSpace(comment))
        {
            Comment = source.Comment;
        }
        else
        {
            string? sourceComment = source.Comment;

            if (!string.IsNullOrWhiteSpace(source.Comment))
            {
                Comment = string.Concat(comment, Environment.NewLine, Environment.NewLine, sourceComment);
            }
        }

        MergeEmailAddresses(source);

        MergeInstantMessengerHandles(source);

        MergePhoneNumbers(source);

        /////////////////////////////////////////////////
        
        static T? MergeMergeableObjects<T>(T? x, T? source) where T : MergeableObject<T>, ICloneable
        {
            if (x?.IsMergeableWith(source) ?? true)
            {
                return x?.Merge(source) ?? (T?)source?.Clone();
            }

            return x;
        }


        void MergeEmailAddresses(Contact source)
        {
            IEnumerable<string?>? emailAdresses = EmailAddresses;

            if (emailAdresses is null)
            {
                EmailAddresses = source.EmailAddresses?.ToArray();
            }
            else
            {
                IEnumerable<string?>? sourceEmailAdresses = source.EmailAddresses;

                if (sourceEmailAdresses is not null)
                {
                    var list = emailAdresses.ToList();
                    list.AddRange(sourceEmailAdresses);
                    EmailAddresses = list.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                }
            }
        }

        void MergeInstantMessengerHandles(Contact source)
        {
            IEnumerable<string?>? imAddresses = InstantMessengerHandles;

            if (imAddresses is null)
            {
                InstantMessengerHandles = source.InstantMessengerHandles?.ToArray();
            }
            else
            {
                IEnumerable<string?>? sourceIMAddresses = source.InstantMessengerHandles;

                if (sourceIMAddresses is not null)
                {
                    var list = imAddresses.ToList();
                    list.AddRange(sourceIMAddresses);
                    InstantMessengerHandles = list.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                }
            }
        }

        void MergePhoneNumbers(Contact source)
        {
            IEnumerable<PhoneNumber?>? phoneNumbers = PhoneNumbers;

            if (phoneNumbers is null)
            {
                PhoneNumbers = source.PhoneNumbers?.ToArray();
            }
            else
            {
                IEnumerable<PhoneNumber?>? sourcePhoneNumbers = source.PhoneNumbers;

                if (sourcePhoneNumbers is not null)
                {
                    var list = phoneNumbers.ToList();
                    list.AddRange(sourcePhoneNumbers.Select(x => (PhoneNumber?)x?.Clone()));

                    IGrouping<PhoneNumber, PhoneNumber>[] groups = list.Where(x => x != null && !x.IsEmpty).GroupBy(x => x, PhoneNumberComparer.Instance).ToArray()!;

                    list.Clear();

                    foreach (IGrouping<PhoneNumber, PhoneNumber> group in groups)
                    {
                        PhoneNumber number = group.Key;
                        list.Add(number);

                        foreach (PhoneNumber telNumber in group)
                        {
                            _ = number.Merge(telNumber);
                        }
                    }

                    PhoneNumbers = list;
                }
            }
        }
    }

}
