using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PZ3.Model
{
    [Serializable]
    public class NetworkModel
    {
        [XmlArray("Substations")]
        [XmlArrayItem("SubstationEntity", typeof(SubstationEntity))]
        public List<SubstationEntity> substationEntities { get; set; }

        [XmlArray("Nodes")]
        [XmlArrayItem("NodeEntity", typeof(NodeEntity))]
        public List<NodeEntity> nodeEntities { get; set; }

        [XmlArray("Switches")]
        [XmlArrayItem("SwitchEntity", typeof(SwitchEntity))]
        public List<SwitchEntity> switchEntities { get; set; }

        [XmlArray("Lines")]
        [XmlArrayItem("LineEntity", typeof(LineEntity))]
        public List<LineEntity> vertices { get; set; }

    }
}
