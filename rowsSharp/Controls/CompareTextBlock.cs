using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RowsSharp.Controls
{
    public class HighlightingTextBlock : CompareTextBlock
    {
        public HighlightingTextBlock()
        {
            HighlightForeground = Brushes.Black;
            HighlightBackground = Brushes.Transparent;

            HighlightBackground3 = Brushes.Yellow;
        }

        protected override IReadOnlyCollection<TextGroup> Compare(string mainText, string compareText)
        {
            return LookUpComparer.CompareText(mainText, compareText).ToArray();
        }
    }

    /// <summary>
    /// based on <a href="https://github.com/deanchalk/SearchMatchTextblock"></a>
    /// </summary>
    [TemplatePart(Name = compareTextBlockName, Type = typeof(TextBlock))]
    public class CompareTextBlock : TextBlock
    {
        private const string compareTextBlockName = "PART_CompareTextblock";

        private static readonly DependencyPropertyKey AdditionsCountPropertyKey = DependencyProperty.RegisterReadOnly("AdditionsCount", typeof(int), typeof(CompareTextBlock), new PropertyMetadata(0));
        public static readonly DependencyProperty AdditionsCountProperty = AdditionsCountPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey SubtractionsCountPropertyKey = DependencyProperty.RegisterReadOnly("SubtractionsCount", typeof(int), typeof(CompareTextBlock), new PropertyMetadata(0));
        public static readonly DependencyProperty SubtractionsCountProperty = SubtractionsCountPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey OtherCountPropertyKey = DependencyProperty.RegisterReadOnly("OtherCount", typeof(int), typeof(CompareTextBlock), new PropertyMetadata(0));
        public static readonly DependencyProperty OtherCountProperty = OtherCountPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey Other2CountPropertyKey = DependencyProperty.RegisterReadOnly("Other2Count", typeof(int), typeof(CompareTextBlock), new PropertyMetadata(0));
        public static readonly DependencyProperty Other2CountProperty = Other2CountPropertyKey.DependencyProperty;

        public static readonly DependencyProperty CompareTextProperty = DependencyProperty.Register("CompareText", typeof(string), typeof(CompareTextBlock), new PropertyMetadata(string.Empty, OnCompareTextPropertyChanged));
        public static readonly DependencyProperty OriginalTextProperty = DependencyProperty.Register("OriginalText", typeof(string), typeof(CompareTextBlock), new PropertyMetadata(string.Empty, OnOriginalTextPropertyChanged));
        //public static readonly DependencyProperty TextProperty = TextBlock.TextProperty.AddOwner(typeof(CompareTextBlock), new PropertyMetadata(string.Empty, OnTextPropertyChanged));
        //public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(CompareTextBlock), new PropertyMetadata(TextWrapping.NoWrap));
        //public static readonly DependencyProperty TextTrimmingProperty = TextBlock.TextTrimmingProperty.AddOwner(typeof(CompareTextBlock), new PropertyMetadata(TextTrimming.None));

        public static readonly DependencyProperty HighlightForegroundProperty = DependencyProperty.Register("HighlightForeground", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty HighlightForeground2Property = DependencyProperty.Register("HighlightForeground2", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty HighlightForeground3Property = DependencyProperty.Register("HighlightForeground3", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty HighlightForeground4Property = DependencyProperty.Register("HighlightForeground4", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty HighlightBackgroundProperty = DependencyProperty.Register("HighlightBackground", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty HighlightBackground2Property = DependencyProperty.Register("HighlightBackground2", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.LightGreen));
        public static readonly DependencyProperty HighlightBackground3Property = DependencyProperty.Register("HighlightBackground3", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.LightPink));
        public static readonly DependencyProperty HighlightBackground4Property = DependencyProperty.Register("HighlightBackground4", typeof(Brush), typeof(CompareTextBlock), new PropertyMetadata(Brushes.Purple));



        static CompareTextBlock()
        {
            //TextProperty.OverrideMetadata(typeof(CompareTextBlock), new FrameworkPropertyMetadata(string.Empty, OnTextPropertyChanged));
            TextWrappingProperty.OverrideMetadata(typeof(CompareTextBlock), new FrameworkPropertyMetadata(TextWrapping.NoWrap));
            TextTrimmingProperty.OverrideMetadata(typeof(CompareTextBlock), new FrameworkPropertyMetadata(TextTrimming.None));
        }

        #region properties

        public int AdditionsCount
        {
            get => (int)GetValue(AdditionsCountProperty);
            protected set => SetValue(AdditionsCountPropertyKey, value);
        }

        public Brush HighlightBackground
        {
            get => (Brush)GetValue(HighlightBackgroundProperty);
            set => SetValue(HighlightBackgroundProperty, value);
        }

        public Brush HighlightForeground
        {
            get => (Brush)GetValue(HighlightForegroundProperty);
            set => SetValue(HighlightForegroundProperty, value);
        }

        public Brush HighlightBackground2
        {
            get => (Brush)GetValue(HighlightBackground2Property);
            set => SetValue(HighlightBackground2Property, value);
        }

        public Brush HighlightForeground2
        {
            get => (Brush)GetValue(HighlightForeground2Property);
            set => SetValue(HighlightForeground2Property, value);
        }

        public Brush HighlightBackground3
        {
            get => (Brush)GetValue(HighlightBackground3Property);
            set => SetValue(HighlightBackground3Property, value);
        }

        public Brush HighlightForeground3
        {
            get => (Brush)GetValue(HighlightForeground3Property);
            set => SetValue(HighlightForeground3Property, value);
        }

        public Brush HighlightBackground4
        {
            get => (Brush)GetValue(HighlightBackground4Property);
            set => SetValue(HighlightBackground4Property, value);
        }

        public Brush HighlightForeground4
        {
            get => (Brush)GetValue(HighlightForeground4Property);
            set => SetValue(HighlightForeground4Property, value);
        }

        public string CompareText
        {
            get => (string)GetValue(CompareTextProperty);
            set => SetValue(CompareTextProperty, value);
        }

        public string OriginalText
        {
            get => (string)GetValue(OriginalTextProperty);
            set => SetValue(OriginalTextProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }

        #endregion properties

        private static void OnCompareTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textblock = (CompareTextBlock)d;
            textblock.ProcessTextChanged(textblock.OriginalText, e.NewValue as string ?? throw new Exception("sddd fd3 4!!4"));
        }

        private static void OnOriginalTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textblock = (CompareTextBlock)d;
            textblock.ProcessTextChanged(e.NewValue as string ?? throw new Exception("sdfd3 4!!4"), textblock.CompareText);
        }

        private CancellationTokenSource tokenSource = new();

        private async void ProcessTextChanged(string mainText, string compareText)
        {
            int count = 0;
            Reset();
            if (Validate(mainText, CompareText) == false)
                return;
            if (tokenSource.IsCancellationRequested == false)
                tokenSource.Cancel(false);
            else
            {
            }
            tokenSource = new();
            Task<IEnumerable<dynamic>> task = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                return Compare(mainText, compareText);
            }, tokenSource.Token);
            var result = await task;
            this.Inlines.Clear();
            foreach (var group in result)
            {
                foreach (TextGroup l in group.list)
                {
                    var (foreground, background) = GetGrounds(l.Difference);
                    if (l.Difference != 0)
                    {
                        count++;
                    }
                    //else
                    //    continue;
                    this.Inlines.Add(new Run(l.Text) { Background = background, Foreground = foreground });
                }
                this.Inlines.Add(new LineBreak());
            }
            AdditionsCount = count;
        }

        private (Brush foreground, Brush background) GetGrounds(short diff)
        {
            return diff switch
            {
                0 => (HighlightForeground, HighlightBackground),
                1 => (HighlightForeground2, HighlightBackground2),
                -1 => (HighlightForeground3, HighlightBackground3),
                2 => (HighlightForeground4, HighlightBackground4),
                _ => throw new Exception("dfssdf  sdfsdf"),
            };
        }

        private bool Validate(string mainText, string compareText)
        {
            return !(this == null ||
                string.IsNullOrWhiteSpace(mainText) ||
                string.IsNullOrWhiteSpace(compareText));
        }

        private void Reset()
        {
            this?.Inlines.Clear();
            SetValue(AdditionsCountPropertyKey, 0);
        }

        public override void OnApplyTemplate()
        {
            ProcessTextChanged(Text, CompareText);
        }

        protected virtual IEnumerable<dynamic> Compare(string mainText, string compareText)
        {
            //return DiffComparer.CompareText(mainText, compareText).ToArray();
            var originalSplit = mainText.Split(new[] { '\r', '\n' });
            var compareSplit = CompareText.Split(new[] { '\r', '\n' });
            for (int i = 0; i < compareSplit.Length; i++)
            {
                if (originalSplit.Length > i)
                    if (originalSplit[i] != compareSplit[i])
                    {
                        var dd = new { list = _ddsf(originalSplit[i], compareSplit[i]) };



                        yield return dd;

                    }
            }

        }


        static IEnumerable<TextGroup> _ddsf(string s1, string s2)
        {

            List<string> diff;
            string[] set1 = s1.Split(',');
            string[] set2 = s2.Split(',');


            for (int i = 0; i < set1.Length; i++)
            {
                if (set2.Length > i)
                    if (set1[i] != set2[i])
                    {
                        var compare = DiffComparer.CompareText(set1[i], set2[i]).ToArray();
                        foreach (var c in compare)
                            yield return new TextGroup(c.Text, s2.IndexOf(set2[i]) + c.Index, c.Difference);

                    }
                    else
                    {
                        yield return new TextGroup(set1[i] + ",", s2.IndexOf(set1[i]), 0);

                    }
            }
        }



        public class TextGroup
        {
            public TextGroup(string text, int index, short difference)
            {
                Text = text;
                Index = index;
                Difference = difference;
            }

            public string Text { get; }
            public int Index { get; }
            public short Difference { get; }
        }

        internal class DiffComparer
        {
            public static IEnumerable<TextGroup> CompareText(string mainText, string CompareText)
            {
                var differences = Netsoft.Diff.Differences.Between(
                    mainText.Split(new[] { '\r', '\n' }),
                    CompareText.Split(new[] { '\r', '\n' }));
                int i = 0;
                int changeIndex = 0;
                StringBuilder stringBuilder = new StringBuilder();
                short? action = 0;
                foreach (var difference in differences)
                {
                    if (action.HasValue && difference.Action != action)
                    {
                        yield return new TextGroup(stringBuilder.ToString(), changeIndex, action.Value);
                        stringBuilder.Clear();
                        changeIndex = i;
                    }
                    stringBuilder.Append(difference.Value);
                    i++;
                    action = difference.Action;
                }

                if (stringBuilder.Length > 0)
                    yield return new TextGroup(stringBuilder.ToString(), changeIndex, action.Value);
            }
        }

        internal class LookUpComparer
        {
            public static IEnumerable<TextGroup> CompareText(string mainText, string CompareText)
            {
                if (string.IsNullOrWhiteSpace(CompareText))
                {
                    yield return new TextGroup(mainText, 0, Convert.ToInt16(false));
                    yield break;
                }

                var find = 0;
                var searchTextLength = CompareText.Length;
                while (find >= 0)
                {
                    var oldFind = find;
                    find = mainText.IndexOf(CompareText, find, StringComparison.InvariantCultureIgnoreCase);
                    yield return GetTextGroup(mainText, ref find, searchTextLength, oldFind);
                }
            }

            private static TextGroup GetTextGroup(string mainText, ref int find, int searchTextLength, int oldFind)
            {
                if (find == -1)
                {
                    return
                       oldFind > 0
                           ? Create(mainText.Substring(oldFind, mainText.Length - oldFind), find, false)
                           : Create(mainText, find, false);
                }
                else if (oldFind == find)
                {
                    find += searchTextLength;
                    return Create(mainText.Substring(oldFind, searchTextLength), find, true);
                }
                else
                    return Create(mainText.Substring(oldFind, find - oldFind), find, false);

                TextGroup Create(string text, int index, bool isHighlighted)
                {
                    return new TextGroup(text, index, Convert.ToInt16(isHighlighted));
                }
            }
        }
    }
}