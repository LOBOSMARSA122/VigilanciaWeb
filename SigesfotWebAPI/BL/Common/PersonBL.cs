using BE.Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common
{
    public class PersonBL
    {
        private DatabaseContext ctx = new DatabaseContext();

        #region CRUD
        public PersonDto GetPerson(string personId)
        {
            try
            {
                var objEntity = (from a in ctx.Person
                                 where a.v_PersonId == personId
                                 select a).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {               
                return null;
            }
        }

        public List<PersonDto> GetAllPerson()
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;
                var objEntity = (from a in ctx.Person
                                 where a.i_IsDeleted == isDeleted
                                 select new PersonDto()
                                 {
                                     v_PersonId = a.v_PersonId,
                                     v_FirstName = a.v_FirstName,
                                     v_FirstLastName = a.v_FirstLastName,
                                     v_SecondLastName = a.v_SecondLastName,
                                     i_DocTypeId = a.i_DocTypeId,
                                     v_DocNumber = a.v_DocNumber,
                                     //d_Birthdate = a.d_Birthdate,
                                     //v_BirthPlace = a.v_BirthPlace,
                                     //i_SexTypeId = a.i_SexTypeId,
                                     //i_MaritalStatusId = a.i_MaritalStatusId,
                                     //i_LevelOfId = a.i_LevelOfId,
                                     v_TelephoneNumber = a.v_TelephoneNumber,
                                     //v_AdressLocation = a.v_AdressLocation,
                                     //v_GeografyLocationId = a.v_GeografyLocationId,
                                     //v_ContactName = a.v_ContactName,
                                     //v_EmergencyPhone = a.v_EmergencyPhone,
                                     //b_PersonImage = a.b_PersonImage,
                                     //v_Mail = a.v_Mail,
                                     //i_BloodGroupId = a.i_BloodGroupId,
                                     //i_BloodFactorId = a.i_BloodFactorId,
                                     //b_FingerPrintTemplate = a.b_FingerPrintTemplate,
                                     //b_RubricImage = a.b_RubricImage,
                                     //b_FingerPrintImage = a.b_FingerPrintImage,
                                     //t_RubricImageText = a.t_RubricImageText,
                                     //v_CurrentOccupation = a.v_CurrentOccupation,
                                     //i_DepartmentId = a.i_DepartmentId,
                                     //i_ProvinceId = a.i_ProvinceId,
                                     //i_DistrictId = a.i_DistrictId,
                                     //i_ResidenceInWorkplaceId = a.i_ResidenceInWorkplaceId,
                                     //v_ResidenceTimeInWorkplace = a.v_ResidenceTimeInWorkplace,
                                     //i_TypeOfInsuranceId = a.i_TypeOfInsuranceId,
                                     //i_NumberLivingChildren = a.i_NumberLivingChildren,
                                     //i_NumberDependentChildren = a.i_NumberDependentChildren,
                                     //i_OccupationTypeId = a.i_OccupationTypeId,
                                     //v_OwnerName = a.v_OwnerName,
                                     //i_NumberLiveChildren = a.i_NumberLiveChildren,
                                     //i_NumberDeadChildren = a.i_NumberDeadChildren,
                                     i_IsDeleted = a.i_IsDeleted,
                                     i_InsertUserId = a.i_InsertUserId,
                                     d_InsertDate = a.d_InsertDate,
                                     i_UpdateUserId = a.i_UpdateUserId,
                                     d_UpdateDate = a.d_UpdateDate
                                 }).ToList();

                return objEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public string AddPerson(PersonDto person, int systemUserId)
        {
            try
            {
                PersonDto oPersonBE = new PersonDto()
                {
                    v_PersonId = new DAL.Common.Utils().GetPrimaryKey(1, 8, "PP"),                   
                    v_FirstName = person.v_FirstName,
                    v_FirstLastName = person.v_FirstLastName,
                    v_SecondLastName = person.v_SecondLastName,
                    i_DocTypeId = person.i_DocTypeId,
                    v_DocNumber = person.v_DocNumber,
                    d_Birthdate = person.d_Birthdate,
                    v_BirthPlace = person.v_BirthPlace,
                    i_SexTypeId = person.i_SexTypeId,
                    i_MaritalStatusId = person.i_MaritalStatusId,
                    i_LevelOfId = person.i_LevelOfId,
                    v_TelephoneNumber = person.v_TelephoneNumber,
                    v_AdressLocation = person.v_AdressLocation,
                    v_GeografyLocationId = person.v_GeografyLocationId,
                    v_ContactName = person.v_ContactName,
                    v_EmergencyPhone = person.v_EmergencyPhone,
                    b_PersonImage = person.b_PersonImage,
                    v_Mail = person.v_Mail,
                    i_BloodGroupId = person.i_BloodGroupId,
                    i_BloodFactorId = person.i_BloodFactorId,
                    b_FingerPrintTemplate = person.b_FingerPrintTemplate,
                    b_RubricImage = person.b_RubricImage,
                    b_FingerPrintImage = person.b_FingerPrintImage,
                    t_RubricImageText = person.t_RubricImageText,
                    v_CurrentOccupation = person.v_CurrentOccupation,
                    i_DepartmentId = person.i_DepartmentId,
                    i_ProvinceId = person.i_ProvinceId,
                    i_DistrictId = person.i_DistrictId,
                    i_ResidenceInWorkplaceId = person.i_ResidenceInWorkplaceId,
                    v_ResidenceTimeInWorkplace = person.v_ResidenceTimeInWorkplace,
                    i_TypeOfInsuranceId = person.i_TypeOfInsuranceId,
                    i_NumberLivingChildren = person.i_NumberLivingChildren,
                    i_NumberDependentChildren = person.i_NumberDependentChildren,
                    //i_OccupationTypeId = person.i_OccupationTypeId,
                    v_OwnerName = person.v_OwnerName,
                    //i_NumberLiveChildren = person.i_NumberLiveChildren,
                    i_NumberDeadChildren = person.i_NumberDeadChildren,

                    //Auditoria
                    i_IsDeleted = (int)Enumeratores.SiNo.No,
                    d_InsertDate = DateTime.UtcNow,
                    i_InsertUserId = systemUserId
                };

                ctx.Person.Add(oPersonBE);

                int rows = ctx.SaveChanges();

                if (rows > 0)
                return oPersonBE.v_PersonId;

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public bool UpdatePerson(PersonDto person, int systemUserId)
        {
            try
            {
                var oPerson = (from a in ctx.Person where a.v_PersonId == person.v_PersonId select a).FirstOrDefault();

                if (oPerson == null)
                    return false;

                oPerson.v_FirstName = person.v_FirstName;
                oPerson.v_FirstLastName = person.v_FirstLastName;
                oPerson.v_SecondLastName = person.v_SecondLastName;
                oPerson.i_DocTypeId = person.i_DocTypeId;
                oPerson.v_DocNumber = person.v_DocNumber;
                //oPerson.Birthdate = person.Birthdate;
                //oPerson.BirthPlace = person.BirthPlace;
                //oPerson.SexTypeId = person.SexTypeId;
                //oPerson.MaritalStatusId = person.MaritalStatusId;
                //oPerson.LevelOfId = person.LevelOfId;
                oPerson.v_TelephoneNumber = person.v_TelephoneNumber;
                //oPerson.AdressLocation = person.AdressLocation;
                //oPerson.GeografyLocationId = person.GeografyLocationId;
                //oPerson.ContactName = person.ContactName;
                //oPerson.EmergencyPhone = person.EmergencyPhone;
                //oPerson.PersonImage = person.PersonImage;
                //oPerson.Mail = person.Mail;
                //oPerson.BloodGroupId = person.BloodGroupId;
                //oPerson.BloodFactorId = person.BloodFactorId;
                //oPerson.FingerPrintTemplate = person.FingerPrintTemplate;
                //oPerson.RubricImage = person.RubricImage;
                //oPerson.FingerPrintImage = person.FingerPrintImage;
                //oPerson.RubricImageText = person.RubricImageText;
                //oPerson.CurrentOccupation = person.CurrentOccupation;
                //oPerson.DepartmentId = person.DepartmentId;
                //oPerson.ProvinceId = person.ProvinceId;
                //oPerson.DistrictId = person.DistrictId;
                //oPerson.ResidenceInWorkplaceId = person.ResidenceInWorkplaceId;
                //oPerson.ResidenceTimeInWorkplace = person.ResidenceTimeInWorkplace;
                //oPerson.TypeOfInsuranceId = person.TypeOfInsuranceId;
                //oPerson.NumberLivingChildren = person.NumberLivingChildren;
                //oPerson.NumberDependentChildren = person.NumberDependentChildren;
                //oPerson.OccupationTypeId = person.OccupationTypeId;
                //oPerson.OwnerName = person.OwnerName;
                //oPerson.NumberLiveChildren = person.NumberLiveChildren;
                //oPerson.NumberDeadChildren = person.NumberDeadChildren;

                //Auditoria
                oPerson.d_UpdateDate = DateTime.UtcNow;
                oPerson.i_UpdateUserId = systemUserId;

                int rows = ctx.SaveChanges();

                if( rows > 0)
                return true;

                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

            
        }

        public bool DeletePerson(string personId, int systemUserId)
        {
            //try
            //{
            //    var oPerson = (from a in ctx.Person where a.PersonId == personId select a).FirstOrDefault();

            //    if (oPerson == null)
            //        return false;

            //    oPerson.UpdateUserId = systemUserId;
            //    oPerson.UpdateDate = DateTime.UtcNow;
            //    oPerson.IsDeleted = (int)Enumeratores.SiNo.Si;

            //    int rows = ctx.SaveChanges();

            //    return rows > 0;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

            return true;
        }
        #endregion

        #region Bussines Logic

      
        #endregion

    }
}
