using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : ICleanable
{
    /// <inheritdoc />
    public override bool IsEmpty
    {
        get
        {
            foreach (KeyValuePair<Prop, object> kvp in _propDic)
            {
                switch (kvp.Value)
                {
                    case ICleanable cleanable when !cleanable.IsEmpty:
                    case string s when !string.IsNullOrWhiteSpace(s):
                    case DateTime dt when dt != default:
                    case IEnumerable<string?> strColl when strColl.Any(x => !string.IsNullOrWhiteSpace(x)):
                    case IEnumerable<ICleanable?> cleanableColl when cleanableColl.Any(x => !(x?.IsEmpty ?? true)):
                        return false;
                }
            }

            return true;
        }
    }


    /// <inheritdoc />
    public override void Clean()
    {
        KeyValuePair<Prop, object>[]? props = _propDic.ToArray();
        StringComparer comp = StringComparer.Ordinal;

        for (int i = 0; i < props.Length; i++)
        {
            KeyValuePair<Prop, object> kvp = props[i];

            switch (kvp.Value)
            {
                case IEnumerable<string?> strings:
                    {
                        string[] arr = strings.Select(x => StringCleaner.CleanDataEntry(x)).Where(x => x != null).Distinct(comp).ToArray()!;

                        if (arr.Length == 0)
                        {
                            Set(kvp.Key, null);
                        }
                        else
                        {
                            Set(kvp.Key, arr);
                        }
                    }
                    break;
                case IEnumerable<PhoneNumber?> phoneNumbers:
                    {
                        List<PhoneNumber> numbers = phoneNumbers.Where(x => x != null && !x.IsEmpty).ToList()!;
                        numbers.ForEach(x => x.Clean());

                        IGrouping<PhoneNumber, PhoneNumber>[] groups = numbers.GroupBy(x => x, PhoneNumberComparer.Instance).ToArray();

                        numbers.Clear();

                        foreach (IGrouping<PhoneNumber, PhoneNumber> group in groups)
                        {
                            PhoneNumber number = group.Key;
                            numbers.Add(number);

                            foreach (PhoneNumber telNumber in group)
                            {
                                number.Merge(telNumber);
                            }
                        }

                        if (numbers.Count == 0)
                        {
                            Set(kvp.Key, null);
                        }
                        else
                        {
                            numbers.TrimExcess();
                            Set(kvp.Key, numbers);
                        }
                    }
                    break;
                case string s:
                    {
                        if (kvp.Key == Prop.Comment)
                        {
                            Set(kvp.Key, StringCleaner.CleanComment(s));
                        }
                        else
                        {
                            Set(kvp.Key, StringCleaner.CleanDataEntry(s));
                        }
                    }
                    break;
                case DateTime dt when dt.IsEmptyTimeStamp():
                    {
                        Set(kvp.Key, null);
                    }
                    break;
                case ICleanable adr:
                    {
                        adr.Clean();
                        if (adr.IsEmpty)
                        {
                            Set(kvp.Key, null);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

#if !NETSTANDARD2_0 && !NET462
        _propDic.TrimExcess();
#endif

    }

}
