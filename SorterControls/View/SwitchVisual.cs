using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MathUtils.Rand;
using Sorting.KeyPairs;
using SortNetwork.Sorters;

namespace SorterControls.Views.Entities
{
    public class SwitchVisual : Canvas
    {
        public SwitchVisual()
        {
            SizeChanged += (s, e) => DrawVisual();
        }

        private readonly List<SolidColorBrush> _solidColorBrushes = new List<SolidColorBrush>();
        private DrawingVisual _switchVisual;


        double HalfThickness
        {
            get { return 0.05 / Switch.KeyPair.KeyCount; }
        }

        double ActualHalfKeyHeight
        {
            get { return (0.5 * ActualHeight) / Switch.KeyPair.KeyCount; }
        }

        double ActualHalfThickness
        {
            get { return ActualHeight * HalfThickness; }
        }

        void DrawVisual()
        {
            if (Switch == null)
            {
                return;
            }
            DrawKeyLines();
            DrawSwitch();
        }

        void SetupResources()
        {
            if (Switch == null) { return; }

            _switchVisual = new DrawingVisual();

            var randy = Randy.Fast(333).ToDouble();
            for (var i = 0; i < Switch.KeyPair.KeyCount; i++)
            {
                var klvCur = new DrawingVisual();
                _keyLines.Add(klvCur);
                AddVisualChild(klvCur);
                AddLogicalChild(klvCur);
                var scb = new SolidColorBrush(
                    new Color
                    {
                        ScA = (float)1.0,
                        ScB = (float)randy.Next(),
                        ScG = (float)randy.Next(),
                        ScR = (float)randy.Next()
                    });
                scb.Freeze();
                _solidColorBrushes.Add(scb);
            }

        }

        void DrawKeyLines()
        {
            for (var keyDex = 0; keyDex < Switch.KeyPair.KeyCount; keyDex++)
            {
                using (var dc = _keyLines[keyDex].RenderOpen())
                {
                    dc.DrawGeometry(_solidColorBrushes[keyDex], null, CreateKeyLineGeometry(keyDex));
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
            //if (!GeometryNeedsRefresh)
            //{
            //    return geometry;
            //}

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
            var lineHeight = ActualHalfKeyHeight + ActualHeight * keyLineDex / KeyPair.KeyCount;

            yield return new Point(0,           lineHeight - ActualHalfThickness);
            yield return new Point(ActualWidth, lineHeight - ActualHalfThickness);
            yield return new Point(ActualWidth, lineHeight + ActualHalfThickness);
            yield return new Point(0,           lineHeight + ActualHalfThickness);
        }

        IEnumerable<Point> SwitchPoints
        {
            get
            {
                var topLineHeight = ActualHalfKeyHeight + ActualHeight * Switch.KeyPair.HiKey / Switch.KeyPair.KeyCount;
                var bottomLineHeight = ActualHalfKeyHeight + ActualHeight * Switch.KeyPair.LowKey / Switch.KeyPair.KeyCount;

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
                return (Switch == null) ? 0 : KeyPair.KeyCount + 1;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return index < _keyLines.Count ? _keyLines[index] : _switchVisual;
        }

        
        readonly List<DrawingVisual> _keyLines = new List<DrawingVisual>();

        #region sorterSwitch

        [Category("Custom Properties")]
        public IKeyPair KeyPair
        {
            get { return (IKeyPair)GetValue(KeyPairProperty); }
            set { SetValue(KeyPairProperty, value); }
        }

        public static readonly DependencyProperty KeyPairProperty =
            DependencyProperty.Register("KeyPair", typeof(IKeyPair), typeof(SwitchVisual),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnSamplePlateVmPropertyChanged));

        private static void OnSamplePlateVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var switchVisual = d as SwitchVisual;
            if (switchVisual == null) return;
            switchVisual.SetupResources();
            switchVisual.DrawVisual();
        }

        #endregion
    }
}
