using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class PieceInfo
    {
        public int MES_PCE_IDENT_NO { get; set; }

        public string PCE_DISPLAY_NO { get; set; }

        public string LIFT_NO { get; set; }

        public string EXTRL_PCE_DISPLAY_NO { get; set; }

        public double PCE_WT { get; set; }

        public double PCE_IMP_WT { get; set; }

        public double PCE_WDT { get; set; }

        public double PCE_IMP_WDT { get; set; }

        public double PCE_THK { get; set; }

        public double PCE_IMP_THK { get; set; }

        public string LOC_CD { get; set; }

        public string FIELD_CD { get; set; }

        public string FIELD_SHORT_DESC { get; set; }

        public string PREV_LOC_CD { get; set; }

        public string PREV_PREV_LOC_CD { get; set; }

        public string LEGACY_LINEUP_NO { get; set; }

        public string LINEUP_NO { get; set; }

        public string NEXT_OP_CD { get; set; }

        public string NEXT_OP_OUTSIDE_PROCESS_CD { get; set; }

        public string NEXT_OP_PROCESS_TYPE { get; set; }

        public string PREV_OP_CD { get; set; }

        public string BLK_FLG { get; set; }

        public double? PCE_OUT_DIAM { get; set; }

        public double? PCE_IMP_OUT_DIAM { get; set; }

        public string PKG_INSTR_CD { get; set; }

        public string ABBREV_CUST_NAME { get; set; }

        public int? SALES_ORD_NO { get; set; }

        public string SALES_ORD_REL_NO { get; set; }

        public int? CAT_NO { get; set; }

        public string LUB_FLG { get; set; }

        public string LUB_CD { get; set; }

        public string HI_LEV_PROD { get; set; }

        public string PCE_SOURCE_CD { get; set; }

        public string PURCH_PCE_FORM_CD { get; set; }

        public string PCE_COMNT { get; set; }

        public double PCE_LGT { get; set; }

        public double PCE_IMP_LGT { get; set; }

        public int HT_NO { get; set; }

        public string GRD_CD { get; set; }

        public string COAT_CD { get; set; }

        public string SURF_FIN_CD { get; set; }

        public string ANN_CD { get; set; }

        public string TRANS_SLAB_FLAG { get; set; }

        public double PCE_IN_DIAM { get; set; }

        public double PCE_IMP_IN_DIAM { get; set; }

        public string PAINT_FLG { get; set; }

        public string PAINT_DESC { get; set; }

        public string PCE_CNT { get; set; }

        public string BILL_OF_LADING_NO { get; set; }

        public string MATL_STATE_CD { get; set; }

        public string PROD_ORD_NO { get; set; }

        public int? MILL_ORD_NO { get; set; }

        public string PROCESS_COIL_SEQ_NO { get; set; }

        public string MATL_PHYS_FORM_CD { get; set; }

        public int? PROCESS_STEP_IDENT_NO { get; set; }

        public string PCE_DOUBLE_UP_FLG { get; set; }

        public string OVRRIDE_MET_HOLD_FLG { get; set; }

        public string ANN_LOT_NO { get; set; }

        public string LOC_CD_UPD_EST { get; set; }

        public string PREV_OP_CD_EST { get; set; }

        public string NEXT_NEXT_OP_CD { get; set; }

        public string ORIGNL_PCE_DISPLAY_NO { get; set; }

        public string ORIGN_COUNTRY_CD { get; set; }

        public string STOR_FLG { get; set; }

        public DateTime? HMILL_ROLL_EST { get; set; }

        public string CUSTOM_COUNTRY_CD { get; set; }

        public string ORIGN_COUNTRY_CD_TEXT { get; set; }

        public string CUSTOM_COUNTRY_CD_TEXT { get; set; }

        public string LABEL_COUNTRY_CD { get; set; }

        public string LABEL_COUNTRY_CD_TEXT { get; set; }

        public string CORE_LINR_WT { get; set; }

        public string CORE_LINR_WT_IMP { get; set; }

        public string TUBE_ROUND_OUT_DIM { get; set; }

        public string TUBE_ROUND_OUT_DIM_IMP { get; set; }

        public string BASIC_PROD_CD { get; set; }

        public int? LEGACY_CUST_NO { get; set; }

        public int? CUST_NO { get; set; }

        public string DEVAL_SCRAP_FLG { get; set; }

        public string STL_FAM_CD { get; set; }

        public string TYP { get; set; }

        public string LUB_COAT_WT_CD { get; set; }

        public string SLIT_FLG { get; set; }

        public string HSLA_GRD_FLG { get; set; }

        public string DEVAL_FLG { get; set; }

        public string CHEM_TREAT_FLG { get; set; }

        public int CAMPGNID { get; set; }

        public int CAMPID { get; set; }

        public string LAST_PROD_ORD_NO { get; set; }

        public string DEVAL_SCRAP_EST { get; set; }

        public double GEWBRUTTO { get; set; }

        public double GEWBRUTTO_IMP { get; set; }

        public string WEIGHTDEFAULT { get; set; }

        public string WEIGHTDEFAULT_IMP { get; set; }
        public string VCId { get; set; }
    }
    public class HSCode
    {

        public int MES_PCE_IDENT_NO { get; set; }

        public string HSCODE { get; set; }

        public string ALLOY { get; set; }
    }
    public class DashBoardView
    {
        public int Coil { get; set; }
        public string CountryofOrigin { get; set; }
        public string CountryofMeltPour { get; set; }
        public string TypeofTechnology { get; set; }
        public string Grade { get; set; }
        public string HSCode10digits { get; set; }
        public string Guage { get; set; }

        public List<ProductResult> lstActiveProducts { get; set; }
    }

    public class Port
    {

        public int Id { get; set; }

        public string Town { get; set; }

        public string Province { get; set; }

        public string Ports { get; set; }

        public string ReceiptLocation { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
