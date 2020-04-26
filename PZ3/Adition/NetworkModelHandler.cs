using PZ3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3.Adition
{
    public static class NetworkModelHandler
    {
        public static NetworkModel InitModel(string path)
        {
            NetworkModel networkModel = XMLHandler.Load<NetworkModel>(path);
            return PositionHandler.DoParse(networkModel);
        }
    }
}
