using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests
{
    [TestClass()]
    public class AddressTests
    {
        [TestMethod()]
        public void CloneTest()
        {
            var addr1 = new Address
            {
                Street = "Priorauer Str. 32",
                City = "   Raguhn",
                PostalCode = "   "
            };

            var addr2 = (Address)addr1.Clone();


            Assert.AreNotSame(addr1, addr2);
            Assert.IsTrue(addr1 == addr2);
        }


        [TestMethod()]
        public void AddressTest()
        {
            var adr = new Address();

            Assert.IsTrue(adr.IsEmpty);
        }


        [TestMethod()]
        public void CleanTest()
        {
            var addr1 = new Address
            {
                Street = "Priorauer Str. 32",
                City = "   Raguhn ",
                PostalCode = "   "
            };

            addr1.Clean();

            Assert.AreEqual("Priorauer Str. 32", addr1.Street);
            Assert.AreEqual("Raguhn", addr1.City);
            Assert.IsNull(addr1.PostalCode);

            Assert.IsFalse(addr1.IsEmpty);
        }


        [TestMethod()]
        public void EqualsTest01()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            object adr2 = new Address
            {
                PostalCode = "4711",
                City = "Berlin (Wilmersdorf)",
                Street = "Gänseweg 4"
            };

            Assert.AreEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest02()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            object adr2 = new Address
            {
                PostalCode = "4711",
                City = "Berlin (Wilmersdorf)",
                Street = "Gänseweg 5"
            };

            Assert.AreNotEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest03()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            object adr2 = new Address
            {
                PostalCode = "4712",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Assert.AreNotEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest04()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            object adr2 = new Address
            {
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Assert.AreEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest05()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Assert.AreNotEqual(adr1, null);
        }

        [TestMethod()]
        public void EqualsTest06()
        {
            Address adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Address adr2 = new Address
            {
                PostalCode = "4711",
                City = "Berlin (Wilmersdorf)",
                Street = "Gänseweg 4"
            };

            Assert.AreEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest07()
        {
            Address adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Address adr2 = new Address
            {
                PostalCode = "4711",
                City = "Berlin (Wilmersdorf)",
                Street = "Gänseweg 5"
            };

            Assert.AreNotEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest08()
        {
            Address adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Address adr2 = new Address
            {
                PostalCode = "4712",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Assert.AreNotEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest09()
        {
            Address adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Address adr2 = new Address
            {
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Assert.AreEqual(adr1, adr2);
        }

        [TestMethod()]
        public void EqualsTest10()
        {
            Address adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Assert.IsFalse(adr1.Equals(null));
        }



        [TestMethod()]
        public void GetHashCodeTest1()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            object adr2 = new Address
            {
                PostalCode = "4711",
                City = "Berlin (Wilmersdorf)",
                Street = "Gänseweg 4"
            };

            Assert.AreEqual(adr1.GetHashCode(), adr2.GetHashCode());
        }


        [TestMethod()]
        public void GetHashCodeTest2()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            object adr2 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 5"
            };

            Assert.AreNotEqual(adr1, adr2);
        }


        [TestMethod()]
        public void GetHashCodeTest3()
        {
            object adr1 = new Address
            {
                PostalCode = "4711",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            object adr2 = new Address
            {
                PostalCode = "4712",
                City = "Berlin",
                Street = "Gänseweg 4"
            };

            Assert.AreNotEqual(adr1.GetHashCode(), adr2.GetHashCode());
        }

        

        [TestMethod()]
        public void ToStringTest()
        {
            var adr = new Address();

            string? s = adr.ToString();

            Assert.IsNotNull(s);
            Assert.AreEqual(0, s.Length);
        }
    }
}