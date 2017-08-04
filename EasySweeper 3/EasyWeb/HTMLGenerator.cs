using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace EasyWeb
{
    public static class HTMLGenerator
    {
        public static XElement GenerateTable<T>(IEnumerable<T> objects, params string[] propertiesToUse)
        {
            var methods = typeof(T)
                .GetExtensionMethods()
                .Where((method, index) =>
                {
                    var attr = method.GetCustomAttribute(typeof(HTMLColumnAttribute));
                    return attr != null && propertiesToUse.Contains((attr as HTMLColumnAttribute).ColumnName);
                });


            XElement table = new XElement("table");
            XElement thead = new XElement("thead");
            XElement headTr = new XElement("tr");

            thead.Add(headTr);
            table.Add(thead);

            XElement tbody = new XElement("tbody");
            XElement tr = new XElement("tr");

            bool headTrPopulated = false;

            foreach (T t in objects)
            {
                tr = new XElement("tr");
                tbody.Add(tr);

                foreach (MethodInfo m in methods)
                {
                    if (!headTrPopulated)
                        headTr.Add(new XElement("th") { Value = (m.GetCustomAttribute(typeof(HTMLColumnAttribute)) as HTMLColumnAttribute).ColumnName });

                    tr.Add(new XElement("td") { Value = (string)m.Invoke(t, new object[] { t }) });
                }

                headTrPopulated = true;
            }

            table.Add(tbody);

            return table;
        }
    }
}