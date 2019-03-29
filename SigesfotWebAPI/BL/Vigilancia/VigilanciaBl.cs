using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Vigilancia;
using DAL.Vigilancia;

namespace BL.Vigilancia
{
    public class VigilanciaBl
    {
        public string SendVigilancia(VigilanciaDto oVigilanciaDto, int nodeId, int systemUserId)
        {
            oVigilanciaDto.i_WasNotifiedId = SendNotification(oVigilanciaDto.v_PersonId);
            oVigilanciaDto.d_StartDate = DateTime.Now;  
            oVigilanciaDto.i_DoctorRespondibleId = systemUserId;
            oVigilanciaDto.i_StateVigilanciaId = (int) Enumeratores.StateVigilancia.Iniciado;
            
            //Verificar si ya está en un plan igual e iniciado
            if (VerifyPlanStarted(oVigilanciaDto)) return @"El trabajador ya tiene un plan iniciado";

            return new VigilanciaDal().AddVigilancia(oVigilanciaDto, nodeId, systemUserId);
        }

        private bool VerifyPlanStarted(VigilanciaDto oVigilanciaDto)
        {
            return new VigilanciaDal().VerifyPlanStarted(oVigilanciaDto.v_PersonId, oVigilanciaDto.v_PlanVigilanciaId);
        }

        public bool FinishVigilancia(string vigilanciaId, int systemUserId)
        {
            return new VigilanciaDal().FinishVigilancia(vigilanciaId, systemUserId);
        }

        private int SendNotification(string personId)
        {
            return (int) Enumeratores.SiNo.Si;
        }
    }
}