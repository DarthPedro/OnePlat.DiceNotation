using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnePlat.DiceNotation.DiceTerms;
using OnePlat.DiceNotation.DieRoller;
using OnePlat.DiceNotation.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnePlat.DiceNotation.UnitTests.DiceTerms
{
    /// <summary>
    /// Summary description for DiceTermTests
    /// </summary>
    [TestClass]
    public class DiceTermTests
    {
        private IDieRoller dieRoller = new RandomDieRoller();
        private IDieRoller constantRoller = new ConstantDieRoller();

        public DiceTermTests()
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
        public void DiceTerm_ConstructorTest()
        {
            // setup test

            // run test
            IExpressionTerm term = new DiceTerm(1, 20);

            // validate results
            Assert.IsNotNull(term);
            Assert.IsInstanceOfType(term, typeof(IExpressionTerm));
            Assert.IsInstanceOfType(term, typeof(DiceTerm));
        }

        [TestMethod]
        public void DiceTerm_ConstructorInvalidNumDiceTest()
        {
            // setup test

            // run test
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(0, 6));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(-5, 6));

            // validate results
        }

        [TestMethod]
        public void DiceTerm_ConstructorInvalidSidesTest()
        {
            // setup test

            // run test
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(3, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(1, -20));

            // validate results
        }

        [TestMethod]
        public void DiceTerm_ConstructorInvalidScalarTest()
        {
            // setup test

            // run test
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(2, 8, 0));

            // validate results
        }

        [TestMethod]
        public void DiceTerm_ConstructorInvalidChooseTest()
        {
            // setup test

            // run test
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(3, 6, choose: 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(3, 6, choose: -4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(3, 6, choose: 4));

            // validate results
        }

        [TestMethod]
        public void DiceTerm_ConstructorInvalidExplodingTest()
        {
            // setup test

            // run test
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(3, 6, exploding: 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(3, 6, exploding: -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DiceTerm(3, 6, exploding: 7));

            // validate results
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(1, 20);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(dieRoller);

            // validate results
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            TermResult r = results.FirstOrDefault();
            Assert.IsNotNull(r);
            Assert.AreEqual(1, r.Scalar);
            AssertHelpers.IsWithinRangeInclusive(1, 20, r.Value);
            Assert.AreEqual("DiceTerm.d20", r.Type);
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsMultipleDiceTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(3, 6);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(constantRoller);

            // validate results
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
            foreach (TermResult r in results)
            {
                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Scalar);
                Assert.AreEqual(1, r.Value);
                Assert.AreEqual("DiceTerm.d6", r.Type);
            }
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsChooseDiceTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(5, 6, choose: 3);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(constantRoller);

            // validate results
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count);
            int included = 0;
            foreach (TermResult r in results)
            {
                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Scalar);
                Assert.AreEqual(1, r.Value);
                Assert.AreEqual("DiceTerm.d6", r.Type);
                if (r.AppliesToResultCalculation) included++;
            }
            Assert.AreEqual(3, included);
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsExplodingNoneDiceTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(5, 6, exploding: 6);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(constantRoller);

            // validate results
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count);
            foreach (TermResult r in results)
            {
                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Scalar);
                Assert.AreEqual(1, r.Value);
                Assert.AreEqual("DiceTerm.d6", r.Type);
            }
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsExplodingRandomDiceTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(10, 6, exploding: 6);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(new RandomDieRoller());

            // validate results
            Assert.IsNotNull(results);
            int count = 10;
            foreach (TermResult r in results)
            {
                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Scalar);
                AssertHelpers.IsWithinRangeInclusive(1, 6, r.Value);
                if (r.Value >= 6) count++;
                Assert.AreEqual("DiceTerm.d6", r.Type);
            }
            Assert.AreEqual(count, results.Count);
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsExplodingLowerThanMaxTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(10, 12, exploding: 9);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(new RandomDieRoller());

            // validate results
            Assert.IsNotNull(results);
            int count = 10;
            foreach (TermResult r in results)
            {
                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Scalar);
                AssertHelpers.IsWithinRangeInclusive(1, 12, r.Value);
                if (r.Value >= 9) count++;
                Assert.AreEqual("DiceTerm.d12", r.Type);
            }
            Assert.AreEqual(count, results.Count);
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsExplodingAndChooseTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(10, 12, choose: 8, exploding: 9);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(new RandomDieRoller());

            // validate results
            Assert.IsNotNull(results);
            int included = 0;
            foreach (TermResult r in results)
            {
                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Scalar);
                AssertHelpers.IsWithinRangeInclusive(1, 12, r.Value);
                Assert.AreEqual("DiceTerm.d12", r.Type);
                if (r.AppliesToResultCalculation) included++;
            }
            Assert.AreEqual(8, included);
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsErrorMaxRerollsTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(10, 12, exploding: 9);

            // run test
            Assert.ThrowsException<OverflowException>(() => term.CalculateResults(new ConstantDieRoller(10)));

            // validate results
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsMultiplierDiceTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(2, 8, 10);

            // run test
            IReadOnlyList<TermResult> results = term.CalculateResults(constantRoller);

            // validate results
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            foreach (TermResult r in results)
            {
                Assert.IsNotNull(r);
                Assert.AreEqual(10, r.Scalar);
                Assert.AreEqual(1, r.Value);
                Assert.AreEqual("DiceTerm.d8", r.Type);
            }
        }

        [TestMethod]
        public void DiceTerm_CalculateResultsNullDieRollerTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(1, 10);

            // run test
            Assert.ThrowsException<ArgumentNullException>(() => term.CalculateResults(null));

            // validate results
        }

        [TestMethod]
        public void DiceTerm_ToStringTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(2, 10);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("2d10", result);
        }

        [TestMethod]
        public void DiceTerm_ToStringChooseTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(5, 6, choose: 3);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("5d6k3", result);
        }

        [TestMethod]
        public void DiceTerm_ToStringMultiplierTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(2, 8, 10);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("2d8x10", result);
        }

        [TestMethod]
        public void DiceTerm_ToStringExplodingNoneDiceTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(5, 6, exploding: 6);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("5d6!6", result);
        }

        [TestMethod]
        public void DiceTerm_ToStringExplodingLowerThanMaxTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(10, 12, exploding: 9);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("10d12!9", result);
        }

        [TestMethod]
        public void DiceTerm_ToStringAllTermsTest()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(4, 6, 10, 3, 6);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("4d6k3!6x10", result);
        }

        [TestMethod]
        public void DiceTerm_ToStringTest_NegativeScalar()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(1, 4, -1);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("-1d4", result);
        }

        [TestMethod]
        public void DiceTerm_ToStringTest_FractionalScalar()
        {
            // setup test
            IExpressionTerm term = new DiceTerm(1, 4, 0.5);

            // run test
            string result = term.ToString();

            // validate results
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("1d4/2", result);
        }
    }
}
