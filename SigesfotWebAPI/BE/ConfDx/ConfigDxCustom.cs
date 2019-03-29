using System;
using System.Collections.Generic;

namespace BE.ConfDx
{
    public class BoardConfigDx
    {
        public int? Take { get; set; }
        public int? Index { get; set; }
        public int? TotalRecords { get; set; }
        public List<ConfigDxCustom> List { get; set; }
    }
    public class ConfigDxCustom
    {
        public string v_ConfigDxId { get; set; }
        public string v_DiseaseId { get; set; }
        public string v_ProductId { get; set; }
        public int i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_ProductName { get; set; }
        public string v_DiseaseName { get; set; }
        public float? r_StockActual { get; set; }

        public int RecordStatus { get; set; }
        public int RecordType { get; set; }
    }
}
