using BE.Common;
using BE.Diagnostic;
using DAL.Diagnostic;
using DAL.Plan;
using SAMBHSDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;
using static BE.Eso.RecipesCustom;

namespace DAL.Warehouse
{
    public class WarehouseDal
    {
        public List<string> SearchProduct(string name)
        {
            using (var ctx = new DatabaseSAMBHSContext())
            {
                var query = (from a in ctx.Producto
                             join pd in ctx.ProductoDetalle on a.v_IdProducto equals pd.v_IdProducto
                             join pa in ctx.ProductoAlmacen on pd.v_IdProductoDetalle equals pa.v_ProductoDetalleId

                    where (a.v_CodInterno.Contains(name) || a.v_Descripcion.Contains(name) || name == null) && a.i_Eliminado == (int)SiNo.No && a.i_EsActivo == (int)SiNo.Si && pa.i_IdAlmacen == 1
                             select new
                    {
                        value = a.v_Descripcion + "|" + a.v_IdProducto + "|" + a.v_CodInterno  + "|" + a.v_IdLinea + "|" + pd.v_IdProductoDetalle + "|" + a.d_PrecioVenta + "|" + a.d_PrecioMayorista
                    }).ToList();
                query = query.GroupBy(x => x.value).Select(z => z.First()).ToList();
                return query.Select(p => p.value).ToList();
            }
        }

        public bool SaveUpdateRecipe(BoardPrintRecipes data, int userId, int nodeId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();

                foreach (var item in data.ListProducts)
                {
                    DiagnosticCustom diag = new DiagnosticCustom();
                    diag.ComponentId = item.ComponentId;
                    diag.ServiceId = data.ServiceId;
                    diag.DiseaseId = item.DiseaseId;
                    if (item.RecetaId == null)
                    {
                        string diagnosticRespositoryId = new DiagnosticDal().AddDiagnosticRepository(diag, userId, nodeId);
                        if (diagnosticRespositoryId == null)
                        {
                            return false;
                        }
                        else
                        {
                            item.DiagnosticRespository = diagnosticRespositoryId;
                        }
                    }
                    else
                    {
                        var objReceta = ctx.Receta.Where(x => x.i_IdReceta == item.RecetaId).FirstOrDefault();
                        item.DiagnosticRespository = objReceta.v_DiagnosticRepositoryId;
                    }
                    

                    

                    var getPlan = new PlanDal().GetPlan(data.ServiceId, item.IdLinea);
                    bool tienePlan = false;
                    if (getPlan.Count > 0)
                    {
                        tienePlan = true;
                    }

                    if (tienePlan)
                    {
                        if (getPlan[0].i_EsCoaseguro == (int)SiNo.Si)
                        {
                            var planCoaseguro = new PlanDal().GetPlanForImporte(data.ServiceId, item.DiagnosticRespository);
                            if (planCoaseguro.Count > 0)
                            {
                                item.SaldoPaciente = (planCoaseguro[0].d_ImporteCo / 100) * (item.Price * decimal.Parse(item.Quantity.ToString()));
                                item.SaldoAseguradora = (item.Price * decimal.Parse(item.Quantity.ToString())) - item.SaldoPaciente;
                            }                            
                        }

                    }
                    else
                    {
                        item.SaldoPaciente = item.Price;
                    }


                    if (item.RecetaId == null)//Creo
                    {
                        RecetaBE _RecetaDto = new RecetaBE();
                        _RecetaDto.v_DiagnosticRepositoryId = item.DiagnosticRespository;
                        _RecetaDto.d_Cantidad = decimal.Parse(item.Quantity.ToString());
                        _RecetaDto.v_Duracion = item.Duration;
                        _RecetaDto.v_Posologia = item.Posologia;
                        _RecetaDto.t_FechaFin = item.FechaFin;
                        _RecetaDto.v_IdProductoDetalle = item.ProductoDetalleId;
                        _RecetaDto.v_IdUnidadProductiva = item.IdLinea;
                        _RecetaDto.v_ServiceId = data.ServiceId;
                        _RecetaDto.d_SaldoPaciente = item.SaldoPaciente;
                        _RecetaDto.d_SaldoAseguradora = item.SaldoAseguradora;
                        
                        ctx.Receta.Add(_RecetaDto);
                    }
                    else//Actualizo
                    {
                        var objReceta = ctx.Receta.Where(x => x.i_IdReceta == item.RecetaId).FirstOrDefault();
                        objReceta.v_DiagnosticRepositoryId = item.DiagnosticRespository;
                        objReceta.d_Cantidad = decimal.Parse(item.Quantity.ToString());
                        objReceta.v_Duracion = item.Duration;
                        objReceta.v_Posologia = item.Posologia;
                        objReceta.t_FechaFin = item.FechaFin;
                        objReceta.v_IdProductoDetalle = item.ProductoDetalleId;
                        objReceta.v_IdUnidadProductiva = item.IdLinea;
                        objReceta.v_ServiceId = data.ServiceId;
                        objReceta.d_SaldoPaciente = item.SaldoPaciente;
                        objReceta.d_SaldoAseguradora = item.SaldoAseguradora;
                    }
                    

                }


                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex )
            {
                return false;
            }
        }

        public List<Recipes> GetRecipesByServiceId(string serviceId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var listReceta = (from rec in ctx.Receta
                                 join dry in ctx.DiagnosticRepository on rec.v_DiagnosticRepositoryId equals dry.v_DiagnosticRepositoryId
                                 join dis in ctx.Diseases on dry.v_DiseasesId equals dis.v_DiseasesId
                                 where dry.v_ServiceId == serviceId
                                 select new Recipes
                                 {
                                     RecetaId = rec.i_IdReceta,
                                     ProductoDetalleId = rec.v_IdProductoDetalle,
                                     IdLinea = rec.v_IdUnidadProductiva,
                                     d_Quantity = rec.d_Cantidad,
                                     DiseaseId = dis.v_DiseasesId,
                                     DiseaseName = dis.v_Name,
                                     Posologia = rec.v_Posologia,
                                     Duration = rec.v_Duracion,
                                     FechaFin = rec.t_FechaFin,
                                     ComponentId = dry.v_ComponentId,
                                 }).ToList();
                listReceta = listReceta.GroupBy(x => x.RecetaId).Select(z => z.First()).ToList();

                //Sambhs
                DatabaseSAMBHSContext ctxsamb = new DatabaseSAMBHSContext();
                string periodo = DateTime.Now.Year.ToString();
                foreach (var objReceta in listReceta)
                {
                    var objProdutc = (from pral in ctxsamb.ProductoAlmacen
                                      join prod in ctxsamb.ProductoDetalle on pral.v_ProductoDetalleId equals prod.v_IdProductoDetalle
                                      join pro in ctxsamb.Producto on prod.v_IdProducto equals pro.v_IdProducto
                                       where prod.v_IdProductoDetalle == objReceta.ProductoDetalleId && pral.i_IdAlmacen == 1 && pral.v_Periodo == periodo
                                      select new {
                                         pral.d_StockActual,
                                         prod.v_IdProducto,
                                         pro.v_Descripcion,
                                         pro.d_PrecioVenta,
                                      }).FirstOrDefault();
                    if (objProdutc != null)
                    {
                        objReceta.StockActual = float.Parse(objProdutc.d_StockActual.Value.ToString("N2"));
                        objReceta.ProductId = objProdutc.v_IdProducto;
                        objReceta.ProductName = objProdutc.v_Descripcion;
                        objReceta.Price = objProdutc.d_PrecioVenta;
                    }
                }

                return listReceta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
