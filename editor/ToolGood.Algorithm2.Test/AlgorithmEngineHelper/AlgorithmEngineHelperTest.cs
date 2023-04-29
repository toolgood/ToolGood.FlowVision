using PetaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Algorithm2;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    public class AlgorithmEngineHelperTest
    {
        [Test]
        public void Test()
        {
            DiyNameInfo p = AlgorithmEngineHelper.GetDiyNames("[dd]");
            Assert.AreEqual("dd", p.Parameters[0].Name);


            DiyNameInfo p3 = AlgorithmEngineHelper.GetDiyNames("【dd】");
            Assert.AreEqual("dd", p3.Parameters[0].Name);

            DiyNameInfo p4 = AlgorithmEngineHelper.GetDiyNames("@ddd+2");
            Assert.AreEqual("ddd", p4.Parameters[0].Name);

            DiyNameInfo p6 = AlgorithmEngineHelper.GetDiyNames("长");
            Assert.AreEqual("长", p6.Parameters[0].Name);

            DiyNameInfo p7 = AlgorithmEngineHelper.GetDiyNames("#ddd#+2");
            Assert.AreEqual("ddd", p7.Parameters[0].Name);

        }

        [Test]
        public void Test2()
        {
            var b = AlgorithmEngineHelper.IsKeywords("false");
            Assert.IsTrue(b);


        }



    }
}
