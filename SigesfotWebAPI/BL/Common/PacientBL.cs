using BE.Common;
using BE.Component;
using BE.Sigesoft;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.PlanIntegral;
namespace BL.Common
{
    public class PacientBL
    {
        private DatabaseContext ctx = new DatabaseContext();

        #region CRUD



        #endregion

        #region Bussiness Logic
        public BoardPacient GetAllPacients(BoardPacient data)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;
                int groupDocTypeId = (int)Enumeratores.DataHierarchy.TypeDoc;
                int skip = (data.Index - 1) * data.Take;

                //filters
                string filterPacient = string.IsNullOrWhiteSpace(data.Pacient) ? "" : data.Pacient;
                string filterDocNumber = string.IsNullOrWhiteSpace(data.DocNumber) ? "" : data.DocNumber;

                var list = (from a in ctx.Pacient
                            join b in ctx.Person on a.v_PersonId equals b.v_PersonId
                            join c in ctx.DataHierarchy on new { a = b.i_DocTypeId.Value, b = groupDocTypeId } equals new { a = c.i_ItemId, b = c.i_GroupId }
                            where a.i_IsDeleted == isDeleted 
                                    && (b.v_FirstName.Contains(filterPacient) || b.v_FirstLastName.Contains(filterPacient) || b.v_SecondLastName.Contains(filterPacient) )
                                    && (b.v_DocNumber.Contains(filterDocNumber))
                                    && (data.DocTypeId == -1 || b.i_DocTypeId == data.DocTypeId)
                            select new Pacients
                            {
                                PacientId = a.v_PersonId,
                                PacientFullName = b.v_FirstName + " " + b.v_FirstLastName + " " + b.v_SecondLastName,
                                DocType = c.v_Value1,
                                DocNumber = b.v_DocNumber,
                                TelephoneNumber = b.v_TelephoneNumber

                            }).ToList();

                int totalRecords = list.Count;

                if (data.Take > 0)
                    list = list.Skip(skip).Take(data.Take).ToList();

                data.TotalRecords = totalRecords;
                data.List = list;

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Pacients GetPacientById(string pacientId)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;

