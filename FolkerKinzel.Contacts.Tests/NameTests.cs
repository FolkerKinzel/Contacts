﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FolkerKinzel.Contacts.Tests
{
    [TestClass()]
    public class NameTests
    {
        private const string FIRST_NAME = "FirstName";
        private const string LAST_NAME = "LastName";
        private const string MIDDLE_NAME = "MiddleName";
        private const string PREFIX = "Prefix";
        private const string SUFFIX = "Suffix";

        private static Name InitFullName()
        {
            return new Name()
            {
                FirstName = FIRST_NAME,
                LastName = LAST_NAME,
                MiddleName = MIDDLE_NAME,
                Prefix = PREFIX,
                Suffix = SUFFIX
            };
        }

        [TestMethod]
        public void NameTest()
        {
            var name = new Name();

            Assert.IsTrue(name.IsEmpty);
        }

        [TestMethod()]
        public void CleanTest()
        {
            var name = InitFullName();

            name.LastName = "  " + LAST_NAME + " ";
            name.FirstName = " First Name";
            name.MiddleName = "";
            name.Prefix = "  ";

            name.Clean();

            Assert.AreEqual("First Name", name.FirstName);
            Assert.AreEqual(LAST_NAME, name.LastName);
            Assert.IsNull(name.MiddleName);
            Assert.IsNull(name.Prefix);
            Assert.AreEqual(SUFFIX, name.Suffix);
        }

        [TestMethod()]
        public void CloneTest()
        {
            Name name1 = InitFullName();

            Name name2 = (Name)name1.Clone();

            Assert.AreNotSame(name1, name2);

            Assert.AreEqual(name2.FirstName, name1.FirstName);
            Assert.AreEqual(name2.LastName, name1.LastName);
            Assert.AreEqual(name2.MiddleName, name1.MiddleName);
            Assert.AreEqual(name2.Prefix, name1.Prefix);
            Assert.AreEqual(name2.Suffix, name1.Suffix);
        }



        [TestMethod()]
        public void EqualsTest1()
        {
            object name1 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };

            object name2 = new Name()
            {
                Prefix = "Dr.",
                LastName = "Davis",
                FirstName = "sammy",
                Suffix = "Jr."
            };

            Assert.IsTrue(name1.Equals(name2));
        }

        [TestMethod()]
        public void EqualsTest2()
        {
            object name1 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };

            object name2 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy"
            };

            Assert.IsFalse(name1.Equals(name2));
        }

        [TestMethod()]
        public void EqualsTest3()
        {
            Name name1 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };

            Name name2 = new Name()
            {
                Prefix = "Dr.",
                LastName = "Davis",
                FirstName = "sammy",
                Suffix = "Jr."
            };

            Assert.IsTrue(name1.Equals(name2));
        }

        [TestMethod()]
        public void EqualsTest4()
        {
            Name name1 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };

            Name name2 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy"
            };

            Assert.IsFalse(name1.Equals(name2));
        }

        [TestMethod()]
        public void EqualsTest5()
        {
            Name name1 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };



            Assert.IsFalse(name1.Equals(null));
        }

        [TestMethod()]
        public void GetHashCodeTest1()
        {
            object name1 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };

            object name2 = new Name()
            {
                Prefix = "Dr.",
                LastName = "Davis",
                FirstName = "sammy",
                Suffix = "Jr."
            };

            Assert.AreEqual(name1.GetHashCode(), name2.GetHashCode());
        }

        [TestMethod()]
        public void GetHashCodeTest2()
        {
            object name1 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };

            object name2 = new Name()
            {
                LastName = "Davis",
                FirstName = "Sammy"
            };



            Assert.AreNotEqual(name1.GetHashCode(), name2.GetHashCode());

        }

        [TestMethod()]
        public void ToStringTest()
        {
            object name1 = new Name()
            {
                Prefix = "Dr.",
                LastName = "Davis",
                FirstName = "Sammy",
                Suffix = "jr."
            };

            Assert.AreEqual("Dr. Sammy Davis jr.", name1.ToString());
        }
    }
}