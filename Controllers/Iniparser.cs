using MVCtutorial.Graph.Models;
using System;
using System.Collections.Generic;

namespace MVCtutorial.Controllers
{
    /// <summary>
    /// Class for parsing .ini configs using reading all lines. -- Bad written ini files (without sections)
    /// </summary>
    class Iniparser
    {
        private static string CfgPath;
        private static string NamePath;
        public Iniparser(string aCfgPath, string aNamePath)
        {
            CfgPath = aCfgPath;
            NamePath = aNamePath;
        }
        public void ParseLangs(CIniFile config)
        {
            string[] lines = System.IO.File.ReadAllLines(CfgPath, System.Text.Encoding.Default);

            for (int i = 0; i < lines.Length; i++)
            {
                if (!(lines[i].StartsWith("#")) && (lines[i].Length != 0))
                {
                    string[] langs = lines[i].Split(Const.separ_dollar, StringSplitOptions.None);
                    for (int j = 0; j < langs.Length; j++)
                    {
                        if (langs[j].Length != 0)
                        {
                            LangDefinition.Add(config, LangDefinition.LangDefList[j].LangAbbreviation);
                        }
                    }
                    i = lines.Length;
                }
            }
        }

        public void ParseCfg(CIniFile config, string[] separators)
        {
            string[] separeted_string = null;

            CView lastView = null;
            CField lastField = null;
            CSignal lastSignal = null;
            CSigMultitext lastSigMultitext = null;
            string[] lines = System.IO.File.ReadAllLines(CfgPath, System.Text.Encoding.Default);

            for (int i = 0; i < lines.Length; i++)
            {
                separeted_string = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (!(lines[i].StartsWith("#")) && (lines[i].Length != 0))
                {
                    switch (separeted_string[0])
                    {
                        case "View":
                            separeted_string = lines[i].Split(Const.separators_view, StringSplitOptions.RemoveEmptyEntries);
                            lastView = parseView(config, separeted_string);
                            config.AddView(lastView);
                            break;
                        case "Field":
                            lastField = parseField(config, separeted_string);
                            lastView.AddField(lastField);
                            break;
                        case "Signal":
                            separeted_string = lines[i].Split(Const.separators_signal, StringSplitOptions.RemoveEmptyEntries);
                            lastSignal = parseSignal(config, separeted_string);
                            lastField.AddSignal(lastSignal);
                            break;
                        case "SigMultitext":
                            separeted_string = lines[i].Split(Const.separators_signal, StringSplitOptions.RemoveEmptyEntries);
                            lastSigMultitext = parseSigMultitext(config, separeted_string);
                            lastField.AddSignalMultitext(lastSigMultitext);
                            break;
                    }
                }
            }
        }

        private CSigMultitext parseSigMultitext(CIniFile config,string[] separeted_string)
        {
            CSigMultitext sigMultitext = CSigMultitext.FromIni(config, separeted_string);
            return sigMultitext;
        }

        private CSignal parseSignal(CIniFile config, string[] separeted_string)
        {
           CSignal signal =  CSignal.FromIni(config, separeted_string);
           return signal;
        }

        private CField parseField(CIniFile config, string[] separeted_string)
        {
            CField field = CField.FromIni(separeted_string);
            return field;
        }

        private CView parseView(CIniFile config, string[] separeted_string)
        {
            CView view =CView.FromIni(separeted_string);
            return view;
        }