                var data = (from a in ctx.Person
                            where a.i_IsDeleted == isDeleted && a.v_PersonId == pacientId
                            select new Pacients()
                            {
                                PacientId = a.v_PersonId,
                                PacientFullName = a.v_FirstName + " " + a.v_FirstLastName + " " + a.v_SecondLastName,
                                DocTypeId = a.i_DocTypeId.Value,
                                DocNumber = a.v_DocNumber,
                                TelephoneNumber = a.v_TelephoneNumber,

                                FirstName = a.v_FirstName,
                                FirstLastName = a.v_FirstLastName,
                                SecondLastName = a.v_SecondLastName,
                                
                                //Birthdate = a.d_Birthdate,
                                //BirthPlace = a.v_BirthPlace,
                                //SexTypeId = a.i_SexTypeId.Value,
                                //MaritalStatusId = a.i_MaritalStatusId.Value,
                                //LevelOfId = a.i_LevelOfId.Value,
                                //AdressLocation = a.v_AdressLocation,
                               // GeografyLocationId = a.v_GeografyLocationId,
                                //ContactName = a.v_ContactName,
                                //EmergencyPhone = a.v_EmergencyPhone,
                               // PersonImage = a.b_PersonImage,
                               // Mail = a.v_Mail,
                                //BloodGroupId = a.i_BloodGroupId.Value,
                                //BloodFactorId = a.i_BloodFactorId.Value,
                                //FingerPrintTemplate = a.b_FingerPrintTemplate,
                               // RubricImage = a.b_RubricImage,
                                //FingerPrintImage = a.b_FingerPrintImage,
                               // RubricImageText = a.t_RubricImageText,
                                //CurrentOccupation = a.v_CurrentOccupation,
                                //DepartmentId = a.i_DepartmentId.Value,
                                //ProvinceId = a.i_ProvinceId.Value,
                                //DistrictId = a.i_DistrictId.Value,
                               // ResidenceInWorkplaceId = a.i_ResidenceInWorkplaceId.Value,
                               // ResidenceTimeInWorkplace = a.v_ResidenceTimeInWorkplace,
                               // TypeOfInsuranceId = a.i_TypeOfInsuranceId.Value,
                                //NumberLivingChildren = a.i_NumberLivingChildren.Value,
                                //NumberDependentChildren = a.i_NumberDependentChildren.Value,
                                //OccupationTypeId = a.i_OccupationTypeId.Value,
                               //OwnerName = a.v_OwnerName,
                                //NumberLiveChildren = a.i_NumberLiveChildren.Value,
                                //NumberDeadChildren = a.i_NumberDeadChildren.Value,
                            }).FirstOrDefault();

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool AddPacient(Pacients pacient, int systemUserId)
        {
            PersonBL oPersonBL = new PersonBL();

            try
            {
                var oPersonBE = new PersonDto
                {
                    v_FirstName = pacient.FirstName,
                    v_FirstLastName = pacient.FirstLastName,
                    v_SecondLastName = pacient.SecondLastName,
                    i_DocTypeId = pacient.DocTypeId,
                    v_DocNumber = pacient.DocNumber,
                    v_TelephoneNumber = pacient.TelephoneNumber
                };

                var personId = oPersonBL.AddPerson(oPersonBE, systemUserId);
                //aaa
                if (personId != "")
                {
                    var oPacient = new PacientBE
                    {
                        v_PersonId = personId,
                        i_IsDeleted = (int)Enumeratores.SiNo.No,
                        d_InsertDate = DateTime.UtcNow,
                        i_InsertUserId = systemUserId
                    };


                    ctx.Pacient.Add(oPacient);

                    int rows = ctx.SaveChanges();

                    if (rows > 0)
                        return true;

                    return false;
                }
                else
                {
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public bool EditPacient(Pacients pacient, int systemUserId)
        {
            PersonBL oPersonBL = new PersonBL();
            try
            {
                var opacient = (from a in ctx.Person where a.v_PersonId == pacient.PacientId select a).FirstOrDefault();

                if (opacient == null)
                    return false;

                    opacient.v_FirstName = pacient.FirstName;
                    opacient.v_FirstLastName = pacient.FirstLastName;
                    opacient.v_SecondLastName = pacient.SecondLastName;
                    opacient.i_DocTypeId = pacient.DocTypeId;
                    opacient.v_DocNumber = pacient.DocNumber;
                    opacient.v_TelephoneNumber = pacient.TelephoneNumber;
                return false;// oPersonBL.UpdatePerson(opacient, systemUserId); 
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public bool DeletePacient(string pacientId, int systemUserId)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;
                var empresa = (from a in ctx.Pacient where a.v_PersonId == pacientId && a.i_IsDeleted == isDeleted select a).FirstOrDefault();

                empresa.i_UpdateUserId = systemUserId;
                empresa.d_UpdateDate = DateTime.UtcNow;
                empresa.i_IsDeleted = (int)Enumeratores.SiNo.Si;

                int rows = ctx.SaveChanges();

                return rows > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public PacientList DevolverDatosPaciente(string pstrServiceId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var objEntity = (from a in dbContext.Service
                                 join b in dbContext.Person on a.v_PersonId equals b.v_PersonId

                                 join c in dbContext.SystemParameter on new { a = b.i_SexTypeId.Value, b = 100 }
                                    equals new { a = c.i_ParameterId, b = c.i_GroupId }  // GENERO
                                 join d in dbContext.Protocol on a.v_ProtocolId equals d.v_ProtocolId


                                 join E in dbContext.DataHierarchy on new { a = b.i_DepartmentId.Value, b = 113 }
                                                       equals new { a = E.i_ItemId, b = E.i_GroupId } into E_join
                                 from E in E_join.DefaultIfEmpty()

                                 join F in dbContext.DataHierarchy on new { a = b.i_ProvinceId.Value, b = 113 }
                                                       equals new { a = F.i_ItemId, b = F.i_GroupId } into F_join
                                 from F in F_join.DefaultIfEmpty()

                                 join G in dbContext.DataHierarchy on new { a = b.i_DistrictId.Value, b = 113 }
                                                       equals new { a = G.i_ItemId, b = G.i_GroupId } into G_join
                                 from G in G_join.DefaultIfEmpty()

                                 join H in dbContext.Person on a.v_PersonId equals H.v_PersonId into H_join
                                 from H in H_join.DefaultIfEmpty()

                                 join I in dbContext.DataHierarchy on new { a = H.i_DepartmentId.Value, b = 113 }
                                                       equals new { a = I.i_ItemId, b = I.i_GroupId } into I_join
                                 from I in I_join.DefaultIfEmpty()

                                 join J in dbContext.DataHierarchy on new { a = H.i_ProvinceId.Value, b = 113 }
                                                       equals new { a = J.i_ItemId, b = J.i_GroupId } into J_join
                                 from J in J_join.DefaultIfEmpty()

                                 join K in dbContext.DataHierarchy on new { a = H.i_DistrictId.Value, b = 113 }
                                                       equals new { a = K.i_ItemId, b = K.i_GroupId } into K_join
                                 from K in K_join.DefaultIfEmpty()

                                 join M in dbContext.SystemParameter on new { a = H.i_MaritalStatusId.Value, b = 101 }
                                              equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                                 from M in M_join.DefaultIfEmpty()

                                 join N in dbContext.DataHierarchy on new { a = H.i_LevelOfId.Value, b = 108 }
                                                 equals new { a = N.i_ItemId, b = N.i_GroupId } into N_join
                                 from N in N_join.DefaultIfEmpty()

                                 join P in dbContext.SystemParameter on new { a = b.i_BloodGroupId.Value, b = 154 }
                                                 equals new { a = P.i_ParameterId, b = P.i_GroupId } into P_join
                                 from P in P_join.DefaultIfEmpty()

                                 join Q in dbContext.SystemParameter on new { a = b.i_BloodFactorId.Value, b = 155 }
                                                 equals new { a = Q.i_ParameterId, b = Q.i_GroupId } into Q_join
                                 from Q in Q_join.DefaultIfEmpty()

                                 join r in dbContext.ServiceComponent on a.v_ServiceId equals r.v_ServiceId

                                 // Empresa / Sede Cliente ******************************************************
                                 join oc in dbContext.Organization on new { a = d.v_CustomerOrganizationId }
                                         equals new { a = oc.v_OrganizationId } into oc_join
                                 from oc in oc_join.DefaultIfEmpty()

                                 join z in dbContext.Organization on new { a = d.v_EmployerOrganizationId }
                                         equals new { a = z.v_OrganizationId } into z_join
                                 from z in z_join.DefaultIfEmpty()

                                 join lc in dbContext.Location on new { a = d.v_CustomerOrganizationId, b = d.v_CustomerLocationId }
                                       equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                                 from lc in lc_join.DefaultIfEmpty()

                                     //**********************************************************************************
                                 join O in dbContext.SystemParameter on new { a = 134, b = a.i_MacId.Value }
                                                      equals new { a = O.i_GroupId, b = O.i_ParameterId } into O_join
                                 from O in O_join.DefaultIfEmpty()
                                 join J4 in dbContext.SystemParameter on new { ItemId = a.i_AptitudeStatusId.Value, groupId = 124 }
                                      equals new { ItemId = J4.i_ParameterId, groupId = J4.i_GroupId } into J4_join
                                 from J4 in J4_join.DefaultIfEmpty()

                                 join su in dbContext.SystemUser on a.i_UpdateUserMedicalAnalystId.Value equals su.i_SystemUserId into su_join
                                 from su in su_join.DefaultIfEmpty()

                                 join pr in dbContext.Professional on su.v_PersonId equals pr.v_PersonId into pr_join
                                 from pr in pr_join.DefaultIfEmpty()

                                 join su1 in dbContext.SystemUser on a.i_UpdateUserOccupationalMedicaltId.Value equals su1.i_SystemUserId into su1_join
                                 from su1 in su1_join.DefaultIfEmpty()

                                 join pr2 in dbContext.Professional on su1.v_PersonId equals pr2.v_PersonId into pr2_join
                                 from pr2 in pr2_join.DefaultIfEmpty()

                                 where a.v_ServiceId == pstrServiceId && a.i_IsDeleted == 0
                                 select new PacientList
                                 {
                                     Trabajador = b.v_FirstLastName + " " + b.v_SecondLastName + " " + b.v_FirstName,
                                     d_Birthdate = b.d_Birthdate.Value,
                                     //
                                     v_PersonId = b.v_PersonId,

                                     v_FirstLastName = b.v_FirstLastName,
                                     v_SecondLastName = b.v_SecondLastName,
                                     v_FirstName = b.v_FirstName,
                                     v_BirthPlace = b.v_BirthPlace,
                                     v_DepartamentName = I.v_Value1,
                                     v_ProvinceName = J.v_Value1,
                                     v_DistrictName = K.v_Value1,
                                     v_AdressLocation = b.v_AdressLocation,
                                     GradoInstruccion = N.v_Value1,
                                     v_CentroEducativo = b.v_CentroEducativo,
                                     v_MaritalStatus = M.v_Value1,
                                     v_BloodGroupName = P.v_Value1,
                                     v_BloodFactorName = Q.v_Value1,
                                     v_IdService = a.v_ServiceId,
                                     v_OrganitationName = oc.v_Name,
                                     i_NumberLivingChildren = b.i_NumberLivingChildren,
                                     FechaCaducidad = a.d_GlobalExpirationDate,
                                     FechaActualizacion = a.d_UpdateDate,
                                     N_Informe = r.v_ServiceComponentId,
                                     v_Religion = b.v_Religion,
                                     v_Nacionalidad = b.v_Nacionalidad,
                                     v_ResidenciaAnterior = b.v_ResidenciaAnterior,
                                     i_DocTypeId = b.i_DocTypeId,
                                     v_OwnerName = b.v_OwnerName,
                                     v_Employer = z.v_Name,
                                     v_ContactName = b.v_ContactName,
                                     i_Relationship = b.i_Relationship,
                                     v_EmergencyPhone = b.v_EmergencyPhone,
                                     //
                                     Genero = c.v_Value1,
                                     i_SexTypeId = b.i_SexTypeId,
                                     v_DocNumber = b.v_DocNumber,
                                     v_TelephoneNumber = b.v_TelephoneNumber,
                                     Empresa = oc.v_Name,
                                     Sede = lc.v_Name,
                                     v_CurrentOccupation = b.v_CurrentOccupation,
                                     FechaServicio = a.d_ServiceDate.Value,
                                     i_MaritalStatusId = b.i_MaritalStatusId,
                                     // Antecedentes ginecologicos
                                     d_PAP = a.d_PAP.Value,
                                     d_Mamografia = a.d_Mamografia.Value,
                                     v_CiruGine = a.v_CiruGine,
                                     v_Gestapara = a.v_Gestapara,
                                     v_Menarquia = a.v_Menarquia,
                                     v_Findings = a.v_Findings,
                                     d_Fur = a.d_Fur,
                                     v_CatemenialRegime = a.v_CatemenialRegime,
                                     i_MacId = a.i_MacId,
                                     v_Mac = O.v_Value1,
                                     v_Story = a.v_Story,
                                     Aptitud = J4.v_Value1,
                                     b_FirmaAuditor = pr.b_SignatureImage,
                                     FirmaTrabajador = b.b_RubricImage,
                                     HuellaTrabajador = b.b_FingerPrintImage,
                                     FirmaDoctorAuditor = pr2.b_SignatureImage,
                                 }
                                ).ToList();
                var DatosMedicoMedicinaEvaluador = ObtenerDatosMedicoMedicina(pstrServiceId, Constants.EXAMEN_FISICO_ID, Constants.EXAMEN_FISICO_7C_ID);
                //var DatosMedicoMedicinaAuditor = ObtenerDatosMedicoMedicinaAuditor(pstrServiceId, Constants.EXAMEN_FISICO_ID, Constants.EXAMEN_FISICO_7C_ID);

                var result = (from a in objEntity
                              select new PacientList
                              {
                                  Trabajador = a.Trabajador,
                                  d_Birthdate = a.d_Birthdate,
                                  v_ContactName = a.v_ContactName,
                                  v_EmergencyPhone = a.v_EmergencyPhone,
                                  i_Relationship = a.i_Relationship,
                                  //
                                  v_PersonId = a.v_PersonId,
                                  v_FirstLastName = a.v_FirstLastName,
                                  v_SecondLastName = a.v_SecondLastName,
                                  v_FirstName = a.v_FirstName,
                                  v_BirthPlace = a.v_BirthPlace,
                                  v_DepartamentName = a.v_DepartamentName,
                                  v_ProvinceName = a.v_ProvinceName,
                                  v_DistrictName = a.v_DistrictName,
                                  GradoInstruccion = a.GradoInstruccion,
                                  v_MaritalStatus = a.v_MaritalStatus,
                                  v_BloodGroupName = a.v_BloodGroupName,
                                  v_BloodFactorName = a.v_BloodFactorName,
                                  v_AdressLocation = a.v_AdressLocation,
                                  v_IdService = a.v_IdService,
                                  v_OrganitationName = a.v_OrganitationName,
                                  i_NumberLivingChildren = a.i_NumberLivingChildren,
                                  v_CentroEducativo = a.v_CentroEducativo,
                                  FechaCaducidad = a.FechaCaducidad,
                                  FechaActualizacion = a.FechaActualizacion,
                                  N_Informe = a.N_Informe,
                                  v_Religion = a.v_Religion,
                                  v_Nacionalidad = a.v_Nacionalidad,
                                  v_ResidenciaAnterior = a.v_ResidenciaAnterior,
                                  i_DocTypeId = a.i_DocTypeId,
                                  v_OwnerName = a.v_OwnerName,
                                  v_Employer = a.v_Employer,
                                  i_MaritalStatusId = a.i_MaritalStatusId,
                                  //
                                  Edad = GetEdad(a.d_Birthdate.Value),
                                  Genero = a.Genero,
                                  i_SexTypeId = a.i_SexTypeId,
                                  v_DocNumber = a.v_DocNumber,
                                  v_TelephoneNumber = a.v_TelephoneNumber,
                                  Empresa = a.Empresa,
                                  Sede = a.Sede,
                                  v_CurrentOccupation = a.v_CurrentOccupation,
                                  FechaServicio = a.FechaServicio,
                                  MedicoGrabaMedicina = DatosMedicoMedicinaEvaluador == null ? "" : DatosMedicoMedicinaEvaluador.ApellidosDoctor + " " + DatosMedicoMedicinaEvaluador.NombreDoctor,
                                  // Antecedentes ginecologicos
                                  d_PAP = a.d_PAP,
                                  d_Mamografia = a.d_Mamografia,
                                  v_CiruGine = a.v_CiruGine,
                                  v_Gestapara = a.v_Gestapara,
                                  v_Menarquia = a.v_Menarquia,
                                  v_Findings = a.v_Findings,
                                  d_Fur = a.d_Fur,
                                  v_CatemenialRegime = a.v_CatemenialRegime,
                                  i_MacId = a.i_MacId,
                                  v_Mac = a.v_Mac,
                                  v_Story = a.v_Story,
                                  Aptitud = a.Aptitud,
                                  b_FirmaEvaluador = DatosMedicoMedicinaEvaluador == null ? null : DatosMedicoMedicinaEvaluador.FirmaMedicoMedicina,
                                  b_FirmaAuditor = a.b_FirmaAuditor,
                                  FirmaTrabajador = a.FirmaTrabajador,
                                  HuellaTrabajador = a.HuellaTrabajador,
                                  FirmaDoctorAuditor = a.FirmaDoctorAuditor
                              }
                            ).FirstOrDefault();


                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int GetEdad(DateTime BirthDate)
        {
            int edad = DateTime.Today.AddTicks(-BirthDate.Ticks).Year - 1;
            return edad;
        }

        public DatosDoctorMedicina ObtenerDatosMedicoMedicina(string pstrServiceId, string p1, string p2)
        {
            DatabaseContext dbContext = new DatabaseContext();

            var objEntity = (from E in dbContext.ServiceComponent

                             join me in dbContext.SystemUser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                             from me in me_join.DefaultIfEmpty()

                             join pme in dbContext.Professional on me.v_PersonId equals pme.v_PersonId into pme_join
                             from pme in pme_join.DefaultIfEmpty()

                             join a in dbContext.Person on me.v_PersonId equals a.v_PersonId

                             where E.v_ServiceId == pstrServiceId &&
                             (E.v_ComponentId == p1 || E.v_ComponentId == p2)
                             select new DatosDoctorMedicina
                             {
                                 FirmaMedicoMedicina = pme.b_SignatureImage,
                                 ApellidosDoctor = a.v_FirstLastName + " " + a.v_SecondLastName,
                                 DireccionDoctor = a.v_AdressLocation,
                                 NombreDoctor = a.v_FirstName,
                                 CMP = pme.v_ProfessionalCode,

                             }).FirstOrDefault();

            return objEntity;
        }


        public DatosAtencion GetDatosPersonalesAtencion(string serviceId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objEntity = (from s in dbContext.Service
                                 join pe in dbContext.Person on s.v_PersonId equals pe.v_PersonId
                                 join C in dbContext.SystemParameter on new { a = pe.i_SexTypeId.Value, b = 100 }
                                                              equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                                 from C in C_join.DefaultIfEmpty()
                                 where s.v_ServiceId == serviceId
                                 select new DatosAtencion
                                 {
                                     Paciente = pe.v_FirstLastName,
                                     Genero = C.v_Value1
                                 }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        #endregion
    }
}
