using BE.ConfDx;
using System;
using System.Collections.Generic;
using System.Linq;
using static BE.Common.Enumeratores;
using System.Text;
using System.Threading.Tasks;
using BE.Common;

namespace DAL.ConfigDx
{
    public class ConfigDxDal
    {
        private static readonly DatabaseContext Ctx = new DatabaseContext();

        public void SaveConfigDX(List<ConfigDxCustom> pobjConfigDx, int nodeId, int systemUserId)
        {
            //mon.IsActive = true;
            string NewId0 = "(No generado)";
            string componentId = null;

            if (pobjConfigDx != null)
            {
                foreach (var dr in pobjConfigDx)
                {

                    #region DiagnosticRepository -> ADD / UPDATE / DELETE

                    // ADD
                    if (dr.RecordType == (int)RecordType.Temporal && (dr.RecordStatus == (int)RecordStatus.Agregado || dr.RecordStatus == (int)RecordStatus.Editado))
                    {
                        ConfigDxDto objEntity = new ConfigDxDto();
                     
                        objEntity.v_DiseaseId = dr.v_DiseaseId;
                        objEntity.v_ProductId = dr.v_ProductId;
                        
                        objEntity.d_InsertDate = DateTime.Now;
                        objEntity.i_InsertUserId = systemUserId;
                        objEntity.i_IsDeleted = 0;

                        // Autogeneramos el Pk de la tabla                      
                        NewId0 = new Common.Utils().GetPrimaryKey(nodeId, 370, "WD");
                        objEntity.v_ConfigDxId = NewId0;

                        Ctx.ConfigDx.Add(objEntity);

                    } // UPDATE
                    else if (dr.RecordType == (int)RecordType.NoTemporal && dr.RecordStatus == (int)RecordStatus.Editado)
                    {
                        // Obtener la entidad fuente
                        var objEntitySource = (from a in Ctx.ConfigDx
                                               where a.v_ConfigDxId == dr.v_ConfigDxId
                                               select a).FirstOrDefault();

                        
                        objEntitySource.d_UpdateDate = DateTime.Now;
                        objEntitySource.i_UpdateUserId = systemUserId;

                    } // DELETE
                    else if (dr.RecordType == (int)RecordType.NoTemporal && dr.RecordStatus == (int)RecordStatus.Eliminado)
                    {
                        // Obtener la entidad fuente
                        var objEntitySource = (from a in Ctx.ConfigDx
                            where a.v_ConfigDxId == dr.v_ConfigDxId
                            select a).FirstOrDefault();

                        // Crear la entidad con los datos actualizados                                                           
                        objEntitySource.d_UpdateDate = DateTime.Now;
                        objEntitySource.i_UpdateUserId = systemUserId;
                        objEntitySource.i_IsDeleted = 1;
                    }

                    #endregion
                }
                Ctx.SaveChanges();
            }
        }

        public BoardConfigDx GetAllConfigDxCustom(int index, int take) {
            int skip = (index - 1) * take;
            BoardConfigDx board = new BoardConfigDx();
            var query = (from cdx in Ctx.ConfigDx
                         join pro in Ctx.Product on cdx.v_ProductId equals pro.v_ProductId
                         join dis in Ctx.Diseases on cdx.v_DiseaseId equals dis.v_DiseasesId
                         where cdx.i_IsDeleted == (int)Enumeratores.SiNo.No
                         select new ConfigDxCustom() {
                             v_ConfigDxId = cdx.v_ConfigDxId,
                             v_DiseaseId = cdx.v_DiseaseId,
                             v_ProductId = cdx.v_ProductId,
                             v_DiseaseName = dis.v_Name,
                             v_ProductName = pro.v_Name,
                             RecordStatus = (int)Enumeratores.RecordStatus.Grabado,
                             RecordType = (int)Enumeratores.RecordType.NoTemporal,
                         }).ToList();
            board.TotalRecords = query.Count;
            if (take > 0)
            {
               query = query.Skip(skip).Take(take).ToList();
            }
            board.Take = take;
            board.Index = index;
            board.List = query;
            return board;
        }

        public List<ConfigDxCustom> GetConfigDxCustomByDiseasesId(string diseasesId, string warehouseId)
        {


            var query = (from cdx in Ctx.ConfigDx
                         join pro in Ctx.Product on cdx.v_ProductId equals pro.v_ProductId
                         join dis in Ctx.Diseases on cdx.v_DiseaseId equals dis.v_DiseasesId
                         join prw in Ctx.ProductWarehouse on new { a = warehouseId , b = cdx.v_ProductId }
                                                              equals new { a = prw.v_WarehouseId, b = prw.v_ProductId } into prw_join
                         from prw in prw_join.DefaultIfEmpty()
                         where cdx.i_IsDeleted == (int)Enumeratores.SiNo.No && cdx.v_DiseaseId == diseasesId
                         select new ConfigDxCustom()
                         {
                             v_ConfigDxId = cdx.v_ConfigDxId,
                             v_DiseaseName = dis.v_Name,
                             v_ProductId = pro.v_ProductId,
                             v_ProductName = pro.v_Name,
                             r_StockActual = prw.r_StockActual,
                         }).ToList();

            return query;
        }

    }
}
