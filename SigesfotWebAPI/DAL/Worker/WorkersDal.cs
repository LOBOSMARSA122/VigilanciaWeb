using System;
using BE.Common;
using BE.MedicalAssistance;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using BE.Subscription;
using BE.Worker;

namespace DAL
{
    public class WorkersDal
    {
        private readonly DatabaseContext _ctx = new DatabaseContext();

        public List<ServiceWorkerBE> WorkesWhitServices(out int totalRecords, BoardPatient data)
        {
                int groupDocTypeId = (int)Enumeratores.DataHierarchy.TypeDoc;
                int genderId = (int)Enumeratores.Parameters.Gender;
                int skip = (data.Index - 1) * data.Take;

                string filterPacient = string.IsNullOrWhiteSpace(data.Patient) ? "" : data.Patient;
            
                //AMC____
                var protocols = (from a in _ctx.ProtocolSystemUser
                                 join b in _ctx.Protocol on a.v_ProtocolId equals b.v_ProtocolId
                                where (data.SystemUserId == -1 ||a.i_SystemUserId == data.SystemUserId)
                                     &&(b.v_CustomerOrganizationId == data.SystemUserByOrganizationId || b.v_EmployerOrganizationId == data.SystemUserByOrganizationId || b.v_WorkingOrganizationId == data.SystemUserByOrganizationId)
                                select a.v_ProtocolId).ToList();

                protocols = protocols.GroupBy(g => g).Select(s => s.First()).ToList();

                var services = (from a in _ctx.Service
                               join b in _ctx.Person on a.v_PersonId equals b.v_PersonId
                               join c in _ctx.DataHierarchy on new { a = b.i_DocTypeId.Value, b = groupDocTypeId } equals new { a = c.i_ItemId, b = c.i_GroupId }
                               join d in _ctx.SystemParameter on new { a = b.i_SexTypeId.Value, b = genderId } equals new { a = d.i_ParameterId, b = d.i_GroupId }
                               join e in _ctx.Protocol on a.v_ProtocolId equals e.v_ProtocolId
                               join f in _ctx.Organization on e.v_EmployerOrganizationId equals f.v_OrganizationId
                               join g in _ctx.Vigilancia on a.v_PersonId equals  g.v_PersonId into gJoin
                               from g in gJoin.DefaultIfEmpty()
                               join h in _ctx.PlanVigilancia on g.v_PlanVigilanciaId equals h.v_PlanVigilanciaId into hJoin
                               from h in hJoin.DefaultIfEmpty()
                                where protocols.Contains(a.v_ProtocolId)
                                    && ((b.v_FirstName + " " + b.v_FirstLastName + " " + b.v_SecondLastName).Contains(filterPacient) 
                                    || b.v_DocNumber.Contains(filterPacient))
                                    && (data.PlanVigilanciaId == "-1"  || g.v_PlanVigilanciaId == data.PlanVigilanciaId)
                                    &&(g.i_StateVigilanciaId != (int)Enumeratores.StateVigilancia.Finalizado)
                                select new Patients
                               {
                                   ServiceId =  a.v_ServiceId,
                                   PatientId = a.v_PersonId,
                                   PatientFullName = b.v_FirstName + " " + b.v_FirstLastName + " " + b.v_SecondLastName,
                                   DocumentType = c.v_Value1,
                                   DocumentNumber = b.v_DocNumber,
                                   Occupation = b.v_CurrentOccupation,
                                   Birthdate = b.d_Birthdate,
                                   Gender = d.v_Value1,
                                   StatusVigilanciaId = a.i_StatusVigilanciaId.Value,
                                   OrganizationLocation = f.v_Name,
                                   IsRevisedHistoryId = a.i_IsRevisedHistoryId.Value,
                                   ProtocolId = e.v_ProtocolId,
                                   ServiceDate = a.d_ServiceDate,
                                   PlanVigilancia = h.v_Name,
                                   VigilanciaId = g.v_VigilanciaId
                                }).ToList();

                //.. lógica para filtar por UsuarioEmpresa

                var workers = services.GroupBy(g => g.PatientId).Select(s => s.First()).ToList();
                var list = new List<ServiceWorkerBE>();
                totalRecords = workers.Count;

                var patientses = new List<Patients>();
                if (data.Take > 0)
                    patientses = workers.Skip(skip).Take(data.Take).ToList();


                foreach (var worker in patientses)
                {
                    var oServiceWorkerBE = new ServiceWorkerBE();

                    oServiceWorkerBE.PatientId = worker.PatientId;
                    oServiceWorkerBE.PatientFullName = worker.PatientFullName;
                    oServiceWorkerBE.DocumentType = worker.DocumentType;
                    oServiceWorkerBE.DocumentNumber = worker.DocumentNumber;
                    oServiceWorkerBE.Occupation = worker.Occupation;
                    oServiceWorkerBE.Birthdate = worker.Birthdate;
                    oServiceWorkerBE.Gender = worker.Gender;
                    oServiceWorkerBE.PlanVigilancia = worker.PlanVigilancia;
                    oServiceWorkerBE.VigilanciaId = worker.VigilanciaId;
                    oServiceWorkerBE.Age = Common.Utils.GetAge(worker.Birthdate.Value);
                    var servicesByWorker = services.FindAll(p => p.PatientId == worker.PatientId).ToList();

                    var oServices = new List<ServiceWorker>();
                    foreach (var service in servicesByWorker)
                    {
                        var oServiceWorker = new ServiceWorker();
                        oServiceWorker.ServiceId = service.ServiceId;
                        oServiceWorker.ServiceDate = service.ServiceDate;
                        oServiceWorker.ProtocolId = service.ProtocolId;
                        oServiceWorker.IsRevisedHistoryId = service.IsRevisedHistoryId;
                        oServiceWorker.StatusVigilanciaId = service.StatusVigilanciaId;
                        oServiceWorker.ListDiseasesService = (from a in _ctx.DiagnosticRepository
                                                            where a.v_ServiceId == service.ServiceId
                                                                  && a.i_FinalQualificationId == (int)Enumeratores.FinalQualification.Definitivo
                                                            select new DiseasesService
                                                            {
                                                                ServiceId = a.v_ServiceId,
                                                                DiseasesId = a.v_DiseasesId
                                                            }).ToList();
                        oServices.Add(oServiceWorker);
                    }

                    oServiceWorkerBE.Services = oServices;

                    list.Add(oServiceWorkerBE);
                }

              
                //if (data.Take > 0)
                //    list = list.Skip(skip).Take(data.Take).ToList();

                return list;
            
        }

