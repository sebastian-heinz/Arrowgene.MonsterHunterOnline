using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.IIPS;

public class TsvSheet
{
    public const string SheetNameKey = "@Sheet=";
    public const char Lf = '\n';
    public const char Cr = '\r';
    public const char TsvSeparator = '\t';

    private static readonly ILogger Logger = LogProvider.Logger(typeof(TsvSheet));


    public TsvSheet(string content)
    {
        Content = content;
        Parse(content);
    }

    public string Content { get; }
    public string Name { get; set; }
    public string[] TableHead { get; set; }
    public string[][] Table { get; set; }

    private void Parse(string content)
    {
        Name = null;
        TableHead = null;
        Table = null;

        List<string> rows = new List<string>();
        int sheetIndex = 0;
        StringReader s = new StringReader(content);
        StringBuilder r = new StringBuilder();
        bool read = true;

        while (read)
        {
            int c = s.Read();
            if (c == -1)
            {
                rows.Add(r.ToString());
                read = false;
                continue;
            }

            int n = s.Peek();

            if (Name == null)
            {
                if (sheetIndex == SheetNameKey.Length)
                {
                    Name = "";
                    while (true)
                    {
                        if (c == -1)
                        {
                            read = false;
                            break;
                        }

                        n = s.Peek();

                        if (c == Cr && n == Lf)
                        {
                            s.Read();
                            break;
                        }

                        if (c == Lf)
                        {
                            break;
                        }

                        Name += (char)c;
                        c = s.Read();
                    }
                }
                else if (c == SheetNameKey[sheetIndex])
                {
                    sheetIndex++;
                }
                else
                {
                    sheetIndex = 0;
                }

                continue;
            }
            
            // TODO after header row, should try to evaluate column count,
            // in `phonetics.dat` is a case that looks to be a TSV over multiline
            
            if (c == Cr && n == Lf)
            {
                s.Read();
                rows.Add(r.ToString());
                r.Clear();
                continue;
            }
            
            if (c == Lf)
            {
                rows.Add(r.ToString());
                r.Clear();
                continue;
            }

            r.Append((char)c);
        }

        int totalRowCount = rows.Count;
        if (totalRowCount <= 0)
        {
            return;
        }

        TableHead = GetColumns(rows[0]);

        int bodyRowCount = totalRowCount - 1;
        int columnCount = TableHead.Length;

        if (bodyRowCount <= 0)
        {
            return;
        }

        Table = new string[bodyRowCount][];
        for (int i = 0; i < bodyRowCount; i++)
        {
            string[] bodyColumns = GetColumns(rows[i + 1]);
            if (bodyColumns.Length != columnCount)
            {
                Logger.Error($"bodyColumns.Length != columnCount ({bodyColumns.Length} != {columnCount})");
            }

            Table[i] = bodyColumns;
        }
    }

    public string ToCsv()
    {
        StringBuilder sb = new StringBuilder();
        if (TableHead == null)
        {
            return sb.ToString();
        }
        for (int c = 0; c < TableHead.Length; c++)
        {
            WriteCsvColumn(sb, TableHead[c]);
            if (c != TableHead.Length - 1)
            {
                sb.Append(',');
            }
        }
        
        if (Table == null)
        {
            return sb.ToString();
        }
        sb.Append("\r\n");

        for (int r = 0; r < Table.Length; r++)
        {
            string[] row = Table[r];
            for (int c = 0; c < row.Length; c++)
            {
                WriteCsvColumn(sb, row[c]);
                if (c != row.Length - 1)
                {
                    sb.Append(',');
                }
            }

            sb.Append("\r\n");
        }

        return sb.ToString();
    }

    private void WriteCsvColumn(StringBuilder sb, string column)
    {
        if (column.Contains('"'))
        {
            column = column.Replace("\"", "\"\"");
        }

        if (
            column.Contains('\r')
            || column.Contains('\n')
            || column.Contains('"')
            || column.Contains(',')
        )
        {
            column = $"\"{column}\"";
        }

        sb.Append(column);
    }

    private string[] GetColumns(string row)
    {
        return row.Split(TsvSeparator);
    }
}