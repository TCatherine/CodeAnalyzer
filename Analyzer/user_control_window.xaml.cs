using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System;
using Microsoft.VisualStudio.Shell;
using System.IO;
using System.Text.RegularExpressions;

namespace Analyzer
{
    public partial class CodeAnalyzerControl : UserControl
    {
        public CodeAnalyzerControl()
        {
            this.InitializeComponent();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]


        private string result = "";
        String[] code;
        string KeyWords =
            "\\b(alignas|alignof|and|and_eq|asm|auto|bitand|bitor|bool|break|case|catch|char|char16_t|char32_t" +
                "|class|compl|const|constexpr|const_cast|continue|decltype|default|delete|do|double|dynamic_cast" +
                "|else|enum|explicit|export|extern|false|float|for|friend|goto|if|inline|int|long|mutable|namespace" +
                "|new|noexcept|not|not_eq|nullptr|operator|or|or_eq|private|protected|public|register|reinterpret_cast" +
                "|return|short|signed|sizeof|static|static_assert|static_cast|struct|switch|template|this|thread_local" +
                "|throw|true|try|typedef|typeid|typename|union|unsigned|using|virtual|void|volatile|wchar_t|while|xor|xor_eq)\\b";

        private void header_display()
        {
            Grid new_grid = new Grid();
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            new_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(45) });

            Button button = new Button();
            button.Content = "Update";
            button.Name = "ButtonUpdate";
            button.Background = System.Windows.Media.Brushes.SteelBlue;
            button.Foreground = System.Windows.Media.Brushes.White;
            button.Height = 25;
            button.Click += new RoutedEventHandler(ButtonUpdate);

            new_grid.Children.Add(button);
            Grid.SetRow(button, 0);
            Grid.SetColumn(button, 0);
            Grid.SetColumnSpan(button, 4);

            TextBlock function_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkBlue,
                Text = "Function Name",
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top
            };

            new_grid.Children.Add(function_block);
            Grid.SetRow(function_block, 1);
            Grid.SetColumn(function_block, 0);

            TextBlock total_line_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkBlue,
                Text = "Line Number",
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            new_grid.Children.Add(total_line_block);
            Grid.SetRow(total_line_block, 1);
            Grid.SetColumn(total_line_block, 1);

            TextBlock line_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkBlue,
                Text = "Comment Line\nNumber",
                Height = 40,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            new_grid.Children.Add(line_block);
            Grid.SetRow(line_block, 1);
            Grid.SetColumn(line_block, 2);

            TextBlock keywords_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.DarkBlue,
                Text = "Keywords\nNumber",
                Height = 40,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            new_grid.Children.Add(keywords_block);
            Grid.SetRow(keywords_block, 1);
            Grid.SetColumn(keywords_block, 3);

            this.FunctionsInformation.Children.Add(new_grid);
        }

        private void append_row(string function_name, int count_line, int count_line_comments = 0, int count_line_keyboards = 0)
        {
            
            Grid new_grid = new Grid();
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.ColumnDefinitions.Add(new ColumnDefinition());
            new_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });

            TextBlock function_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.SteelBlue,
                Text = function_name,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top
            };

            new_grid.Children.Add(function_block);
            Grid.SetRow(function_block, 0);
            Grid.SetColumn(function_block, 0);

            TextBlock total_line_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.SteelBlue,
                Text = (count_line).ToString(),
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            new_grid.Children.Add(total_line_block);
            Grid.SetRow(total_line_block, 0);
            Grid.SetColumn(total_line_block, 1);

            TextBlock line_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.SteelBlue,
                Text = count_line_comments.ToString(),
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            new_grid.Children.Add(line_block);
            Grid.SetRow(line_block, 0);
            Grid.SetColumn(line_block, 2);

            TextBlock keywords_block = new TextBlock()
            {
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.SteelBlue,
                Text = count_line_keyboards.ToString(),
                Height = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            new_grid.Children.Add(keywords_block);
            Grid.SetRow(keywords_block, 0);
            Grid.SetColumn(keywords_block, 3);

            this.FunctionsInformation.Children.Add(new_grid);
            this.UpdateLayout();
        }

        int search_comments(ref string function_code)
        {
            int num_line = 0;
            string comments = @"(?:\/\/(?:\\\n|[^\n])*\n)|(?:\/\*[\s\S]*?\*\/)|((?:R""([^(\\\s]{0,16})\([^)]*\)\2"")|(?:@""[^""]*?"")|(?:""(?:\?\?'|\\\\|\\""|\\\n|[^""])*?"")|(?:'(?:\\\\|\\'|\\\n|[^'])*?'))";

            Regex rg_comments = new Regex(comments);
            MatchCollection matched = rg_comments.Matches(function_code);

            for (int i = 0; i < matched.Count; i++)
            {
                if (matched[i].Groups[1].Length == 0)
                {
                    string[] split_comment = matched[i].Value.Split('\n');
                    num_line += split_comment.Length;
                    if (matched[i].Value[matched[i].Length - 1] == '\n')
                        num_line--;
                    function_code = function_code.Replace(matched[i].Value, "");
                }
            }
            return num_line;
        }

        int count_keywords(string functions_code)
        {
            Regex rg_word = new Regex(KeyWords);
            MatchCollection matched = rg_word.Matches(functions_code);
            return  matched.Count;
        }
        private void ButtonUpdate(object sender, RoutedEventArgs e)
        {
            this.FunctionsInformation.Children.Clear();
            header_display();

            EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));
            code = File.ReadAllLines(System.IO.Path.GetFullPath(dte.ActiveDocument.FullName));
            result = "";

            string line_code = "";
            foreach (string s in code)
            {
                line_code += s;
                line_code += '\n';
            }
            string begin_function = "[\\s\\w]+\\w+\\s+\\w+\\s*\\([\\s\\w]*\\)\\s*{";
            Regex rg_function = new Regex(begin_function);
            MatchCollection matched = rg_function.Matches(line_code);
            for (int i = 0; i < matched.Count; i++)
            {
                if (matched[i].Value.Contains("namespace") || matched[i].Value.Contains("class") 
                    || matched[i].Value.Contains("template") || matched[i].Value.Contains("struct"))
                    continue;
                else
                {
                    int count_line = 1;
                    int k = 0;
                    while (matched[i].Value[k] == '\n' && k < matched[i].Value.Length) k++;
                    for (k = k; k < matched[i].Value.Length; k++)
                        if (matched[i].Value[k] == '\n')
                            count_line++;
                    string function_name = Regex.Replace(matched[i].Value, "\\s+", " ");
                    function_name = Regex.Replace(function_name, "{", "");
                    int idx = matched[i].Index + matched[i].Length;
                    int count_bracket = 0;
                    for (int j = idx; j < line_code.Length; j++)
                    {
                        if (line_code[j] == '\n')
                        {
                            count_line++;
                            continue;
                        }

                        if (line_code[j] == '{')
                        {
                            count_bracket++;
                            continue;
                        }

                        if (line_code[j] == '}')
                        {
                            if (count_bracket != 0)
                            {
                                count_bracket--;
                                continue;
                            }

                            string substring = line_code.Substring(idx, j - idx);
                            int num_line = search_comments(ref substring);
                            int num_keyword = count_keywords(substring);
                            append_row(function_name, count_line, num_line, num_keyword);
                        }
                    
                    }

                }
            }
        }

    }
}
