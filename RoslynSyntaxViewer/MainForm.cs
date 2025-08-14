using RoslynQuoter;
using System;
using System.Windows.Forms;

namespace RoslynSyntaxViewer
{
    public class MainForm : Form
    {
        private TextBox inputTextBox;
        private TextBox outputTextBox;
        private Button parseButton;

        public MainForm()
        {
            Text = "Roslyn CompilationUnit Viewer";
            Width = 900;
            Height = 600;

            inputTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Width = 400,
                Height = 500,
                Left = 10,
                Top = 10
            };

            outputTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Width = 440,
                Height = 500,
                Left = 420,
                Top = 10,
                ReadOnly = true
            };

            parseButton = new Button
            {
                Text = "Parse",
                Width = 100,
                Height = 30,
                Left = 10,
                Top = 520
            };

            parseButton.Click += ParseButton_Click;

            Controls.Add(inputTextBox);
            Controls.Add(outputTextBox);
            Controls.Add(parseButton);
        }

        private void ParseButton_Click(object sender, EventArgs e)
        {
            var code = inputTextBox.Text;
            try
            {
                var quoter = new Quoter
                {
                    OpenParenthesisOnNewLine = false,
                    ClosingParenthesisOnNewLine = false,
                    UseDefaultFormatting = true
                };

                var result = quoter.Quote(code);  // خروجی: ApiCall
                outputTextBox.Text = result.ToString();  // تبدیل به متن
            }
            catch (Exception ex)
            {
                outputTextBox.Text = "Error: " + ex.Message;
            }
        }
    }
}