        public void ParseNames(CIniFile config, string[] separators)
        {

            string[] separeted_string = null;
            int result, rowNumber = 0;
            
            string[] lines = System.IO.File.ReadAllLines(NamePath, System.Text.Encoding.Default);

            for (int i = 0; i < lines.Length; i++)
            {
                separeted_string = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (!(separeted_string.Length == 0))
                {
                    if (separeted_string[0].Contains("DefineMultitext"))
                    {
                        rowNumber = parseTextListDefinition(config, separators, lines, separeted_string, i);
                    }

                    if (int.TryParse(separeted_string[0], out result) == true)
                    {
                        try
                        {
                            rowNumber = parseNameDefinition(config, separators, lines, i);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
                if (rowNumber != 0)
                {
                    i = rowNumber;
                    rowNumber = 0;
                }
            }
        }
        private int parseTextListDefinition(CIniFile config, string[] separators, string[] lines, string[] separeted_string, int startLineIdx)
        {
            string[] multitext_line = null, line_with_id = null;
            string multitext_name = null, line_without_id = null;
            List<string[]> multitext_lines = new List<string[]>();
            List<int> Idxs = new List<int>();
            int position;
            int id;

            multitext_name = separeted_string[1];

            for (int i = startLineIdx + 1; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    TextlistDefinition.Add(config, multitext_name, multitext_lines, Idxs);
                    return i;
                }
                else
                {
                    line_with_id = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    if (int.TryParse(line_with_id[0], out id) == true) {
                        Idxs.Add(id);
                        position = lines[i].IndexOf(';');
                        line_without_id = lines[i].Substring(position + 1); // Because of spliting this string. Before semicolon is first index
                        multitext_line = line_without_id.Split(Const.separ_semicolon, StringSplitOptions.RemoveEmptyEntries);
                        multitext_lines.Add(multitext_line);
                    }
                }
            }
            return 0;
        }
        private int parseNameDefinitionNew(CIniFile config, string[] lines, int startLineIdx)
        {
            for(int i = startLineIdx; i < lines.Length; i++) {
                if (!(lines[i].StartsWith("#")) && (lines[i].Length != 0))
                {

                }
                else {
                    return i;
                }
            }
            int i = 0;
            return i;
        }
        private int parseNameDefinition(CIniFile config, string[] separators, string[] lines, int startLineIdx)
        {
            string[] nameLine = null, nameLineFirstPart = null, nameLineLangMutate = null;
            string tableName = null;
            List<string[]> multitext_lines = new List<string[]>();

            for (int i = startLineIdx; i < lines.Length; i++)
            {
                List<string> units = new List<string>();
                List<string> langs = new List<string>();
                if (!(lines[i].StartsWith("#")) && (lines[i].Length != 0))
                {
                    nameLine = lines[i].Split(Const.separ_equate, StringSplitOptions.RemoveEmptyEntries);
                    nameLineFirstPart = nameLine[0].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    nameLineLangMutate = nameLine[1].Split(Const.separ_names, StringSplitOptions.RemoveEmptyEntries);
                    foreach (CView view in config.ViewList) {
                        foreach (CField field in view.FieldList) {
                            foreach (CSignal signal in field.SigList) {
                                if (signal.column.Contains(nameLineFirstPart[1])) {
                                    tableName = signal.table;
                                }
                            }
                        }
                    }
                    int j = 0;
                    for (int idx = 0; idx < nameLineLangMutate.Length; idx = idx + 2)
                    {
                        if (!(nameLineLangMutate[idx + 1].Contains(@"\")) && !(nameLineLangMutate[idx + 1].Contains("multitext:")))
                        {
                            if (nameLineLangMutate.Length > config.LangEnbList.Count) {
                                langs.Insert(j, nameLineLangMutate[idx]);
                                units.Insert(j, nameLineLangMutate[idx + 1]);
                                j++;
                            } else {
                                langs.Insert(j, nameLineLangMutate[idx]);
                                j++;
                            }
                        }
                        else {
                            langs.Insert(j, nameLineLangMutate[idx]);
                            if (!(nameLineLangMutate[idx + 1].Contains("multitext:")))
                            {
                                multitextInline(config, tableName, nameLine[1]);
                                idx = nameLineLangMutate.Length;
                            }
                            else
                            {
                                int pos = nameLineLangMutate[idx + 1].LastIndexOf(":");
                                string TextlistName = nameLineLangMutate[idx + 1].Substring(pos+1);
                                ColumnTextlistDefine.Add(nameLineFirstPart[1], TextlistName);
                            }
                        }
                    }
                    NameDefinition.Add(config, nameLineFirstPart[1], langs, units, tableName);
                }
                else
                {
                    return i;
                }
            }
            return 0;
        }
        private void multitextInline (CIniFile config,string tableName, string nameLineSecondPart) {
            List<string[]> textListValues = new List<string[]>();
            List<int> Idxs = new List<int>();
            string[] separatedFromDollars = nameLineSecondPart.Split(Const.separ_dollar, StringSplitOptions.RemoveEmptyEntries);
            string[] langs = new string[separatedFromDollars.Length];
            string[] valsForLength = separatedFromDollars[0].Split(Const.separ_backslash, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < valsForLength.Length; i++)
            {
                for (int j = 0; j < separatedFromDollars.Length; j++) {
                    string[] values = separatedFromDollars[j].Split(Const.separ_backslash, StringSplitOptions.RemoveEmptyEntries);
                    langs[j] = values[i];
                }
                textListValues.Add(langs);
                Idxs.Add(i - 1);
                TextlistDefinition.Add(config, tableName, textListValues, Idxs);
            }
        }
    }
}
