using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FolkerKinzel.Contacts
{
    public class StringCollection : KeyedCollection<string, string>
    {
        public StringCollection() : base(StringComparer.Ordinal, 10)
        {
         
        }

        protected override string GetKeyForItem(string item)
        {
            return item;
        }

        protected override void InsertItem(int index, string item)
        {
            if (item != null)
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, string item)
        {
            if (item is null)
            {
                this.RemoveAt(index);
            }
            else
            {
                base.SetItem(index, item);
            }
        }
        
    }
}
