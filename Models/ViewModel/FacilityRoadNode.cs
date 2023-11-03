using System.Collections.Generic;
using RoadAppWEB.Models; // 引入您的节点模型和 Facility 模型

namespace RoadAppWEB.Models.ViewModel
{
    public class FacilityRoadNode
    {
        public IEnumerable<node> Nodes { get; set; }
        public IEnumerable<road> Roads { get; set; }
        public IEnumerable<facility> Facilities { get; set; }
    }
}
