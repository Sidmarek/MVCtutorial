using System.Collections.Generic;
//using System.Web.UI.WebControls;
using System.Drawing;
using System;

namespace MVCtutorial.Graph.Models
{
    public class LangDef
    {
        public string LangAbbreviation; //Abreviation = "shortcut" 
    }

    public static class LangDefinition
    {
        public static List<LangDef> LangDefList;
        ///
        public static string Find(string lang)
        {
            foreach (LangDef LangDef in LangDefList)
            {
                if (LangDef.LangAbbreviation.Contains(lang))
                {
                    return LangDef.LangAbbreviation;
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang">lang which you want to add</param>
        /// <param name="position">Optional parameter</param>
        public static void Add(string lang, int position = 0)
        {
            if (position == 0)
            {
                LangDefList.Add(new LangDef() { LangAbbreviation = lang });
            }
            else
            {
                LangDefList.Insert(position, new LangDef() { LangAbbreviation = lang });
            }
        }
    }

    public class TableDef
    {
        public string shortName;
        public int Idx;
        public string tabName;
    }

    public static class TableDefinition
    {
        public static List<TableDef> TableDefList;
        // public static List 
        public static string Find(int ConnNo, string TabName)
        {
            foreach (TableDef TableDef in TableDefList)
            {
                if (TableDef.Idx == ConnNo & TableDef.tabName.Contains(TabName))
                {
                    return TableDef.shortName;
                }
            }
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

    public class TextlistDef
    {
        public string textlist;
        public List<Values> values;
        
    }

    public static class TextlistDefinition
    {
        public static List<TextlistDef> TextlistDefList;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textlist"></param>
        /// <returns></returns>
        public static string Find(string textlist)
        {
            foreach (TextlistDef TextlistDef in TextlistDefList) {
                if (TextlistDef.textlist.Contains(textlist))
                {
                    return TextlistDef.textlist;
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="textsArray"></param>
        /// <param name="Idxs"></param>
        /// <returns></returns>
        public static TextlistDef Add(string name,string[][] textsArray, int[] Idxs)
        {
            List<Values> tempValues = new List<Values>();
            int i = 0;
            foreach (string[] text in textsArray)
            {
                tempValues.Add(new Values() { idx=Idxs[i], texts= new List<string>(text)});
                i++;
            }
            return new TextlistDef() { textlist = name, values = tempValues };
        }
    }

    public class Values
    {
        public int idx;
        public List<string> texts;
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
        public Color Color;
        public TextlistDef textlist;
        CSigMultitext(string asTabDefName, string asColumn, Color acColor, TextlistDef atTextListDef)
        {
            TableName = asTabDefName;
            Column = asColumn;
            Color = acColor;
            textlist = atTextListDef;
        }
        public static CSigMultitext FromIni() {

            return new CSigMultitext();
         }
    }

    public class CSignal
    {
        public string type = "analog";
        public string SignalName;
        public string TableName;
        public int Decimal;
        public Color Color;
        CSignal(string asSigName, string asTabDefName, int aiDecimal, Color acColor)
        {
            SignalName = asSigName;
            TableName = asTabDefName;
            Decimal = aiDecimal;
            Color = acColor;
            CField.AddSignal(this);
        }

        public static CSignal FromIni(string[] separ_string)
        {
            int ConnectionStringNumber;
            string SignalName;
            string TableName;
            string TableDefName;
            int Decimal;
            Color Color;         
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
            Color = Color.FromArgb(int.Parse(separ_string[5]), int.Parse(separ_string[6]), int.Parse(separ_string[7]));
            
            return new CSignal(SignalName, TableDefName, Decimal, Color);
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

    public class CField
    {
        public static readonly int minY = 0;
        public int maxY;
        public int relSize;
        public static List<CSignal> SigList;
        public static List<CSigMultitext> SigMultiList;
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
        public static void AddSignalMultitext(CSigMultitext aSigMulti)
        {
            SigMultiList.Add(aSigMulti);
        }
    }

    public class CView
    {
        public List<string> Names;
        public static List<CField> FieldList;

        public CView FromIni(string[] separ_string)
        {
            string s;
            //Cylcle starts on two beacause of skiping position of word View and number of view (ex. 3)
            for (int i=2;i<separ_string.Length;i++) {
                s = separ_string[i-1];
                Names.Add(s);
            }
            return new CView();
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
}