using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Node
{
    public class NodeDal
    {
        public List<KeyValueDTO> GetAllNodeForCombo(bool remoto, int nodeId)
        {
            using (var oCtx = new DatabaseContext())
            {
                try
                {
                    if (remoto)
                    {
                        var query = (from a in oCtx.Node
                                     where a.i_IsDeleted == (int)Enumeratores.SiNo.No && a.i_NodeId != nodeId
                                     select a);

                        var queryList = from quer in query.ToList()
                                        select new KeyValueDTO
                                        {
                                            Id = quer.i_NodeId.ToString(),
                                            Value = quer.v_Description
                                        };
                        List<KeyValueDTO> NodeList = queryList.OrderBy(p => p.Value).ToList();
                        return NodeList;
                    }
                    else
                    {
                        var query = (from a in oCtx.Node
                                     where a.i_IsDeleted == (int)Enumeratores.SiNo.No && a.i_NodeId == nodeId
                                     select a);

                        var queryList = from quer in query.ToList()
                                        select new KeyValueDTO
                                        {
                                            Id = quer.i_NodeId.ToString(),
                                            Value = quer.v_Description
                                        };
                        List<KeyValueDTO> NodeList = queryList.OrderBy(p => p.Value).ToList();
                        return NodeList;
                    }
                             
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
