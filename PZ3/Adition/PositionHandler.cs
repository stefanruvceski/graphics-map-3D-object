using PZ3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Adition
{
    public static class PositionHandler
    {
        public static NetworkModel DoParse(NetworkModel networkModel)
        {
            List<UInt64> ids = new List<UInt64>();
            for (int i = 0; i < networkModel.substationEntities.Count; i++)
            {
                ids.Add(networkModel.substationEntities[i].Id);
                double latitude, longitude;
                ToLatLon(networkModel.substationEntities[i].X, networkModel.substationEntities[i].Y, 34, out latitude, out longitude);
                networkModel.substationEntities[i].X = latitude;
                networkModel.substationEntities[i].Y = longitude;

                int row, column;
                FromCoordsToIndex(networkModel.substationEntities[i].X, networkModel.substationEntities[i].Y, out row, out column);
                networkModel.substationEntities[i].Row = row;
                networkModel.substationEntities[i].Column = column;


            }

            for (int i = 0; i < networkModel.nodeEntities.Count; i++)
            {
                ids.Add(networkModel.nodeEntities[i].Id);
                double latitude, longitude;
                ToLatLon(networkModel.nodeEntities[i].X, networkModel.nodeEntities[i].Y, 34, out latitude, out longitude);
                networkModel.nodeEntities[i].X = latitude;
                networkModel.nodeEntities[i].Y = longitude;

                int row, column;
                FromCoordsToIndex(networkModel.nodeEntities[i].X, networkModel.nodeEntities[i].Y, out row, out column);
                networkModel.nodeEntities[i].Row = row;
                networkModel.nodeEntities[i].Column = column;
            }

            for (int i = 0; i < networkModel.switchEntities.Count; i++)
            {
                ids.Add(networkModel.switchEntities[i].Id);
                double latitude, longitude;
                ToLatLon(networkModel.switchEntities[i].X, networkModel.switchEntities[i].Y, 34, out latitude, out longitude);
                networkModel.switchEntities[i].X = latitude;
                networkModel.switchEntities[i].Y = longitude;

                int row, column;
                FromCoordsToIndex(networkModel.switchEntities[i].X, networkModel.switchEntities[i].Y, out row, out column);
                networkModel.switchEntities[i].Row = row;
                networkModel.switchEntities[i].Column = column;
            }

            List<LineEntity> newList = new List<LineEntity>();

            foreach (LineEntity item in networkModel.vertices)
            {
                bool exist = false;
                if (ids.Contains(item.SecondEnd) && ids.Contains(item.FirstEnd))
                {
                    for (int i = 0; i < newList.Count; i++)
                    {
                        if((newList[i].FirstEnd == item.FirstEnd && newList[i].SecondEnd == item.SecondEnd) || (newList[i].FirstEnd == item.SecondEnd && newList[i].SecondEnd == item.FirstEnd))
                        {
                            exist = true;
                            break;
                        }
                    }
                    if(!exist)
                        newList.Add(item);
                }
            }

            networkModel.vertices = new List<LineEntity>(newList);
            return networkModel;
        }


        //From UTM to Latitude and longitude in decimal
        private static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }

        private static void FromCoordsToIndex(double latitude, double longitude, out int indexI, out int indexJ)
        {
            indexI = 1000 - (int)((latitude - 45) * 1000);
            indexJ = (int)((longitude - 19) * 1000);

        }
    }
}
