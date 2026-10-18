namespace ClientTokenProvider.Shared.Drawables;

internal sealed class CircularSpinnerDrawable :
    BindableObject,
    IDrawable
{
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
       nameof(Size),
       typeof(int),
       typeof(CircularSpinnerDrawable));

    public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(
        nameof(Thickness),
        typeof(int),
        typeof(CircularSpinnerDrawable));

    public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(
        nameof(PrimaryColor),
        typeof(Color),
        typeof(CircularSpinnerDrawable));

    public static readonly BindableProperty SecondaryColorProperty = BindableProperty.Create(
        nameof(SecondaryColor),
        typeof(Color),
        typeof(CircularSpinnerDrawable));

    public static readonly BindableProperty StartAngleProperty = BindableProperty.Create(
        nameof(StartAngle),
        typeof(float),
        typeof(CircularSpinnerDrawable));

    public static readonly BindableProperty EndAngleProperty = BindableProperty.Create(
        nameof(EndAngle),
        typeof(float),
        typeof(CircularSpinnerDrawable));

    public int Size
    {
        get => (int)GetValue(SizeProperty);
        set => SetValue(SizeProperty, Math.Max(value, 0));
    }

    public int Thickness
    {
        get => (int)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, Math.Max(value, 0));
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

    public float StartAngle
    {
        get => (float)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public float EndAngle
    {
        get => (float)GetValue(EndAngleProperty);
        set => SetValue(EndAngleProperty, value);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var x = Thickness / 2f;
        var y = Thickness / 2f;
        float effectiveSize = Size - Thickness;

        DrawArc(canvas, x, y, effectiveSize);
    }

    private void DrawArc(ICanvas canvas, float x, float y, float effectiveSize)
    {
        if (EndAngle == -360 + StartAngle)
        {
            DrawPrimaryColorArc(canvas, x, y, effectiveSize);
        }
        else if (EndAngle == StartAngle)
        {
            DrawSecondaryColorEllipse(canvas, x, y, effectiveSize);
        }
        else
        {
            DrawSecondaryColorEllipse(canvas, x, y, effectiveSize);
            DrawPrimaryColorArc(canvas, x, y, effectiveSize);
        }
    }

    private void DrawPrimaryColorArc(
        ICanvas canvas,
        float x,
        float y,
        float effectiveSize)
    {
        canvas.StrokeColor = PrimaryColor;
        canvas.StrokeSize = Thickness;
        canvas.DrawArc(
            x,
            y,
            effectiveSize,
            effectiveSize,
            StartAngle,
            EndAngle,
            true,
            false);
    }

    private void DrawSecondaryColorEllipse(
        ICanvas canvas,
        float x,
        float y,
        float effectiveSize)
    {
        canvas.StrokeColor = SecondaryColor;
        canvas.StrokeSize = Thickness;
        canvas.DrawEllipse(
            x,
            y,
            effectiveSize,
            effectiveSize);
    }
}
