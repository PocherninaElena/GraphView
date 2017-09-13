﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView
{
    internal class GremlinValueVariable : GremlinScalarTableVariable
    {
        public GremlinContextVariable ProjectVariable { get; set; }

        public GremlinValueVariable(GremlinVariable projectVariable)
        {
            this.ProjectVariable = new GremlinContextVariable(projectVariable);
        }

        internal override List<GremlinVariable> FetchAllVars()
        {
            List<GremlinVariable> variableList = new List<GremlinVariable>() { this };
            variableList.AddRange(this.ProjectVariable.FetchAllVars());
            return variableList;
        }

        public override WTableReference ToTableReference()
        {
            List<WScalarExpression> parameters = new List<WScalarExpression>();
            parameters.Add(this.ProjectVariable.DefaultProjection().ToScalarExpression());
            var tableRef = SqlUtil.GetFunctionTableReference(GremlinKeyword.func.Value, parameters, GetVariableName());
            return SqlUtil.GetCrossApplyTableReference(tableRef);
        }
    }
}
