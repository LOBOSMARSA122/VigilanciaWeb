using BE.Common;
using BE.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Eso.RecipesCustom;

namespace DAL.ProductWarehouse
{
    
    public class InputOutputDal
    {
        
        DatabaseContext Ctx = new DatabaseContext();
        
        public string  AddMovement(BoardPrintRecipes data) {

            try
            {

                string NewId = "(No generado)";
                var supplierId = data.SupplierId == null ? null : data.SupplierId;
                var motiveType = data.MotiveTypeId == null ? 4 : data.MotiveTypeId;
                DateTime _date = data.Date.Value == null ? DateTime.Now : data.Date.Value;
                MovementBE oMovement = new MovementBE();
                
                //New
                NewId = new Common.Utils().GetPrimaryKey(data.NodeId.Value, 370, "MM");
                oMovement.v_MovementId = NewId;
                oMovement.i_MovementTypeId = data.MovementTypeId.Value;
                oMovement.v_WarehouseId = data.WarehouseId;
                oMovement.i_IsLocallyProcessed = (int)Enumeratores.SiNo.No;
                oMovement.r_TotalQuantity = 0;
                oMovement.v_ReferenceDocument = data.ReferenceDocument;
                oMovement.i_MotiveTypeId = motiveType;
                oMovement.v_SupplierId = supplierId;
                oMovement.i_InsertUserId = data.InsertUserId.Value;
                oMovement.d_Date = _date;
                Ctx.Movement.Add(oMovement);                             

                Ctx.SaveChanges();
                return NewId;
            }
            catch (Exception ex)
            {
                return null;
            }
            


        }

