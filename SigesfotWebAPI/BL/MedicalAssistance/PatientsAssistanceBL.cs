
using BE.Common;
using BE.MedicalAssistance;
using BE.Worker;
using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BE.Vigilancia;
using DAL.Vigilancia;
using static BE.Common.Enumeratores;

namespace BL.MedicalAssistance
{
    public class PatientsAssistanceBL
    {
        private DatabaseContext ctx = new DatabaseContext();

        public BoardPatient GetPendingReview_Old(BoardPatient data)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;
                var count = (from a in ctx.Service
                             where a.i_IsDeleted == isDeleted && a.i_MasterServiceId.Value == (int)masterService.Ocupational && a.i_IsRevisedHistoryId != 1
                             select new Patients
                             {
                                 MasterServiceId = a.i_MasterServiceId.Value,
                             }).ToList();
                data.List = count;
                return data;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int GetPendingReview(int systemUserId, string organizationId)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;
                
                //var protocols = (from a in ctx.ProtocolSystemUser where a.i_SystemUserId == systemUserId select a.v_ProtocolId).ToList();

                var protocols = (from a in ctx.ProtocolSystemUser
                    join b in ctx.Protocol on a.v_ProtocolId equals b.v_ProtocolId
                    where (systemUserId == -1 || a.i_SystemUserId == systemUserId)
                          && (b.v_CustomerOrganizationId == organizationId || b.v_EmployerOrganizationId == organizationId || b.v_WorkingOrganizationId == organizationId)
                    select a.v_ProtocolId).ToList();
                protocols = protocols.GroupBy(g => g).Select(s => s.First()).ToList();

                var services = (from a in ctx.Service
                    join b in ctx.Person on a.v_PersonId equals b.v_PersonId
                    where protocols.Contains(a.v_ProtocolId)
                        && a.i_MasterServiceId.Value == (int)masterService.Ocupational
                        && a.i_IsRevisedHistoryId != 1
                        select new
                        {
                            MasterServiceId = a.i_MasterServiceId.Value,
                            personId = a.v_PersonId
                        }).ToList();

