using BE.Common;
using BE.Message;
using BE.Pacient;
using BE.Protocol;
using BE.Security;
using DAL.Organizarion;
using DAL.ProtocolSystemUser;
using DAL.Security;
using DAL.SystemUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static BE.Common.Enumeratores;

namespace DAL.Pacient
{
    public class PacientDal
    {
        
        public BoardPacients GetAllPacient(BoardPacients data)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var isDeleted = (int)Enumeratores.SiNo.No;
                //int groupDocTypeId = (int)Enumeratores.DataHierarchy.TypeDoc;
                int skip = (data.Index - 1) * data.Take;

                //filters
                string filterPacient = string.IsNullOrWhiteSpace(data.Pacient) ? "" : data.Pacient;
                //string filterDocNumber = string.IsNullOrWhiteSpace(data.DocNumber) ? "" : data.DocNumber;
                var list = (from per in ctx.Person
                            //join dhy in ctx.DataHierarchy on new { a = per.i_DocTypeId.Value, b = groupDocTypeId } equals new { a = dhy.i_ItemId, b = dhy.i_GroupId }
                            join sys in ctx.SystemParameter on new { a = per.i_BloodGroupId.Value, b = 154 } equals new { a = sys.i_ParameterId, b = sys.i_GroupId } into sys_join from sys in sys_join.DefaultIfEmpty()
                            join sys2 in ctx.SystemParameter on new { a = per.i_BloodFactorId.Value, b = 155 } equals new { a = sys2.i_ParameterId, b = sys2.i_GroupId } into sys2_join from sys2 in sys2_join.DefaultIfEmpty()
                            where per.i_IsDeleted == isDeleted
                                    && (per.v_FirstName.Contains(filterPacient) || per.v_FirstLastName.Contains(filterPacient) || per.v_SecondLastName.Contains(filterPacient) || filterPacient == "" || per.v_DocNumber.Contains(filterPacient))
                            select new PacientCustom
                            {
                                v_PersonId = per.v_PersonId,
                                v_FirstName = per.v_FirstName,
                                v_FirstLastName = per.v_FirstLastName,
                                v_SecondLastName = per.v_SecondLastName,
                                //i_DocTypeId = per.i_DocTypeId,
                                v_DocNumber = per.v_DocNumber,
                                d_Birthdate = per.d_Birthdate,
                                //v_BirthPlace = per.v_BirthPlace,
                                //i_SexTypeId = per.i_SexTypeId,
                                //i_MaritalStatusId = per.i_MaritalStatusId,
                                //i_LevelOfId = per.i_LevelOfId,
                                v_TelephoneNumber = per.v_TelephoneNumber,
                                v_AdressLocation = per.v_AdressLocation,
                               // v_GeografyLocationId = per.v_GeografyLocationId,
                                //v_ContactName = per.v_ContactName,
                                //v_EmergencyPhone = per.v_EmergencyPhone,
                                b_PersonImage = per.b_PersonImage,
                                v_Mail = per.v_Mail,
                                //i_BloodGroupId = per.i_BloodGroupId,
                                //i_BloodFactorId = per.i_BloodFactorId,
                               // b_FingerPrintTemplate = per.b_FingerPrintTemplate,
                                //b_RubricImage = per.b_RubricImage,
                                //b_FingerPrintImage = per.b_FingerPrintImage,
                                //t_RubricImageText = per.t_RubricImageText,
                                v_CurrentOccupation = per.v_CurrentOccupation,
                                //i_DepartmentId = per.i_DepartmentId,
                               // i_ProvinceId = per.i_ProvinceId,
                                //i_DistrictId = per.i_DistrictId,
                               // i_ResidenceInWorkplaceId = per.i_ResidenceInWorkplaceId,
                               // v_ResidenceTimeInWorkplace = per.v_ResidenceTimeInWorkplace,
                               // i_TypeOfInsuranceId = per.i_TypeOfInsuranceId,
                               // i_NumberLivingChildren = per.i_NumberLivingChildren,
                               // i_NumberDependentChildren = per.i_NumberDependentChildren,
                              //  i_OccupationTypeId = per.i_OccupationTypeId,
                                //v_OwnerName = per.v_OwnerName,
                                //i_NumberLiveChildren = per.i_NumberLiveChildren,
                                //i_NumberDeadChildren = per.i_NumberDeadChildren,
                                //i_IsDeleted = per.i_IsDeleted,
                               // i_InsertNodeId = per.i_InsertNodeId,
                                //i_UpdateNodeId = per.i_UpdateNodeId,
                                //i_Relationship = per.i_Relationship,
                                //v_ExploitedMineral = per.v_ExploitedMineral,
                                //i_AltitudeWorkId = per.i_AltitudeWorkId,
                                //i_PlaceWorkId = per.i_PlaceWorkId,
                                //v_NroPoliza = per.v_NroPoliza,
                                //v_Deducible = per.v_Deducible,
                                //i_NroHermanos = per.i_NroHermanos,
                                //v_Password = per.v_Password,
                                //v_Procedencia = per.v_Procedencia,
                                //v_CentroEducativo = per.v_CentroEducativo,
                                //v_Religion = per.v_Religion,
                                //v_Nacionalidad = per.v_Nacionalidad,
                                //v_ResidenciaAnterior = per.v_ResidenciaAnterior,
                                //v_Subs = per.v_Subs,
                                //v_ComentaryUpdate = per.v_ComentaryUpdate,
                                v_BloodGroupId = sys.v_Value1,
                                v_BloodFactorId = sys2.v_Value1,

                            }).OrderBy(x => x.v_FirstLastName).ToList();
                
