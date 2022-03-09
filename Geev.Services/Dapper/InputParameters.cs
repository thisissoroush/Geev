using System;
using System.Collections.Generic;
using System.Text;

namespace Geev.Services.Dapper
{
    public class InputParameters
    {
        public InputParameters(string name, string value
            , ParameterTypes parametertype = ParameterTypes.String
            , ParameterDirections parameterdirection = ParameterDirections.Input)
        {
            Name = name;
            Value = value;
            ParameterType = parametertype;
            ParameterDirection = parameterdirection;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public ParameterTypes ParameterType { get; set; }
        public ParameterDirections ParameterDirection { get; set; }
    }
}
