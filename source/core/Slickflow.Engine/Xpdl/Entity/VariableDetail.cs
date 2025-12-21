using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Entity
{
    public class VariableDetail
    {
        public string Name { get; set; }
        public VariableDataTypeEnum DataType { get; set; }
        public string DefaultValue { get; set; }
        public VariableDirectionTypeEnum DirectionType { get; set; }
        public Boolean IsReferenced { get; set; }
        public Boolean IsRequired { get; set; }
        public VariableRefDetail VariableRefDetail { get; set; }
    }

    public enum VariableDirectionTypeEnum
    {
        Input = 1,

        Output = 2
    }

    public enum VariableDataTypeEnum
    {
        String = 1,

        Integer = 2,

        Double = 3,

        Boolean = 4,

        DateTime = 5,

        Object = 6
    }

    public class VariableRefDetail
    {
        public string SourceRef { get; set; }
        public string VariableName { get; set; }
    }
}