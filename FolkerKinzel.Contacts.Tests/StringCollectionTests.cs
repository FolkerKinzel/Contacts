using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.Contacts.Tests
{
    [TestClass()]
    public class StringCollectionTests
    {
        [TestMethod()]
        public void AddTest()
        {
            var coll = new StringCollection();

            coll.Add("Hallo");
            coll.Add(null!);

            Assert.AreEqual(1, coll.Count);
        }
    }
}