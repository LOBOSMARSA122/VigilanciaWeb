using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Organization;
using BE.Sigesoft;

namespace DAL.Sigesoft
{
   public class SigesoftDal
    {
        public List<ServiceComponentList> GetServiceComponentsReport(string pstrServiceId)
        {
            //mon.IsActive = true;        
            int isDeleted = 0;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                #region serviceComponentFields

                var serviceComponentFields = (from A in dbContext.ServiceComponent
                                              join B in dbContext.ServiceComponentFields on A.v_ServiceComponentId equals B.v_ServiceComponentId
                                              join C in dbContext.ServiceComponentFieldValues on B.v_ServiceComponentFieldsId equals C.v_ServiceComponentFieldsId
                                              join cfs in dbContext.ComponentFields on B.v_ComponentFieldId equals cfs.v_ComponentFieldId
                                              join D in dbContext.ComponentField on B.v_ComponentFieldId equals D.v_ComponentFieldId
                                              join cm in dbContext.Component on cfs.v_ComponentId equals cm.v_ComponentId

                                              join dh in dbContext.DataHierarchy on new { a = 105, b = D.i_MeasurementUnitId.Value }
                                                                 equals new { a = dh.i_GroupId, b = dh.i_ItemId } into dh_join
                                              from dh in dh_join.DefaultIfEmpty()

                                              where (A.v_ServiceId == pstrServiceId) &&
                                                    //(cm.v_ComponentId == pstrComponentId) &&
                                                    (A.i_IsDeleted == isDeleted) &&
                                                    (B.i_IsDeleted == isDeleted) &&
                                                    (C.i_IsDeleted == isDeleted)

                                              select new ServiceComponentFieldsList
                                              {
                                                  v_ServiceComponentFieldsId = B.v_ServiceComponentFieldsId,
                                                  v_ComponentFieldsId = B.v_ComponentFieldId,
                                                  v_ComponentFielName = D.v_TextLabel,
                                                  v_Value1 = C.v_Value1 == "" ? null : C.v_Value1,
                                                  i_GroupId = D.i_GroupId.Value,
                                                  v_MeasurementUnitName = dh.v_Value1,
                                                  v_ComponentId = cm.v_ComponentId,
                                                  v_ServiceComponentId = A.v_ServiceComponentId
                                              }).ToList();

                int rpta = 0;

                var _finalQuery = (from a in serviceComponentFields
                                   let value1 = int.TryParse(a.v_Value1, out rpta)
                                   join sp in dbContext.SystemParameter on new { a = a.i_GroupId, b = rpta }
                                                   equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                   from sp in sp_join.DefaultIfEmpty()

                                   select new ServiceComponentFieldsList
                                   {
                                       v_ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                       v_ComponentFieldsId = a.v_ComponentFieldsId,
                                       v_ComponentFielName = a.v_ComponentFielName,
                                       i_GroupId = a.i_GroupId,
                                       v_Value1 = a.v_Value1,
                                       v_Value1Name = sp == null ? "" : sp.v_Value1,
                                       v_MeasurementUnitName = a.v_MeasurementUnitName,
                                       v_ComponentId = a.v_ComponentId,
                                       v_ConclusionAndDiagnostic = a.v_Value1 + " / " + GetServiceComponentDiagnosticsReport(pstrServiceId, a.v_ComponentId),
                                       v_ServiceComponentId = a.v_ServiceComponentId
                                   }).ToList();


                #endregion

                var components = (from aaa in dbContext.ServiceComponent
                                  join bbb in dbContext.Component on aaa.v_ComponentId equals bbb.v_ComponentId
                                  join J1 in dbContext.SystemUser on new { i_InsertUserId = aaa.i_InsertUserId }
                                                  equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                  from J1 in J1_join.DefaultIfEmpty()

                                  join J2 in dbContext.SystemUser on new { i_UpdateUserId = aaa.i_UpdateUserId }
                                                                  equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                  from J2 in J2_join.DefaultIfEmpty()

                                  join fff in dbContext.SystemParameter on new { a = bbb.i_CategoryId, b = 116 } // CATEGORIA DEL EXAMEN
                                                                               equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                                  from fff in J5_join.DefaultIfEmpty()

                                      // Usuario Medico Evaluador / Medico Aprobador ****************************
                                  join me in dbContext.SystemUser on aaa.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                  from me in me_join.DefaultIfEmpty()

                                  join pme in dbContext.Professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                  from pme in pme_join.DefaultIfEmpty()

                                      //*********************************************************************

                                  where (aaa.v_ServiceId == pstrServiceId) &&
                                        (bbb.i_ComponentTypeId == (int?)Enumeratores.ComponentType.Examen) &&
                                        (aaa.i_IsDeleted == 0) &&
                                        (aaa.i_IsRequiredId == (int?)Enumeratores.SiNo.Si)

                                  //orderby bbb.i_CategoryId, bbb.v_Name

                                  select new
                                  {
                                      v_ComponentId = bbb.v_ComponentId,
                                      v_ComponentName = bbb.v_Name,
                                      v_ServiceComponentId = aaa.v_ServiceComponentId,
                                      v_CreationUser = J1.v_UserName,
                                      v_UpdateUser = J2.v_UserName,
                                      d_CreationDate = aaa.d_InsertDate,
                                      d_UpdateDate = aaa.d_UpdateDate,
                                      i_IsDeleted = aaa.i_IsDeleted.Value,
                                      i_CategoryId = bbb.i_CategoryId,
                                      v_CategoryName = fff.v_Value1,
                                      i_ServiceComponentStatusId = aaa.i_ServiceComponentStatusId,
                                      DiagnosticRepository = (from dr in aaa.service.diagnosticrepository
                                                              where (dr.v_ServiceId == pstrServiceId) &&
                                                                    (dr.v_ComponentId == aaa.v_ComponentId && dr.i_IsDeleted == 0)
                                                              select new DiagnosticRepositoryList
                                                              {
                                                                  v_DiseasesId = dr.diseases.v_DiseasesId,
                                                                  v_DiseasesName = dr.diseases.v_Name
                                                              }),
                                      FirmaMedico = pme.b_SignatureImage
                                  }).AsEnumerable().Select(p => new ServiceComponentList
                                  {
                                      v_ComponentId = p.v_ComponentId,
                                      v_ComponentName = p.v_ComponentName,
                                      v_ServiceComponentId = p.v_ServiceComponentId,
                                      v_CreationUser = p.v_CreationUser,
                                      v_UpdateUser = p.v_UpdateUser,
                                      d_CreationDate = p.d_CreationDate,
                                      d_UpdateDate = p.d_UpdateDate,
                                      i_IsDeleted = p.i_IsDeleted,
                                      i_CategoryId = p.i_CategoryId,
                                      v_CategoryName = p.v_CategoryName,
                                      i_ServiceComponentStatusId = p.i_ServiceComponentStatusId,
                                      DiagnosticRepository = p.DiagnosticRepository.ToList(),
                                      FirmaMedico = p.FirmaMedico
                                  }).ToList();

                components.Sort((x, y) => x.v_ComponentId.CompareTo(y.v_ComponentId));
                components.ForEach(a => a.ServiceComponentFields = _finalQuery.FindAll(p => p.v_ComponentId == a.v_ComponentId));

                return components;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetServiceComponentDiagnosticsReport(string pstrServiceId, string pstrComponentId)
        {
            
                DatabaseContext dbContext = new DatabaseContext();

                var query = (from ccc in dbContext.DiagnosticRepository
                    join bbb in dbContext.Component on ccc.v_ComponentId equals bbb.v_ComponentId
                    join ddd in dbContext.Diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos 
                    where (ccc.v_ServiceId == pstrServiceId) &&
                          (ccc.v_ComponentId == pstrComponentId) &&
                          (ccc.i_IsDeleted == 0)
                    select new DiagnosticRepositoryList
                    {
                        v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                        v_ServiceId = ccc.v_ServiceId,
                        v_ComponentId = ccc.v_ComponentId,
                        v_DiseasesId = ccc.v_DiseasesId,
                        v_DiseasesName = ddd.v_Name,

                    }).ToList();

                var concat = string.Join(", ", query.Select(p => p.v_DiseasesName));

                return concat;
            
        }

        public OrganizationBE GetInfoMedicalCenter()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                OrganizationDto objDtoEntity = null;
                var objEntity = (from o in dbContext.Organization
                    where o.v_OrganizationId == BE.Sigesoft.Common.OWNER_ORGNIZATION_ID
                    select o).SingleOrDefault();

                var other = (from o in dbContext.Location
                    where o.v_OrganizationId == BE.Sigesoft.Common.OWNER_ORGNIZATION_ID
                    select o).SingleOrDefault();
                objEntity.v_SectorName = other == null ? "" : other.v_Name;
                
                return objEntity;
            }
        }

    }
}
