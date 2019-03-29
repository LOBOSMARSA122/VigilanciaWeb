using BE.Common;
using BE.Warehouse;
using DAL.ProductWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Eso.RecipesCustom;

namespace BL.ProductWarehouse
{
    public class InputOutputBl
    {
        InputOutputDal _InputOutput = new InputOutputDal();

        public string GenerateMovementOutput(BoardPrintRecipes data)
        {
            string message = "";
            try
            {

                string movementId = _InputOutput.AddMovement(data);

                _InputOutput.AddMovementDetail(data, movementId);

                if (movementId != null)
                {
                    message = _InputOutput.ProcessMovementOutput(movementId, data.InsertUserId.Value);
                }

                return movementId;
            }
            catch (Exception ex)
            {
                message = ex.InnerException.Message;
                return message;
            }
        }

        public BoardMovement GetDataMovements(BoardMovement data)
        {
            
            try
            {
                var objData = _InputOutput.GetData(data);
                return objData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GenerateMovementInput(BoardPrintRecipes data)
        {
            string message = "";
            try
            {

                string movementId = _InputOutput.AddMovement(data);

                _InputOutput.AddMovementDetail(data, movementId);

                if (movementId != null)
                {
                    message = _InputOutput.ProcessMovementInput(movementId, data.InsertUserId.Value);
                }

                return movementId;
            }
            catch (Exception ex)
            {
                message = ex.InnerException.Message;
                return message;
            }
        }

        public string ProcessTransfer(BoradTransferProducts data)
        {
            try
            {
                string movemenstId = "";
                foreach (var item in data.List)
                {
                    item.NodeId = data.NodeId;
                    item.InsertUserId = data.InsertUserId;
                    if (item.MovementTypeId == (int)Enumeratores.MovementType.Ingreso)
                    {
                        
                        string movementId = _InputOutput.AddMovement(item);

                        if (movementId != null)
                        {
                            _InputOutput.AddMovementDetail(item, movementId);
                            _InputOutput.ProcessMovementInput(movementId, data.InsertUserId.Value);
                        }
                        movemenstId = movemenstId + movementId;
                    }
                    else if (item.MovementTypeId == (int)Enumeratores.MovementType.Egreso)
                    {

                        string movementId = _InputOutput.AddMovement(item);
                        if (movementId != null)
                        {
                            _InputOutput.AddMovementDetail(item, movementId);
                            _InputOutput.ProcessMovementOutput(movementId, data.InsertUserId.Value);
                        }
                        movemenstId = movemenstId+ "|" + movementId;
                    }
                    
                }
                
                return movemenstId;
            }
            catch (Exception ex)
            {
                return null;
            }
            


        }

    }
}
