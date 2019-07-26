using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BE.Message;
using BL.Eso;
using DAL.Plan;
using DAL.Warehouse;
using static BE.Common.Enumeratores;
using static BE.Eso.RecipesCustom;

namespace BL.WareHouse
{
   public class WarehouseBl
    {
        public List<string> SearchProduct(string name)
        {
            return new WarehouseDal().SearchProduct(name);
        }

        public MessageCustom SaveUpdateRecipe(BoardPrintRecipes data, int userId, int nodeId)
        {

            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {
                    var result = new WarehouseDal().SaveUpdateRecipe(data, userId, nodeId);
                    if (!result)
                    {
                        throw new Exception("Sucedió un error al grabar la receta, por favor vuelva intentar.");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Message = "La receta se guardó correctamente.";
                        var filename = new EsoBl().BuildRecipe(data);
                        if (filename == null)
                        {
                            throw new Exception("Sucedió un error al generar el pdf de la receta, por favor vuelva intentar.");
                        }
                        else
                        {
                            _MessageCustom.Id = filename;
                        }
                    }
                    ts.Complete();
                }

                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = ex.Message;
                return _MessageCustom;
            }
            
        }

        public List<Recipes> GetRecipesByServiceId(string serviceId)
        {
            return new WarehouseDal().GetRecipesByServiceId(serviceId);
        }
    }
}
