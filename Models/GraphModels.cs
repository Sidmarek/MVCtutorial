using System.Collections.Generic;
//using System.Web.UI.WebControls;
using System.Drawing;
using System;

namespace MVCtutorial.Graph.Models
{
    public class TableDef
    {
        public string shortName;
        public int Idx;
        public string tabName;
    }

    public static class TableDefinition
    {
        public static List<TableDef> TableDefList;
        public static string shortName;
        // public static List 
        public static string Find(int ConnNo, string TabNAme)
        {
            //return "norm";
            // else return null
            return null;
        }
        public static void Add(int ConnNo, string TabName)
        {
            int subscoreIdx;
            try {
                subscoreIdx = TabName.LastIndexOf("_");
            } catch (ArgumentNullException e) {
                throw new Exception(e.Message);
            }
            string shortedName = TabName.Substring(subscoreIdx);
            TableDefList.Add(new TableDef() { shortName = shortedName, Idx = ConnNo, tabName = TabName });
        }
    }
    public class TextListDef {
        public string textlist;
        public List<Values> values;
    }
    public class Values {
        public int idx;
        public List<string> textNames;
    }
    public class Const
    {
        public static readonly string[]  Separ_Space = { " ", ".", ""};
    }
    public class CSigMultitext
    {
        public string type = "multitext";
        public string TableName;
        public string Column;
        public Color cColor;
        public List<TextListDef> TextList;
    }
    public class CSignal
    {
        public string type = "analog";
        public string SignalName;
        public string TableName;
        public int Decimal;
        public Color Color;
        CSignal(string asSigName, string asTabDefName, int aiDecimal, Color aiColor)
        {
            SignalName = asSigName;
            TableName = asTabDefName;
            Decimal = aiDecimal;
            Color = aiColor;
            CField.AddSignal(this);
        }

        public static CSignal FromIni(string[] separ_string)
        {
            int ConnectionStringNumber;
            string SignalName;
            string TableName;
            string TableDefName;
            int Decimal;
            Color cColor;         
            // priklad:   Signal=3:iWMU_Temp  arBF_norm 1 255,0,0
            //            0      1 2          3         4 5   6 7

            ConnectionStringNumber =  int.Parse(separ_string[1]);
            SignalName = separ_string[2];
            TableName = separ_string[3];
            TableDefName = TableDefinition.Find(ConnectionStringNumber, TableName);
            if (TableDefName == null )
            {
                TableDefinition.Add(ConnectionStringNumber, TableName);
            }   
            Decimal = int.Parse(separ_string[4]);
            cColor = Color.FromArgb(int.Parse(separ_string[5]), int.Parse(separ_string[6]), int.Parse(separ_string[7]));
            
            return new CSignal(SignalName, TableDefName, Decimal, cColor);
        }

        static public CSignal FromJson(string sJson)
        {
            return null;
        }

        public string ToJson()
        {
            // {"type":"analog", "table":"norm", "column":"diFlourHopper_Mass", "decimal":3, "color":#88FF00, "coef":0.001 }
            return "Not Implemented yet";
        }
    }
4
    public class CField
    {
        public static readonly int minY = 0;
        public int maxY;
        public int relSize;
        public static List<CSignal> SigList;
        CField(int maximalY, int realSize) {
            maxY = maximalY;
            relSize = realSize;
            CView.AddField(this);
        }

        public static CField FromIni(string[] separ_string)
        {
            int maximalY, realSize;
            maximalY = int.Parse(separ_string[1]);
            realSize = int.Parse(separ_string[2]);
            return new CField(maximalY, realSize);
        }

        public static void AddSignal(CSignal aSig)
        {            
            SigList.Add(aSig);
        }

    }

    public class CView
    {
        public List<string> Names;
        public static List<CField> FieldList;
        CView(int minimalY, int maximalY, int realSize)
        {
            minY = minimalY;
            maxY = maximalY;
            relSize = realSize;
            
        }

        public void FromIni(string[] separ_string)
        {
            minY = int.Parse(separ_string[1]);
            maxY = int.Parse(separ_string[2]);

        }

        public static void AddField(CField CFieldInstatance)
        {
            FieldList.Add(CFieldInstatance);
        }


    }


    /// <summary>
    ///     CfgStructure defines Graph config cfg structure
    /// </summary>

    ///     NamesStructure defines Graph config  names structure
    /// </summary>
    public class NamesStructure
    {
        ///
        public void write(int rowNumber, int NamesStructure_position, object toStructure)
        {
            switch (NamesStructure_position)
            {
                case 1:
                    View[rowNumber] = toStructure.ToString();
                    break;
                case 2:
                    Field[rowNumber] = int.Parse(toStructure.ToString());
                    break;
                case 3:
                    Ratio[rowNumber] = int.Parse(toStructure.ToString());
                    break;
                case 4:
                    Signal[rowNumber] = int.Parse(toStructure.ToString());
                    break;
                case 5:
                    SignalName[rowNumber] = toStructure.ToString();
                    break;
                case 6:
                    Decimal[rowNumber] = int.Parse(toStructure.ToString());
                    break;
                case 7:
                    Color[rowNumber] = toStructure.ToString();
                    break;
                default:
                    break;
            }
        }
        public string[] View { get; private set; }
        public int[] Field { get; private set; }
        public int[] Ratio { get; private set; }
        public int[] Signal { get; private set; }
        public string[] SignalName { get; private set; }
        public string[] TableName { get; private set; }
        public int[] Decimal { get; private set; }
        public string[] Color { get; private set; }
    }
}