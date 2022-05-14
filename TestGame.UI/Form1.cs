namespace TestGame.UI;

public partial class Form1 : Form
{
    private IReadOnlyList<IRenderable> _entitiesToRender;
    private Dictionary<Bitmap, Bitmap> _bitmapsCache = new();

    private static GameState _state => GameState.Instance;
    private Player _player => _state.Player;

    private MovingEngine _movingEngine;
    private SpawningEngine _spawningEngine;

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
    }

    private void InitGame()
    {
        _state.Init(ClientSize);
        _movingEngine = new MovingEngine();

        _state.OnAllGameEntitiesChange += OnEntitiesListChange;
        OnEntitiesListChange(this, new GameEntitiesChangeEventArgs(_state.AllGameEntities));

        _spawningEngine = new SpawningEngine();
        _spawningEngine.InitialSpawn();
    }

    private void OnEntitiesListChange(object? sender, GameEntitiesChangeEventArgs e)
    {
        _entitiesToRender = e.Entities.OfType<IRenderable>().ToList();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        DoMoves();
        SpawnEntities();
        Render(g);
    }

    private void SpawnEntities()
    {
        _spawningEngine.Spawn();
    }

    private void DoMoves()
    {
        _movingEngine.Move();
    }

    private void Render(Graphics g)
    {
        foreach (var groundTile in _state.Map.GetGroundTiles())
        {
            DrawDependingOnCamera(g, groundTile);
        }

        foreach (var entity in _entitiesToRender)
        {
            DrawDependingOnCamera(g, entity);
        }

        foreach (var mapObject in _state.Map.GetMapObjects())
        {
            DrawDependingOnCamera(g, mapObject);
        }
    }

    private void DrawDependingOnCamera(Graphics g, IRenderable entity)
    {
        var position = _state.Camera.ToCameraPosition(entity.Position);
        var texture = GetTexture(entity);
        g.DrawImage(texture, position.X, position.Y);
    }

    private Bitmap GetTexture(IRenderable entity)
    {
        var texture = entity.GetTexture();

        if (_bitmapsCache.TryGetValue(texture, out var cachedTexture))
        {
            return cachedTexture;
        }

        var result = new Bitmap(texture, ViewResizer.CalculateTextureSize(entity));

        _bitmapsCache.Add(texture, result);

        return result;
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

    private void Form1_Resize(object sender, EventArgs e)
    {
        _state.Camera.ClientSize = ClientSize;
        _bitmapsCache.Clear();
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
