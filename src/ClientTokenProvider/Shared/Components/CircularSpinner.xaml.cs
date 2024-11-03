namespace ClientTokenProvider.Shared.Components;

public partial class CircularSpinner :
    ContentView
{
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
       nameof(Size),
       typeof(int),
       typeof(CircularSpinner),
       propertyChanged: OnSizePropertyChanged);

    public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(
        nameof(Thickness),
        typeof(int),
        typeof(CircularSpinner));

    public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(
        nameof(PrimaryColor),
        typeof(Color),
        typeof(CircularSpinner));

    public static readonly BindableProperty SecondaryColorProperty = BindableProperty.Create(
        nameof(SecondaryColor),
        typeof(Color),
        typeof(CircularSpinner));

    public static readonly BindableProperty SpeedProperty = BindableProperty.Create(
       nameof(SpeedProperty),
       typeof(int),
       typeof(CircularSpinner),
       propertyChanged: OnSpeedPropertyChanged);

    public static readonly BindableProperty EnabledProperty = BindableProperty.Create(
       nameof(EnabledProperty),
       typeof(bool),
       typeof(CircularSpinner),
       propertyChanged: OnEnabledPropertyChanged);

    private readonly IDispatcherTimer _timer;

    public int Size
    {
        get => (int)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public int Thickness
    {
        get => (int)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    public Color PrimaryColor
    {
        get => (Color)GetValue(PrimaryColorProperty);
        set => SetValue(PrimaryColorProperty, value);
    }

    public Color SecondaryColor
    {
        get => (Color)GetValue(SecondaryColorProperty);
        set => SetValue(SecondaryColorProperty, value);
    }

    /// <summary>
    ///  Gets or sets a speed (count) of angles per each millisecond
    /// </summary>
    public int Speed
    {
        get => (int)GetValue(SpeedProperty);
        set => SetValue(SpeedProperty, value);
    }

    public bool Enabled
    {
        get => (bool)GetValue(EnabledProperty);
        set => SetValue(EnabledProperty, value);
    }


    public CircularSpinner()
    {
        InitializeComponent();

        _timer = Application.Current!.Dispatcher.CreateTimer();
        _timer.IsRepeating = true;
        _timer.Interval = TimeSpan.FromMilliseconds(1);
        _timer.Tick += OnTick;
    }

    private void Redraw()
    {
        GraphicsView.Invalidate();
    }

    private void UpdateBounds()
    {
        HeightRequest = Size;
        WidthRequest = Size;
    }

    private void Start()
    {
        if (!_timer.IsRunning)
        {
            CircularSpinnerDrawableLocal.EndAngle = CircularSpinnerDrawableLocal.StartAngle;

            _timer.Start();
        }
    }

    private void Stop()
    {
        if (_timer.IsRunning)
        {
            _timer.Stop();
        }
    }

    private void StartOrStop()
    {
        if (Enabled && Speed > 0)
        {
            Start();
        }
        else
        {
            Stop();
        }
    }

    private void OnTick(object? sender, EventArgs e)
    {
        var newEndAngle = CircularSpinnerDrawableLocal.EndAngle - Speed;

        if (newEndAngle < -360 + CircularSpinnerDrawableLocal.StartAngle)
        {
            newEndAngle = CircularSpinnerDrawableLocal.StartAngle;
        }

        CircularSpinnerDrawableLocal.EndAngle = newEndAngle;

        Redraw();
    }

    private static void OnSizePropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (CircularSpinner)bindable;
        self.UpdateBounds();
    }

    private static void OnSpeedPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (CircularSpinner)bindable;
        self.StartOrStop();
    }

    private static void OnEnabledPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var self = (CircularSpinner)bindable;
        self.StartOrStop();
    }
}