namespace TestGame.UI;

public partial class Form1 : Form
{
    private static GameState _state => GameState.Instance;
    private Player _player => _state.Player;

    private MovingEngine _movingEngine;
    private SpawningEngine _spawningEngine;
    private AttackingEngine _attackingEngine;
    private Renderer _renderer;

    private Debugger _debuggerWindow;

    public Form1()
    {
        InitializeComponent();

        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        //StartDebugger();
        InitGame();
    }

    private void StartDebugger()
    {
        _debuggerWindow = new Debugger();
        _debuggerWindow.Show();

        Logger.AddDestination(_debuggerWindow);
        Logger.AddLoggerFilter((category, _) => category == LogCategory.Attack);
    }

    private void InitGame()
    {
        var mapsProvider = new InMemoryMapsProvider();
        var map = mapsProvider.GetMapByName("Default");

        _state.Map = map;

        _spawningEngine = new SpawningEngine(map.PlayerSpawnTile);
        _spawningEngine.SpawnPlayer();
        _spawningEngine.InitialSpawn();

        _state.Init(ClientSize);

        _movingEngine = new MovingEngine(_state);
        _attackingEngine = new AttackingEngine(_state);

        _state.Player.Weapon = new LightenedDagger();

        _renderer = new Renderer(_state);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        //Logger.Messure("Processing", () =>
        //{
            DoMoves();
            SpawnEntities();
            ProcessAttacks();
        //});

        //Logger.Messure("Rendering", () =>
        //{
            Render(g);
        //});
    }

    private void SpawnEntities()
    {
        _spawningEngine.Spawn();
    }

    private void DoMoves()
    {
        _movingEngine.Move();
    }

    private void ProcessAttacks()
    {
        _attackingEngine.ProcessAttacks();
    }

    private void Render(Graphics g)
    {
        _renderer.Render(g);
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        Time.CurrentFrameStartedAt = DateTime.Now;
        Refresh();
        Time.PreviousFrameFinishedAt = Time.CurrentFrameStartedAt;
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
        if (e.KeyCode == Keys.Space)
        {
            _player.Attack();
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

    private void Form1_Resize(object sender, EventArgs e)
    {
        _state.Camera.Resize(ClientSize);
        _renderer.ClearBitmapCache();
    }

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }

            _debuggerWindow?.Close();
        }
        base.Dispose(disposing);
    }

}
