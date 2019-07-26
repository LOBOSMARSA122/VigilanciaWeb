using BE.Common;
using BE.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Security
{
    public class SecurityDal
    {

        public int AddSystemUSer(SystemUserDto pobjDtoEntity, int userId, int nodeId)
        {
            //mon.IsActive = true;
            int SecuentialId = 0;
            SecuentialId = new Common.Utils().GetNextSecuentialId(nodeId, 9);
            var conn = ConfigurationManager.ConnectionStrings["BDSigesoft"].ConnectionString;
            string query = " INSERT INTO systemuser( i_SystemUserId, v_PersonId, v_UserName, v_Password, d_ExpireDate, v_SystemUserByOrganizationId, i_IsDeleted, i_InsertUserId, d_InsertDate, i_SystemUserTypeId)" +
                            " VALUES( " + SecuentialId + ", '"+ pobjDtoEntity.v_PersonId + "', '" + pobjDtoEntity.v_UserName + "', '" + pobjDtoEntity.v_Password + "', '" + pobjDtoEntity.d_ExpireDate + "', '" + pobjDtoEntity.v_SystemUserByOrganizationId + "'" + 
                            ", 0, " + userId + ", '" + DateTime.Now + "', "+ pobjDtoEntity.i_SystemUserTypeId + ")";


            SqlConnection sqlConnection = new SqlConnection(conn);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            sqlConnection.Open();

            try
            {
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return SecuentialId;
            }
            catch (Exception exc)
            {
                sqlConnection.Close();
                return -1;
                
            }

            //try
            //{
            //    DatabaseContext dbContext = new DatabaseContext();

            //    pobjDtoEntity.d_InsertDate = DateTime.Now;
            //    pobjDtoEntity.i_InsertUserId = userId;
            //    pobjDtoEntity.i_IsDeleted = 0;
            //    pobjDtoEntity.i_SystemUserId = SecuentialId;
            //    pobjDtoEntity.i_RolVentaId = -1;
            //    dbContext.SystemUser.Add(pobjDtoEntity);
            //    dbContext.SaveChanges();

            //    return SecuentialId;
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

        }

        public int GetSystemUserCount(string userName)
        {
            //mon.IsActive = true;
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var query = (from a in dbContext.SystemUser where a.v_UserName == userName && a.i_IsDeleted == 0 select a).ToList();
                int intResult = query.Count();

                return intResult;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string Encrypt(string pData)
        {
            UnicodeEncoding parser = new UnicodeEncoding();
            byte[] _original = parser.GetBytes(pData);
            MD5CryptoServiceProvider Hash = new MD5CryptoServiceProvider();
            byte[] _encrypt = Hash.ComputeHash(_original);
            return Convert.ToBase64String(_encrypt);
        }


        public SystemUserCustom GetSystemUserAndProfesional(int pintSystemUserId)
        {
            //mon.IsActive = true;
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objEntity = (from a in dbContext.SystemUser
                                 join b in dbContext.Professional on a.v_PersonId equals b.v_PersonId
                                 join c in dbContext.Person on a.v_PersonId equals c.v_PersonId
                                 join K in dbContext.SystemParameter on new { a = b.i_ProfessionId.Value, b = 115 } equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                                 from K in K_join.DefaultIfEmpty()
                                 where a.i_SystemUserId == pintSystemUserId
                                 select new SystemUserCustom
                                 {
                                     v_PersonName = c.v_FirstName + " " + c.v_FirstLastName + " " + c.v_SecondLastName,
                                     Profesion = K.v_Value1
                                 }

                                ).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
