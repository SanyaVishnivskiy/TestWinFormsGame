namespace TestGame.UI
{
    public partial class Debugger : Form, ILoggerDestination
    {
        public Debugger()
        {
            InitializeComponent();
        }

        public void Log(string text)
        {
            outputBox.Text += $"{text}{Environment.NewLine}";
            outputBox.SelectionStart = outputBox.Text.Length;
            outputBox.ScrollToCaret();
        }
    }
}
