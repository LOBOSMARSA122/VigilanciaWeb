using BE.Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        #endregion
    }
}
