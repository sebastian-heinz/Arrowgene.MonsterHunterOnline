using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem
{
    /// <summary>
    /// https://www.rfc-editor.org/rfc/rfc4180
    /// </summary>
    public abstract class CsvReaderWriter<T> : IAssetDeserializer<T>
    {
        private const int BufferSize = 128;
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CsvReaderWriter<T>));

        protected abstract int NumExpectedItems { get; }

        protected delegate bool TryParseHandler<TParse>(string value, out TParse result);

        public CsvReaderWriter()
        {
        }

        protected abstract T CreateInstance(string[] properties);


        /// <summary>
        /// Try to parse value from string.
        /// If default value is specified, it will be returned in case the string is empty.
        /// </summary>
        protected bool TryParse<TParse>(
            string[] source,
            int index,
            out TParse result,
            TryParseHandler<TParse> handler,
            TParse? defaultValue = null) where TParse : struct
        {
            if (string.IsNullOrEmpty(source[index]))
            {
                if (defaultValue != null)
                {
                    result = (TParse)defaultValue;
                    return true;
                }
            }

            if (!handler(source[index], out result))
            {
                Logger.Error($"TryParse '{typeof(TParse)}' from '{source[index]}' failed");
                return false;
            }

            return true;
        }

        public List<T> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                return new List<T>();
            }

            using FileStream stream = File.OpenRead(file.FullName);
            return Read(stream);
        }

        public List<T> Read(Stream stream)
        {
            List<T> items = new List<T>();

            using StreamReader streamReader = new StreamReader(
                stream,
                Encoding.UTF8,
                true,
                BufferSize
            );

            int c;
            int line = 1;
            List<string> fields = new List<string>();
            StringBuilder fieldBuilder = new StringBuilder();
            StringBuilder lineBuilder = new StringBuilder();
            bool isFieldFirstCharacter = true;
            bool isLineFirstCharacter = true;
            bool isFieldQuoted = false;
            bool insideComment = false;
            int previousChar = -1;

            while ((c = streamReader.Read()) > 0)
            {
                if (isLineFirstCharacter)
                {
                    isLineFirstCharacter = false;
                    if (c == '#')
                    {
                        // comment start
                        insideComment = true;
                        previousChar = c;
                        lineBuilder.Append((char)c);
                        continue;
                    }

                    int nextChar = streamReader.Peek();
                    if (
                        (c == '\r' && nextChar == '\n') // Line end on CRLF
                        || (c == '\n') // Line end on LF
                    )
                    {
                        // empty line
                        lineBuilder.Append((char)c);
                        c = streamReader.Read();
                        lineBuilder.Append((char)c);
                        previousChar = c;
                        isLineFirstCharacter = true;
                        isFieldFirstCharacter = true;
                        continue;
                    }
                }

                if (insideComment)
                {
                    if ((previousChar == '\r' && c == '\n') // Line end on CRLF
                        || (c == '\n') // Line end on LF
                       )
                    {
                        // comment end
                        isLineFirstCharacter = true;
                        isFieldFirstCharacter = true;
                        insideComment = false;
                    }

                    previousChar = c;
                    lineBuilder.Append((char)c);
                    continue;
                }

                if (isFieldFirstCharacter)
                {
                    isFieldFirstCharacter = false;
                    if (c == '"')
                    {
                        isFieldQuoted = true;
                        // fieldBuilder.Append((char)c); // exclude quotes on reading data
                        previousChar = c;
                        lineBuilder.Append((char)c);
                        continue;
                    }
                }

                if (isFieldQuoted)
                {
                    int nextChar = streamReader.Peek();
                    bool isQuoteEscaped = c == '"' && nextChar == '"';
                    if (isQuoteEscaped)
                    {
                        // fieldBuilder.Append((char)c); // unescape quote
                        lineBuilder.Append((char)c);
                        c = streamReader.Read();
                        fieldBuilder.Append((char)c);
                        lineBuilder.Append((char)c);
                        previousChar = c;
                        continue;
                    }

                    if (c == '"' && nextChar != '"')
                    {
                        // detect the end
                        isFieldQuoted = false;
                        if (nextChar != ',' // expect end of field, if not
                            && nextChar != '\r' && nextChar != '\n') // and it is not the line end
                        {
                            Logger.Error(
                                $"Unescaped Quote in CSV near:`{lineBuilder}{(char)c}{(char)nextChar}` (expected `{(char)nextChar}` HEX:{nextChar:X8} to be a quote)");
                        }

                        // fieldBuilder.Append((char)c);  // exclude quotes on reading data
                        previousChar = c;
                        lineBuilder.Append((char)c);
                        continue;
                    }

                    fieldBuilder.Append((char)c);
                    previousChar = c;
                    lineBuilder.Append((char)c);
                    continue;
                }

                if (c == ',')
                {
                    isFieldFirstCharacter = true;
                    fields.Add(fieldBuilder.ToString());
                    fieldBuilder.Clear();
                    previousChar = c;
                    lineBuilder.Append((char)c);
                    continue;
                }

                if (
                    (previousChar == '\r' && c == '\n') // Line end on CRLF
                    || (c == '\n') // Line end on LF
                )
                {
                    //carriage return (The Carriage Return (CR) character (0x0D, \r))
                    //line feed (The Line Feed (LF) character (0x0A, \n))

                    string field = fieldBuilder.ToString();

                    if (field.EndsWith('\r'))
                    {
                        // For CRLF case, CR is already committed to field, need to remove it.
                        field = field.Remove(field.Length - 1);
                    }

                    fields.Add(field);
                    fieldBuilder.Clear();

                    ProcessLine(fields, items, lineBuilder.ToString(), line++);
                    fields.Clear();
                    lineBuilder.Clear();
                    isLineFirstCharacter = true;
                    previousChar = c;
                    continue;
                }

                fieldBuilder.Append((char)c);
                lineBuilder.Append((char)c);
                previousChar = c;
            }

            if (fields.Count > 0)
            {
                // process last item if missing CRLF
                string field = fieldBuilder.ToString();
                if (field.EndsWith('\n'))
                {
                    field = field.Remove(field.Length - 1);
                }

                fields.Add(field);
                fieldBuilder.Clear();
                ProcessLine(fields, items, lineBuilder.ToString(), line);
            }

            return items;
        }

        private void ProcessLine(List<string> fields, ICollection<T> items, string line, int lineNumber)
        {
            if (fields.Count <= 0)
            {
                return;
            }

            if (fields.Count < NumExpectedItems)
            {
                Logger.Error(
                    $"Skipping Line {lineNumber}: '{line}' expected {NumExpectedItems} values but got {fields.Count}");
                return;
            }

            T item = default;
            try
            {
                item = CreateInstance(fields.ToArray());
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }


            if (item == null)
            {
                Logger.Error($"Skipping Line {lineNumber}: '{line}' could not be converted");
                return;
            }

            items.Add(item);
        }
    }
}