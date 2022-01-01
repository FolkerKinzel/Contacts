using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.Contacts.Intls
{
    internal class PhoneNumberComparer : IEqualityComparer<PhoneNumber?>
    {
        private PhoneNumberComparer() { }

        public bool Equals(PhoneNumber? x, PhoneNumber? y)
        {
            if(x is null || y is null || x.IsEmpty || y.IsEmpty)
            {
                return (x is null || x.IsEmpty) && (y is null || y.IsEmpty);
            }
            
            return x.IsMergeableWith(y);
        }

        public int GetHashCode(PhoneNumber? obj) => Strip.GetHashCode(obj?.Value);

        public static PhoneNumberComparer Instance { get; } = new PhoneNumberComparer();
    }
}
