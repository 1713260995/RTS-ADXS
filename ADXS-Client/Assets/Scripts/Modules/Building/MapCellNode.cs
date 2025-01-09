using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.Building
{
    public struct MapCellNode
    {
        public float height;        // 格子的中心高度
        public float steepness;     // 格子的梯度
                                    // public Building current;    // 格子中存储的建筑
    }
}
