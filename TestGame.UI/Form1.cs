namespace TestGame.UI
{
    public partial class Form1 : Form
    {
        private static readonly Bitmap HeroAnimations = Resources.ghost;

        private static List<IRenderable> _entitiesToRender = new();

        private Player _player;
        private Camera _camera;

        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            InitGame();
        }

        private void InitGame()
        {
            _player = new Player(new Position(100, 100), HeroAnimations);
            _camera = new Camera(_player, ClientSize);
            _entitiesToRender.Add(_player);
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
            foreach (var entity in _entitiesToRender)
            {
                var position = _camera.ToCameraPosition(entity.Position);
                g.DrawImage(entity.GetFrame(), position.X, position.Y);
            }

            var randomPosition = _camera.ToCameraPosition(new Position(200, 200));
            g.DrawImage(HeroAnimations, randomPosition.X, randomPosition.Y);
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