using System.Windows;
using System.Windows.Media;

namespace Pomidoro
{
    class PointMatrix : FrameworkElement
    {
        public static readonly DependencyProperty FinishedBrushProperty = DependencyProperty.Register("FinishedBrush", typeof(Brush), typeof(PointMatrix),
                                                                          new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush FinishedBrush
        {
            get { return (Brush) GetValue(FinishedBrushProperty); }
            set { SetValue(FinishedBrushProperty, value); }
        }

        public static readonly DependencyProperty PendingBrushProperty = DependencyProperty.Register("PendingBrush", typeof(Brush), typeof(PointMatrix),
                                                                         new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));
            

        public Brush PendingBrush
        {
            get { return (Brush) GetValue(PendingBrushProperty); }
            set { SetValue(PendingBrushProperty, value); }
        }

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(double), typeof(PointMatrix),
                                                                    new FrameworkPropertyMetadata(3.0d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Spacing
        {
            get { return (double) GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DependencyProperty PointSizeProperty = DependencyProperty.Register("PointSize", typeof (int), typeof (PointMatrix),
                                                                      new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender));

        public int PointSize
        {
            get { return (int) GetValue(PointSizeProperty); }
            set { SetValue(PointSizeProperty, value); }
        }

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(PointMatrix),
                                                                     new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Progress
        {
            get { return (double) GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        protected override void OnRender(DrawingContext dc)
        {
            var inColumns = PointsInColumn();
            var inRows = PointsInRow();

            var noOfPoints = inColumns*inRows;

            int finishedPoints = (int) (Progress*noOfPoints);
            int currentPoint = 0;

            for (int rowIndex = 0; rowIndex < inRows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < inColumns; columnIndex++)
                {
                    currentPoint++;
                    
                    var point = new Point((columnIndex + 1)*Spacing + columnIndex*PointSize, Spacing * (rowIndex+1) + rowIndex*PointSize);
                    if (currentPoint > finishedPoints)
                    {
                        dc.DrawRectangle(PendingBrush, null, new Rect(point, new Size(PointSize, PointSize)));
                    }
                    else
                    {
                        dc.DrawRectangle(FinishedBrush, null, new Rect(point, new Size(PointSize, PointSize)));
                    }
                }
            }
        }


        private int PointsInRow()
        {
            int inRows = (int) ((RenderSize.Height - Spacing)/(PointSize + Spacing));
            return inRows;
        }

        private int PointsInColumn()
        {
            int inColumns = (int) ((RenderSize.Width - Spacing)/(PointSize + Spacing));
            return inColumns;
        }
    }
}
