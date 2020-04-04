using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.Contacts.Resources;

namespace FolkerKinzel.Contacts.Tests
{
    [TestClass()]
    public class PhoneNumberTests
    {
        [TestMethod()]
        public void PhoneNumberTest()
        {
            var phone = new PhoneNumber();

            Assert.IsNull(phone.Value);
            Assert.IsFalse(phone.IsMobile);
            Assert.IsFalse(phone.IsFax);
            Assert.IsFalse(phone.IsWork);
            Assert.IsTrue(phone.IsEmpty);
        }

        [TestMethod()]
        public void PhoneNumberTest1()
        {
            var phone = new PhoneNumber("4711", true, true, true);

            Assert.IsNotNull(phone.Value);
            Assert.IsTrue(phone.IsMobile);
            Assert.IsTrue(phone.IsFax);
            Assert.IsTrue(phone.IsWork);
            Assert.IsFalse(phone.IsEmpty);
        }

        [TestMethod()]
        public void ToStringTest1()
        {
            var phone = new PhoneNumber("4711", true, true, true);

            string s = phone.ToString();

            Assert.AreEqual($"4711 ({Res.Fax}, {Res.WorkShort})", s);
        }

        [TestMethod()]
        public void ToStringTest2()
        {
            var phone = new PhoneNumber("4711", true);

            string s = phone.ToString();

            Assert.AreEqual($"4711 ({Res.WorkShort})", s);
        }

        [TestMethod]
        public void IsFaxTest()
        {
            var phone = new PhoneNumber();

            Assert.IsFalse(phone.IsFax);

            phone.IsFax = false;
            Assert.IsFalse(phone.IsFax);

            phone.IsFax = true;
            Assert.IsTrue(phone.IsFax);

            phone.IsFax = true;
            Assert.IsTrue(phone.IsFax);

            phone.IsFax = false;
            Assert.IsFalse(phone.IsFax);
        }


        [TestMethod]
        public void IsCellTest()
        {
            var phone = new PhoneNumber();

            Assert.IsFalse(phone.IsMobile);

            phone.IsMobile = false;
            Assert.IsFalse(phone.IsMobile);

            phone.IsMobile = true;
            Assert.IsTrue(phone.IsMobile);

            phone.IsMobile = true;
            Assert.IsTrue(phone.IsMobile);

            phone.IsMobile = false;
            Assert.IsFalse(phone.IsMobile);
        }

        [TestMethod]
        public void IsWorkTest()
        {
            var phone = new PhoneNumber();

            Assert.IsFalse(phone.IsWork);

            phone.IsWork = false;
            Assert.IsFalse(phone.IsWork);

            phone.IsWork = true;
            Assert.IsTrue(phone.IsWork);

            phone.IsWork = true;
            Assert.IsTrue(phone.IsWork);

            phone.IsWork = false;
            Assert.IsFalse(phone.IsWork);
        }

        [TestMethod()]
        public void ToStringTest3()
        {
            var phone = new PhoneNumber("    ", true);

            string s = phone.ToString();

            Assert.AreEqual("_", s);
        }

        [TestMethod()]
        public void ToStringTest4()
        {
            var phone = new PhoneNumber(null, true);

            string s = phone.ToString();

            Assert.AreEqual("_", s);
        }

        [TestMethod()]
        public void CleanTest1()
        {
            var phone = new PhoneNumber(null);
            phone.Clean();
            Assert.IsNull(phone.Value);
        }

        [TestMethod()]
        public void CleanTest2()
        {
            var phone = new PhoneNumber("");
            phone.Clean();
            Assert.IsNull(phone.Value);
        }

        [TestMethod()]
        public void CleanTest3()
        {
            var phone = new PhoneNumber("  \t  ");
            phone.Clean();
            Assert.IsNull(phone.Value);
        }

        [TestMethod()]
        public void CleanTest4()
        {
            var phone = new PhoneNumber(" 4711 ");
            phone.Clean();
            Assert.AreEqual("4711", phone.Value);
        }

        [TestMethod()]
        public void CloneTest()
        {
            var phone = new PhoneNumber("4711", true, true, true);

            var phone2 = (PhoneNumber)phone.Clone();

            Assert.AreNotSame(phone, phone2);

            Assert.AreEqual("4711", phone2.Value);
            Assert.IsTrue(phone2.IsMobile);
            Assert.IsTrue(phone2.IsFax);
            Assert.IsTrue(phone2.IsWork);
            Assert.IsFalse(phone2.IsEmpty);
        }



        [TestMethod()]
        public void EqualsTest1()
        {
            object o1 = new PhoneNumber("4711");
            object o2 = new PhoneNumber(" 47 - 11");

            Assert.IsTrue(o1.Equals(o2));
        }

        [TestMethod()]
        public void EqualsTest2()
        {
            object o1 = new PhoneNumber("4711");
            object o2 = new PhoneNumber("47111");

            Assert.IsFalse(o1.Equals(o2));
        }

        [TestMethod()]
        public void EqualsTest3()
        {
            object o1 = new PhoneNumber("4711");
            object o2 = new PhoneNumber();

            Assert.IsFalse(o1.Equals(o2));
        }

        [TestMethod()]
        public void EqualsTest4()
        {
            PhoneNumber o1 = new PhoneNumber("4711");
            PhoneNumber o2 = new PhoneNumber(" 47 - 11");

            Assert.IsTrue(o1.Equals(o2));
        }

        [TestMethod()]
        public void EqualsTest5()
        {
            PhoneNumber o1 = new PhoneNumber("4711");
            PhoneNumber o2 = new PhoneNumber("47111");

            Assert.IsFalse(o1.Equals(o2));
        }

        [TestMethod()]
        public void EqualsTest6()
        {
            PhoneNumber o1 = new PhoneNumber("4711");
            PhoneNumber o2 = new PhoneNumber();

            Assert.IsFalse(o1.Equals(o2));
        }

        [TestMethod()]
        public void GetHashCodeTest1()
        {
            PhoneNumber o1 = new PhoneNumber("4711");
            PhoneNumber o2 = new PhoneNumber(" 47 - 11");

            Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());
        }

        [TestMethod()]
        public void GetHashCodeTest2()
        {
            PhoneNumber o1 = new PhoneNumber("4711");
            PhoneNumber o2 = new PhoneNumber("47111");

            Assert.AreNotEqual(o1.GetHashCode(), o2.GetHashCode());

        }

        [TestMethod()]
        public void GetHashCodeTest3()
        {
            PhoneNumber o1 = new PhoneNumber("4711");
            PhoneNumber o2 = new PhoneNumber();

           
            Assert.AreNotEqual(o1.GetHashCode(), o2.GetHashCode());

        }

        [TestMethod()]
        public void GetHashCodeTest4()
        {
            PhoneNumber o1 = new PhoneNumber("  ");
            PhoneNumber o2 = new PhoneNumber();


            Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode());
        }
    }
}

