using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.Contacts.Tests
{
    [TestClass()]
    public class PhoneNumbersTests
    {
        //[TestMethod()]
        //public void PhoneNumbersTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void PhoneNumbersTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ToStringTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void CleanTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void CloneTest()
        {
            var phone1 = new PhoneNumbers
            {
                Phone = "0123-4567",
                Fax = "   "
            };

            var phone2 = (PhoneNumbers)phone1.Clone();

            Assert.IsTrue(phone1 == phone2);

            phone1.Clean();

            Assert.IsFalse(phone1 == phone2);

            phone2.Clean();

            Assert.IsTrue(phone1 == phone2);
        }

        //[TestMethod()]
        //public void EqualsTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void EqualsTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetHashCodeTest()
        //{
        //    Assert.Fail();
        //}
    }
}