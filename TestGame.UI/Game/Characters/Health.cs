namespace TestGame.UI.Game.Characters
{
    public class Health
    {
        public const int HealthBarHeight = 5;

        private readonly Bitmap _healthBarBitmap;
        private readonly Size _entitySize;

        public int Current { get; private set; }
        public int Max { get; }
        public bool IsDead => Current <= 0;
        public float Percent => (float)Current / Max;

        public Health(int max, Size entitySize) : this(max, max, entitySize)
        {
        }

        public Health(int current, int max, Size entitySize)
        {
            Current = current;
            Max = max;
            _entitySize = entitySize;
            _healthBarBitmap = new Bitmap(entitySize.Width, HealthBarHeight);
            UpdateTexture();
        }

        public void Damage(int amount)
        {
            if (IsDead)
            {
                return;
            }

            Current -= amount;
            UpdateTexture();
        }

        public void Heal(int amount)
        {
            if (Current + amount > Max)
            {
                Current = Max;
                return;
            }

            Current += amount;
            UpdateTexture();
        }

        public override string ToString()
        {
            if (Current == Max)
            {
                return Max.ToString();
            }

            return $"{Current}/{Max}";
        }

        public Bitmap GetTexture()
        {
            return _healthBarBitmap;
        }

        private void UpdateTexture()
        {
            var width = _healthBarBitmap.Width;
            var healthWidth = (int)(_entitySize.Width * Percent) - 2;
            using var g = Graphics.FromImage(_healthBarBitmap);
            g.FillRectangle(new SolidBrush(Color.Gray), 0, 0, width, HealthBarHeight);
            g.FillRectangle(new SolidBrush(GetHealthBarColor()), 1, 1, healthWidth, HealthBarHeight - 2);
            g.FillRectangle(new SolidBrush(Color.DarkGray), healthWidth + 1, 1, width - healthWidth, HealthBarHeight - 2);
        }

        private Color GetHealthBarColor()
        {
            if (Percent < 0.25f)
            {
                return Color.Red;
            }

            if (Percent < 0.4f)
            {
                return Color.Orange;
            }

            if (Percent < 0.65f)
            {
                return Color.Yellow;
            }

            return Color.Green;
        }
    }
}
