using BE.ConfDx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.ConfigDx;

namespace BL.ConfigDx
{
    public class ConfigDxBl
    {
        private readonly ConfigDxDal _oConfigDxDal = new ConfigDxDal();

        public void SaveConfigDX(List<ConfigDxCustom> pobjConfigDx, int nodeId, int systemUserId)
        {
            _oConfigDxDal.SaveConfigDX(pobjConfigDx, nodeId, systemUserId);
        }

        public BoardConfigDx GetAllConfigDx(int index, int take) {
            return _oConfigDxDal.GetAllConfigDxCustom(index, take);
        }

        public List<ConfigDxCustom> GetConfigDxByDiseasesId(string diseasesId, string warehouseId)
        {
            return _oConfigDxDal.GetConfigDxCustomByDiseasesId(diseasesId, warehouseId);
        }
    }
}
