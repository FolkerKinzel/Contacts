using System;
using System.Globalization;
using System.Threading;
using FolkerKinzel.Contacts;

namespace Examples
{
    internal class Program
    {
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            // CsvExample.ReadingAndWritingCsv();
            // VCardExample.ReadingAndWritingVCard();

            _ = ContactExample.InitializeContacts();

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture= CultureInfo.InvariantCulture;

            var ct = new Contact();

            ct.Clean();

        }
    }
}