                var servicesByReview = services.GroupBy(g => g.personId).Select(s => s.First()).ToList();
                return servicesByReview.Count();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Schedule> GetSchedule()
        {
            try
            {
                int masterServiceId = (int)Enumeratores.masterService.AtxMedicaParticular;
                var isDeleted = (int)Enumeratores.SiNo.No;
                var list = (from a in ctx.Service
                            join b in ctx.Person on a.v_PersonId equals b.v_PersonId
                            where a.i_IsDeleted == isDeleted
                                     && a.i_MasterServiceId == masterServiceId
                            select new Schedule
                            {
                                PacientId = a.v_PersonId,
                                Pacient = b.v_FirstName + " " + b.v_FirstLastName + " " + b.v_SecondLastName,
                                ServiceDate = a.d_ServiceDate.Value
                            }).Take(2).ToList();

                return list;
                    
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TopDiagnostic> TopDiagnostic(int systemUserId, string organizationId)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;

                //var protocols = (from a in ctx.ProtocolSystemUser where a.i_SystemUserId == systemUserId select a.v_ProtocolId).ToList();
                var protocols = (from a in ctx.ProtocolSystemUser
                    join b in ctx.Protocol on a.v_ProtocolId equals b.v_ProtocolId
                    where (systemUserId == -1 || a.i_SystemUserId == systemUserId)
                          && (b.v_CustomerOrganizationId == organizationId || b.v_EmployerOrganizationId == organizationId || b.v_WorkingOrganizationId == organizationId)
                    select a.v_ProtocolId).ToList();

                protocols = protocols.GroupBy(g => g).Select(s => s.First()).ToList();

                var list = (from a in ctx.Service
                            join b in ctx.DiagnosticRepository on a.v_ServiceId equals b.v_ServiceId
                            join c in ctx.Diseases on b.v_DiseasesId equals c.v_DiseasesId
                            where protocols.Contains(a.v_ProtocolId)
                            && a.i_MasterServiceId.Value == (int)masterService.AtxMedicaParticular
                            && a.i_IsDeleted == (int)SiNo.No && b.i_IsDeleted == (int)SiNo.No
                            //&& (b.i_FinalQualificationId == (int)FinalQualification.Definitivo || b.i_FinalQualificationId == (int)FinalQualification.Presuntivo)
                            select new
                            {
                                DiagnosticId = b.v_DiseasesId,
                                Diagnostic = c.v_Name,
                                MasterServiceId = a.i_MasterServiceId,
                                personId = a.v_PersonId,
                                serviceId = a.v_ServiceId
                            }).ToList();


                var countWorkers = list.GroupBy(g => new { g.serviceId, g.personId }).Select(s => s.First()).ToList();
                var group = list
                            .GroupBy(n => n.Diagnostic)
                            .Select(n => new TopDiagnostic
                            {
                                count = countWorkers.Count,
                                name = n.Key,
                                y = n.Count()
                            }).OrderByDescending(n => n.y).Take(10);


                return group.ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TopDiagnostic> TopDiagnosticOcupational(int systemUserId, string organizationId)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;

                //var protocols = (from a in ctx.ProtocolSystemUser where a.i_SystemUserId == systemUserId select a.v_ProtocolId).ToList();

                var protocols = (from a in ctx.ProtocolSystemUser
                    join b in ctx.Protocol on a.v_ProtocolId equals b.v_ProtocolId
                    where (systemUserId == -1 || a.i_SystemUserId == systemUserId)
                          && (b.v_CustomerOrganizationId == organizationId || b.v_EmployerOrganizationId == organizationId || b.v_WorkingOrganizationId == organizationId)
                    select a.v_ProtocolId).ToList();

                protocols = protocols.GroupBy(g => g).Select(s => s.First()).ToList();

                var list = (from a in ctx.Service
                            join b in ctx.DiagnosticRepository on a.v_ServiceId equals b.v_ServiceId
                            join c in ctx.Diseases on b.v_DiseasesId equals c.v_DiseasesId
                            where protocols.Contains(a.v_ProtocolId)
                            && a.i_MasterServiceId.Value == (int)masterService.Ocupational
                            && a.i_IsDeleted == (int)SiNo.No && b.i_IsDeleted == (int)SiNo.No
                            && (b.i_FinalQualificationId == (int)FinalQualification.Definitivo || b.i_FinalQualificationId == (int)FinalQualification.Presuntivo)
                            select new
                            {
                                DiagnosticId = b.v_DiseasesId,
                                Diagnostic = c.v_Name,
                                MasterServiceId = a.i_MasterServiceId,
                                    personId = a.v_PersonId,
                                    serviceId = a.v_ServiceId
                                }).ToList();


                var countWorkers = list.GroupBy(g => new{ g.serviceId, g.personId}).Select(s => s.First()).ToList(); 
                //var obj =  countWorkers.GroupBy(g => g.personId).Select(s => s.First()).ToList();
                var group = list
                            .GroupBy(n => n.Diagnostic)
                            .Select(n => new TopDiagnostic
                            {
                                count = countWorkers.Count,
                                name = n.Key,
                                y = n.Count()
                            }).OrderByDescending(n => n.y).Take(10);


                return group.ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Indicators IndicatorByPacient(string patientId)
        {
            try
            {
                var serviceData = (from E in ctx.Person
                                   join A in ctx.Service on E.v_PersonId equals A.v_PersonId into A_join
                                   from A in A_join.DefaultIfEmpty()
                                   //join F in ctx.OrganizationPerson on E.v_PersonId equals F.v_PersonId 
                                   //join G in ctx.Organization on F.v_OrganizationId equals G.v_OrganizationId
                                   where E.v_PersonId == patientId && A.i_IsDeleted == 0
                                   select new
                                   {
                                       FullName = E.v_FirstName + " " + E.v_FirstLastName + " " + E.v_SecondLastName,
                                       Puesto = E.v_CurrentOccupation,
                                       Empresa = "",
                                       FechaServicio = A.d_ServiceDate
                                   }).ToList();

                if (serviceData.Count == 0) return null;

                var serviceDataTemp = (from a in serviceData
                    select new
                    {
                        FullName = a.FullName,
                        Puesto = a.Puesto,
                        Empresa = a.Empresa,
                        FechaServicio = a.FechaServicio.Value.Date
                    }).ToList();

                serviceDataTemp = serviceDataTemp.GroupBy(g => g.FechaServicio).Select(s => s.First()).ToList();


                var serviceComponentFieldValues = (from A in ctx.Service
                                                   join B in ctx.ServiceComponent on A.v_ServiceId equals B.v_ServiceId
                                                   join C in ctx.ServiceComponentFields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                   join D in ctx.ServiceComponentFieldValues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                                   where A.v_PersonId == patientId
                                                           && (C.v_ComponentFieldId == Constants.COLESTEROL_TOTAL_Colesterol_Total_Id || C.v_ComponentFieldId == Constants.PERFIL_LIPIDICO_Colesterol_Total_Id || C.v_ComponentFieldId == Constants.GLUCOSA_Glucosa_Id || C.v_ComponentFieldId == Constants.HEMOGLOBINA_Hemoglobina_Id || C.v_ComponentFieldId == Constants.HEMOGRAMA_Hemoglobina_Id || C.v_ComponentFieldId == Constants.FUNCIONES_VITALES_Presion_Sistolica_Id || C.v_ComponentFieldId == Constants.FUNCIONES_VITALES_Presion_Distolica_Id || C.v_ComponentFieldId == Constants.ANTROPOMETRIA_Imc_Id || C.v_ComponentFieldId == Constants.ESPIROMETRIA_Cvf_Id)
                                                           && B.i_IsDeleted == 0
                                                           && C.i_IsDeleted == 0

                                                   select new
                                                   {

                                                       ServiceDate = A.d_ServiceDate,
                                                       ComponentFieldId = C.v_ComponentFieldId,
                                                       ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                       Value1 = D.v_Value1,
                                                   }).ToList();

                var serviceComponentFieldValuesTemp = (from a in serviceComponentFieldValues
                                                        select new
                                                        {
                                                            ServiceDate = a.ServiceDate.Value.Date,
                                                            ComponentFieldId =a.ComponentFieldId,
                                                            ServiceComponentFieldsId = a.ServiceComponentFieldsId,
                                                            Value1 = a.Value1

                                                        }).ToList();

                serviceComponentFieldValuesTemp = serviceComponentFieldValuesTemp.GroupBy(g => new { g.ServiceDate, g.ComponentFieldId}).Select(s => s.First()).ToList();

                Indicators oIndicators = new Indicators();
                oIndicators.PersonId = patientId;

                #region Data
                List<DataPatient> Data = new List<DataPatient>();
                var oDataPatient = new DataPatient();
                oDataPatient.Name = serviceDataTemp[0].FullName;
                oDataPatient.Empresa = serviceDataTemp[0].Empresa;
                oDataPatient.Puesto = serviceDataTemp[0].Puesto;
                Data.Add(oDataPatient);
                oIndicators.DataPatient = Data;
                #endregion

                #region IMC
                List<Weight> Weights = new List<Weight>();
                var ListWeights = serviceComponentFieldValuesTemp.FindAll(p => p.ComponentFieldId == Constants.ANTROPOMETRIA_Imc_Id).ToList();
                var ListWeightsSort = ListWeights.OrderByDescending(o1 => o1.ServiceDate).ToList();
                foreach (var item in ListWeightsSort)
                {
                    var oWeight = new Weight();
                    oWeight.Date = item.ServiceDate.ToString("dd-MM-yyyy");
                    oWeight.y = item.Value1;

                    Weights.Add(oWeight);
                }
                oIndicators.Weights = Weights;
                #endregion

                #region BloodPressureSis
                var BloodPressureSis = new List<BloodPressureSis>();
                var ListBloodPressureSis = serviceComponentFieldValuesTemp.FindAll(p => p.ComponentFieldId == Constants.FUNCIONES_VITALES_Presion_Sistolica_Id).OrderBy(p => p.ServiceDate);
                foreach (var item in ListBloodPressureSis)
                {
                    var oBloodPressureSis = new BloodPressureSis();
                    oBloodPressureSis.Date = item.ServiceDate.ToString("dd-MM-yyyy");
                    oBloodPressureSis.y = item.Value1;

                    BloodPressureSis.Add(oBloodPressureSis);
                }
                oIndicators.BloodPressureSis = BloodPressureSis;
                #endregion

                #region BloodPressureDia
                var BloodPressureDia = new List<BloodPressureDia>();
                var ListBloodPressureDia = serviceComponentFieldValuesTemp.FindAll(p => p.ComponentFieldId == Constants.FUNCIONES_VITALES_Presion_Distolica_Id).OrderBy(p => p.ServiceDate);
                foreach (var item in ListBloodPressureDia)
                {
                    var oBloodPressureDia = new BloodPressureDia();
                    oBloodPressureDia.Date = item.ServiceDate.ToString("dd-MM-yyyy");
                    oBloodPressureDia.y = item.Value1;

                    BloodPressureDia.Add(oBloodPressureDia);
                }
                oIndicators.BloodPressureDia = BloodPressureDia;
                #endregion

                #region Cholesterol
                var Cholesterol = new List<Cholesterol>();
                var ListCholesterol = serviceComponentFieldValuesTemp.FindAll(p => p.ComponentFieldId == Constants.COLESTEROL_TOTAL_Colesterol_Total_Id || p.ComponentFieldId == Constants.PERFIL_LIPIDICO_Colesterol_Total_Id).OrderBy(p => p.ServiceDate);
                foreach (var item in ListCholesterol)
                {
                    var oCholesterol = new Cholesterol();
                    oCholesterol.Date = item.ServiceDate.ToString("dd-MM-yyyy");
                    oCholesterol.y = item.Value1;

                    Cholesterol.Add(oCholesterol);
                }
                oIndicators.Cholesterols = Cholesterol;
                #endregion

                #region Glucoses
                var Glucoses = new List<Glucose>();
                var ListGlucoses = serviceComponentFieldValuesTemp.FindAll(p => p.ComponentFieldId == Constants.GLUCOSA_Glucosa_Id).OrderBy(p => p.ServiceDate);
                foreach (var item in ListGlucoses)
                {
                    var oGlucoses = new Glucose();
                    oGlucoses.Date = item.ServiceDate.ToString("dd-MM-yyyy");
                    oGlucoses.y = item.Value1;

                    Glucoses.Add(oGlucoses);
                }
                oIndicators.Glucoses = Glucoses;
                #endregion

                #region Haemoglobin
                var Haemoglobins = new List<Haemoglobin>();
                var ListHaemoglobins = serviceComponentFieldValuesTemp.FindAll(p => p.ComponentFieldId == Constants.HEMOGLOBINA_Hemoglobina_Id || p.ComponentFieldId == Constants.HEMOGRAMA_Hemoglobina_Id).OrderBy(p => p.ServiceDate);
                foreach (var item in ListHaemoglobins)
                {
                    var oHaemoglobin = new Haemoglobin();
                    oHaemoglobin.Date = item.ServiceDate.ToString("dd-MM-yyyy");
                    oHaemoglobin.y = item.Value1;

                    Haemoglobins.Add(oHaemoglobin);
                }
                oIndicators.Haemoglobins = Haemoglobins;
                #endregion

                #region Espiro

                var Espiros = new List<Espiro>();
                var ListEspiros = serviceComponentFieldValuesTemp.FindAll(p => p.ComponentFieldId == Constants.ESPIROMETRIA_Cvf_Id).OrderBy(p => p.ServiceDate);
                foreach (var item in ListEspiros)
                {
                    var oEspiro = new Espiro();
                    oEspiro.Date = item.ServiceDate.ToString("dd-MM-yyyy");
                    oEspiro.y = item.Value1;

                    Espiros.Add(oEspiro);
                }
                oIndicators.Espiros = Espiros;
                #endregion



                return oIndicators;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public MonthlyControls MonthlyControls()
        {
            var currentDate = DateTime.Today;
            var firstDay = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDay = new DateTime(currentDate.Year, currentDate.Month + 1, 1).AddDays(-1);

            var services = (from A in ctx.Service
                            join B in ctx.Protocol on A.v_ProtocolId equals B.v_ProtocolId
                            where (A.d_ServiceDate >= firstDay && A.d_ServiceDate <= lastDay)
                                    && A.i_IsDeleted == 0 && A.i_MasterServiceId == (int)masterService.AtxMedicaParticular
                            select new
                            {
                                ServiceDate = A.d_ServiceDate,
                                StatusControl = A.i_ServiceStatusId,
                                IsControl = A.i_IsControl
                            }).ToList().OrderBy(o => o.ServiceDate);

            var oMonthlyControls = new MonthlyControls();
            oMonthlyControls.Date = currentDate.Date.Month.ToString();

            var listDays = new List<Day>();
            var listControlDay = new List<ControlDay>();
            var listControlDayCompleted = new List<ControlCompletedDay>();

            for (int i = 1; i <= lastDay.Day; i++)
            {
                #region Days
                var oDay = new Day();
                var fecha = new DateTime(currentDate.Year, currentDate.Month, i);
                oDay.NroDay = fecha.ToString("ddd", new System.Globalization.CultureInfo("es-ES")) + "-" + i;
                listDays.Add(oDay);
                oMonthlyControls.NroDays = listDays;
                #endregion
                #region Controles
                var ControlsDay = services.ToList().FindAll(p => p.ServiceDate.Value.Day == i);

                var oControlDay = new ControlDay();
                oControlDay.Date = new DateTime(currentDate.Year, currentDate.Month, i).ToString();
                oControlDay.y = ControlsDay.Count().ToString();
                listControlDay.Add(oControlDay);

                oMonthlyControls.DailyControls = listControlDay;
                #endregion

                #region Controles Completados
                var ControlsDayCompleted = services.ToList().FindAll(p => p.ServiceDate.Value.Day == i && p.StatusControl == (int)ServiceStatus.Culminado);

                var oControlCompletedDay = new ControlCompletedDay();
                oControlCompletedDay.Date = new DateTime(currentDate.Year, currentDate.Month, i).ToString();
                oControlCompletedDay.y = ControlsDayCompleted.Count().ToString();
                listControlDayCompleted.Add(oControlCompletedDay);

                oMonthlyControls.DailyControlsCompleted = listControlDayCompleted;
                #endregion

            }
            return oMonthlyControls;

        }

        public List<ReviewEMO> ReviewsEMOs(string patientId, string organizationId)
        {
                var isDeleted = (int)Enumeratores.SiNo.No;
                var list = (from a in ctx.Service
                            join b in ctx.SystemParameter on new { a = a.i_AptitudeStatusId.Value, b = 124 } equals new { a = b.i_ParameterId, b = b.i_GroupId }
                            where a.i_IsDeleted == isDeleted 
                                //&& a.i_StatusLiquidation == 2
                                && a.v_PersonId == patientId
                            select new ReviewEMO
                            {
                                PatientId = a.v_PersonId,
                                ServiceId = a.v_ServiceId,
                                Aptitude = b.v_Value1,
                                ServiceDate = a.d_ServiceDate.Value,
                                IsRevisedHistoryId = a.i_IsRevisedHistoryId.Value,
                                MasterServiceId = a.i_MasterServiceId.Value,
                                StatusLiquidation = a.i_StatusLiquidation.Value,
                            }).ToList();

                var result = (from A in list
                              select new ReviewEMO
                              {
                                  PatientId = A.PatientId,
                                  ServiceId = A.ServiceId,
                                  Aptitude = A.Aptitude,
                                  ServiceDate = A.ServiceDate,
                                  IsRevisedHistoryId = A.IsRevisedHistoryId == null ? 0 : 1,
                                  MasterServiceId = A.MasterServiceId,
                                  DiseaseName = ConcatenateDxForServiceAntecedent(A.ServiceId),
                                  VigilanciaRecomendada = RecomendarVigilancia(A.ServiceId, organizationId),
                                  StatusLiquidation = A.StatusLiquidation
                              }).OrderByDescending(p => p.ServiceDate).ToList();

                return result;
        }

        private string RecomendarVigilancia(string serviceId, string organizationId)
        {
            var qryDiseases = (from A in ctx.DiagnosticRepository
                join B in ctx.Diseases on A.v_DiseasesId equals B.v_DiseasesId
                where A.v_ServiceId == serviceId && A.i_FinalQualificationId == (int)FinalQualification.Definitivo && A.i_IsDeleted == 0
                select new
                {
                    DiseaseId = A.v_DiseasesId
                }).ToList();

            var list = new List<string>();
            foreach (var item in qryDiseases)
            {
                list.Add(item.DiseaseId);
            }

            var queryPlan = (from A in ctx.PlanVigilancia
                join B in ctx.PlanVigilanciaDiseases on A.v_PlanVigilanciaId equals B.v_PlanVigilanciaId
                             where list.Contains(B.v_DiseasesId) && A.v_OrganizationId == organizationId
                             select new
                {
                    plan = A.v_Name
                }).ToList();

            var plan =  string.Join(", ", queryPlan.Select(p => p.plan));

            return plan == "" ? "Sin sugerencias" : plan;
        }

        public MemoryStream DownloadFile(string patientId, string directorioESO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(patientId))
                    throw new Exception();

                string PatientId = patientId;

                string path = string.Format("{0}{1}.pdf", directorioESO, PatientId);
                byte[] binaryData;
                FileStream inFile = new FileStream(path, FileMode.Open, FileAccess.Read);
                binaryData = new Byte[inFile.Length];
                inFile.Read(binaryData, 0, (int)inFile.Length);

                MemoryStream ms = new MemoryStream(binaryData);
                return ms;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string ConcatenateDxForServiceAntecedent(string serviceId)
        {
            var qry = (from A in ctx.DiagnosticRepository
                       join B in ctx.Diseases on A.v_DiseasesId equals B.v_DiseasesId
                       where A.v_ServiceId == serviceId && A.i_FinalQualificationId == (int)FinalQualification.Definitivo && A.i_IsDeleted == 0
                       select new
                       {
                           v_DiseasesName = B.v_Name
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_DiseasesName));
        }

        public List<PersonMedicalHistoryList> GetAntecedentConsolidateForService(string patientId)
        {
            //mon.IsActive = true;

            try
            {
                List<PersonMedicalHistoryList> lis = new List<PersonMedicalHistoryList>();

                int isDeleted = (int)SiNo.No;

                #region querys individuales

                // Obtener todos loa antecedentes de una persona (una o varias empresas)
                var historyId = (from a in ctx.History
                                 where a.v_PersonId == patientId && a.i_IsDeleted == isDeleted
                                 select new PersonMedicalHistoryList
                                 {
                                     v_AntecedentTypeName = "Ocupacionales",
                                     v_DiseasesName = null,
                                     v_HistoryId = a.v_HistoryId,
                                     d_StartDate = a.d_StartDate,
                                     d_EndDate = a.d_EndDate,
                                     v_Occupation = a.v_workstation,
                                     v_GroupName = null
                                 }).ToList();

                // personmedicalhistory
                var q1tmp = (from A in ctx.PersonMedicalHistory
                             join D in ctx.Diseases on A.v_DiseasesId equals D.v_DiseasesId
                             where A.i_IsDeleted == isDeleted && A.v_PersonId == patientId

                             select new PersonMedicalHistoryList
                             {
                                 v_AntecedentTypeName = "Medicos-Personales",
                                 v_DiseasesName = D.v_Name,
                                 d_StartDate = A.d_StartDate,
                                 v_GroupName = null

                             }).ToList();

                var q1 = (from A in q1tmp
                          select new PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Medicos-Personales",
                              v_DiseasesName = A.v_DiseasesName,
                              v_DateOrGroup = A.d_StartDate.Value.ToShortDateString(),
                              d_StartDate = A.d_StartDate.Value
                          }).ToList();

                // typeofeep
                var q2 = (from A in historyId
                          select new PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Ocupacionales, " + A.v_Occupation,
                              v_DiseasesName = ConcatenateTypeOfeep(A.v_HistoryId),
                              v_DateOrGroup = A.d_StartDate.Value.ToString("MM/yyyy") + " - " + A.d_EndDate.Value.ToString("MM/yyyy"),
                              d_StartDate = A.d_StartDate,
                          }).ToList();

                // workstationdangers
                var q3 = (from A in historyId
                          select new PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Ocupacionales, " + A.v_Occupation,
                              v_DiseasesName = ConcatenateWorkStationDangers(A.v_HistoryId),
                              v_DateOrGroup = A.d_StartDate.Value.ToString("MM/yyyy") + " - " + A.d_EndDate.Value.ToString("MM/yyyy"),
                              d_StartDate = A.d_StartDate,
                          }).ToList();

                // noxioushabits
                var q4 = (from A in ctx.NoxiousHabits
                          join B in ctx.SystemParameter on new { a = A.i_TypeHabitsId.Value, b = 148 }  // HÁBITOS NOCIVOS
                                                         equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                          from B in B_join.DefaultIfEmpty()
                          where A.i_IsDeleted == 0 && A.v_PersonId == patientId
                          select new PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = "Hábitos Nocivos",
                              v_DiseasesName = B.v_Value1 + ", " + A.v_Frequency,
                          }).ToList();

                // familymedicalantecedents

                var q5tmp = (from A in ctx.FamilyMedicalAntecedents
                             join B in ctx.SystemParameter on new { a = A.i_TypeFamilyId.Value, b = 149 }  // ANTECEDENTES FAMILIARES MÉDICOS
                                                            equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                             from B in B_join.DefaultIfEmpty()
                             join C in ctx.SystemParameter on new { a = B.i_ParentParameterId.Value, b = 149 }
                                                          equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                             from C in C_join.DefaultIfEmpty()
                             where A.i_IsDeleted == 0 && A.v_PersonId == patientId
                             group C by new { C.i_ParameterId, C.v_Value1 } into g
                             select new PersonMedicalHistoryList
                             {
                                 v_AntecedentTypeName = "Familiares",
                                 i_TypeFamilyId = g.Key.i_ParameterId,
                                 v_TypeFamilyName = g.Key.v_Value1
                             }).ToList();

                var q5 = (from A in q5tmp
                          select new PersonMedicalHistoryList
                          {
                              v_AntecedentTypeName = A.v_AntecedentTypeName,
                              v_DiseasesName = ConcatenateFamilyMedicalAntecedents(patientId, A.i_TypeFamilyId),
                              v_DateOrGroup = A.v_TypeFamilyName
                          }).ToList();

                #endregion

                #region Fusion

                if (q1.Count > 0)
                    lis.AddRange(q1);
                if (q2.Count > 0)
                    lis.AddRange(q2);
                if (q3.Count > 0)
                    lis.AddRange(q3);
                if (q4.Count > 0)
                    lis.AddRange(q4);
                if (q5.Count > 0)
                    lis.AddRange(q5);

                #endregion

                return lis.OrderBy(x => x.v_AntecedentTypeName).ToList();


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string ConcatenateTypeOfeep(string pstrHistoryId)
        {
            var qry = (from A in ctx.TypeOfEpp
                       join B in ctx.SystemParameter on new { a = A.i_TypeofEEPId.Value, b = 146 }  // TIPO DE EPP USADO
                                                            equals new { a = B.i_ParameterId, b = B.i_GroupId }
                       where A.v_HistoryId == pstrHistoryId &&
                       A.i_IsDeleted == 0
                       select new
                       {
                           v_DiseasesName = B.v_Value1
                       }).ToList();

            return qry.Count == 0 ? "No usa EPP" : string.Join(", ", qry.Select(p => p.v_DiseasesName));
        }

        private string ConcatenateWorkStationDangers(string pstrHistoryId)
        {
            var qry = (from A in ctx.WorkStationDangers
                       join B in ctx.SystemParameter on new { a = A.i_DangerId.Value, b = 145 } // PELIGROS EN EL PUESTO
                                                              equals new { a = B.i_ParameterId, b = B.i_GroupId }
                       where A.v_HistoryId == pstrHistoryId &&
                       A.i_IsDeleted == 0
                       select new
                       {
                           v_DiseasesName = B.v_Value1
                       }).ToList();

            return qry.Count == 0 ? "No refiere peligros en el puesto" : string.Join(", ", qry.Select(p => p.v_DiseasesName));
        }

        private string ConcatenateFamilyMedicalAntecedents(string pstrPersonId, int pintTypeFamilyId)
        {
            var qry = (from A in ctx.FamilyMedicalAntecedents
                       join B in ctx.SystemParameter on new { a = A.i_TypeFamilyId.Value, b = 149 }  // ANTECEDENTES FAMILIARES MÉDICOS
                                                    equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                       from B in B_join.DefaultIfEmpty()
                       join C in ctx.SystemParameter on new { a = B.i_ParentParameterId.Value, b = 149 }  // [PADRE,MADRE,HERMANOS]
                                                      equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                       from C in C_join.DefaultIfEmpty()
                       join D in ctx.Diseases on new { a = A.v_DiseasesId }
                                                               equals new { a = D.v_DiseasesId } into D_join
                       from D in D_join.DefaultIfEmpty()
                       where A.v_PersonId == pstrPersonId &&
                       A.i_IsDeleted == 0 && C.i_ParameterId == pintTypeFamilyId
                       select new
                       {
                           v_DiseasesName = D.v_Name
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_DiseasesName));
        }

        public bool RevisedStatusEMO(string serviceId, bool status)
        {
            try
            {
                var objEntitySource = (from a in ctx.Service                                      
                                       where a.v_ServiceId == serviceId
                                       select a).FirstOrDefault();

                objEntitySource.i_IsRevisedHistoryId = status == true ? 1 : 0;

                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        public List<string> GetNamePatients(string value)
        {
            try
            {
                var isDeleted = (int)Enumeratores.SiNo.No;
                var list = (from a in ctx.Pacient
                            join b in ctx.Person on a.v_PersonId equals b.v_PersonId
                            where a.i_IsDeleted == isDeleted && (b.v_FirstName.Contains(value) 
                            || b.v_FirstLastName.Contains(value) || b.v_SecondLastName.Contains(value)) && a.v_PersonId == b.v_PersonId
                            select new
                            {
                                Name = b.v_FirstName + " " + b.v_FirstLastName + " " + b.v_SecondLastName,
                            }).ToList();
                return list.Select(x => x.Name).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<KeyValueDTO> GetAllOrganizationEmployers()
        {
            var list = (from a in ctx.Organization
                        where a.i_IsDeleted == 0
                        select new KeyValueDTO
                        {
                            Id    = a.v_OrganizationId,
                            Value = a.v_Name
                        }).ToList();

            return list;
        }

        public List<VigilanciaServiceCustom> VigilanciaServiceDtos(int doctoRespondibleId, string organizationId)
        {
            return new VigilanciaDal().VigilanciaServiceDtos(doctoRespondibleId, organizationId);
        }
    }
}
