using PetaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    partial class AlgorithmEngineTest_csharp
    {

        [Test]
        public void IsRegex_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
            Assert.AreEqual(r, true);
            r = engine.TryEvaluate("IsRegex('abcd','da.*c')", false);
            Assert.AreEqual(r, false);
        }

        //IndexOf
        [Test]
        public void IndexOf()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var dt = engine.TryEvaluate("IndexOf('abcd','cd')", -1);
            Assert.AreEqual(dt, 2);
            dt = engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
            Assert.AreEqual(dt, 2);
        }

        [Test]
        public void Split()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Split('1,2,3,4',',')[3]", null);
            Assert.AreEqual(dt, "3");
        }

        [Test]
        public void TrimStart()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("TrimStart(' 123 ')", null);
            Assert.AreEqual(dt, "123 ");

            dt = engine.TryEvaluate("TrimStart(' 123 ',' 1')", null);
            Assert.AreEqual(dt, "23 ");
        }
        [Test]
        public void TrimEnd()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("TrimEnd(' 123 ')", null);
            Assert.AreEqual(dt, " 123");

            dt = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", null);
            Assert.AreEqual(dt, " 12");
        }

        [Test]
        public void Join()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Join(',',1,2,5,6)", null);
            Assert.AreEqual(dt, "1,2,5,6");
            dt = engine.TryEvaluate("Join(',',1,2,5,6,split('7,8,9',','))", null);
            Assert.AreEqual(dt, "1,2,5,6,7,8,9");
        }

        [Test]
        public void Substring()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Substring('123456789',1,2)", null);
            Assert.AreEqual(dt, "12");
        }
        [Test]
        public void StartsWith()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("StartsWith('123456789','12')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("StartsWith('123456789','127')", false);
            Assert.AreEqual(dt, false);
        }
        [Test]
        public void EndsWith()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("EndsWith('123456789','89')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("EndsWith('123456789','127')", false);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void IsNullOrEmpty()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("IsNullOrEmpty('')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("IsNullOrEmpty(' ')", false);
            Assert.AreEqual(dt, false);
        }
        [Test]
        public void IsNullOrWhiteSpace()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", false);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void RemoveStart()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("RemoveStart('123456789','12')", null);
            Assert.AreEqual(dt, "3456789");
            dt = engine.TryEvaluate("RemoveStart('123456789','127')", null);
            Assert.AreEqual(dt, "123456789");
        }
        [Test]
        public void RemoveEnd()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("RemoveEnd('123456789','89')", null);
            Assert.AreEqual(dt, "1234567");
            dt = engine.TryEvaluate("RemoveEnd('123456789','127')", null);
            Assert.AreEqual(dt, "123456789");
        }
 
        [Test]
        public void Json()
        {

            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", null);
            Assert.AreEqual(dt.ToString(), "51");
            dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Birthday]", null);
            Assert.AreEqual(dt, "04/26/1564 00:00:00");

            dt = engine.TryEvaluate("json('[1,2,3]')[1]", null);
            Assert.AreEqual(dt, "1");

            dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", null);
            Assert.AreEqual(dt, "William Shakespeare");


            dt = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
            Assert.AreEqual(dt, "ill");

            dt = engine.TryEvaluate("json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
            Assert.AreEqual(dt, null);

            dt = engine.TryEvaluate("json('[1,2,3,4,5,6]')[1].Trim()", null);
            Assert.AreEqual(dt, "1");

            dt = engine.TryEvaluate("json('[1,2,3,4,5,6]22')[1].Trim()", null);
            Assert.AreEqual(dt, null);

            dt = engine.TryEvaluate("json('22[1,2,3,4,5,6]')[1].Trim()", null);
            Assert.AreEqual(dt, null);
        }
    }
}
