using PZ3.Adition;
using PZ3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace PZ3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UInt64[,] grid = new UInt64[1000, 1000];
        NetworkModel networkModel = new NetworkModel();
        private DrawingImage drawingImage;
        private DrawingGroup drawingGroup;
        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vEff = 1;

        GeometryGroup geometryGroup = new GeometryGroup();


        short[,] maskGrid = new short[1000, 1000];
        public MainWindow()
        {
            InitializeComponent();

            networkModel = NetworkModelHandler.InitModel(@"..\..\DB\Geographic.xml");

            drawingImage = new DrawingImage();
            mainScreen.Source = drawingImage;

            drawingGroup = new DrawingGroup();
            drawingImage.Drawing = drawingGroup;

            path.Data = geometryGroup;

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    maskGrid[i, j] = 0;
                }
            }

            grid = GridHandler.InitGrid(networkModel, grid, drawingGroup, myCanvas);

            List<LineEntity> l = networkModel.vertices.GetRange(0,50);

            //foreach (LineEntity item in networkModel.vertices)
            //{
            //    BFS(item.FirstEnd, item.SecondEnd);
            //}

            //for (int i = 0; i < Lines.Count; i++)
            //{
            //    for (int j = 0; j < Lines[i].Count - 1; j++)
            //    {
            //        Draw(Lines[i][j].Item1, Lines[i][j].Item2, Lines[i][j + 1].Item1, Lines[i][j + 1].Item2);
            //    }
            //}
            GridHandler.DrawEntities(networkModel,drawingGroup, myCanvas);

        }


        public void Draw(int rowStart,int columnStart, int rowStop, int columnStop)
        {
            double x1 = (columnStart / 1000.0) * myCanvas.Width * 4.4;
            double y1 = (rowStart / 1000.0) * myCanvas.Height * 7;
            double x2 = (columnStop / 1000.0) * myCanvas.Width * 4.4;
            double y2 = (rowStop / 1000.0) * myCanvas.Height * 7;

            // horizontalna linija
            if (rowStart == rowStop)
            {
                // u desno
                if (columnStart < columnStop)
                {
                    ImageDrawing image = new ImageDrawing();
                   
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\horizontalLine.png", UriKind.Relative));

                    image.Rect = new Rect(x1+1.5, y1+0.5, x2 - x1, 2);
                    drawingGroup.Children.Add(image);
                }
                // u levo
                else
                {
                    ImageDrawing image = new ImageDrawing();
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\horizontalLine.png", UriKind.Relative));

                    image.Rect = new Rect(x2+1.5, y1+0.5, x1 - x2, 2);
                    drawingGroup.Children.Add(image);
                }
            }

            // vertikalna linija
            if (columnStart == columnStop)
            {
                // na dole
                if (rowStart < rowStop)
                {
                    ImageDrawing image = new ImageDrawing();
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\verticalLine.png", UriKind.Relative));

                    image.Rect = new Rect(x1+0.5, y1+ 1.5, 2, y2 - y1);
                    drawingGroup.Children.Add(image);
                }
                // na gore
                else
                {
                    ImageDrawing image = new ImageDrawing();
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\verticalLine.png", UriKind.Relative));

                    image.Rect = new Rect(x1+0.5, y2+ 1.5, 2, y1 - y2);
                    drawingGroup.Children.Add(image);
                }
            }
        }

        List<List<Tuple<int, int>>> Lines = new List<List<Tuple<int, int>>>();
        //int lineIndex = (item.Row ) * 1000 + item.Column;
        private void BFS(UInt64 idStart, UInt64 idEnd)
        {
            Node[,] linesGrid = new Node[1000, 1000];
  
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    Node node = new Node();
                    node.cellId = i*1000+j;
                    if (j == 999 || i == 999 || i == 0 || j ==0)
                        node.cell = 3;
                    else
                        node.cell = maskGrid[i, j];
                    node.pred = -1;
                    linesGrid[i, j] = node;
                }
            }

            int countStep = 0;
            Node startNode = new Node(), endNode = new Node();
            foreach (SubstationEntity item in networkModel.substationEntities)
            {
                linesGrid[item.Row,item.Column].entityId = item.Id;

                if (item.Id == idStart)
                    startNode = linesGrid[item.Row, item.Column];
                else if (item.Id == idEnd)
                    endNode = linesGrid[item.Row, item.Column];
                
            }

            foreach (NodeEntity item in networkModel.nodeEntities)
            {
                linesGrid[item.Row, item.Column].entityId = item.Id;

                if (item.Id == idStart)
                    startNode = linesGrid[item.Row, item.Column];
                else if (item.Id == idEnd)
                    endNode = linesGrid[item.Row, item.Column];
            }

            foreach (SwitchEntity item in networkModel.switchEntities)
            {
                linesGrid[item.Row, item.Column].entityId = item.Id;

                if (item.Id == idStart)
                    startNode = linesGrid[item.Row, item.Column];
                else if (item.Id == idEnd)
                    endNode = linesGrid[item.Row, item.Column];
            }

            Queue<Node> Q = new Queue<Node>();
            Q.Enqueue(startNode);

            while (!Q.Count.Equals(0))
            {
                Node u = Q.Dequeue();

                if (u.entityId == endNode.entityId)
                {
                    Node temp = u;
                    Node last = u;
                    List<Tuple<int, int>> path = new List<Tuple<int, int>>();
                    while (!temp.entityId.Equals(startNode.entityId))
                    {
                        int row1 = temp.cellId / 1000;
                        int column1 = temp.cellId % 1000;
                        path.Add(new Tuple<int, int>(row1, column1));
                        maskGrid[row1, column1] = temp.cell;

                        int row2 = temp.pred / 1000;
                        int column2 = temp.pred % 1000;
                        

                        if (temp.cell.Equals(2))
                        {
                            if(row1 > row2)
                            {
                                for (int i = row2+1; i < row1; i++)
                                {
                                    maskGrid[i, column1] = 2;
                                }
                            }
                            else
                            {
                                for (int i = row1 + 1; i < row2; i++)
                                {
                                    maskGrid[i, column1] = 2;
                                }
                            }
                        }
                        else if(temp.cell.Equals(1))
                        {
                            if (column1 > column2)
                            {
                                for (int i = column2 + 1; i < column1; i++)
                                {
                                    maskGrid[row1, i] = 1;
                                }
                            }
                            else
                            {
                                for (int i = column1 + 1; i < column2; i++)
                                {
                                    maskGrid[row1, i] = 1;
                                }
                            }
                        }

                        last = temp;
                        temp = linesGrid[row2, column2];
                    }
                    int row = temp.cellId / 1000;
                    int column = temp.cellId % 1000;
                    path.Add(new Tuple<int, int>(row, column));
                    if ((temp.cellId + 1).Equals(last.cellId) || (temp.cellId - 1).Equals(last.cellId))
                        maskGrid[row, column] = 1;
                    else
                        maskGrid[row, column] = 2;

                    Lines.Add(path);
                    return;

                }
                else
                {
                    int row = u.cellId / 1000;
                    int column = u.cellId % 1000;
                    
                    int exit = 0;
                    int step = 1;
                    bool[] flagArray = new bool[] { false, false, false, false };

                    int endRow = endNode.cellId / 1000;
                    int endColumn = endNode.cellId % 1000;

                    while (!exit.Equals(4))
                    {
                        if (step == 1)
                            countStep++;
                        else
                            countStep = 0;

                        
                        if (step == 1000 || countStep==20)
                            return;
                        try
                        {
                            if(row-step < endRow && !flagArray[0])
                            {
                                exit++;
                                flagArray[0] = true;
                            }
                            else if ((!linesGrid[row - step, column].cell.Equals(0) && !flagArray[0]) || ((row - step) == endRow && !flagArray[0]))
                            {
                                
                                flagArray[0] = true;
                                linesGrid[row - step, column].cell = 2; //vertikalna linija
                                linesGrid[row - step + 1, column].cell = 2;
                                linesGrid[row - step, column].pred = u.cellId;
                                exit++;
                                if(linesGrid[row - step, column].entityId.Equals(endNode.entityId))
                                {
                                    Q.Clear();
                                    Q.Enqueue(linesGrid[row - step, column]);
                                    break;

                                }
                                Q.Enqueue(linesGrid[row - step, column]);
                            }
                        }
                        catch { }
                        try
                        {
                            if (row + step > endRow && !flagArray[1])
                            {
                                exit++;
                                flagArray[1] = true;
                            }
                            else
                               if ((!linesGrid[row + step, column].cell.Equals(0) && !flagArray[1]) || ((row + step) == endRow && !flagArray[1]))
                            {
                               
                                flagArray[1] = true;
                                linesGrid[row + step, column].cell = 2; //vertikalna linija
                                linesGrid[row + step - 1, column].cell = 2; //vertikalna linija
                                linesGrid[row + step, column].pred = u.cellId;
                                exit++;
                                if (linesGrid[row + step, column].entityId.Equals(endNode.entityId))
                                {
                                    Q.Clear();
                                    Q.Enqueue(linesGrid[row + step, column]);
                                    break;

                                }
                                Q.Enqueue(linesGrid[row + step, column]);
                            }
                        }
                        catch { }
                        try
                        {
                            if (column - step < endColumn && !flagArray[2])
                            {
                                exit++;
                                flagArray[2] = true;
                            }
                            else
                               if ((!linesGrid[row, column - step].cell.Equals(0) && !flagArray[2]) || ((column - step) == endColumn && !flagArray[2]))
                            {
                                
                                flagArray[2] = true;
                                linesGrid[row, column - step].cell = 1; //horizontalna linija
                                linesGrid[row, column - step + 1].cell = 1; //horizontalna linija
                                linesGrid[row, column - step].pred = u.cellId;
                                exit++;
                                if (linesGrid[row, column - step].entityId.Equals(endNode.entityId))
                                {
                                    Q.Clear();
                                    Q.Enqueue(linesGrid[row, column - step]);
                                    break;

                                }
                                Q.Enqueue(linesGrid[row, column - step]);
                            }
                        }
                        catch { }
                        try
                        {
                            if (column + step > endColumn && !flagArray[3])
                            {
                                exit++;
                                flagArray[3] = true;
                            }
                            else
                               if ((!linesGrid[row, column + step].cell.Equals(0) && !flagArray[3]) || ((column + step) == endColumn && !flagArray[3]))
                            {
                                
                                flagArray[3] = true;
                                linesGrid[row, column + step].cell = 1; //horizontalna linija
                                linesGrid[row, column + step - 1].cell = 1; //horizontalna linija
                                linesGrid[row, column + step].pred = u.cellId;
                                exit++;
                                if (linesGrid[row, column + step].entityId.Equals(endNode.entityId))
                                {
                                    Q.Clear();
                                    Q.Enqueue(linesGrid[row, column + step]);
                                    break;

                                }
                                Q.Enqueue(linesGrid[row, column + step]);
                            }
                        }
                        catch { }
                        step++;
                    }
                }
            }
        }
        public struct Node
        {
            public UInt64 entityId;
            public int cellId;
            public short cell;
            public int pred;
        }

        #region Scroll
        private void scrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(scrollViewer);
            hOff = scrollViewer.HorizontalOffset;
            vEff = scrollViewer.VerticalOffset;
            scrollViewer.CaptureMouse();
        }

        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (scrollViewer.IsMouseCaptured)
            {
                scrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(scrollViewer).X));
                scrollViewer.ScrollToVerticalOffset(vEff + (scrollMousePoint.Y - e.GetPosition(scrollViewer).Y));
            }
        }

        private void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.ReleaseMouseCapture();
        }
        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point mouseAtImage = e.GetPosition(myCanvas);
            Point mouseAtScrollViewer = e.GetPosition(scrollViewer);

            ScaleTransform st = myCanvas.LayoutTransform as ScaleTransform;
            if (st == null)
            {
                st = new ScaleTransform();
                myCanvas.LayoutTransform = st;
            }

            if (e.Delta > 0)
            {
                st.ScaleX = st.ScaleY = st.ScaleX * 1.25;
                if (st.ScaleX > 64) st.ScaleX = st.ScaleY = 64;
            }
            else
            {
                st.ScaleX = st.ScaleY = st.ScaleX / 1.25;
                if (st.ScaleX < 1) st.ScaleX = st.ScaleY = 1;
            }
            #region [this step is critical for offset]
            scrollViewer.ScrollToHorizontalOffset(0);
            scrollViewer.ScrollToVerticalOffset(0);
            this.UpdateLayout();
            #endregion

            Vector offset = myCanvas.TranslatePoint(mouseAtImage, scrollViewer) - mouseAtScrollViewer; // (Vector)middleOfScrollViewer;
            scrollViewer.ScrollToHorizontalOffset(offset.X);
            scrollViewer.ScrollToVerticalOffset(offset.Y);
            this.UpdateLayout();

            e.Handled = true;
        }


        #endregion

        private void scrollViewer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            var relativePosition = e.GetPosition(myCanvas);
            var point = PointToScreen(relativePosition);
            MessageBox.Show( point.X+ "-"+point.Y);
        }
    }
}
