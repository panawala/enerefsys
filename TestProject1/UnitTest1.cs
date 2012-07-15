using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataContext;
using System.Data.Entity;
using EnerefsysDAL.Model;

namespace TestProject1
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestInitialize()]
        public void CodeFisrtTestInitialize()
        {
            //Database.SetInitializer<EnerefsysContext>(new DropCreateDatabaseIfModelChanges<EnerefsysContext>());
            try
            {
                Database.SetInitializer<EnerefsysContext>(new EnerefsysInitializer());
            }
            catch (Exception e)
            {
 
            }

        }

        [TestMethod]
        public void TestMethod1()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    context.PumpInfoes.Add(new PumpInfo
                    {
                        PumpCount = 3,
                        PumpDesignFlow = 23,
                        PumpFlow = 3,
                        PumpInfoID = 1,
                        PumpType = "tupe"
                    });
                }
                catch (Exception e)
                { 
                }
            }
            //
            // TODO: Add test logic here
            //
            //var eds = EnerefsysDAL.ElectronicData.GetAllElectroinc();
            //var dls = EnerefsysDAL.DayLoadData.GetAllDayloads();
        }
    }
}
