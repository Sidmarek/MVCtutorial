using System.Collections.Generic;
//using System.Web.UI.WebControls;
using System.Drawing;
using System;
using MVCtutorial.Controllers;
using System.Reflection;
using System.Text;
using MVCtutorial.Graph.Models;

namespace MVCtutorial.Graph.Models
{
    public class LangDef
    {
        public string LangAbbreviation; //Abreviation = "shortcut" 
    }

    public class LangDefinition
    {
        public static List<LangDef> LangDefList = new List<LangDef>() { new LangDef() { LangAbbreviation = "EN" }, new LangDef() { LangAbbreviation = "CZ" }, new LangDef() { LangAbbreviation = "DE" }, new LangDef() { LangAbbreviation = "PL" } };
        public int Find(CIniFile config, string lang)
        {
            foreach (LangDef LangDef in config.LangDefList)
            {
                if (LangDef.LangAbbreviation.Contains(lang))
                {
                    return config.LangDefList.IndexOf(LangDef);
                }
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang">lang which you want to add</param>
        /// <param name="position">Optional parameter</param>
        public static void Add(CIniFile config, string lang, int position = 0)
        {
            if (position == 0)
            {
                config.LangDefList.Add(new LangDef() { LangAbbreviation = lang });
            }
            else
            {
                config.LangDefList.Insert(position, new LangDef() { LangAbbreviation = lang });
            }
        }
        public static string toJSON(CIniFile config)
        {
            string json = "\"LangDef\": [";
            foreach (LangDef LangDef in config.LangDefList) {
                json += "\"" + LangDef.LangAbbreviation + "\",";
            }
            json = json.Substring(0, json.Length - 1);
            json += "]";
            return json;
        }
    }

    public class TableDef
    {
        public string shortName;
        public int dbIdx;
        public string tabName;
    }

    public class TableDefinition
    {
        
        // public static List 
        public static string Find(CIniFile config, int ConnNo, string TabName)
        {
            foreach (TableDef TableDef in config.TableDefList)
            {
                if (TableDef.dbIdx == ConnNo & TableDef.tabName.Contains(TabName))
                {
                    return TableDef.shortName;
                }
            }
            return null;
        }
        public static string Add(CIniFile config, int ConnNo, string TabName)
        {
            int subscoreIdx;
            try {
                subscoreIdx = TabName.LastIndexOf("_");
            } catch (ArgumentNullException e) {
                throw new Exception(e.Message);
            }
            string shortedName = TabName.Substring(subscoreIdx+1);
            config.TableDefList.Add(new TableDef() { shortName = shortedName, dbIdx = ConnNo, tabName = TabName });
            return shortedName;
        }

        public static string toJSON(CIniFile config)
        {
            string json = "\"TableDef\": [ ";
            foreach (TableDef TableDef in config.TableDefList)
            {
                json += "{";
                json += "\"shortName\":\"" + TableDef.shortName + "\",";
                json += "\"dbIdx\":" + TableDef.dbIdx + ",";
                json += "\"tabName\":\"" + TableDef.shortName + "\"";
                json += "},";
            }
            json = json.Substring(0, json.Length - 1);
            json += "]";
            return json;
        }
    }

    public class Values
    {
        public int idx;
        public string[] langTexts;
    }

    public class TextlistDef
    {
        public string textlist;
        public List<Values> values;   
    }

    public class TextlistDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textlist"></param>
        /// <returns></returns>
        public static string Find(CIniFile config,string textlist)
        {
            foreach (TextlistDef TextlistDef in config.TextlistDefList) {
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
        public static string Add(CIniFile config,string name,List<string[]> textsArray, List<int> Idxs)
        {
            List<Values> tempValues = new List<Values>();
            
            for (int i = 0;i<textsArray.Count;i++)//First row is name row - textlist name 
            {
                tempValues.Add(new Values() { idx= Idxs[i], langTexts= textsArray[i]});
            }
            config.TextlistDefList.Add(new TextlistDef() { textlist = name, values = tempValues });
            return name;
        }
        public  static bool  UpdateName(CIniFile config, string oldName, string newName) {
            foreach (TextlistDef TextlistDef in config.TextlistDefList)
            {
                if (TextlistDef.textlist.Contains(oldName))
                {
                    TextlistDef.textlist = newName;
                    return true;
                }
            }
            return false;
        }

        public static string toJSON(CIniFile config) {
            string json = "\"TextlistDef\": [";
            foreach (TextlistDef TextlistDef in config.TextlistDefList)
            {
                json += "{\"textlist\": \"" + TextlistDef.textlist + "\",";
                json += "\"values\": [";
                foreach (Values Values in TextlistDef.values)
                {
                    json += "{\"idx\":" + Values.idx + ",";
                    for (int i=0;i<Values.langTexts.Length; i++)
                    {
                        LangDef LangDef = config.LangDefList[i];
                        json += "\"text_" + LangDef.LangAbbreviation + "\":\"" + Values.langTexts[i] + "\",";
                    }
                    json = json.Substring(0, json.Length - 1);
                    json += "},";
                }
                json = json.Substring(0, json.Length - 1);
                json += "]},";
            }
            json = json.Substring(0, json.Length - 1);
            json += "]";
            return json;
        }
    }

    public class ColumnTextlist {
        public string column;
        public string TextlistName;
    }

    public class ColumnTextlistDefine {
        public static List<ColumnTextlist> ColumnTextlistList = new List<ColumnTextlist>();

        public static string FindtextlistName(string column)
        {
            foreach (ColumnTextlist ColumntextlistInstance in ColumnTextlistList)
            {
                if (ColumntextlistInstance.column.Contains(column))
                {
                    return ColumntextlistInstance.TextlistName;
                }
            }
            return null;
        }
        public static string Findcolumn(string TextlistName)
        {
            foreach (ColumnTextlist ColumntextlistInstance in ColumnTextlistList)
            {
                if (ColumntextlistInstance.TextlistName.Contains(TextlistName))
                {
                    return ColumntextlistInstance.column;
                }
            }
            return null;
        }
        public static void Add(string column, string TextlistName) {
            ColumnTextlistList.Add(new ColumnTextlist() { column = column, TextlistName = TextlistName });
        }
    }
    public class NameDef {
        public string table;
        public string column;
        public List<string> fullNames;
        public List<string> units;
    }
    public class NameDefinition {
        
        public static string Find(CIniFile config, string column)
        {
            foreach (NameDef NameDef in config.NameDefList)
            {
                if (NameDef.column.Contains(column))
                {
                    return NameDef.column;
                }
            }
            return null;
        }
        public static string Add(CIniFile config, string ascolumn, List<string> asfullNames, List<string> asunits, string astable = null)
        {
            config.NameDefList.Add(new NameDef() { table = astable, column = ascolumn, fullNames = asfullNames, units = asunits });
            return ascolumn;
        }

        public static string toJSON(CIniFile config) {
            string json = "\"NameDef\": [";
            foreach (NameDef NameDef in config.NameDefList)
            {
                json += "{";
                json += "\"table\":\"" + NameDef.table + "\", \"column\":\"" + NameDef.column +"\",";
                for (int i = 0; i < NameDef.fullNames.Count; i++)
                {
                    LangDef LangDef = config.LangDefList[i];
                    json += "\"fullName_" + LangDef.LangAbbreviation + "\":\"" + NameDef.fullNames[i] + "\",";
                }
                for (int i = 0; i < NameDef.units.Count; i++)
                {
                    LangDef LangDef = config.LangDefList[i];
                    json += "\"unit_" + LangDef.LangAbbreviation + "\":\"" + NameDef.units[i] + "\",";
                }
                json = json.Substring(0, json.Length - 1);
                json += "},";
            }
            json = json.Substring(0, json.Length - 1);
            json += "]";
            return json;
        }
    }

    public class Const
    {
        public static readonly string[] separators = {":", "=", "  ", "             ", ";", "\n" };
        public static readonly string[] separators_signal = { ":", "=", " ", ",", "  ", "             ", ";;", "\n" };
        public static readonly string[] separators_view = { "$", ",", ":", "=", "  ", "             ", ";;", "\n" };
        public static readonly string[] separ_equate = { "=" };
        public static readonly string[] separ_dollar = { "$" };
        public static readonly string[] separ_names = { "$", "             ", ";" };
        public static readonly string[] separ_backslash = { @"\", "$", "             ", ";" };
    }

    public class CSigMultitext
    {
        public string type = "multitext";
        public string Table;
        public string Column;
        public Color Color;
        public string textlist;
        CSigMultitext(string asTabDefName, string asColumn, Color acColor, string asTextListDef)
        {
            Table = asTabDefName;
            Column = asColumn;
            Color = acColor;
            textlist = asTextListDef;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="separ_names_string">First row of that </param>
        /// <param name="separ_cfg_string"></param>
        /// <returns></returns>
        public static CSigMultitext FromIni(CIniFile config, string[] separ_cfg_string) {
            string TableDefName, Column, TableName, textlist;
            int ConnectionStringNumber;
            Color Color;

            ConnectionStringNumber = int.Parse(separ_cfg_string[1]);
            Column = separ_cfg_string[2];
            TableName = separ_cfg_string[3];
            TableDefName = TableDefinition.Find(config, ConnectionStringNumber, TableName);
            if (TableDefName == null)
            {
                TableDefinition.Add(config, ConnectionStringNumber, TableName);
            }
            Color = Color.FromArgb(int.Parse(separ_cfg_string[5]), int.Parse(separ_cfg_string[6]), int.Parse(separ_cfg_string[7]));

            textlist = ColumnTextlistDefine.FindtextlistName(separ_cfg_string[2]);
            if (textlist == null) {

            }

            return new CSigMultitext(TableDefName, Column, Color, textlist);
        }

        public string toJSON(CSigMultitext SigMultitext) {
            string json = @"{";

            json += "\"type\": \"" + SigMultitext.type + "\", ";
            json += "\"table\": \"" + SigMultitext.Table + "\", ";
            json += "\"color\": \"" + ColorTranslator.ToHtml(SigMultitext.Color).ToString() + "\", ";
            json += "\"textlist\":\"" + SigMultitext.textlist + "\"},";

            return json;
        }
    }

    public class CSignal
    {
        public string type = "analog";
        public string table;
        public string column;//column 
        public Color Color;
        public int Decimal;
        CSignal(string asSigName, string asTabDefName, int aiDecimal, Color acColor)
        {
            column = asSigName;
            table = asTabDefName;
            Decimal = aiDecimal;
            Color = acColor;
        }

        public static CSignal FromIni(CIniFile config, string[] separ_cfg_string)
        {
            int ConnectionStringNumber;
            string SignalName, TableName, TableDefName;
            int Decimal;
            Color Color;         
            // priklad:   Signal=3:iWMU_Temp  arBF_norm 1 255,0,0
            //            0      1 2          3         4 5   6 7

            ConnectionStringNumber =  int.Parse(separ_cfg_string[1]);
            SignalName = separ_cfg_string[2];
            TableName = separ_cfg_string[3];
            TableDefName = TableDefinition.Find(config, ConnectionStringNumber, TableName);
            if (TableDefName == null )
            {
                TableDefName = TableDefinition.Add(config, ConnectionStringNumber, TableName);
            }   
            Decimal = int.Parse(separ_cfg_string[4]);
            Color = Color.FromArgb(int.Parse(separ_cfg_string[5]), int.Parse(separ_cfg_string[6]), int.Parse(separ_cfg_string[7]));          
            
            return new CSignal(SignalName, TableDefName, Decimal, Color);
        }

        static public CSignal FromJson(string sJson)
        {
            return null;
        }

        public  string toJSON(CSignal signal)
        {
            string json =  @"{";

            json += "\"type\": \"" + signal.type + "\", ";
            json += "\"table\": \"" + signal.table + "\", ";
            json += "\"color\": \"" + ColorTranslator.ToHtml(signal.Color).ToString() + "\", ";
            json += "\"decimal\":" + signal.Decimal + "},";

            return json;
        }
    }

    public class CField
    {
        public readonly int minY = 0;
        public int maxY;
        public int relSize;
        public List<CSignal> SigList = new List<CSignal>();
        public List<CSigMultitext> SigMultiList = new List<CSigMultitext>();
        CField(int maximalY, int realSize) {
            maxY = maximalY;
            relSize = realSize;
        }

        public static CField FromIni(string[] separ_string)
        {
            int maximalY, realSize;
            maximalY = int.Parse(separ_string[1]);
            realSize = int.Parse(separ_string[2]);
            return new CField(maximalY, realSize);
        }

        public void AddSignal(CSignal aSig)
        {            
            SigList.Add(aSig);
        }
        public void AddSignalMultitext(CSigMultitext aSigMulti)
        {
            SigMultiList.Add(aSigMulti);
        }

        public string toJSON(CField field)
        {
            string json = "{";
            json += "\"minY\":" + field.minY + ", ";
            json += "\"maxY\":" + field.maxY + ", ";
            //json += "\"unit\":" + field.unit + ", ";
            json += "\"relSize\":" + field.relSize + ", ";
            json += "\"signal\":[";
            if (SigList.Count != 0)
            {
                
                foreach (CSignal signal in SigList)
                {
                    string signalJSON = signal.toJSON(signal);
                    json += signalJSON;
                }
            }
            if (SigMultiList.Count != 0)
            {
                foreach (CSigMultitext SigMultitext in SigMultiList)
                {
                    string sigMultitextJSON = SigMultitext.toJSON(SigMultitext);
                    json += sigMultitextJSON;
                }
            }
            json = json.Substring(0, json.Length - 1);
            json += "]";
            json += "},";
            return json;
        }

    }

    public class CView
    {
        public List<string> Names;
        public string defLang;
        public List<CField> FieldList = new List<CField>();
        CView(List<string> anames) {
            Names = anames;
            defLang = LangDefinition.LangDefList[0].LangAbbreviation; // first index is default lang
        }
        public static CView FromIni(string[] separ_string)
        {
            List<string> tempNames = new List<string>();
            string s;
            //Cycle starts on two beacause of skiping position of word View and number of view (ex. 3)
            for (int i=3;i<=separ_string.Length;i++) {
                s = separ_string[i-1];
                tempNames.Add(s);
            }
            return new CView(tempNames);
        }

        public void AddField(CField CFieldInstatance)
        {
            FieldList.Add(CFieldInstatance);
        }
        public string toJSON(CView view)
        {
            string json = "{";
            int pos = 0;
            foreach (LangDef LangDef in  LangDefinition.LangDefList)
            {
                if (pos < view.Names.Count) {
                    json += "\"name_" + LangDef.LangAbbreviation + "\":\""+ view.Names[pos] +"\", ";
                }
                pos++;
            }
            json += "\"defLang\": \"" + view.defLang + "\", ";
            if (FieldList.Count != 0) {
                json += "\"field\":[";
                foreach (CField field in FieldList)
                {
                    string fieldJSON = field.toJSON(field);
                    json += fieldJSON;
                }
                json = json.Substring(0, json.Length - 1);
                json += "]";
            }
            else {
                json = json.Substring(0, json.Length - 1);
            }
            json += "},";
            return json;
        }
    }

    public class CIniFile {
        public List<CView> ViewList = new List<CView>();
        public List<NameDef> NameDefList = new List<NameDef>();
        public List<TextlistDef> TextlistDefList = new List<TextlistDef>();
        public List<TableDef> TableDefList = new List<TableDef>();
        public List<LangDef> LangDefList = new List<LangDef>() { new LangDef() { LangAbbreviation = "EN" }, new LangDef() { LangAbbreviation = "CZ" }, new LangDef() { LangAbbreviation = "DE" }, new LangDef() { LangAbbreviation = "PL" } };
        public void AddView(CView CViewInstance)
        {
            ViewList.Add(CViewInstance);
        }

        public string toJSON(CIniFile IniFile)
        {
            string json = "{\"Config\":";
            json += "[{";
            json += LangDefinition.toJSON(this);
            json += "},";
            json += "{";
            json += TableDefinition.toJSON(this);
            json += "},";
            json += "{";
            json += NameDefinition.toJSON(this);
            json += "},";
            json += "{";
            json += TextlistDefinition.toJSON(this);
            json += "},";
            json += "{";
            json += "\"View\":[";
            foreach (CView view in ViewList)
            {
                json += view.toJSON(view);
            }
            json = json.Substring(0, json.Length - 1);
            json += "]";
            json += "}]}";
            return json;
        }
    }
    /// <summary>
    ///     CfgStructure defines Graph config cfg structure
    /// </summary>

    ///     NamesStructure defines Graph config  names structure
    /// </summary>
}