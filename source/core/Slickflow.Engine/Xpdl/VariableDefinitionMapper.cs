using System;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.WebUtility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// Maps <see cref="VariableEntity"/> (wf_variable) to runtime <see cref="VariableDetail"/>.
    /// </summary>
    internal static class VariableDefinitionMapper
    {
        public static VariableDetail ToVariableDetail(VariableEntity e)
        {
            if (e == null) return null;

            var v = new VariableDetail
            {
                Name = e.Name,
                DataType = EnumHelper.TryParseEnum<VariableDataTypeEnum>(string.IsNullOrEmpty(e.Type) ? "String" : e.Type),
                DefaultValue = e.DefaultValue,
                DirectionType = string.Equals(e.Direction, "Output", StringComparison.OrdinalIgnoreCase)
                    ? VariableDirectionTypeEnum.Output
                    : VariableDirectionTypeEnum.Input,
                IsReferenced = e.IsReferenced == 1,
                IsRequired = e.IsRequired == 1
            };

            if (v.IsReferenced && (!string.IsNullOrEmpty(e.SourceRef) || !string.IsNullOrEmpty(e.SourceVariableName)))
            {
                v.VariableRefDetail = new VariableRefDetail
                {
                    SourceRef = e.SourceRef,
                    VariableName = e.SourceVariableName
                };
            }

            return v;
        }
    }
}
