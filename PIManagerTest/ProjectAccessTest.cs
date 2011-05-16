using PIManager.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using PIManager.Models;
using System.Collections.Generic;
using System.Web;

namespace PIManagerTest
{
    
    
    /// <summary>
    ///This is a test class for ProjectAccessTest and is intended
    ///to contain all ProjectAccessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProjectAccessTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for addProject
        ///</summary>
        [TestMethod()]
        public void addProjectTest()
        {
            ProjectAccess target = new ProjectAccess();
            Project newProject = new Project("TestProject", "ab1", "<paragraph>paragraphe1</paragraph>",3,3);
            List<Technology> projectTechnos = new List<Technology>();
            TechnologyAccess technoAccess = new TechnologyAccess();
            Dictionary<int,Technology> technologies = technoAccess.getTechnologies();
            foreach (var techno in technologies)
                if (techno.Value.Equals(".NET 2010") || techno.Value.Equals("SQL Server 2008 R2"))
                    projectTechnos.Add(techno.Value);

            target.addProject(newProject, projectTechnos, new byte[0]);

            List<Project> projects = target.getProjects();
            foreach (var proj in projects)
                if (proj.Name.Equals("TestProject"))
                {
                    Assert.IsTrue(newProject.isEquivalent(proj));
                    break;
                }
        }
    }
}
