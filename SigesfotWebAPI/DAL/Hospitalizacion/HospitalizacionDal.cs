using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace DAL.Hospitalizacion
{
    public class HospitalizacionDal
    {
        public bool AddHospitalizacion(string personId, string serviceId, int nodeId, int userId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var hospitalizacionId = new Common.Utils().GetPrimaryKey(nodeId, 350, "HP");
                HospitalizacionBE _HospitalizacionBE = new HospitalizacionBE();

                _HospitalizacionBE.v_HopitalizacionId = hospitalizacionId;
                _HospitalizacionBE.v_PersonId = personId;
                _HospitalizacionBE.d_FechaIngreso = DateTime.Now;
                _HospitalizacionBE.i_IsDeleted = (int)SiNo.No;
                _HospitalizacionBE.i_InsertUserId = userId;
                _HospitalizacionBE.d_InsertDate = DateTime.Now;
                cnx.Hospitalizacion.Add(_HospitalizacionBE);
                cnx.SaveChanges();

                bool result = AddHospitalizacionService(hospitalizacionId, serviceId, nodeId, userId);

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddHospitalizacionService(string hospitalizacionId, string serviceId, int nodeId, int userId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var hospitalizacionServiceId = new Common.Utils().GetPrimaryKey(nodeId, 351, "HS");
                HospitalizacionServiceBE _HospitalizacionServiceBE = new HospitalizacionServiceBE();
                _HospitalizacionServiceBE.v_HospitalizacionServiceId = hospitalizacionServiceId;
                _HospitalizacionServiceBE.v_HopitalizacionId = hospitalizacionId;
                _HospitalizacionServiceBE.v_ServiceId = serviceId;
                _HospitalizacionServiceBE.i_IsDeleted = (int)SiNo.No;
                _HospitalizacionServiceBE.i_InsertUserId = userId;
                _HospitalizacionServiceBE.d_InsertDate = DateTime.Now;

                cnx.HospitalizacionService.Add(_HospitalizacionServiceBE);
                
                return cnx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string AddHospitalizacion(string serviceId, int nodeId, int userId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();

                var objCalendar = (from cal in cnx.Calendar
                                   where cal.v_ServiceId == serviceId && cal.i_IsDeleted == 0
                                   select cal).FirstOrDefault();

                
                var hospitalizacionId = new Common.Utils().GetPrimaryKey(nodeId, 350, "HP");

                HospitalizacionBE objHospitalizacionDto = new HospitalizacionBE();
                objHospitalizacionDto.v_HopitalizacionId = hospitalizacionId;
                objHospitalizacionDto.v_PersonId = objCalendar.v_PersonId;
                objHospitalizacionDto.i_InsertUserId = userId;
                objHospitalizacionDto.d_FechaIngreso = objCalendar.d_EntryTimeCM.Value;
                objHospitalizacionDto.d_InsertDate = DateTime.Now;
                objHospitalizacionDto.i_IsDeleted = (int)SiNo.No;


                cnx.Hospitalizacion.Add(objHospitalizacionDto);
                cnx.SaveChanges();

                return hospitalizacionId;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
