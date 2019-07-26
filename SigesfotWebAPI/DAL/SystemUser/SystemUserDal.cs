using BE.ProtocolSystemUser;
using BE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SystemUser
{
    public class SystemUserDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public static List<SystemUserCustom> GetSystemUserExternal(string protocolId)
        {
            try
            {
                var query = (from su1 in ctx.SystemUser
                             join B in ctx.ProtocolSystemUser on su1.i_SystemUserId equals B.i_SystemUserId
                             join C in ctx.Person on su1.v_PersonId equals C.v_PersonId
                             
                             join su2 in ctx.SystemUser on new { i_InsertUserId = su1.i_InsertUserId.Value }
                                                           equals new { i_InsertUserId = su2.i_SystemUserId } into su2_join
                             from su2 in su2_join.DefaultIfEmpty()

                             join su3 in ctx.SystemUser on new { i_UpdateUserId = su1.i_UpdateUserId.Value }
                                                           equals new { i_UpdateUserId = su3.i_SystemUserId } into su3_join
                             from su3 in su3_join.DefaultIfEmpty()
                             where su1.i_IsDeleted == 0 && B.v_ProtocolId == protocolId && B.i_IsDeleted == 0
                             select new SystemUserCustom
                             {
                                 i_SystemUserId = su1.i_SystemUserId,
                                 v_PersonId = su1.v_PersonId,
                                 v_UserName = su1.v_UserName,
                                 v_Password = su1.v_Password,
                                 v_SecretQuestion = su1.v_SecretQuestion,
                                 v_SecretAnswer = su1.v_SecretAnswer,
                                 i_IsDeleted = su1.i_IsDeleted,
                                 i_InsertUserId = su1.i_InsertUserId,
                                 d_InsertDate = su1.d_InsertDate,
                                 i_UpdateUserId = su1.i_UpdateUserId,
                                 d_UpdateDate = su1.d_UpdateDate,
                                 v_InsertUser = su2.v_UserName,
                                 v_UpdateUser = su3.v_UserName,
                                 v_PersonName = C.v_FirstName + " " + C.v_FirstLastName + " " + C.v_SecondLastName,
                                 v_DocNumber = C.v_DocNumber,
                                 d_ExpireDate = su1.d_ExpireDate,
                                 v_ProtocolId = B.v_ProtocolId
                             }
                            ).ToList().GroupBy(x => x.i_SystemUserId).Select(x => x.First()).ToList();

                return query;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static SystemUserCustom GetSystemUserById(int systemuserId)
        {
            try
            {
                var obj = (from sus in ctx.SystemUser
                           join per in ctx.Person on sus.v_PersonId equals per.v_PersonId
                           join prof in ctx.Professional on per.v_PersonId equals prof.v_PersonId into prof_join
                           from prof in prof_join.DefaultIfEmpty()
                           where sus.i_SystemUserId == systemuserId && sus.i_IsDeleted == 0
                           select new SystemUserCustom
                           {
                               v_FirstName = per.v_FirstName,
                               v_SecondLastName = per.v_SecondLastName,
                               v_FirstLastName = per.v_FirstLastName,
                               i_DocTypeId = per.i_DocTypeId,
                               i_SexTypeId = per.i_SexTypeId,
                               i_LevelOfId = per.i_LevelOfId,
                               i_MaritalStatusId = per.i_MaritalStatusId,
                               i_ProfesionId = prof.i_ProfessionId == null ? -1 : prof.i_ProfessionId,
                               v_DocNumber = per.v_DocNumber,
                               v_Mail = per.v_Mail,
                               v_BirthPlace = per.v_BirthPlace,
                               v_TelephoneNumber = per.v_TelephoneNumber,
                               v_AdressLocation = per.v_AdressLocation,
                               v_ProfessionalCode = prof.v_ProfessionalCode == null ? "" : prof.v_ProfessionalCode,
                               v_ProfessionalInformation = prof.v_ProfessionalCode == null ? "" : prof.v_ProfessionalCode,
                               v_UserName = sus.v_UserName,
                               v_Password = sus.v_Password,
                               d_ExpireDate = sus.d_ExpireDate,
                               d_Birthdate = per.d_Birthdate,
                               i_SystemUserId = sus.i_SystemUserId,
                               v_PersonId = sus.v_PersonId,
                           }).FirstOrDefault();

                obj.ListPremisos = (from prot in ctx.ProtocolSystemUser
                                    where prot.i_SystemUserId == systemuserId && prot.i_IsDeleted == 0
                                    select new ProtocolSystemUserCustom
                                    {
                                        v_ProtocolId = prot.v_ProtocolId,
                                        i_ApplicationHierarchyId = prot.i_ApplicationHierarchyId,
                                    }).ToList();

                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool DeletedSystemUser(int systemUserId)
        {
            try
            {
                var objSystem = ctx.SystemUser.Where(x => x.i_SystemUserId == systemUserId).FirstOrDefault();
                objSystem.i_IsDeleted = 1;
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
