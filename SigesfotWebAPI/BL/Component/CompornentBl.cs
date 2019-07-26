using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Component;
using BL.Service;
using DAL.Component;

namespace BL.Component
{
    public class ComponentBl
    {
        public List<AdditionalExams> ListOfAdditionalExams()
        {
            return new ComponentDal().ListOfAdditionalExams();
        }

        public List<KeyValueDTO> GetAllComponent(int nodeId, int rolenodeId)
        {
            var components = new ComponentDal().GetAllComponents();
            var temp = components.FindAll(p => p.Value4 != -1);
            List<KeyValueDTO> groupComponentList = temp.GroupBy(x => x.Value4).Select(group => group.First()).ToList();
            groupComponentList.AddRange(components.ToList().FindAll(p => p.Value4 == -1));
            var componentProfile = new ServiceBl().GetRoleNodeComponentProfileByRoleNodeId(nodeId, rolenodeId);
            var results = groupComponentList.FindAll(f => componentProfile.Any(t => t.v_ComponentId == f.Value2));
            return results;
        }

        public BoardExamsCustom GetExamsForConsult(BoardExamsCustom data)
        {
            return new ComponentDal().GetExamsForConsult(data);
        }

        public List<ExamsCustom> GetMedicalExam(string componentName)
        {
            return new ComponentDal().GetMedicalExam(componentName);
        }
    }
}
