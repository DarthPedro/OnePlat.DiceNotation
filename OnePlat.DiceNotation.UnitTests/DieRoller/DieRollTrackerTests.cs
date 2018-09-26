using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnePlat.DiceNotation.DieRoller;
using OnePlat.DiceNotation.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnePlat.DiceNotation.UnitTests.DieRoller
{
    /// <summary>
    /// Summary description for DieRollTrackerTests
    /// </summary>
    [TestClass]
    public class DieRollTrackerTests
    {
        public DieRollTrackerTests()
        {
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DieRollTracker_ConstructorTest()
        {
            // setup test

            // run test
            IDieRollTracker t = new DieRollTracker();

            // validate results
            Assert.IsNotNull(t);
            Assert.IsInstanceOfType(t, typeof(IDieRollTracker));
            Assert.IsInstanceOfType(t, typeof(DieRollTracker));
        }

        [TestMethod]
        public void DieRollTracker_AddDieRollTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();

            // run test
            t.AddDieRoll(6, 4, typeof(RandomDieRoller));
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> d = task.Result;

            // validate results
            Assert.IsNotNull(d);
            Assert.AreEqual(1, d.Count);
            Assert.AreEqual("RandomDieRoller", d[0].RollerType);
            Assert.AreEqual("6", d[0].DieSides);
            Assert.AreEqual(4, d[0].Result);
            Assert.IsInstanceOfType(d[0].Id, typeof(Guid));
            Assert.IsInstanceOfType(d[0].Timpstamp, typeof(DateTime));
        }

        [TestMethod]
        public void DieRollTracker_AddMultipleDieRollTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();

            // run test
            t.AddDieRoll(6, 4, typeof(RandomDieRoller));
            t.AddDieRoll(6, 3, typeof(RandomDieRoller));
            t.AddDieRoll(6, 1, typeof(RandomDieRoller));
            t.AddDieRoll(6, 6, typeof(RandomDieRoller));
            t.AddDieRoll(6, 2, typeof(RandomDieRoller));
            t.AddDieRoll(6, 4, typeof(RandomDieRoller));

            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> d = task.Result;

            // validate results
            Assert.IsNotNull(d);
            Assert.AreEqual(6, d.Count);
            foreach (DieTrackingData e in d)
            {
                Assert.AreEqual("RandomDieRoller", e.RollerType);
                Assert.AreEqual("6", e.DieSides);
                AssertHelpers.IsWithinRangeInclusive(1, 6, e.Result);
            }
        }

        [TestMethod]
        public void DieRollTracker_AddMultipleDieRollDieSidesTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();

            // run test
            t.AddDieRoll(6, 4, typeof(RandomDieRoller));
            t.AddDieRoll(6, 3, typeof(RandomDieRoller));
            t.AddDieRoll(6, 1, typeof(RandomDieRoller));
            t.AddDieRoll(6, 6, typeof(RandomDieRoller));
            t.AddDieRoll(8, 2, typeof(RandomDieRoller));
            t.AddDieRoll(8, 4, typeof(RandomDieRoller));

            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> d = task.Result;

            // validate results
            Assert.IsNotNull(d);
            Assert.AreEqual(6, d.Count);
            int i;
            for (i = 0; i < 4; i++)
            {
                DieTrackingData e = d[i];
                Assert.AreEqual("RandomDieRoller", e.RollerType);
                Assert.AreEqual("6", e.DieSides);
                AssertHelpers.IsWithinRangeInclusive(1, 6, e.Result);
            }

            for (int x = i; x < 5; x++)
            {
                DieTrackingData e = d[x];
                Assert.AreEqual("RandomDieRoller", e.RollerType);
                Assert.AreEqual("8", e.DieSides);
                AssertHelpers.IsWithinRangeInclusive(1, 8, e.Result);
            }
        }

        [TestMethod]
        public void DieRollTracker_AddMultipleDieRollDieTypesTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();

            // run test
            t.AddDieRoll(6, 4, typeof(RandomDieRoller));
            t.AddDieRoll(6, 3, typeof(RandomDieRoller));
            t.AddDieRoll(6, 5, typeof(RandomDieRoller));
            t.AddDieRoll(6, 6, typeof(RandomDieRoller));
            t.AddDieRoll(6, 2, typeof(ConstantDieRoller));
            t.AddDieRoll(6, 2, typeof(ConstantDieRoller));

            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> d = task.Result;

            // validate results
            Assert.IsNotNull(d);
            Assert.AreEqual(6, d.Count);
            int i;
            for (i = 0; i < 2; i++)
            {
                DieTrackingData e = d[i];
                Assert.AreEqual("ConstantDieRoller", e.RollerType);
                Assert.AreEqual("6", e.DieSides);
                AssertHelpers.IsWithinRangeInclusive(1, 6, e.Result);
            }

            for (int x = i; x < 6; x++)
            {
                DieTrackingData e = d[x];
                Assert.AreEqual("RandomDieRoller", e.RollerType);
                Assert.AreEqual("6", e.DieSides);
                AssertHelpers.IsWithinRangeInclusive(1, 6, e.Result);
            }
        }

        [TestMethod]
        public void DieRollTracker_AddDieRollErrorsTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();

            // run test
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.AddDieRoll(1, 1, typeof(RandomDieRoller)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.AddDieRoll(0, 5, typeof(RandomDieRoller)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.AddDieRoll(-4, 5, typeof(RandomDieRoller)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.AddDieRoll(6, 8, typeof(RandomDieRoller)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.AddDieRoll(6, -2, typeof(RandomDieRoller)));
            Assert.ThrowsException<ArgumentNullException>(() => t.AddDieRoll(6, 4, null));
            Assert.ThrowsException<ArgumentException>(() => t.AddDieRoll(6, 4, this.GetType()));

            // validate results
        }

        [TestMethod]
        public void DieRollTracker_ClearTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            t.AddDieRoll(6, 4, typeof(RandomDieRoller));
            t.AddDieRoll(6, 3, typeof(RandomDieRoller));
            t.AddDieRoll(6, 1, typeof(RandomDieRoller));
            t.AddDieRoll(6, 6, typeof(RandomDieRoller));
            t.AddDieRoll(6, 2, typeof(RandomDieRoller));
            t.AddDieRoll(6, 4, typeof(RandomDieRoller));

            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> d = task.Result;

            // run test
            t.Clear();

            // validate results
            task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> r = task.Result;
            Assert.AreEqual(0, r.Count);
        }

        [TestMethod]
        public void DieRollTracker_GetTrackingDataAllTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);

            // run test
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> data = task.Result;

            // validate results
            Assert.AreEqual(27, data.Count);
        }

        [TestMethod]
        public void DieRollTracker_GetTrackingDataFilterDieTypeTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);

            // run test
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync("RandomDieRoller");
            task.Wait();
            IList<DieTrackingData> data = task.Result;

            // validate results
            Assert.AreEqual(19, data.Count);
            foreach(DieTrackingData e in data)
            {
                Assert.AreEqual("RandomDieRoller", e.RollerType);
            }
        }

        [TestMethod]
        public void DieRollTracker_GetTrackingDataFilterDieSidesTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);

            // run test
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync(dieSides: "10");
            task.Wait();
            IList<DieTrackingData> data = task.Result;

            // validate results
            Assert.AreEqual(4, data.Count);
            foreach (DieTrackingData e in data)
            {
                Assert.AreEqual("10", e.DieSides);
            }
        }

        [TestMethod]
        public void DieRollTracker_GetTrackingDataFilterDieTypeAndSidesTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);

            // run test
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync("RandomDieRoller", "20");
            task.Wait();
            IList<DieTrackingData> data = task.Result;

            // validate results
            Assert.AreEqual(14, data.Count);
            foreach (DieTrackingData e in data)
            {
                Assert.AreEqual("RandomDieRoller", e.RollerType);
                Assert.AreEqual("20", e.DieSides);
            }
        }

        [TestMethod]
        public void DieRollTracker_GetTrackingDataFilterDieTypeErrorTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);

            // run test
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync("FooDieRoller", "20");
            task.Wait();
            IList<DieTrackingData> data = task.Result;

            // validate results
            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void DieRollTracker_GetTrackingDataFilterDieSidesErrorTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);

            // run test
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync("RandomDieRoller", "foo");
            task.Wait();
            IList<DieTrackingData> data = task.Result;

            // validate results
            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void DieRollTracker_ToJsonTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);

            // run test
            Task<string> task = t.ToJsonAsync();
            task.Wait();
            string data = task.Result;

            // validate results
            Assert.IsFalse (string.IsNullOrEmpty(data));
        }

        [TestMethod]
        public void DieRollTracker_LaodFromJsonTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            this.SetupTrackingSampleData(t);
            Task<string> task = t.ToJsonAsync();
            task.Wait();
            string data = task.Result;
            Assert.IsFalse(string.IsNullOrEmpty(data));

            // run test
            IDieRollTracker other = new DieRollTracker();
            Task task2 = other.LoadFromJsonAsync(data);
            task2.Wait();

            Task<IList<DieTrackingData>> task3 = other.GetTrackingDataAsync("RandomDieRoller", "20");
            task.Wait();
            IList<DieTrackingData> list = task3.Result;

            // validate results
            Assert.AreEqual(14, list.Count);
            foreach (DieTrackingData e in list)
            {
                Assert.AreEqual("RandomDieRoller", e.RollerType);
                Assert.AreEqual("20", e.DieSides);
            }
        }


        [TestMethod]
        public void DieRollTracker_LaodFromJsonTest_NoData()
        {
            // setup test
            string data = "";

            // run test
            IDieRollTracker other = new DieRollTracker();
            Task task2 = other.LoadFromJsonAsync(data);
            task2.Wait();

            Task<IList<DieTrackingData>> task3 = other.GetTrackingDataAsync("RandomDieRoller", "20");
            task3.Wait();
            IList<DieTrackingData> list = task3.Result;

            // validate results
            Assert.AreEqual(0, list.Count);
        }
        [TestMethod]
        public void DieRollTracker_GetFrequencyDataTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();
            List<AggregateDieTrackingData> aggExpected = this.SetupStatisticalTrackingData(t);

            // run test
            Task<IList<AggregateDieTrackingData>> view = t.GetFrequencyDataViewAsync();
            view.Wait();
            IList<AggregateDieTrackingData> data = view.Result;

            // validate results
            Task<IList<DieTrackingData>> task = t.GetTrackingDataAsync();
            task.Wait();
            IList<DieTrackingData> d = task.Result;

            Assert.IsNotNull(data);
            Assert.IsInstanceOfType(data, typeof(IList<AggregateDieTrackingData>));
            Assert.AreEqual(45, d.Count);
            Assert.AreEqual(23, data.Count);

            Assert.AreEqual(aggExpected.Count, data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                Assert.AreEqual(aggExpected[i].RollerType, data[i].RollerType, "Failed roller type for item: " + i.ToString());
                Assert.AreEqual(aggExpected[i].DieSides, data[i].DieSides, "Failed die sides for item: " + i.ToString());
                Assert.AreEqual(aggExpected[i].Result, data[i].Result, "Failed result for item: " + i.ToString());
                Assert.AreEqual(aggExpected[i].Count, data[i].Count, "Failed count for item: " + i.ToString());
                Assert.AreEqual(aggExpected[i].Percentage, data[i].Percentage, "Failed percentage for item: " + i.ToString());
            }
        }

        [TestMethod]
        public void DieRollTracker_GetFrequencyDataEmptyTest()
        {
            // setup test
            IDieRollTracker t = new DieRollTracker();

            // run test
            Task<IList<AggregateDieTrackingData>> view = t.GetFrequencyDataViewAsync();
            view.Wait();
            IList<AggregateDieTrackingData> data = view.Result;

            // validate results
            Assert.IsNotNull(data);
            Assert.IsInstanceOfType(data, typeof(IList<AggregateDieTrackingData>));
            Assert.AreEqual(0, data.Count);
        }

        private void SetupTrackingSampleData(IDieRollTracker tracker)
        {
            tracker.AddDieRoll(6, 4, typeof(RandomDieRoller));
            tracker.AddDieRoll(6, 3, typeof(RandomDieRoller));
            tracker.AddDieRoll(6, 1, typeof(RandomDieRoller));
            tracker.AddDieRoll(6, 6, typeof(RandomDieRoller));
            tracker.AddDieRoll(8, 2, typeof(ConstantDieRoller));
            tracker.AddDieRoll(8, 4, typeof(ConstantDieRoller));
            tracker.AddDieRoll(10, 2, typeof(ConstantDieRoller));
            tracker.AddDieRoll(10, 8, typeof(ConstantDieRoller));
            tracker.AddDieRoll(10, 9, typeof(ConstantDieRoller));
            tracker.AddDieRoll(10, 4, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 5, typeof(ConstantDieRoller));
            tracker.AddDieRoll(20, 18, typeof(ConstantDieRoller));
            tracker.AddDieRoll(20, 11, typeof(ConstantDieRoller));
            tracker.AddDieRoll(20, 5, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 5, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 17, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 9, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 12, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 20, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 8, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 11, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 9, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 12, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 20, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 14, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 13, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 9, typeof(RandomDieRoller));
        }

        private List<AggregateDieTrackingData> SetupStatisticalTrackingData(IDieRollTracker tracker)
        {
            this.SetupTrackingSampleData(tracker);

            tracker.AddDieRoll(20, 5, typeof(ConstantDieRoller));
            tracker.AddDieRoll(20, 18, typeof(ConstantDieRoller));
            tracker.AddDieRoll(20, 11, typeof(ConstantDieRoller));
            tracker.AddDieRoll(20, 5, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 5, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 17, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 9, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 12, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 20, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 8, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 11, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 9, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 10, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 12, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 20, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 14, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 13, typeof(RandomDieRoller));
            tracker.AddDieRoll(20, 9, typeof(RandomDieRoller));

            List<AggregateDieTrackingData> expectedAggegate = new List<AggregateDieTrackingData>();
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "10", Result = 2, Count = 1, Percentage = 33.3f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "10", Result = 8, Count = 1, Percentage = 33.3f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "10", Result = 9, Count = 1, Percentage = 33.3f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "20", Result = 5, Count = 2, Percentage = 33.3f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "20", Result = 11, Count = 2, Percentage = 33.3f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "20", Result = 18, Count = 2, Percentage = 33.3f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "8", Result = 2, Count = 1, Percentage = 50.0f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "ConstantDieRoller", DieSides = "8", Result = 4, Count = 1, Percentage = 50.0f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "10", Result = 4, Count = 1, Percentage = 100.0f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 5, Count = 4, Percentage = 13.8f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 8, Count = 2, Percentage = 6.9f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 9, Count = 6, Percentage = 20.7f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 10, Count = 1, Percentage = 3.4f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 11, Count = 2, Percentage = 6.9f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 12, Count = 4, Percentage = 13.8f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 13, Count = 2, Percentage = 6.9f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 14, Count = 2, Percentage = 6.9f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 17, Count = 2, Percentage = 6.9f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "20", Result = 20, Count = 4, Percentage = 13.8f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "6", Result = 1, Count = 1, Percentage = 25.0f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "6", Result = 3, Count = 1, Percentage = 25.0f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "6", Result = 4, Count = 1, Percentage = 25.0f });
            expectedAggegate.Add(new AggregateDieTrackingData { RollerType = "RandomDieRoller", DieSides = "6", Result = 6, Count = 1, Percentage = 25.0f });

            return expectedAggegate;

            /* Expected aggregate data view
                ConstantDieRoller	10	2	1
                ConstantDieRoller	10	8	1
                ConstantDieRoller	10	9	1
                ConstantDieRoller	20	5	2
                ConstantDieRoller	20	11	2
                ConstantDieRoller	20	18	2
                ConstantDieRoller	8	2	1
                ConstantDieRoller	8	4	1
                RandomDieRoller	    10	4	1
                RandomDieRoller	    20	5	4
                RandomDieRoller	    20	8	2
                RandomDieRoller	    20	9	6
                RandomDieRoller	    20	10	1
                RandomDieRoller	    20	11	2
                RandomDieRoller	    20	12	4
                RandomDieRoller	    20	13	2
                RandomDieRoller	    20	14	2
                RandomDieRoller	    20	17	2
                RandomDieRoller	    20	20	4
                RandomDieRoller	    6	1	1
                RandomDieRoller	    6	3	1
                RandomDieRoller	    6	4	1
                RandomDieRoller	    6	6	1
            */
        }
    }
}
