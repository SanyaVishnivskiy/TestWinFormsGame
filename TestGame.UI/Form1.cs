using TestGame.UI.Game.Characters;

namespace TestGame.UI
{
    public partial class Form1 : Form
    {
        private static readonly Bitmap HeroAnimations = Resources.ghost;

        private Player _player;

        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            InitGame();
        }

        private void InitGame()
        {
            _player = new Player(new Position(100, 100), HeroAnimations);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DoMoves();
            Render(g);
        }

        private void DoMoves()
        {
            _player.Move();
        }

        private void Render(Graphics g)
        {
            g.DrawImage(_player.GetNextFrame(), _player.Position.X, _player.Position.Y);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                _player.StartMoving(MoveDirection.Up);
            }
            if (e.KeyCode == Keys.S)
            {
                _player.StartMoving(MoveDirection.Down);
            }
            if (e.KeyCode == Keys.A)
            {
                _player.StartMoving(MoveDirection.Left);
            }
            if (e.KeyCode == Keys.D)
            {
                _player.StartMoving(MoveDirection.Right);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                _player.FinishMoving(MoveDirection.Up);
            }
            if (e.KeyCode == Keys.S)
            {
                _player.FinishMoving(MoveDirection.Down);
            }
            if (e.KeyCode == Keys.A)
            {
                _player.FinishMoving(MoveDirection.Left);
            }
            if (e.KeyCode == Keys.D)
            {
                _player.FinishMoving(MoveDirection.Right);
            }
        }
    }
}