        public WorkerPwa WorkerInformationPwa(string personId)
        {
            var docTypeId = (int)Enumeratores.DataHierarchy.TypeDoc;
            var martialStatusId = (int)Enumeratores.Parameters.MartialStatus;
            var levelOfId = (int)Enumeratores.DataHierarchy.LevelOf;
            var result = (from a in _ctx.Person
                        join b in _ctx.SystemParameter on new { a = a.i_DocTypeId.Value, b = docTypeId } equals new { a = b.i_ParameterId, b = b.i_GroupId }
                        join c in _ctx.SystemParameter on new { a = a.i_MaritalStatusId.Value, b = martialStatusId } equals new { a = c.i_ParameterId, b = c.i_GroupId }
                        join d in _ctx.SystemParameter on new { a = a.i_LevelOfId.Value, b = martialStatusId } equals new { a = d.i_ParameterId, b = d.i_GroupId }

                          where a.v_PersonId == personId
                select new WorkerPwa
                {
                    PersonId = a.v_PersonId,
                    DocTypeId = a.i_DocTypeId.Value, 
                    DocTypeName = b.v_Value1,
                    DocNumber = a.v_DocNumber,
                    BirthDate = a.d_Birthdate.Value,
                    //Edad = a.d_Birthdate == null ? 0 : Common.Utils.GetAge(a.d_Birthdate.Value),
                    MartialStatusId =  a.i_MaritalStatusId.Value, 
                    MartialStatus = c.v_Value1,
                    LevelOfId = a.i_LevelOfId.Value,
                    LevelOf = d.v_Value1,
                    TelephoneNumber = a.v_TelephoneNumber, 
                    AdressLocation = a.v_AdressLocation,
                    CurrentOcupacion = a.v_CurrentOccupation,
                    Email = a.v_Mail,
                    CountNewNotifications = (from subA in _ctx.Notification where subA.v_PersonId == a.v_PersonId && subA.i_IsRead == (int)Enumeratores.SiNo.No select subA).Count()
                }).FirstOrDefault();

            return result;
        }

        public bool UpdateWorker(WorkerPwa oWorkerPwa)
        {
            using (var ctx = new DatabaseContext())
            {
                var objEntity = (from a in ctx.Person where a.v_PersonId == oWorkerPwa.PersonId select a).FirstOrDefault();

                if (objEntity == null) return false;
                
                objEntity.v_TelephoneNumber = oWorkerPwa.TelephoneNumber;
                objEntity.v_Mail = oWorkerPwa.Email;

                ctx.SaveChanges();
            }
            return true;
        }
    }
}
