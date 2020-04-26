using PZ3.Model;
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
    public static class GridHandler
    {
        public static UInt64[,] InitGrid(NetworkModel networkModel,UInt64[,] grid, DrawingGroup drawingGroup, Canvas myCanvas)
        {
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    grid[i, j] = 0;
                }
            }

            return LoadNetworkModelToGrid(networkModel,grid,drawingGroup,myCanvas);
        }

        private static  void FindPlaceInGrid(UInt64[,] grid,int indexI, int indexJ, out int row, out int column)
        {
            bool spaceFound = false;
            int step = 1;

            while (!spaceFound)
            {
                if (grid[indexI, (indexJ - step) % 1000] == 0)
                {
                    indexJ = (indexJ - step) % 1000;
                    spaceFound = true;
                }
                else if (grid[(indexI - step) % 1000, (indexJ - step) % 1000] == 0)
                {
                    indexI = (indexI - step) % 1000;
                    indexJ = (indexJ - step) % 1000;
                    spaceFound = true;
                }
                else if (grid[(indexI - step) % 1000, indexJ] == 0)
                {
                    indexI = (indexI - step) % 1000;
                    spaceFound = true;
                }
                else if (grid[indexI, (indexJ + step) % 1000] == 0)
                {
                    indexJ = (indexJ + step) % 1000;
                    spaceFound = true;
                }
                else if (grid[(indexI + step) % 1000, (indexJ + step) % 1000] == 0)
                {
                    indexI = (indexI + step) % 1000;
                    indexJ = (indexJ + step) % 1000;
                    spaceFound = true;
                }
                else if (grid[(indexI + step) % 1000, indexJ] == 0)
                {
                    indexI = (indexI + step) % 1000; ;
                    spaceFound = true;
                }

                if (++step == 16)
                {
                    break;
                }
            }
            row = indexI;
            column = indexJ;
        }

        public static void DrawEntities(NetworkModel networkModel, DrawingGroup drawingGroup, Canvas myCanvas)
        {
            for (int i = 0; i < networkModel.substationEntities.Count; i++)
            {
                ImageDrawing image = DrawingHandler.DrawSubstationImage(networkModel.substationEntities[i].Row, networkModel.substationEntities[i].Column, myCanvas);
                drawingGroup.Children.Add(image);
            }

            for (int i = 0; i < networkModel.nodeEntities.Count; i++)
            {
                ImageDrawing image = DrawingHandler.DrawNodeImage(networkModel.nodeEntities[i].Row, networkModel.nodeEntities[i].Column, myCanvas);
           
                drawingGroup.Children.Add(image);
            }

            for (int i = 0; i < networkModel.switchEntities.Count; i++)
            {
                ImageDrawing image = DrawingHandler.DrawSwitchImage(networkModel.switchEntities[i].Row, networkModel.switchEntities[i].Column, myCanvas);
                drawingGroup.Children.Add(image);
            }
        }

        private static UInt64[,] LoadNetworkModelToGrid(NetworkModel networkModel,UInt64[,] grid, DrawingGroup drawingGroup,Canvas myCanvas)
        {
            for (int i = 0; i < networkModel.substationEntities.Count; i++)
            {
                int indexI = networkModel.substationEntities[i].Row, indexJ = networkModel.substationEntities[i].Column;

                if (grid[indexI, indexJ] == 0)
                {
                    grid[indexI, indexJ] = networkModel.substationEntities[i].Id;
                }
                else
                {
                    GridHandler.FindPlaceInGrid(grid, indexI, indexJ, out indexI, out indexJ);
                    grid[indexI, indexJ] = networkModel.substationEntities[i].Id;

                    networkModel.substationEntities[i].Row = indexI;
                    networkModel.substationEntities[i].Column = indexJ;
                }

                //ImageDrawing image = DrawingHandler.DrawSubstationImage(indexI, indexJ, myCanvas);
                //drawingGroup.Children.Add(image);
                

            }

            for (int i = 0; i < networkModel.nodeEntities.Count; i++)
            {
                int indexI = networkModel.nodeEntities[i].Row, indexJ = networkModel.nodeEntities[i].Column;

                if (grid[indexI, indexJ] == 0)
                {
                    grid[indexI, indexJ] = networkModel.nodeEntities[i].Id;
                }
                else
                {
                    GridHandler.FindPlaceInGrid(grid, indexI, indexJ, out indexI, out indexJ);
                    grid[indexI, indexJ] = networkModel.nodeEntities[i].Id;

                    networkModel.nodeEntities[i].Row = indexI;
                    networkModel.nodeEntities[i].Column = indexJ;
                }

                //ImageDrawing image = DrawingHandler.DrawNodeImage(indexI, indexJ, myCanvas);
                //drawingGroup.Children.Add(image);
            }
            for (int i = 0; i < networkModel.switchEntities.Count; i++)
            {
                int indexI = networkModel.switchEntities[i].Row, indexJ = networkModel.switchEntities[i].Column;

                if (grid[indexI, indexJ] == 0)
                {
                    grid[indexI, indexJ] = networkModel.switchEntities[i].Id;
                }
                else
                {
                    GridHandler.FindPlaceInGrid(grid, indexI, indexJ, out indexI, out indexJ);
                    grid[indexI, indexJ] = networkModel.switchEntities[i].Id;

                    networkModel.switchEntities[i].Row = indexI;
                    networkModel.switchEntities[i].Column = indexJ;
                }

                //ImageDrawing image = DrawingHandler.DrawSwitchImage(indexI, indexJ, myCanvas);
                //drawingGroup.Children.Add(image);
            }
            return grid;

        }


    }
}
