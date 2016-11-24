using System.Collections.Generic;
//using System.Web.UI.WebControls;
using System.Drawing;
using System;

namespace MVCtutorial.Graph.Models
{
    public class TableDefefinition
    {
        public string shortName;
        public int Idx;
        public string tabName;
    }
    public static class TableDefinition
    {
        public static List<TableDefefinition> TableDefList;
        public static string shortName;
        // public static List 
        public static string Find(int ConnNo, string TabNAme)
        {
            //return "norm";
            // else return null
            return null;
        }
        public static void Add (int ConnNo, string TabName)
        {
            int subscoreIdx;
            try {
                subscoreIdx = TabName.LastIndexOf("_");
            } catch (ArgumentNullException e ) {
                throw new Exception (e.Message); 
            }
            string shortedName = TabName.Substring(subscoreIdx);
            TableDefList.Add(new TableDefefinition() { shortName = shortedName, Idx = ConnNo, tabName = TabName });
        }
    }
 
    public class Const
    {
        public static readonly string[]  Separ_Space = { " ", ".", ""};
    }

    public class CSignal
    {
        public string type;
        public string SignalName;
        public string table;
        public int decimal;
        public Color color;
        CSignal(string asSigName, string asTabDefName, int aiDecimal, Color aiColor)
        {
            SignalName = asSigName;
            TabDefName = asTabDefName;
            Decimal = aiDecimal;
            Color = aiColor;

        }

        static public CSignal FromIni(string[] separ_string)
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
            TableDefName = TableDefefinition.Find(ConnectionStringNumber, TableName);
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
        }
    }

    public class CField
    {
        public int minY;
        //public CSignal[]   Sig;
        public List<CSignal> SigList;

        public static CField FromIni(string aCfgLine)
        {
            
            return new CField();
        }

        public bool AddSignal(CSignal aSig)
        {
            SigList.Add(aSig);
            return true;
        }

    }

    public class 


    /// <summary>
    ///     CfgStructure defines Graph config cfg structure
    /// </summary>
    public class CfgStructure
    {
        public List<View> ListOfView;

        public int View {
            get { return Viewer; }
            set {
                View s = new View(1);
                ListOfView.Add()
            } }
        public int[] Field { get; set; }
        public int[] Signal { get; set; }
    }        
   public struct View {
        public int ViewIdx { get; set; }
        public string ViewName { get; set; }
        public struct Field {
            public int Ratio { get; set; }
            public struct Signal {
                public int ConnectionStringNumber { get; set; }
                public string SignalName { get; set; }
                public string TableName { get; set; }
                public int Decimal { get; set; }
                public string Color { get; set; }
            }
        }
   } 
    /// <summary>
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