using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL.Common
{
    public class Utils
    {
        private DatabaseContext ctx = new DatabaseContext();

        #region PK
        public string GetPrimaryKey(int nodeId, int tableId, string pre)
        {
            var secuentialId = GetNextSecuentialId(nodeId, tableId);

            return string.Format("N{0}-{1}{2}", nodeId.ToString("000"), pre, secuentialId.ToString("000000000"));

        }
        //public string GetPrimaryKey(int nodeId, int tableId, string pre)
        //{
        //    var scope = new TransactionScope(
        //        TransactionScopeOption.RequiresNew,
        //        new TransactionOptions()
        //        {

        //            IsolationLevel = IsolationLevel.Snapshot

        //        });

        //    using (scope)
        //    {
        //        using (var ts = new TransactionScope())
        //        {
        //            var secuentialId = GetNextSecuentialId(nodeId, tableId);

        //            ts.Complete();
        //            scope.Complete();
        //            return string.Format("N{0}-{1}{2}", nodeId.ToString("000"), pre, secuentialId.ToString("000000000"));

        //        }

        //    }

        //}

        public string FormarPk(int nodeId, int tableId, string pre, int secuentialId)
        {
            return string.Format("N{0}-{1}{2}", nodeId.ToString("000"), pre, secuentialId.ToString("000000000"));

        }

        public int GetFirstPrimaryKey(int nodeId, int tableId, int lot)
        {
            //var scope = new TransactionScope(
            //    TransactionScopeOption.RequiresNew,
            //    new TransactionOptions()
            //    {

            //        IsolationLevel = IsolationLevel.RepeatableRead

            //    });

            //using (scope)
            //{
                using (var ts = new TransactionScope())
                {
                    var lastSecuential = ReserveRangeSecuentials(nodeId, tableId, lot);

                    lastSecuential = lastSecuential + 1;

                    var firstSecuential = lastSecuential - lot;

                    ts.Complete();
                    //scope.Complete();
                    return firstSecuential;

                }

            //}

        }

        public int ReserveRangeSecuentials(int pintNodeId, int pintTableId, int lot)
        {
            var value = 0;
            var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {

                    IsolationLevel = IsolationLevel.RepeatableRead

                });

            using (scope)
            {
                using (var oCtx = new DatabaseContext())
                {
                    var objSecuential = (from a in oCtx.Secuential
                        where a.i_TableId == pintTableId && a.i_NodeId == pintNodeId
                        select a).SingleOrDefault();

                    if (objSecuential != null)
                    {
                        objSecuential.i_SecuentialId = objSecuential.i_SecuentialId + lot;
                    }
                    else
                    {
                        objSecuential = new SecuentialBE();
                        objSecuential.i_NodeId = pintNodeId;
                        objSecuential.i_TableId = pintTableId;
                        objSecuential.i_SecuentialId = 0;
                        oCtx.Secuential.Add(objSecuential);
                    }

                    oCtx.SaveChanges();

                    value = objSecuential.i_SecuentialId.Value;
                }

                scope.Complete();
                return value;
            }
        }

        public int GetNextSecuentialId(int pintNodeId, int pintTableId)
        {
            var value = 0;
            var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.RepeatableRead
                });

            using (scope)
            {
                using (var oCtx = new DatabaseContext())
                {
                    var objSecuential = (from a in oCtx.Secuential
                        where a.i_TableId == pintTableId && a.i_NodeId == pintNodeId
                        select a).SingleOrDefault();

                    // Actualizar el campo con el nuevo valor a efectos de reservar el ID autogenerado para evitar colisiones entre otros nodos
                    if (objSecuential != null)
                    {
                        objSecuential.i_SecuentialId = objSecuential.i_SecuentialId + 1;
                    }
                    else
                    {
                        objSecuential = new SecuentialBE();
                        objSecuential.i_NodeId = pintNodeId;
                        objSecuential.i_TableId = pintTableId;
                        objSecuential.i_SecuentialId = 0;
                        oCtx.Secuential.Add(objSecuential);
                    }

                    int rows = oCtx.SaveChanges();

                    value = objSecuential.i_SecuentialId.Value;
                }

                scope.Complete();
                return value;
            }
        }

        public int GetNextSecuentialId_(int pintNodeId, int pintTableId)
        {
            using (var oCtx = new DatabaseContext())
            {
                var objSecuential = (from a in oCtx.Secuential
                    where a.i_TableId == pintTableId && a.i_NodeId == pintNodeId
                    select a).SingleOrDefault();

                // Actualizar el campo con el nuevo valor a efectos de reservar el ID autogenerado para evitar colisiones entre otros nodos
                if (objSecuential != null)
                {
                    objSecuential.i_SecuentialId = objSecuential.i_SecuentialId + 1;
                }
                else
                {
                    objSecuential = new SecuentialBE();
                    objSecuential.i_NodeId = pintNodeId;
                    objSecuential.i_TableId = pintTableId;
                    objSecuential.i_SecuentialId = 0;
                    oCtx.Secuential.Add(objSecuential);
                }

                oCtx.SaveChanges();
                return objSecuential.i_SecuentialId.Value; 
            }
        }

        #endregion

        public static int GetAge(DateTime birthdate)
        {
            return int.Parse((DateTime.Today.AddTicks(-birthdate.Ticks).Year - 1).ToString());
        }

        #region Exception Handling
        public static string ExceptionFormatter(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MESSAGE: " + ex.Message);
            sb.AppendLine("STACK TRACE: " + ex.StackTrace);
            sb.AppendLine("SOURCE: " + ex.Source);
            if (ex.InnerException != null)
            {
                sb.AppendLine("INNER EXCEPTION MESSAGE: " + ex.InnerException.Message);
                sb.AppendLine("INNER EXCEPTION STACK TRACE: " + ex.InnerException.StackTrace);
            }
            return sb.ToString();
        }
        #endregion



    }
}
