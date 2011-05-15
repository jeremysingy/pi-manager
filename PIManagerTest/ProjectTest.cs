using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIManager.Models;

namespace PIManagerTest
{
    [TestClass]
    public class ProjectTest
    {
        [TestMethod]
        public void Test_isEquivalent()
        {
            Project p1 = new Project("project1","ab1","<project><title>project1</title><abreviation>ab1</abreviation><description><paragraph>paragraphe1</paragraph></description></project>",1);
            Project p2 = new Project("project1", "ab1", "<project><title>project1</title><abreviation>ab1</abreviation><description><paragraph>paragraphe1</paragraph></description></project>", 1);
            Project p3 = new Project("project3", "ab1", "<project><title>project1</title><abreviation>ab1</abreviation><description><paragraph>paragraphe1</paragraph></description></project>", 1);
            Project p4 = new Project("project1", "ab4", "<project><title>project1</title><abreviation>ab1</abreviation><description><paragraph>paragraphe1</paragraph></description></project>", 1);
            Project p5 = new Project("project1", "ab1", "<project><title>project5</title><abreviation>ab5</abreviation><description><paragraph>paragraphe5</paragraph></description></project>", 1);
            Project p6 = new Project("project1", "ab1", "<project><title>project1</title><abreviation>ab1</abreviation><description><paragraph>paragraphe1</paragraph></description></project>", 3);

            Assert.IsTrue(p1.isEquivalent(p2));
            Assert.IsFalse(p1.isEquivalent(p3));
            Assert.IsFalse(p1.isEquivalent(p4));
            Assert.IsFalse(p1.isEquivalent(p5));
            Assert.IsFalse(p1.isEquivalent(p6));
        }
    }
}
