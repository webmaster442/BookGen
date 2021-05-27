using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookGen.AssemblyDocument
{
    public class MarkdownBuilder
    {
        private StringBuilder _buffer;

        public MarkdownBuilder(int capacity = 4096)
        {
            _buffer = new StringBuilder(capacity);
        }

        public MarkdownBuilder H1(string text)
        {
            _buffer.Append($"# {text}\n\n");
            return this;
        }

        public MarkdownBuilder H2(string text)
        {
            _buffer.Append($"## {text}\n\n");
            return this;
        }

        public MarkdownBuilder H3(string text)
        {
            _buffer.Append($"### {text}\n\n");
            return this;
        }

        public MarkdownBuilder H4(string text)
        {
            _buffer.Append($"#### {text}\n\n");
            return this;
        }

        public MarkdownBuilder Paragraph(string text)
        {
            _buffer.Append($"{text}\n\n");
            return this;
        }

        public MarkdownBuilder OrderedList<T>(IEnumerable<T> items, int startindex = 1)
        {
            int index = startindex;
            foreach (var item in items)
            {
                _buffer.Append($"{index}. {item}\n");
                ++index;
            }
            return this;
        }

        public MarkdownBuilder UnorderedList<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _buffer.Append($"* {item}\n");
            }
            return this;
        }

        public MarkdownBuilder Table<TKey, TValue>(IDictionary<TKey, TValue> dictionary, string keyHeader, string valueHeader)
        {
            int MaxKeyWidth = dictionary.Max(x => x.Key?.ToString()?.Length ?? 0);
            int MaxValueWidth = dictionary.Max(x => x.Value?.ToString()?.Length ?? 0);

            _buffer.Append($"| {keyHeader.PadRight(MaxKeyWidth)} | {valueHeader.PadRight(MaxValueWidth)}\n");
            _buffer.Append($"| {":".PadRight(MaxKeyWidth)} | {":".PadRight(MaxValueWidth)}\n");
            foreach (var item in dictionary)
            {
                _buffer.Append($"| {item.Key?.ToString()?.PadRight(MaxKeyWidth)} | {item.Value?.ToString()?.PadRight(MaxValueWidth)}\n");
            }
            return this;
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }
    }
}
