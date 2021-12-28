﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.Contacts.Intls
{
    internal class PhoneNumberComparer : IEqualityComparer<PhoneNumber>
    {
        private PhoneNumberComparer() { }

        public bool Equals(PhoneNumber? x, PhoneNumber? y)
        {
            if(x is null || y is null || x.IsEmpty || y.IsEmpty)
            {
                return (x is null || x.IsEmpty) && (y is null || y.IsEmpty);
            }
            
            return x.CanBeMergedWith(y);
        }

        public int GetHashCode(PhoneNumber obj) => 42;

        public static PhoneNumberComparer Instance { get; } = new PhoneNumberComparer();
    }
}