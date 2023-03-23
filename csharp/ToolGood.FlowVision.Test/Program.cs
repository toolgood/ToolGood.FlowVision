using System.Diagnostics;
using ToolGood.FlowVision.Engines;
using ToolGood.FlowVision.Flows;

namespace ToolGood.FlowVision.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            KSHS();//使用的是快速上手 案例 生成导出的文件

            AL2(); //案例二




        }
        static void KSHS()
        {
            var project = ProjectWork.LoadJson("dict/第一个项目_20221211121519.json");
            FlowEngine flowEngine = new FlowEngine(project);
            flowEngine.BuildTreeNode("Flow", "Project1", "{\"数量\":800}");
            flowEngine.EvaluateInputNum();
            var t = flowEngine.TryEvaluate("总价", 0);
            Debug.Assert(6400 == t);


            var project2 = ProjectWork.LoadJsonWithRsa("dict/第一个项目_20221211121522.data");
            FlowEngine flowEngine2 = new FlowEngine(project2);
            flowEngine2.BuildTreeNode("Flow", "Project1", "{\"数量\":80}");
            flowEngine2.EvaluateInputNum();
            var t2 = flowEngine2.TryEvaluate("总价", 0);
            Debug.Assert(800 == t2);
        }


        static void AL2()
        {
            var al2 = ProjectWork.LoadJson("dict/第二个案例_20230323135702.json");
            // 没房没车，本科，月薪少于3000，申请不通过
            FlowEngine flowEngine = new FlowEngine(al2);
            flowEngine.BuildTreeNode("Flow", "default", "{\"姓名\":\"12312\",\"性别\":\"123\",\"年龄\":\"20\",\"学历\":\"本科\",\"手机\":\"123\",\"月收入\":\"2000\",\"地址\":\"123\",\"有房\":\"false\",\"有车\":\"false\",\"信用卡数量\":\"1\"}");
            flowEngine.EvaluateInputNum();
            var t1 = flowEngine.TryEvaluate("审核结果", false);
            var t2 = flowEngine.TryEvaluate("额度", 0);
            Debug.Assert(false == t1);
            Debug.Assert(0 == t2);


            // 有房有车  额度是15000
            flowEngine = new FlowEngine(al2);
            flowEngine.BuildTreeNode("Flow", "default", "{\"姓名\":\"12312\",\"性别\":\"123\",\"年龄\":\"20\",\"学历\":\"本科\",\"手机\":\"123\",\"月收入\":\"2000\",\"地址\":\"123\",\"有房\":\"有\",\"有车\":\"有\",\"信用卡数量\":\"1\"}");
            flowEngine.EvaluateInputNum();
            t1 = flowEngine.TryEvaluate("审核结果", false);
            t2 = flowEngine.TryEvaluate("额度", 0);
            Debug.Assert(true == t1);
            Debug.Assert(15000 == t2);
        


        }



    }
}