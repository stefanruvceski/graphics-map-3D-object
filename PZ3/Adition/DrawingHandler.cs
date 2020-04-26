using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ3.Adition
{
    public static class DrawingHandler
    {
        public static ImageDrawing DrawSubstationImage(int indexI, int indexJ, Canvas myCanvas)
        {
            ImageDrawing image = new ImageDrawing();
            image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\substation.png", UriKind.Relative));
            image.Rect = new Rect(((indexJ / 1000.0) * myCanvas.Width) * 4.4, ((indexI / 1000.0) * myCanvas.Height) * 7, 3, 3);

            return image;
            
        }

        public static ImageDrawing DrawSwitchImage(int indexI, int indexJ, Canvas myCanvas)
        {
            ImageDrawing image = new ImageDrawing();
            image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\switch1.png", UriKind.Relative));
            image.Rect = new Rect(((indexJ / 1000.0) * myCanvas.Width) * 4.4, ((indexI / 1000.0) * myCanvas.Height) * 7, 3, 3);
            
            return image;
        }

        public static ImageDrawing DrawNodeImage(int indexI, int indexJ, Canvas myCanvas)
        {
            ImageDrawing image = new ImageDrawing();
            image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\node1.png", UriKind.Relative));
            image.Rect = new Rect(((indexJ / 1000.0) * myCanvas.Width) * 4.4, ((indexI / 1000.0) * myCanvas.Height) * 7, 3, 3);
            
            return image;
        }

        public static ImageDrawing DrawHorizontalLineImage(int indexI, int indexJ, Canvas myCanvas)
        {
            ImageDrawing image = new ImageDrawing();
            image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\horizontalLine.png", UriKind.Relative));
            image.Rect = new Rect(((indexJ / 1000.0) * myCanvas.Width) * 4.4+1, ((indexI / 1000.0) * myCanvas.Height) * 7+1, 2, 2);

            return image;
        }

        public static ImageDrawing DrawVerticalLineImage(int indexI, int indexJ, Canvas myCanvas)
        {
            ImageDrawing image = new ImageDrawing();
            image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\verticalLine.png", UriKind.Relative));
            image.Rect = new Rect(((indexJ / 1000.0) * myCanvas.Width) * 4.4+1, ((indexI / 1000.0) * myCanvas.Height) * 7+1,2 , 2);

            return image;
        }

        public  static ImageDrawing DrawLineImage(Tuple<int,int> point1,Tuple<int,int>point2,Canvas myCanvas)
        {
            ImageDrawing image = new ImageDrawing();

            if (point1.Item1.Equals(point2.Item1)) //horizontalna linija
            {
                if(point1.Item2 <= point2.Item2) //Linija ka desno
                {
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\horizontalLine.png", UriKind.Relative));
                    image.Rect = new Rect(((point1.Item1 / 1000.0) * myCanvas.Width) * 4.4 + 1, ((point1.Item2 / 1000.0) * myCanvas.Height) * 7 + 1, (((point2.Item2 - point1.Item2) / 1000.0) * myCanvas.Width) * 7,5 );

                }
                else                                //Linija ka levo
                {
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\horizontalLine.png", UriKind.Relative));
                    image.Rect = new Rect(((point2.Item1 / 1000.0) * myCanvas.Width) * 4.4 + 1, ((point2.Item2 / 1000.0) * myCanvas.Height) * 7 + 1, (((point1.Item2 - point2.Item2) / 1000.0) * myCanvas.Width) * 7, 5);

                }
            }
            else if (point1.Item2.Equals(point2.Item2))//vertikalna linija
            {
                if (point1.Item1 <= point2.Item1) //Linija ka dole
                {
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\verticalLine.png", UriKind.Relative));
                    image.Rect = new Rect(((point1.Item1 / 1000.0) * myCanvas.Width) * 4.4 + 1, ((point1.Item2 / 1000.0) * myCanvas.Height) * 7 + 1,5, (((point2.Item1 - point1.Item1) / 1000.0) * myCanvas.Height) * 4.4);
                    
                }
                else                                //Linija ka gore
                {
                    image.ImageSource = new BitmapImage(new Uri(@"..\..\Images\verticalLine.png", UriKind.Relative));
                    image.Rect = new Rect(((point2.Item1 / 1000.0) * myCanvas.Width) * 4.4 + 1, ((point2.Item2 / 1000.0) * myCanvas.Height) * 7 + 1,5, (((point1.Item1 - point2.Item1) / 1000.0) * myCanvas.Height) * 4.4);

                }
            }

            return image;
        }
    }
}
