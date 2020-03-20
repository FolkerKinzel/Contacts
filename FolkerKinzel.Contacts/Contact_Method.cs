using FolkerKinzel.Contacts.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (IsEmpty) return string.Empty;

            StringBuilder sb = new StringBuilder();
            var keys = _propDic.Keys.OrderBy(x => x).ToArray();
            string[] topics = new string[keys.Length];

            const string indent = "        ";

            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];

                var value = _propDic[keys[i]];

                switch (value)
                {
                    case Person person:
                        sb.AppendLine(Res.Person);
                        person.AppendTo(sb, indent);
                        sb.AppendLine(Environment.NewLine);
                        break;
                    case Address address:
                        sb.AppendLine(Res.AddressHome);
                        address.AppendTo(sb, indent);
                        sb.AppendLine(Environment.NewLine);
                        break;
                    case IEnumerable<string?> strings:
                        sb.AppendLine(keys[i] == Prop.EmailAdresses ? Res.EmailAddresses : Res.InstantMessengers);
                        foreach (var str in strings)
                        {
                            sb.Append(indent).AppendLine(str);
                        }
                        sb.AppendLine();
                        break;
                    case IEnumerable<PhoneNumber?> phoneNumbers:
                        sb.AppendLine(Res.PhoneNumbers);
                        foreach (var phoneNumber in phoneNumbers)
                        {
                            if (phoneNumber != null)
                            {
                                phoneNumber.AppendTo(sb, indent);
                                sb.AppendLine();
                            }
                        }
                        sb.AppendLine();
                        break;    
                    case Work work:
                        sb.AppendLine(Res.Work);
                        work.AppendTo(sb, indent);
                        sb.AppendLine(Environment.NewLine);
                        break;
                    case DateTime dt:
                        sb.AppendLine(Res.TimeStamp);
                        sb.Append(indent).Append(dt.ToShortDateString()).Append(' ').AppendLine(dt.ToLongTimeString());
                        sb.AppendLine();
                        break;
                    default:
                        string header = key switch
                        {
                            Prop.DisplayName => Res.DisplayName,
                            Prop.HomePagePersonal => Res.HomePagePersonal,
                            Prop.HomePageWork => Res.HomePageWork,
                            Prop.Comment => Res.Comment,
                            _ => ""
                        };
                        sb.AppendLine(header);

                        sb.Append(indent).Append(_propDic[key]).AppendLine(Environment.NewLine);
                        break;
                }
            }

            sb.Length -= 2 * Environment.NewLine.Length;

            return sb.ToString();
        }



    }
}
