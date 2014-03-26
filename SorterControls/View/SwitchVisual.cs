using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sorting.KeyPairs;

namespace SorterControls.View
{
    public class SwitchVisual : Canvas
    {
        public SwitchVisual()
        {
            SizeChanged += (s, e) => DrawVisual();
        }

        private DrawingVisual _switchVisual;

        double HalfThickness
        {
            get { return 0.05 / KeyCount; }
        }

        double ActualHalfKeyHeight
        {
            get { return (0.5 * ActualHeight) / KeyCount; }
        }

        double ActualHalfThickness
        {
            get { return ActualHeight * HalfThickness; }
        }

        void DrawVisual()
        {
            if (KeyPair == null)
            {
                return;
            }
            DrawKeyLines();
            DrawSwitch();
        }

        void SetupResources()
        {
            if (KeyPair == null) { return; }

            _switchVisual = new DrawingVisual();

            for (var i = 0; i < KeyCount; i++)
            {
                var klvCur = new DrawingVisual();
                _keyLines.Add(klvCur);
                AddVisualChild(klvCur);
                AddLogicalChild(klvCur);
            }

        }

        void DrawKeyLines()
        {
            for (var keyDex = 0; keyDex < KeyCount; keyDex++)
            {
                using (var dc = _keyLines[keyDex].RenderOpen())
                {
                    dc.DrawGeometry(LineBrushes[keyDex], null, CreateKeyLineGeometry(keyDex));
                }
            }
        }

        void DrawSwitch()
        {
            using (var dc = _switchVisual.RenderOpen())
            {
                dc.DrawGeometry(Brushes.Green, null, CreateSwitchGeometry());
            }
        }

        private StreamGeometry CreateKeyLineGeometry(int keyDex)
        {
            var geometry = new StreamGeometry();

            using (var ctx = geometry.Open())
            {
                var firstPoint = true;

                //System.Diagnostics.Debug.WriteLine("Begin");
                foreach (var pt in KeyLinePoints(keyDex))
                {
                    if (firstPoint)
                    {
                        ctx.BeginFigure(pt, true, false);
                        firstPoint = false;
                    }
                    else
                    {
                        ctx.LineTo(pt, true, false);
                    }
                    //System.Diagnostics.Debug.WriteLine(pt.X.ToString("0.00") + ", " + pt.Y.ToString("0.00"));
                }
                //System.Diagnostics.Debug.WriteLine("End");
            }

            return geometry;
        }

        private StreamGeometry CreateSwitchGeometry()
        {
            var geometry = new StreamGeometry();

            using (var ctx = geometry.Open())
            {
                var firstPoint = true;

                foreach (var pt in SwitchPoints)
                {
                    if (firstPoint)
                    {
                        ctx.BeginFigure(pt, true, false);
                        firstPoint = false;
                    }
                    else
                    {
                        ctx.LineTo(pt, true, false);
                    }
                }
            }

            return geometry;
        }

        IEnumerable<Point> KeyLinePoints(int keyLineDex)
        {
            var lineHeight = ActualHalfKeyHeight + ActualHeight * keyLineDex / KeyCount;

            yield return new Point(0,           lineHeight - ActualHalfThickness);
            yield return new Point(ActualWidth, lineHeight - ActualHalfThickness);
            yield return new Point(ActualWidth, lineHeight + ActualHalfThickness);
            yield return new Point(0,           lineHeight + ActualHalfThickness);
        }

        IEnumerable<Point> SwitchPoints
        {
            get
            {
                var topLineHeight = ActualHalfKeyHeight + ActualHeight * KeyPair.HiKey / KeyCount;
                var bottomLineHeight = ActualHalfKeyHeight + ActualHeight * KeyPair.LowKey / KeyCount;

                yield return new Point(ActualWidth / 2 - ActualHalfThickness, topLineHeight + ActualHalfThickness);
                yield return new Point(ActualWidth / 2 + ActualHalfThickness, topLineHeight + ActualHalfThickness);
                yield return new Point(ActualWidth / 2 + ActualHalfThickness, bottomLineHeight - ActualHalfThickness);
                yield return new Point(ActualWidth / 2 - ActualHalfThickness, bottomLineHeight - ActualHalfThickness);
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return (KeyPair == null) ? 0 : KeyCount + 1;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return index < _keyLines.Count ? _keyLines[index] : _switchVisual;
        }

        
        readonly List<DrawingVisual> _keyLines = new List<DrawingVisual>();

        #region KeyPair

        [Category("Custom Properties")]
        public IKeyPair KeyPair
        {
            get { return (IKeyPair)GetValue(KeyPairProperty); }
            set { SetValue(KeyPairProperty, value); }
        }

        public static readonly DependencyProperty KeyPairProperty =
            DependencyProperty.Register("KeyPair", typeof(IKeyPair), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnKeyPairPropertyChanged));

        private static void OnKeyPairPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;
            if (switchVisual.KeyCount == DefaultKeyCount) return;
            if (switchVisual.LineBrushes.Count == 0) return;

            //switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion


        #region KeyCount

        private const int DefaultKeyCount = -1;

        [Category("Custom Properties")]
        public int KeyCount
        {
            get { return (int)GetValue(KeyCountProperty); }
            set { SetValue(KeyCountProperty, value); }
        }

        public static readonly DependencyProperty KeyCountProperty =
            DependencyProperty.Register("KeyCount", typeof(int), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(DefaultKeyCount, FrameworkPropertyMetadataOptions.AffectsRender, OnKeyCountPropertyChanged));

        private static void OnKeyCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;
            if (switchVisual.KeyPair == null) return;
            if (switchVisual.LineBrushes.Count == 0) return;

            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion


        #region LineBrushes

        private static readonly List<Brush> DefaultLineBrushes = new List<Brush>();

        [Category("Custom Properties")]
        public List<Brush> LineBrushes
        {
            get { return (List<Brush>)GetValue(LineBrushesProperty); }
            set { SetValue(LineBrushesProperty, value); }
        }

        public static readonly DependencyProperty LineBrushesProperty =
            DependencyProperty.Register("LineBrushes", typeof(List<Brush>), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(DefaultLineBrushes, FrameworkPropertyMetadataOptions.AffectsRender, OnDefaultLineBrushesChanged));

        private static void OnDefaultLineBrushesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;
            if (switchVisual.KeyCount == DefaultKeyCount) return;
            if (switchVisual.KeyPair == null) return;


            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion

    }
}
