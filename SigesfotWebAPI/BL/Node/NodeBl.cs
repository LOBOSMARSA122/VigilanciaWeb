using BE.Common;
using DAL.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Node
{
    public class NodeBl
    {
        NodeDal _NodeDal = new NodeDal();

        public List<KeyValueDTO> GetAllNodeForCombo(bool remoto, int nodeId)
        {
            var list = _NodeDal.GetAllNodeForCombo(remoto, nodeId);
            return list;
        }
    }
}