        public bool AddMovementDetail(BoardPrintRecipes data, string movementId)
        { 
            try
            {
                foreach (var item in data.ListProducts)
                {
                    var objProduct = (from pro in Ctx.Product
                                      where pro.v_ProductId == item.ProductId
                                      select pro).FirstOrDefault();
                    var oMovementeDetailBE = new MovementDetailBE
                    {
                        v_MovementId = movementId,
                        v_WarehouseId = data.WarehouseId,
                        v_ProductId = item.ProductId,
                        r_Quantity = item.Quantity.Value,
                        i_MovementTypeId = data.MovementTypeId,
                        r_Price = objProduct.r_ReferentialCostPrice,
                        r_SubTotal = objProduct.r_ReferentialCostPrice * item.Quantity.Value,
                    };
                    Ctx.MovementDetail.Add(oMovementeDetailBE);
                }
                    
                    
                Ctx.SaveChanges();
                
            
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string ProcessMovementOutput(string movementId, int systemUserId)
        {
            string message = "";
            bool IsProcessMovement = false;
            try
            {
                float total = 0;
                var objMovement = (from mov in Ctx.Movement
                                    where mov.v_MovementId == movementId
                                   select mov).FirstOrDefault();

                var listMovementsDetails = (from mdt in Ctx.MovementDetail
                                            where mdt.v_MovementId == objMovement.v_MovementId
                                            select mdt).ToList();

                //Si existen detalles del movimiento
                if (listMovementsDetails != null)
                {
                    
                    foreach (var item in listMovementsDetails)
                    {
                        var objProductWarehouse = (from prw in Ctx.ProductWarehouse
                                                    where prw.v_WarehouseId == item.v_WarehouseId && prw.v_ProductId == item.v_ProductId
                                                    select prw).FirstOrDefault();
                        //Si no hay stock suficiente
                        if (objProductWarehouse != null)
                        {
                            total = total + item.r_Quantity;
                            if (item.r_Quantity > objProductWarehouse.r_StockActual)
                            {
                                IsProcessMovement = false;

                                var objProduct = (from pro in Ctx.Product
                                                    where pro.v_ProductId == item.v_ProductId
                                                    select pro).FirstOrDefault();
                                message = string.Format("El stock del producto {0} (Stock Actual={1}) es insuficiente. No se procesará el movimiento.",
                                    objProduct.v_Name, objProductWarehouse.r_StockActual);
                                break;
                            }
                            else
                            {
                                IsProcessMovement = true;

                                //Reducimos el stock
                                objProductWarehouse.r_StockActual = objProductWarehouse.r_StockActual - item.r_Quantity;

                                //Auditoría
                                objProductWarehouse.i_UpdateUserId = systemUserId;
                                objProductWarehouse.d_UpdateDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            message = "No existen productos en el almcén indicado";
                        }

                    }
                    if (IsProcessMovement)
                    {
                        //Definimos como procesado = si
                        objMovement.i_IsLocallyProcessed = (int)Enumeratores.SiNo.Si;
                        objMovement.d_UpdateDate = DateTime.Now;
                        objMovement.i_UpdateUserId = systemUserId;
                        objMovement.r_TotalQuantity = total;
                        objMovement.i_ProcessTypeId = (int)Enumeratores.SiNo.Si;
                        message = "Se generó el movimiento satisfactoriamente.";

                            
                    }

                }
                

                Ctx.SaveChanges();
                return message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
        }

        public BoardMovement GetData(BoardMovement data)
        {
            try
            {
                int skip = (data.Index - 1) * data.Take;
                var query = (from mov in Ctx.Movement
                                 // SystemUsers (LEFT JOIN) - i_InsertUserId

                             join sys in Ctx.SystemUser on new { i_InsertUserId = mov.i_InsertUserId }
                                                     equals new { i_InsertUserId = sys.i_SystemUserId } into sys_join
                             from sys in sys_join.DefaultIfEmpty()

                                 // SystemUsers (LEFT JOIN) - i_UpdateUserId
                             join sys2 in Ctx.SystemUser on new { i_UpdateUserId = mov.i_UpdateUserId }
                                                     equals new { i_UpdateUserId = sys2.i_SystemUserId } into sys2_join
                             from sys2 in sys2_join.DefaultIfEmpty()

                                 // Node (LEFT JOIN) - i_UpdateNodeId
                             join nod in Ctx.Node on new { i_UpdateNodeId = mov.i_UpdateNodeId }
                                 equals new { i_UpdateNodeId = nod.i_NodeId } into nod_join
                             from nod in nod_join.DefaultIfEmpty()

                                 // SystemParameter (INNER JOIN) - i_IsProcessed
                             join syp in Ctx.SystemParameter on new { a = 111, b = mov.i_IsLocallyProcessed.Value }
                                                                 equals new { a = syp.i_GroupId, b = syp.i_ParameterId }

                                                                 // Supplier (LEFT JOIN) - i_SupplierId
                             join sup in Ctx.Supplier on new { a = mov.v_SupplierId } equals new { a = sup.v_SupplierId } into sup_join
                             from sup in sup_join.DefaultIfEmpty()

                                 // SystemParameter (INNER JOIN) - i_MovementTypeId
                             join syp2 in Ctx.SystemParameter on new { a = 109, b = mov.i_MovementTypeId.Value }
                                                                 equals new { a = syp2.i_GroupId, b = syp2.i_ParameterId }

                                                                 // SystemParameter (INNER JOIN) - i_MotiveTypeId
                             join syp3 in Ctx.SystemParameter on new { a = 110, b = mov.i_MotiveTypeId.Value }
                                                                 equals new { a = syp3.i_GroupId, b = syp3.i_ParameterId }

                                                                 // Condiciones
                             where mov.v_WarehouseId == data.WarehouseId &&  (data.MovementType == -1 || mov.i_MovementTypeId == data.MovementType) &&
                                   (mov.d_Date >= data.StartDate && mov.d_Date <= data.EndDate)
                                   

                             // Ordenamiento
                             orderby mov.d_Date descending
                             select new MovementList
                             {
                                 v_MovementId = mov.v_MovementId,
                                 v_SupplierId = mov.v_SupplierId,
                                 v_SupplierName = sup.v_Name,
                                 v_IsProcessed = syp.v_Value1,
                                 i_ProcessTypeId = mov.i_ProcessTypeId,
                                 i_MovementTypeId = mov.i_MovementTypeId,
                                 v_MovementTypeDescription = syp2.v_Value1,
                                 i_MotiveTypeId = mov.i_MotiveTypeId,
                                 v_MotiveTypeDescription = syp3.v_Value1,
                                 d_MovementDate = mov.d_Date,
                                 r_TotalQuantity = mov.r_TotalQuantity,
                                 v_WarehouseId = mov.v_WarehouseId,
                                 v_ReferentDocument = mov.v_ReferenceDocument,
                                 //v_ProductName = pro.v_Name,

                                 v_CreationUser = sys.v_UserName,
                                 v_UpdateUser = sys2.v_UserName,
                                 d_UpdateDate = mov.d_UpdateDate,
                                 v_UpdateNodeName = nod.v_Description
                             }).ToList();

                if (data.Take > 0)
                    query = query.Skip(skip).Take(data.Take).ToList();



                data.TotalRecords = query.ToList().Count;
                data.List = query.ToList();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ProcessMovementInput(string movementId, int systemUserId)
        {
            string message = "";
            bool IsProcessMovement = false;
            try
            {
                float total = 0;
                var objMovement = (from mov in Ctx.Movement
                                   where mov.v_MovementId == movementId
                                   select mov).FirstOrDefault();

                var listMovementsDetails = (from mdt in Ctx.MovementDetail
                                            where mdt.v_MovementId == objMovement.v_MovementId
                                            select mdt).ToList();

                //Si existen detalles del movimiento
                if (listMovementsDetails != null)
                {

                    foreach (var item in listMovementsDetails)
                    {
                        var objProductWarehouse = (from prw in Ctx.ProductWarehouse
                                                   where prw.v_WarehouseId == item.v_WarehouseId && prw.v_ProductId == item.v_ProductId
                                                   select prw).FirstOrDefault();

                        //si existe el producto con el almacén indicado lo actualiza
                        if (objProductWarehouse != null)
                        {
                            total = total + item.r_Quantity;
                            //Si no hay stock suficiente
                            if (item.r_Quantity > objProductWarehouse.r_StockActual)
                            {
                                IsProcessMovement = false;

                                var objProduct = (from pro in Ctx.Product
                                                  where pro.v_ProductId == item.v_ProductId
                                                  select pro).FirstOrDefault();
                                message = string.Format("El stock del producto {0} (Stock Actual={1}) es insuficiente. No se procesará el movimiento.",
                                    objProduct.v_Name, objProductWarehouse.r_StockActual);
                                break;
                            }
                            else
                            {
                                IsProcessMovement = true;

                                //Aumentamos el stock
                                objProductWarehouse.r_StockActual = objProductWarehouse.r_StockActual + item.r_Quantity;

                                //Auditoría
                                objProductWarehouse.i_UpdateUserId = systemUserId;
                                objProductWarehouse.d_UpdateDate = DateTime.Now;
                            }
                        }

                        //lo agrega
                        else
                        {
                            total = total + item.r_Quantity;
                            IsProcessMovement = true;
                            var oProductWarehouseBE = new ProductWarehouseBE
                            {
                                v_WarehouseId = item.v_WarehouseId,
                                v_ProductId = item.v_ProductId,
                                r_StockActual = item.r_Quantity,
                                r_StockMax = item.r_Quantity,
                                r_StockMin = 0,
                                i_InsertUserId = systemUserId,
                                d_InsertDate = DateTime.Now,
                            };
                            Ctx.ProductWarehouse.Add(oProductWarehouseBE);
                        }

                    }
                    if (IsProcessMovement)
                    {
                        //Definimos como procesado = si
                        objMovement.i_IsLocallyProcessed = (int)Enumeratores.SiNo.Si;
                        objMovement.d_UpdateDate = DateTime.Now;
                        objMovement.i_UpdateUserId = systemUserId;
                        objMovement.r_TotalQuantity = total;
                        objMovement.i_ProcessTypeId = (int)Enumeratores.SiNo.Si;
                        message = "Se generó el movimiento satisfactoriamente.";

                    }

                }


                Ctx.SaveChanges();
                return message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
        }

        //public bool DiscardMovement(string movementId)
        //{

        //}

    }
}
