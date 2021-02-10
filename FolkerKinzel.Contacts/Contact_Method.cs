using FolkerKinzel.Contacts.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolkerKinzel.Contacts
{
    public sealed partial class Contact
    {
        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Contact"/>-Objekts.
        /// </summary>
        /// <returns>Der Inhalt des <see cref="Contact"/>-Objekts als <see cref="string"/>.</returns>
        public override string ToString()
        {
            if (IsEmpty)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            Prop[] keys = _propDic.Keys.OrderBy(x => x).ToArray();
            string[] topics = new string[keys.Length];

            const string indent = "        ";

            for (int i = 0; i < keys.Length; i++)
            {
                Prop key = keys[i];

                var value = _propDic[keys[i]];

                switch (value)
                {
                    case Person person:
                        _ = sb.AppendLine(Res.Person);
                        _ = person.AppendTo(sb, indent);
                        _ = sb.AppendLine(Environment.NewLine);
                        break;
                    case Address address:
                        _ = sb.AppendLine(Res.AddressHome);
                        _ = address.AppendTo(sb, indent);
                        _ = sb.AppendLine(Environment.NewLine);
                        break;
                    case IEnumerable<string?> strings:
                        _ = sb.AppendLine(keys[i] == Prop.EmailAdresses ? Res.EmailAddresses : Res.InstantMessengers);
                        foreach (var str in strings)
                        {
                            _ = sb.Append(indent).AppendLine(str);
                        }
                        _ = sb.AppendLine();
                        break;
                    case IEnumerable<PhoneNumber?> phoneNumbers:
                        _ = sb.AppendLine(Res.PhoneNumbers);
                        foreach (PhoneNumber? phoneNumber in phoneNumbers)
                        {
                            if (phoneNumber != null)
                            {
                                _ = phoneNumber.AppendTo(sb, indent);
                                _ = sb.AppendLine();
                            }
                        }
                        _ = sb.AppendLine();
                        break;    
                    case Work work:
                        _ = sb.AppendLine(Res.Work);
                        _ = work.AppendTo(sb, indent);
                        _ = sb.AppendLine(Environment.NewLine);
                        break;
                    case DateTime dt:
                        _ = sb.AppendLine(Res.TimeStamp);
                        _ = sb.Append(indent).Append(dt.ToShortDateString()).Append(' ').AppendLine(dt.ToLongTimeString());
                        _ = sb.AppendLine();
                        break;
                    default:
                        string header = key switch
                        {
                            Prop.DisplayName => Res.DisplayName,
                            Prop.WebPagePersonal => Res.HomePagePersonal,
                            Prop.WebPageWork => Res.HomePageWork,
                            Prop.Comment => Res.Comment,
                            _ => ""
                        };
                        _ = sb.AppendLine(header);

                        _ = sb.Append(indent).Append(_propDic[key]).AppendLine(Environment.NewLine);
                        break;
                }
            }

            sb.Length -= 2 * Environment.NewLine.Length;

            return sb.ToString();
        }



    }
}
