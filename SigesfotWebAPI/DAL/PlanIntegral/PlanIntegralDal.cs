using BE.Common;
using BE.PlanIntegral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.PlanIntegral
{
    public class PlanIntegralDal
    {
        public List<PlanIntegralList> GetPlanIntegralAndFiltered(string pstrPersonId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var query = from A in dbContext.PlanIntegral
                            join B in dbContext.SystemParameter on new { a = A.i_TipoId.Value, b = 281 }  // CATEGORIA DEL EXAMEN
                                                      equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                            from B in B_join.DefaultIfEmpty()
                            where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

                            select new PlanIntegralList
                            {
                                v_PlanIntegral = A.v_PlanIntegral,
                                v_PersonId = A.v_PersonId,
                                i_TipoId = A.i_TipoId.Value,
                                v_Descripcion = A.v_Descripcion,
                                d_Fecha = A.d_Fecha.Value,
                                v_Lugar = A.v_Lugar,
                                v_Tipo = B.v_Value1
                            };
                List<PlanIntegralList> objData = query.ToList();
                return objData;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProblemaList> GetProblemaPagedAndFiltered(string pstrPersonId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var query = from A in dbContext.Problema
                            join B in dbContext.SystemParameter on new { a = A.i_EsControlado.Value, b = 111 }  // CATEGORIA DEL EXAMEN
                                                        equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                            from B in B_join.DefaultIfEmpty()
                            where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

                            select new ProblemaList
                            {
                                v_ProblemaId = A.v_ProblemaId,
                                i_Tipo = A.i_Tipo,
                                v_PersonId = A.v_PersonId,
                                d_Fecha = A.d_Fecha.Value,
                                v_Descripcion = A.v_Descripcion,
                                i_EsControlado = A.i_EsControlado,
                                v_Observacion = A.v_Observacion,
                                v_EsControlado = B.v_Value1
                            };

                List<ProblemaList> objData = query.ToList();

                return objData;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool AddPlanIntegral(PlanIntegralBE data, int nodeId, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                data.v_PlanIntegral = new Common.Utils().GetPrimaryKey(nodeId, 327, "PL");
                data.d_InsertDate = DateTime.Now;
                data.i_InsertUserId = userId;
                data.i_IsDeleted = 0;
                ctx.PlanIntegral.Add(data);

                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddProblema(ProblemaBE data, int nodeId, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                data.v_ProblemaId = new Common.Utils().GetPrimaryKey(nodeId, 326, "PM");
                data.d_InsertDate = DateTime.Now;
                data.i_InsertUserId = userId;
                data.i_IsDeleted = 0;
                ctx.Problema.Add(data);

                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TipoAtencionList> GetPlanIntegral(string personId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var tiposAtencion = (from A in dbContext.SystemParameter
                                     where A.i_GroupId == 281
                                     select new
                                     {
                                         Id = A.i_ParameterId,
                                         Value = A.v_Value1
                                     }).ToList();

                var planes = (from A in dbContext.PlanIntegral
                              join B in dbContext.SystemParameter on new { a = A.i_TipoId.Value, b = 281 }
                                                                   equals new { a = B.i_ParameterId, b = B.i_GroupId }
                              where A.v_PersonId == personId && A.i_IsDeleted == 0
                              select new
                              {
                                  v_PlanIntegral = A.v_PlanIntegral,
                                  v_PersonId = A.v_PersonId,
                                  i_TipoId = A.i_TipoId,
                                  v_Descripcion = A.v_Descripcion,
                                  d_Fecha = A.d_Fecha,
                                  v_Lugar = A.v_Lugar,
                                  v_Tipo = B.v_Value1
                              }).ToList();


                TipoAtencionList tipoAtencionList = null;
                List<TipoAtencionList> listaAtenciones = new List<TipoAtencionList>();
                foreach (var atencion in tiposAtencion)
                {
                    tipoAtencionList = new TipoAtencionList();
                    tipoAtencionList.Id = atencion.Id;
                    tipoAtencionList.Value = atencion.Value;
                    var detalles = planes.FindAll(p => p.i_TipoId == atencion.Id);
                    List<PlanIntegralList> List = new List<PlanIntegralList>();
                    PlanIntegralList planAtencionIntegral;
                    if (detalles.Count == 0)
                    {
                        planAtencionIntegral = new PlanIntegralList();
                        planAtencionIntegral.v_Descripcion = "";
                        planAtencionIntegral.v_Fecha = "";
                        planAtencionIntegral.v_Lugar = "";
                        List.Add(planAtencionIntegral);
                    }
                    else
                    {
                        foreach (var detalle in detalles)
                        {
                            planAtencionIntegral = new PlanIntegralList();
                            planAtencionIntegral.v_Descripcion = detalle.v_Descripcion;
                            planAtencionIntegral.v_Fecha = detalle.d_Fecha.Value.ToShortDateString();
                            planAtencionIntegral.v_Lugar = detalle.v_Lugar;
                            List.Add(planAtencionIntegral);
                        }
                    }

                    tipoAtencionList.List = List;
                    listaAtenciones.Add(tipoAtencionList);
                }

                List<TipoAtencionList> objData = listaAtenciones;
                return objData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
