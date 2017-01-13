using MVCtutorial.Graph.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MVCtutorial.Controllers { 
    public class ToJSON
{
        public string ToJson<T>(Type Instance)
        {
            int i = 0;
            string[] paramNames = new string[typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Length];
            string[] paramValues = new string[typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Length];
            // {"type":"analog", "table":"norm", "column":"diFlourHopper_Mass", "decimal":3, "color":#88FF00, "coef":0.001 }
            string json = "{";
            switch (Instance.GetType().FullName)
#pragma warning disable CS1522 // Empty switch block
            {
#pragma warning restore CS1522 // Empty switch block

            }
            foreach (FieldInfo FI in typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                string param = FI.Name.ToLower();
                object obj = FI.GetValue(this);
                string paramValue = obj.ToString();
                paramNames[i] = param;
                paramValues[i] = paramValue;
                json += "\"" + paramNames[i] + "\":";
                switch (FI.FieldType.Name)
                {
                    case "String":
                        json += "\"" + paramValues[i] + "\", ";
                        break;
                    case "Color":
                        Color color = (Color)FI.GetValue(this);
                        if (color != null)
                        {
                            json += ColorTranslator.ToHtml(color).ToString() + ", ";
                        }
                        break;
                    default:
                        json += paramValues[i] + ", ";
                        break;
                }
                i++;
            }
            json = json.Remove(json.LastIndexOf(", "), 2);
            json += "}";

            return json;
        }


}
}