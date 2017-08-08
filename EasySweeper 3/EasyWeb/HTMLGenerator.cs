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
        public static XElement GenerateTable<T>(IEnumerable<T> objects, string id = "", params string[] propertiesToUse)
        {
            var methods = typeof(T)
                .GetExtensionMethods()
                .Where((method, index) =>
                {
                    var attr = method.GetCustomAttribute(typeof(HTMLColumnAttribute));
                    return attr != null && propertiesToUse.Contains((attr as HTMLColumnAttribute).ColumnName);
                });


            XElement table = new XElement("table");
            table.SetAttributeValue("class", typeof(T).Name + "Table");
            table.SetAttributeValue("id", id ?? "");
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

                var loop = methods.GetEnumerator();
                for (int i=0; loop.MoveNext(); i++ )
                {
                    MethodInfo m = loop.Current;
                    if (!headTrPopulated)
                        headTr.Add(new XElement("th") { Value = (m.GetCustomAttribute(typeof(HTMLColumnAttribute)) as HTMLColumnAttribute).ColumnName });
                    string value = (string)m.Invoke(t, new object[] { t });
                    var el = XElement.Parse("<td>" + value + "</td>");
                    tr.Add(el);
                }

                headTrPopulated = true;
            }
            table.Add(tbody);

            return table;
        }
    }
}