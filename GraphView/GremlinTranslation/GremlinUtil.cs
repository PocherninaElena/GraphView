﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView
{
    internal class GremlinUtil
    {
        protected static int _vertexCount = 0;
        protected static int _edgeCount = 0;
        protected static int _tableCount = 0;

        internal static string GenerateTableAlias(GremlinVariableType variableType)
        {
            switch (variableType)
            {
                case GremlinVariableType.Vertex:
                    return "N_" + _vertexCount++;
                case GremlinVariableType.Edge:
                    return "E_" + _edgeCount++;
            }
            return "R_" + _tableCount++;
        }

        internal static bool IsTheSameOutputType(List<GremlinToSqlContext> contextList)
        {
            if (contextList.Count <= 1) return true;
            bool isSameType = true;
            for (var i = 1; i < contextList.Count; i++)
            {
                isSameType = contextList[i - 1].PivotVariable.GetVariableType() ==
                              contextList[i].PivotVariable.GetVariableType();
                             //|| contextList[i - 1].PivotVariable.GetVariableType() == GremlinVariableType.Table
                             //|| contextList[i].PivotVariable.GetVariableType() == GremlinVariableType.Table;
                if (isSameType == false) return false;
            }
            return isSameType;
        }

        internal static bool IsTheSameType(List<GremlinVariable> variableList)
        {
            if (variableList.Count <= 1) return true;
            bool isSameType = true;
            for (var i = 1; i < variableList.Count; i++)
            {
                isSameType = variableList[i - 1].GetVariableType() ==
                             variableList[i].GetVariableType();
                if (isSameType == false) return false;
            }
            return isSameType;
        }

        internal static bool IsVertexProperty(string property)
        {
            if (property == GremlinKeyword.NodeID
                || property == GremlinKeyword.EdgeAdj
                || property == GremlinKeyword.ReverseEdgeAdj)
            {
                return true;
            }
            return false;
        }

        internal static bool IsEdgeProperty(string property)
        {
            if (property == GremlinKeyword.EdgeID
                || property == GremlinKeyword.EdgeSourceV
                || property == GremlinKeyword.EdgeSinkV
                || property == GremlinKeyword.EdgeOtherV
            )
            {
                return true;
            }
            return false;
        }
    }

    public class GremlinVertexProperty
    {
        public GremlinVertexProperty(GremlinKeyword.PropertyCardinality cardinality,
            string key, object value, Dictionary<string, object> metaProperties)
        {
            Cardinality = cardinality;
            Key = key;
            Value = value;
            MetaProperties = metaProperties ?? new Dictionary<string, object>();
        }

        public GremlinKeyword.PropertyCardinality Cardinality { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public Dictionary<string, object> MetaProperties { get; set; }

        public WPropertyExpression ToVertexPropertyExpr()
        {
            Dictionary<WValueExpression, WValueExpression> metaPropertiesExpr = new Dictionary<WValueExpression, WValueExpression>();
            foreach (var property in MetaProperties)
            {
                metaPropertiesExpr[SqlUtil.GetValueExpr(property.Key)] = SqlUtil.GetValueExpr(property.Value);
            }
            return new WPropertyExpression()
            {
                Cardinality = Cardinality,
                Key = SqlUtil.GetValueExpr(Key),
                Value = SqlUtil.GetValueExpr(Value),
                MetaProperties = metaPropertiesExpr
            };
        }
    }
}
