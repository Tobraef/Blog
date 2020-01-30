using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Blog.Models
{
    public class NewEntry
    {
        [DataType(DataType.Date)]
        public DateTime Date
        {
            get;
            set;
        }

        [MaxLength(100)]
        public string Topic
        {
            get;
            set;
        }

        [HeadersValidation(ErrorMessage = "Not all paragraphs have dot at the end or are not properly sectioned")]
        public string Text
        {
            get;
            set;
        }

        public string Tags
        {
            get;
            set;
        }
    }

    public class HeadersValidation : ValidationAttribute
    {
        private bool HasLastDot(string line)
        {
            return line.Last() == '.';
        }

        bool isFirst(List<int> root)
        {
            return root.First() == 1 && root.Count == 1;
        }

        bool isValidChild(List<int> parent, List<int> child)
        {
            return parent.Count == child.Count - 1 && toLesserEqual(parent, child);
        }

        bool toLesserEqual<T>(IEnumerable<T> first, IEnumerable<T> last)
        {
            return first.SequenceEqual(last.Take(first.Count()));
        }

        bool isValidNextSibiling(List<int> first, List<int> second)
        {
            return
                first.Count == second.Count &&
                first.Last() == second.Last() - 1 &&
                toLesserEqual(first.Take(first.Count - 1), second);
        }

        bool isValidFirstChild(List<int> parent, List<int> child)
        {
            return isValidChild(parent, child) && child.Last() == 1;
        }

        bool isOlder(List<int> parent, List<int> child)
        {
            return child.Count < parent.Count;
        }

        bool Matches(IEnumerable<string> paragraphs)
        {
            var paras = paragraphs
                .Select(p => p.Split('.').Select(i => string.IsNullOrWhiteSpace(i) ? 0 : int.Parse(i)).ToList()).ToList();
            foreach (var para in paras)
            {
                para.RemoveAt(para.Count - 1);
            }
            Stack<List<int>> parents = new Stack<List<int>>();
            if (isFirst(paras.First()))
            {
                parents.Push(paras.First());
                for (int index = 1; index < paras.Count(); ++index)
                {
                    var toTest = paras[index];
                    if (isValidFirstChild(parents.Peek(), toTest))
                    {
                        parents.Push(toTest);
                    }
                    else if (isValidNextSibiling(parents.Peek(), toTest))
                    {
                        parents.Pop();
                        parents.Push(toTest);
                    }
                    else if (isOlder(parents.Peek(), toTest))
                    {
                        while (toTest.Count != parents.Peek().Count)
                        {
                            parents.Pop();
                        }
                        if (isValidNextSibiling(parents.Peek(), toTest))
                        {
                            parents.Pop();
                            parents.Push(toTest);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override bool IsValid(object value)
        {
            var text = Regex.Matches((string)value, "([0-9]\\.)+( |[0-9])");
            var paras = new List<string>();
            foreach (var t in text)
            {
                paras.Add(((Match)t).Value.TrimEnd(' '));
            }
            return paras.All(s => HasLastDot(s)) && Matches(paras);
        }
    }
}