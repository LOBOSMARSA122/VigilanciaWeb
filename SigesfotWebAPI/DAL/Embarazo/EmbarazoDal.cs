using BE.Common;
using BE.Embarazo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Embarazo
{
    public class EmbarazoDal
    {
        public bool AddEmbarazo(EmbarazoBE objEmbarazo, int nodeId, int userId)
        {
            string NewId0 = null;
            try
            {
                NewId0 = new Common.Utils().GetPrimaryKey(nodeId, 357, "EM");
                DatabaseContext dbContext = new DatabaseContext();

                objEmbarazo.v_EmbarazoId = NewId0;
                objEmbarazo.d_InsertDate = DateTime.Now;
                objEmbarazo.i_InsertUserId = userId;
                objEmbarazo.i_IsDeleted = 0;

                dbContext.Embarazo.Add(objEmbarazo);

                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public List<EmbarazoCustom> GetEmbarazo(string pstrPersonId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var query = from A in dbContext.Embarazo
                            join B in dbContext.Person on A.v_PersonId equals B.v_PersonId
                            where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

                            select new EmbarazoCustom
                            {
                                EmbarazoId = A.v_EmbarazoId,
                                PersonId = A.v_PersonId,
                                Anio = A.v_Anio,
                                Cpn = A.v_Cpn,
                                Complicacion = A.v_Complicacion,
                                Parto = A.v_Parto,
                                PesoRn = A.v_PesoRn,
                                Puerpio = A.v_Puerpio
                            };



                List<EmbarazoCustom> objData = query.ToList();
                return objData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
