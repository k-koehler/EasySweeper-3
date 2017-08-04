using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyWeb
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HTMLColumnAttribute : Attribute
    {
        public string ColumnName { get; private set; }
        public HTMLColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}