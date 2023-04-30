using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
    public class FactoryProcedureEditDto: DbFactoryProcedure
    {
        public List<FactoryProcedureEditDtoItem> Factorys { get; set; }

    }
    public class FactoryProcedureEditDtoItem
    {
        public int Id { get; set; }
        public int FactoryId { get; set; }

        public bool Used { get; set; }

        public string CategoryCode { get; set; }

        public string Category { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

    }
}
