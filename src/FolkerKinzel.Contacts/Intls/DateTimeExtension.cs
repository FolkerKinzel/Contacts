using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.Contacts.Intls
{
    internal static class DateTimeExtension
    {
        internal static bool IsEmptyTimeStamp(this DateTime timeStamp) => timeStamp < new DateTime(1900, 1, 1);
    }
}
