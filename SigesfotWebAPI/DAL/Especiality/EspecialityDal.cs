using BE;
using BE.Especiality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Especiality
{
    public class EspecialityDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public static BoardEspeciality GetAllEspeciality(BoardEspeciality data)
        {
            try
            {
                int skip = (data.Index - 1) * data.Take;

                string filterEspeciality = string.IsNullOrWhiteSpace(data.Especiality) ? "" : data.Especiality;

                var list = (from esp in ctx.Especiality
                            where esp.i_IsDeleted == 0 && (esp.v_EspecialityName.Contains(filterEspeciality) || filterEspeciality == "")
                            select new EspecialityCustom
                            {
                                v_EspecialityId = esp.v_EspecialityId,
                                v_EspecialityName = esp.v_EspecialityName,
                                b_EspecialityPicture = esp.b_EspecialityPicture,
                                t_TimeForAttention = esp.t_TimeForAttention,
                                t_StartTime = esp.t_StartTime,
                                t_EndTime = esp.t_EndTime,
                                t_StartTime2 = esp.t_StartTime2,
                                t_EndTime2 = esp.t_EndTime2,
                                r_Cost = esp.r_Cost,
                                v_Description = esp.v_Description,
                                v_ProtocolId = esp.v_ProtocolId,
                            }).ToList();

               
                int totalRecords = list.Count;

                if (data.Take > 0)
                    list = list.Skip(skip).Take(data.Take).ToList();

                data.TotalRecords = totalRecords;

                foreach (var item in list)
                {
                    item.v_EspecialityPicture = item.b_EspecialityPicture == null ? null : Convert.ToBase64String(item.b_EspecialityPicture);
                }

                data.List = list;

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static EspecialityCustom GetEspecialityById(string especialityId)
        {
            try
            {
                var obj = ctx.Especiality.Where(x => x.v_EspecialityId == especialityId && x.i_IsDeleted == 0).FirstOrDefault();
                EspecialityCustom final = new EspecialityCustom();
                final.r_Cost = obj.r_Cost;
                final.v_EspecialityId = obj.v_EspecialityId;
                final.t_TimeForAttention = obj.t_TimeForAttention;
                final.t_EndTime = obj.t_EndTime;
                final.t_EndTime2 = obj.t_EndTime2;
                final.t_StartTime = obj.t_StartTime;
                final.t_StartTime2 = obj.t_StartTime2;
                final.v_Description = obj.v_Description;
                final.v_EspecialityName = obj.v_EspecialityName;
                final.v_EspecialityPicture = obj.b_EspecialityPicture == null ? null : Convert.ToBase64String(obj.b_EspecialityPicture);
                final.v_ProtocolId = obj.v_ProtocolId;


                return final;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool AddEspeciality(EspecialityBE data)
        {
            try
            {
                if (data.v_EspecialityId == null)
                {
                    data.i_IsDeleted = 0;
                    data.v_EspecialityId = new Common.Utils().GetPrimaryKey(1, 49, "ES");
                    ctx.Especiality.Add(data);
                }
                else
                {
                    var obj = ctx.Especiality.Where(x => x.v_EspecialityId == data.v_EspecialityId).FirstOrDefault();

                    obj.v_Description = data.v_Description;
                    obj.v_ProtocolId = data.v_ProtocolId;
                    obj.v_EspecialityName = data.v_EspecialityName;
                    obj.r_Cost = data.r_Cost;
                    obj.t_TimeForAttention = data.t_TimeForAttention;
                    obj.b_EspecialityPicture = data.b_EspecialityPicture;
                    obj.t_StartTime = data.t_StartTime;
                    obj.t_StartTime2 = data.t_StartTime2;
                    obj.t_EndTime = data.t_EndTime;
                    obj.t_EndTime2 = data.t_EndTime2;
                }
                

                ctx.SaveChanges();



                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeletedEspeciality(string especialityId)
        {
            try
            {
                var obj = ctx.Especiality.Where(x => x.v_EspecialityId == especialityId).FirstOrDefault();
                obj.i_IsDeleted = 1;
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
    }
}