                int totalRecords = list.Count;

                if (data.Take > 0)
                    list = list.Skip(skip).Take(data.Take).ToList();

                data.TotalRecords = totalRecords;
                foreach (var item in list)
                {
                    item.PersonImage = item.b_PersonImage == null ? null : Convert.ToBase64String(item.b_PersonImage);
                }
                data.List = list;

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void CreatePacient(string personId, int userId, int nodeId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var oPacient = new PacientBE
                {
                    v_PersonId = personId,
                    i_IsDeleted = (int)Enumeratores.SiNo.No,
                    d_InsertDate = DateTime.UtcNow,
                    i_InsertUserId = userId
                };


                cnx.Pacient.Add(oPacient);
                cnx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public string CreatePerson(PacientCustom data, int userId, int nodeId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var newId = new Common.Utils().GetPrimaryKey(nodeId, 8, "PP");
                PersonDto oPersonDto = new PersonDto();
                oPersonDto.v_PersonId = newId;
                oPersonDto.v_FirstName = data.v_FirstName;
                oPersonDto.v_FirstLastName = data.v_FirstLastName;
                oPersonDto.v_SecondLastName = data.v_SecondLastName;
                oPersonDto.i_DocTypeId = data.i_DocTypeId;
                oPersonDto.v_DocNumber = data.v_DocNumber;
                oPersonDto.d_Birthdate = data.d_Birthdate;
                oPersonDto.v_BirthPlace = data.v_BirthPlace;
                oPersonDto.i_SexTypeId = data.i_SexTypeId;
                oPersonDto.i_MaritalStatusId = data.i_MaritalStatusId;
                oPersonDto.i_LevelOfId = data.i_LevelOfId;
                oPersonDto.v_TelephoneNumber = data.v_TelephoneNumber;
                oPersonDto.v_AdressLocation = data.v_AdressLocation;
                oPersonDto.v_GeografyLocationId = data.v_GeografyLocationId;
                oPersonDto.v_ContactName = data.v_ContactName;
                oPersonDto.v_EmergencyPhone = data.v_EmergencyPhone;
                oPersonDto.b_PersonImage = data.b_PersonImage;
                oPersonDto.v_Mail = data.v_Mail;
                oPersonDto.i_BloodGroupId = data.i_BloodGroupId;
                oPersonDto.i_BloodFactorId = data.i_BloodFactorId;
                oPersonDto.b_FingerPrintTemplate = data.b_FingerPrintTemplate;
                oPersonDto.b_RubricImage = data.b_RubricImage;
                oPersonDto.b_FingerPrintImage = data.b_FingerPrintImage;
                oPersonDto.t_RubricImageText = data.t_RubricImageText;
                oPersonDto.v_CurrentOccupation = data.v_CurrentOccupation;
                oPersonDto.i_DepartmentId = data.i_DepartmentId;
                oPersonDto.i_ProvinceId = data.i_ProvinceId;
                oPersonDto.i_DistrictId = data.i_DistrictId;
                oPersonDto.i_ResidenceInWorkplaceId = data.i_ResidenceInWorkplaceId;
                oPersonDto.v_ResidenceTimeInWorkplace = data.v_ResidenceTimeInWorkplace;
                oPersonDto.i_TypeOfInsuranceId = data.i_TypeOfInsuranceId;
                oPersonDto.i_NumberLivingChildren = data.i_NumberLivingChildren;
                oPersonDto.i_NumberDependentChildren = data.i_NumberDependentChildren;
                oPersonDto.i_OccupationTypeId = data.i_OccupationTypeId;
                oPersonDto.v_OwnerName = data.v_OwnerName;
                oPersonDto.i_NumberLiveChildren = data.i_NumberLiveChildren;
                oPersonDto.i_NumberDeadChildren = data.i_NumberDeadChildren;
                oPersonDto.i_IsDeleted = 0;
                oPersonDto.i_InsertNodeId = data.i_InsertNodeId;
                oPersonDto.i_UpdateNodeId = data.i_UpdateNodeId;
                oPersonDto.i_Relationship = data.i_Relationship;
                oPersonDto.v_ExploitedMineral = data.v_ExploitedMineral;
                oPersonDto.i_AltitudeWorkId = data.i_AltitudeWorkId;
                oPersonDto.i_PlaceWorkId = data.i_PlaceWorkId;
                oPersonDto.v_NroPoliza = data.v_NroPoliza;
                oPersonDto.v_Deducible = data.v_Deducible;
                oPersonDto.i_NroHermanos = data.i_NroHermanos;
                oPersonDto.v_Password = data.v_Password;
                oPersonDto.v_Procedencia = data.v_Procedencia;
                oPersonDto.v_CentroEducativo = data.v_CentroEducativo;
                oPersonDto.v_Religion = data.v_Religion;
                oPersonDto.v_Nacionalidad = data.v_Nacionalidad;
                oPersonDto.v_ResidenciaAnterior = data.v_ResidenciaAnterior;
                oPersonDto.v_Subs = data.v_Subs;
                oPersonDto.v_ComentaryUpdate = data.v_ComentaryUpdate;
                //Auditoria
                oPersonDto.i_InsertUserId = userId;
                oPersonDto.d_InsertDate = DateTime.Now;

                cnx.Person.Add(oPersonDto);
                cnx.SaveChanges();
                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        
        public string UpdatePacient(PacientCustom data, int userId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();

                var objPerson = cnx.Person.Where(x => x.v_PersonId == data.v_PersonId && x.i_IsDeleted == 0).FirstOrDefault();

                if (objPerson != null)
                {
                    objPerson.v_FirstName = data.v_FirstName;
                    objPerson.v_FirstLastName = data.v_FirstLastName;
                    objPerson.v_SecondLastName = data.v_SecondLastName;
                    objPerson.i_DocTypeId = data.i_DocTypeId == null ? -1 : data.i_DocTypeId;
                    objPerson.v_DocNumber = data.v_DocNumber;
                    objPerson.d_Birthdate = data.d_Birthdate;
                    objPerson.v_BirthPlace = data.v_BirthPlace;
                    objPerson.i_SexTypeId = data.i_SexTypeId == null ? -1 : data.i_SexTypeId;
                    objPerson.i_MaritalStatusId = data.i_MaritalStatusId == null ? -1 : data.i_MaritalStatusId;
                    objPerson.i_LevelOfId = data.i_LevelOfId == null ? -1 : data.i_LevelOfId;
                    objPerson.v_TelephoneNumber = data.v_TelephoneNumber;
                    objPerson.v_AdressLocation = data.v_AdressLocation;
                    objPerson.v_GeografyLocationId = data.v_GeografyLocationId;
                    objPerson.v_ContactName = data.v_ContactName;
                    objPerson.v_EmergencyPhone = data.v_EmergencyPhone;
                    objPerson.b_PersonImage = data.b_PersonImage;
                    objPerson.v_Mail = data.v_Mail;
                    objPerson.i_BloodGroupId = data.i_BloodGroupId == null ? -1 : data.i_BloodGroupId;
                    objPerson.i_BloodFactorId = data.i_BloodFactorId == null ? -1 : data.i_BloodFactorId;
                    objPerson.b_FingerPrintTemplate = data.b_FingerPrintTemplate;
                    objPerson.b_RubricImage = data.b_RubricImage;
                    objPerson.b_FingerPrintImage = data.b_FingerPrintImage;
                    objPerson.t_RubricImageText = data.t_RubricImageText;
                    objPerson.v_CurrentOccupation = data.v_CurrentOccupation;
                    objPerson.i_DepartmentId = data.i_DepartmentId == null ? -1 : data.i_DepartmentId;
                    objPerson.i_ProvinceId = data.i_ProvinceId == null ? -1 : data.i_ProvinceId;
                    objPerson.i_DistrictId = data.i_DistrictId == null ? -1 : data.i_DistrictId;
                    objPerson.i_ResidenceInWorkplaceId = data.i_ResidenceInWorkplaceId == null ? -1 : data.i_ResidenceInWorkplaceId;
                    objPerson.v_ResidenceTimeInWorkplace = data.v_ResidenceTimeInWorkplace;
                    objPerson.i_TypeOfInsuranceId = data.i_TypeOfInsuranceId == null ? -1 : data.i_TypeOfInsuranceId;
                    objPerson.i_NumberLivingChildren = data.i_NumberLivingChildren;
                    objPerson.i_NumberDependentChildren = data.i_NumberDependentChildren;
                    objPerson.i_OccupationTypeId = data.i_OccupationTypeId == null ? -1 : data.i_OccupationTypeId;
                    objPerson.v_OwnerName = data.v_OwnerName;
                    objPerson.i_NumberLiveChildren = data.i_NumberLiveChildren;
                    objPerson.i_NumberDeadChildren = data.i_NumberDeadChildren;
                    objPerson.i_IsDeleted = 0;
                    objPerson.i_InsertNodeId = data.i_InsertNodeId;
                    objPerson.i_UpdateNodeId = data.i_UpdateNodeId;
                    objPerson.i_Relationship = data.i_Relationship == null ? -1 : data.i_Relationship;
                    objPerson.v_ExploitedMineral = data.v_ExploitedMineral;
                    objPerson.i_AltitudeWorkId = data.i_AltitudeWorkId == null ? -1 : data.i_AltitudeWorkId;
                    objPerson.i_PlaceWorkId = data.i_PlaceWorkId == null ? -1 : data.i_PlaceWorkId;
                    objPerson.v_NroPoliza = data.v_NroPoliza;
                    objPerson.v_Deducible = data.v_Deducible;
                    objPerson.i_NroHermanos = data.i_NroHermanos;
                    objPerson.v_Password = data.v_Password;
                    objPerson.v_Procedencia = data.v_Procedencia;
                    objPerson.v_CentroEducativo = data.v_CentroEducativo;
                    objPerson.v_Religion = data.v_Religion;
                    objPerson.v_Nacionalidad = data.v_Nacionalidad;
                    objPerson.v_ResidenciaAnterior = data.v_ResidenciaAnterior;
                    objPerson.v_Subs = data.v_Subs;
                    objPerson.v_ComentaryUpdate = data.v_ComentaryUpdate;


                    objPerson.i_UpdateUserId = userId;
                    objPerson.d_UpdateDate = DateTime.Now;

                    cnx.SaveChanges();

                    return data.v_PersonId;

                }

                return null;
                
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

        public PacientCustom FindPacientByDocNumberOrPersonId(string value)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                PacientCustom _PacientCustom = new PacientCustom();
                var objPacient = cnx.Person.Where(x => (x.v_DocNumber == value || x.v_PersonId == value) && x.i_IsDeleted == 0).FirstOrDefault();
                if (objPacient != null)
                {
                    _PacientCustom.BaseDatos = true;
                    _PacientCustom.v_PersonId = objPacient.v_PersonId;
                    _PacientCustom.v_FirstName = objPacient.v_FirstName;
                    _PacientCustom.v_FirstLastName = objPacient.v_FirstLastName;
                    _PacientCustom.v_SecondLastName = objPacient.v_SecondLastName;
                    _PacientCustom.i_DocTypeId = objPacient.i_DocTypeId;
                    _PacientCustom.v_DocNumber = objPacient.v_DocNumber;
                    _PacientCustom.d_Birthdate = objPacient.d_Birthdate;
                    _PacientCustom.v_BirthPlace = objPacient.v_BirthPlace;
                    _PacientCustom.i_SexTypeId = objPacient.i_SexTypeId;
                    _PacientCustom.i_MaritalStatusId = objPacient.i_MaritalStatusId;
                    _PacientCustom.i_LevelOfId = objPacient.i_LevelOfId;
                    _PacientCustom.v_TelephoneNumber = objPacient.v_TelephoneNumber;
                    _PacientCustom.v_AdressLocation = objPacient.v_AdressLocation;
                    _PacientCustom.v_GeografyLocationId = objPacient.v_GeografyLocationId;
                    _PacientCustom.v_ContactName = objPacient.v_ContactName;
                    _PacientCustom.v_EmergencyPhone = objPacient.v_EmergencyPhone;
                    _PacientCustom.b_PersonImage = objPacient.b_PersonImage;
                    _PacientCustom.v_Mail = objPacient.v_Mail;
                    _PacientCustom.i_BloodGroupId = objPacient.i_BloodGroupId;
                    _PacientCustom.i_BloodFactorId = objPacient.i_BloodFactorId;
                    _PacientCustom.b_FingerPrintTemplate = objPacient.b_FingerPrintTemplate;
                    _PacientCustom.b_RubricImage = objPacient.b_RubricImage;
                    _PacientCustom.b_FingerPrintImage = objPacient.b_FingerPrintImage;
                    _PacientCustom.t_RubricImageText = objPacient.t_RubricImageText;
                    _PacientCustom.v_CurrentOccupation = objPacient.v_CurrentOccupation;
                    _PacientCustom.i_DepartmentId = objPacient.i_DepartmentId;
                    _PacientCustom.i_ProvinceId = objPacient.i_ProvinceId;
                    _PacientCustom.i_DistrictId = objPacient.i_DistrictId;
                    _PacientCustom.i_ResidenceInWorkplaceId = objPacient.i_ResidenceInWorkplaceId;
                    _PacientCustom.v_ResidenceTimeInWorkplace = objPacient.v_ResidenceTimeInWorkplace;
                    _PacientCustom.i_TypeOfInsuranceId = objPacient.i_TypeOfInsuranceId;
                    _PacientCustom.i_NumberLivingChildren = objPacient.i_NumberLivingChildren;
                    _PacientCustom.i_NumberDependentChildren = objPacient.i_NumberDependentChildren;
                    _PacientCustom.i_OccupationTypeId = objPacient.i_OccupationTypeId;
                    _PacientCustom.v_OwnerName = objPacient.v_OwnerName;
                    _PacientCustom.i_NumberLiveChildren = objPacient.i_NumberLiveChildren;
                    _PacientCustom.i_NumberDeadChildren = objPacient.i_NumberDeadChildren;
                    _PacientCustom.i_IsDeleted = objPacient.i_IsDeleted;
                    _PacientCustom.i_InsertNodeId = objPacient.i_InsertNodeId;
                    _PacientCustom.i_UpdateNodeId = objPacient.i_UpdateNodeId;
                    _PacientCustom.i_Relationship = objPacient.i_Relationship;
                    _PacientCustom.v_ExploitedMineral = objPacient.v_ExploitedMineral;
                    _PacientCustom.i_AltitudeWorkId = objPacient.i_AltitudeWorkId;
                    _PacientCustom.i_PlaceWorkId = objPacient.i_PlaceWorkId;
                    _PacientCustom.v_NroPoliza = objPacient.v_NroPoliza;
                    _PacientCustom.v_Deducible = objPacient.v_Deducible;
                    _PacientCustom.i_NroHermanos = objPacient.i_NroHermanos;
                    _PacientCustom.v_Password = objPacient.v_Password;
                    _PacientCustom.v_Procedencia = objPacient.v_Procedencia;
                    _PacientCustom.v_CentroEducativo = objPacient.v_CentroEducativo;
                    _PacientCustom.v_Religion = objPacient.v_Religion;
                    _PacientCustom.v_Nacionalidad = objPacient.v_Nacionalidad;
                    _PacientCustom.v_ResidenciaAnterior = objPacient.v_ResidenciaAnterior;
                    _PacientCustom.v_Subs = objPacient.v_Subs;
                    _PacientCustom.i_Edad = GetEdad(objPacient.d_Birthdate.Value);
                    _PacientCustom.v_ComentaryUpdate = objPacient.v_ComentaryUpdate;
                    _PacientCustom.PersonImage = objPacient.b_PersonImage == null? null : Convert.ToBase64String(objPacient.b_PersonImage);
                    _PacientCustom.PersonHuella = objPacient.b_FingerPrintImage == null ? null : Convert.ToBase64String(objPacient.b_FingerPrintImage);
                    _PacientCustom.PersonFirma = objPacient.b_RubricImage == null ? null : Convert.ToBase64String(objPacient.b_RubricImage);
                    _PacientCustom.TieneRegistroHuella = objPacient.b_FingerPrintImage == null ? "NO REGISTRADO" : "REGISTRADO";
                    _PacientCustom.TieneRegistroFirma = objPacient.b_RubricImage == null ? "NO REGISTRADO" : "REGISTRADO";
                    return _PacientCustom;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public MessageCustom AddSystemUserExternal(PersonDto pobjPerson, ProfessionalBE pobjProfessional, SystemUserDto pobjSystemUser, List<ProtocolSystemUserBE> ListProtocolSystemUser, int userId, int nodeId)
        {
            //mon.IsActive = true;
            string newId = string.Empty;
            int systemUserId = -1;
            MessageCustom msg = new MessageCustom();
            OperationResult objOperationResult = new OperationResult();

            try
            {
                using (var ts = new TransactionScope())
                {
                    #region Validations
                    // Validar el DNI de la persona
                    DatabaseContext dbContext = new DatabaseContext();
                    if (pobjPerson != null)
                    {
                        if (pobjSystemUser.i_SystemUserId == -1) //-1 es nuevo
                        {                           
                            // Grabar Persona
                            var _recordCount1 = GetPersonCount(pobjPerson.v_DocNumber);
                            if (_recordCount1 != 0) throw new Exception("El número de documento <strong>" + pobjPerson.v_DocNumber + "</strong> ya se encuentra registrado. Por favor ingrese otro número de documento.");
                            
                            pobjPerson.d_InsertDate = DateTime.Now;
                            pobjPerson.i_InsertUserId = userId;
                            pobjPerson.i_IsDeleted = 0;
                            // Autogeneramos el Pk de la tabla
                            newId = new Common.Utils().GetPrimaryKey(nodeId, 8, "PP");
                            pobjPerson.v_PersonId = newId;

                            dbContext.Person.Add(pobjPerson);
                            dbContext.SaveChanges();


                            // Grabar Profesional
                            pobjProfessional.v_PersonId = pobjPerson.v_PersonId;
                            bool resultProf = AddProfessional(pobjProfessional, userId, nodeId);
                            if (!resultProf) throw new Exception("Sucedió un error al guardar el profesional, por favor actualice la pagina y vuelva a intentar");
                        }
                        else
                        {//actualiza
                            var objPerson = dbContext.Person.Where(x => x.v_PersonId == pobjPerson.v_PersonId).FirstOrDefault();
                            objPerson.v_FirstName = pobjPerson.v_FirstName;
                            objPerson.v_FirstLastName = pobjPerson.v_FirstLastName;
                            objPerson.v_SecondLastName = pobjPerson.v_SecondLastName;
                            objPerson.i_DocTypeId = pobjPerson.i_DocTypeId;
                            objPerson.v_DocNumber = pobjPerson.v_DocNumber;
                            objPerson.i_SexTypeId = pobjPerson.i_SexTypeId;
                            objPerson.i_MaritalStatusId = pobjPerson.i_MaritalStatusId;
                            objPerson.i_LevelOfId = pobjPerson.i_LevelOfId;
                            objPerson.v_Mail = pobjPerson.v_Mail;
                            objPerson.v_BirthPlace = pobjPerson.v_BirthPlace;
                            objPerson.v_TelephoneNumber = pobjPerson.v_TelephoneNumber;
                            objPerson.d_Birthdate = pobjPerson.d_Birthdate;
                            objPerson.v_AdressLocation = pobjPerson.v_AdressLocation;
                            objPerson.i_UpdateUserId = userId;
                            objPerson.d_UpdateDate = DateTime.Now;
                            dbContext.SaveChanges();

                            var objProfessional = dbContext.Professional.Where(x => x.v_PersonId == pobjPerson.v_PersonId).FirstOrDefault();
                            objProfessional.i_ProfessionId = pobjProfessional.i_ProfessionId;
                            objProfessional.v_ProfessionalCode = pobjProfessional.v_ProfessionalCode;
                            objProfessional.v_ProfessionalInformation = pobjProfessional.v_ProfessionalInformation;
                            objProfessional.i_UpdateUserId = userId;
                            objProfessional.d_UpdateDate = DateTime.Now;
                            dbContext.SaveChanges();
                        }                     
                    }

                    // Validar existencia de UserName en la BD
                    if (pobjSystemUser != null)
                    {
                        if (pobjSystemUser.i_SystemUserId == -1)
                        {
                            OperationResult objOperationResult7 = new OperationResult();
                            var _recordCount2 = new SecurityDal().GetSystemUserCount(pobjSystemUser.v_UserName);

                            if (_recordCount2 != 0) throw new Exception("El nombre de usuario  <strong>" + pobjSystemUser.v_UserName + "</strong> ya se encuentra registrado.<br> Por favor ingrese otro nombre de usuario.");

                        }

                    }
                    #endregion

                    // Grabar Usuario
                    if (pobjSystemUser != null)
                    {
                        if (pobjSystemUser.i_SystemUserId == -1)//-1 es nuevo
                        {
                            pobjSystemUser.v_PersonId = pobjPerson.v_PersonId;
                            pobjSystemUser.i_SystemUserTypeId = (int)SystemUserTypeId.External;
                            pobjSystemUser.i_RolVentaId = -1;
                            pobjSystemUser.v_SystemUserByOrganizationId = ListProtocolSystemUser != null ? OrganizationDal.GetOrganizationIdByProtocolId(ListProtocolSystemUser[0].v_ProtocolId) : "";
                            systemUserId = new SecurityDal().AddSystemUSer(pobjSystemUser, userId, nodeId);
                            if (systemUserId == -1) throw new Exception("Sucedió un error al guardar el usuario, por favor actualice la pagina y vuelva a intentar");
                        }
                        else
                        {//actualiza
                            var objUser = dbContext.SystemUser.Where(x => x.i_SystemUserId == pobjSystemUser.i_SystemUserId).FirstOrDefault();
                            objUser.v_UserName = pobjSystemUser.v_UserName;
                            objUser.v_Password = pobjSystemUser.v_Password;
                            objUser.d_ExpireDate = pobjSystemUser.d_ExpireDate;
                            objUser.i_UpdateUserId = userId;
                            objUser.d_UpdateDate = DateTime.Now;
                            systemUserId = objUser.i_SystemUserId;
                            dbContext.SaveChanges();
                        }

                    }

                    #region GRABA ProtocolSystemUser

                    if (ListProtocolSystemUser != null)
                    {
                        if (pobjSystemUser.i_SystemUserId == -1)//-1 es nuevo
                        {
                            bool resultProt = new ProtocolSystemUserDal().AddProtocolSystemUser(ListProtocolSystemUser, systemUserId, userId, nodeId);
                            if (!resultProt) throw new Exception("Sucedió un error al guardar los protocolos del usuario, por favor actualice la pagina y vuelva a intentar");
                        }
                        else{//actualiza
                            bool deletedProt = ProtocolSystemUserDal.DeletedProtocolSystemUser(systemUserId, userId);
                            if (!deletedProt) throw new Exception("Sucedió un error al actualizar los protocolos del usuario, por favor actualice la pagina y vuelva a intentar");
                            bool resultProt = new ProtocolSystemUserDal().AddProtocolSystemUser(ListProtocolSystemUser, systemUserId, userId, nodeId);
                            if (!resultProt) throw new Exception("Sucedió un error al actualizar los protocolos del usuario, por favor actualice la pagina y vuelva a intentar");
                        }
                        
                    }

                    #endregion

                    msg.Error = false;
                    msg.Status = (int)HttpStatusCode.Created;
                    msg.Message = "Se guardó correctamente";

                    ts.Complete();
                }
                
                return msg;

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Status = (int)HttpStatusCode.BadRequest;
                msg.Message = ex.Message;
                return msg;

            }
        }

        public int GetPersonCount(string docNumber)
        {
            //mon.IsActive = true;
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var query = (from a in dbContext.Person where a.v_DocNumber == docNumber && a.i_IsDeleted == 0 select a).ToList();


                int intResult = query.Count();

                return intResult;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public bool AddProfessional(ProfessionalBE pobjDtoEntity, int userId, int nodeId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();


                pobjDtoEntity.d_InsertDate = DateTime.Now;
                pobjDtoEntity.i_InsertUserId = userId;
                pobjDtoEntity.i_IsDeleted = 0;

                dbContext.Professional.Add(pobjDtoEntity);
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

    }
}
