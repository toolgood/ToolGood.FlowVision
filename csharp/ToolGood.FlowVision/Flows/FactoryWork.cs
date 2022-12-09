using System.Text.Json.Serialization;

namespace ToolGood.FlowVision.Flows
{
    public sealed class FactoryWork
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public ProjectWork Project;// { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Category { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }
        /// <summary>
        /// 简化名称
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SimplifyName { get; set; }

   

        internal void Init(ProjectWork work)
        {
            Project = work;
        }
    }


}
