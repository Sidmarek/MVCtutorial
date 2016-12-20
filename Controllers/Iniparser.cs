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

        public void ParseCfg(string[] separators)
        {
            string[] separeted_string = null;

            string[] lines = System.IO.File.ReadAllLines(CfgPath);
            for (int i = 0; i < lines.Length; i++)
            {
                separeted_string = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (!(lines[i].StartsWith("#")) && (lines[i].Length != 0))
                {
                    switch (separeted_string[0])
                    {
                        case "View":
                            parseView(separeted_string);
                            break;
                        case "Field":
                            parseField(separeted_string);
                            break;
                        case "Signal":
                            separeted_string = lines[i].Split(Const.separators_signal, StringSplitOptions.RemoveEmptyEntries);
                            parseSignal(separeted_string);
                            break;
                        case "SigMultitext":
                            separeted_string = lines[i].Split(Const.separators_signal, StringSplitOptions.RemoveEmptyEntries);
                            parseSigMultitext(separeted_string);
                            break;
                    }
                }
            }
        }

        private void parseSigMultitext(string[] separeted_string)
        {
            CSigMultitext SigMultitext = CSigMultitext.FromIni(separeted_string);
            CField.AddSignalMultitext(SigMultitext);
        }

        private void parseSignal(string[] separeted_string)
        {
            CSignal Signal = CSignal.FromIni(separeted_string);
            CField.AddSignal(Signal);
        }

        private void parseField(string[] separeted_string)
        {
            CField Field = CField.FromIni(separeted_string);
            CView.AddField(Field);
        }

        private void parseView(string[] separeted_string)
        {
            CView View = CView.FromIni(separeted_string);
            CIniFile.AddView(View);
        }

        public void ParseNames(string[] separators)
        {

            string[] separeted_string = null;
            int result, rowNumber = 0;

            string[] lines = System.IO.File.ReadAllLines(NamePath);

            for (int i = 0; i < lines.Length; i++)
            {
                separeted_string = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (!(separeted_string.Length == 0))
                {
                    if (separeted_string[0].Contains("DefineMultitext"))
                    {
                        rowNumber = parseTextListDefinition(separators, lines, separeted_string, i);
                    }

                    if (int.TryParse(separeted_string[0], out result) == true)
                    {
                        rowNumber = parseNameDefinition(separators, lines, i);
                    }
                }
                if (rowNumber != 0)
                {

                    i = rowNumber;
                    rowNumber = 0;
                }
            }
        }
        private int parseTextListDefinition(string[] separators, string[] lines, string[] separeted_string, int startLineIdx)
        {
            string[] multitext_line = null, line_with_id = null;
            string multitext_name = null, line_without_id = null;
            List<string[]> multitext_lines = new List<string[]>();
            List<int> Idxs = new List<int>();
            int position;

            multitext_name = separeted_string[1];

            for (int i = startLineIdx + 1; i < lines.Length; i++)
            {
                if (lines[i].Length == 0)
                {
                    TextlistDefinition.Add(multitext_name, multitext_lines, Idxs);
                    return i;
                }
                else
                {
                    line_with_id = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    Idxs.Add(int.Parse(line_with_id[0]));
                    position = lines[i].LastIndexOf(';');
                    line_without_id = lines[i].Substring(position + 1);
                    multitext_line = line_without_id.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    multitext_lines.Add(multitext_line);
                }
            }
            return 0;
        }

        private int parseNameDefinition(string[] separators, string[] lines, int startLineIdx)
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

                    int j = 0;
                    for (int idx = 0; idx < nameLineLangMutate.Length; idx = idx + 2)
                    {
                        if (!(nameLineLangMutate[idx + 1].Contains(@"\")) && !(nameLineLangMutate[idx + 1].Contains("multitext:")))
                        {
                            langs.Insert(j, nameLineLangMutate[idx]);
                            units.Insert(j, nameLineLangMutate[idx + 1]);
                            j++;
                        }
                        else {
                            langs.Insert(j, nameLineLangMutate[idx]);
                            if (!(nameLineLangMutate[idx + 1].Contains("multitext:")))
                            {
                                multitextInline(tableName, nameLine[1]);
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
                    NameDefinition.Add(nameLineFirstPart[1], langs, units);
                }
                else
                {
                    return i;
                }
            }
            return 0;
        }
        public void multitextInline (string tableName, string nameLineSecondPart) {
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
                TextlistDefinition.Add(tableName, textListValues, Idxs);
            }
        }

        /// <summary>
        /// Reads all lines from config files and gives them to array (2D)
        /// </summary>
        /// <param name="separators">Parameter defines seperators as '=', '#', '$' etc.</param>
        /// <returns> Two dimensional Array where first diemension is for structure of config and second for rows</returns>
        public string[][] readAllLines(string[] separators)
        {
            string[] lines = System.IO.File.ReadAllLines(CfgPath);
            string[] separated_string = null;
            string[][] separated_lines = null;
            int i = 0;
            foreach (string line in lines)
            {
                separated_string = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                separated_lines[i] = separated_string;
                i++;
            }
            return separated_lines;
        }

    }
}
