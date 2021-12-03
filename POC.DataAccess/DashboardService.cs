using POC.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF = POC.Model;
namespace POC.DataAccess
{
    public class DashboardService : ConnectionHelper
    {
        EF.POCEntities db = null;
        public DashboardService()
        {
            db = new EF.POCEntities(EntityConnectionString);
        }
        public DashboardService(ObjectContext context)
        {
            db = context as EF.POCEntities;
        }
        public ObjectContext DbContext
        {
            get
            {
                return db as ObjectContext;
            }
        }


        public IQueryable<PieceInfo> PieceInfos
        {
            get
            {
                return from r in db.PieceInfo
                       select new PieceInfo
                       {
                           MES_PCE_IDENT_NO = r.MES_PCE_IDENT_NO,
                           PCE_DISPLAY_NO = r.PCE_DISPLAY_NO,
                           LIFT_NO = r.LIFT_NO,
                           EXTRL_PCE_DISPLAY_NO = r.EXTRL_PCE_DISPLAY_NO,
                           PCE_WT = r.PCE_WT,
                           PCE_IMP_WT = r.PCE_IMP_WT,
                           PCE_WDT = r.PCE_WDT,
                           PCE_IMP_WDT = r.PCE_IMP_WDT,
                           PCE_THK = r.PCE_THK,
                           PCE_IMP_THK = r.PCE_IMP_THK,
                           LOC_CD = r.LOC_CD,
                           FIELD_CD = r.FIELD_CD,
                           FIELD_SHORT_DESC = r.FIELD_SHORT_DESC,
                           PREV_LOC_CD = r.PREV_LOC_CD,
                           PREV_PREV_LOC_CD = r.PREV_PREV_LOC_CD,
                           LEGACY_LINEUP_NO = r.LEGACY_LINEUP_NO,
                           LINEUP_NO = r.LINEUP_NO,
                           NEXT_OP_CD = r.NEXT_OP_CD,
                           NEXT_OP_OUTSIDE_PROCESS_CD = r.NEXT_OP_OUTSIDE_PROCESS_CD,
                           NEXT_OP_PROCESS_TYPE = r.NEXT_OP_PROCESS_TYPE,
                           PREV_OP_CD = r.PREV_OP_CD,
                           BLK_FLG = r.BLK_FLG,
                           PCE_OUT_DIAM = r.PCE_OUT_DIAM,
                           PCE_IMP_OUT_DIAM = r.PCE_IMP_OUT_DIAM,
                           PKG_INSTR_CD = r.PKG_INSTR_CD,
                           ABBREV_CUST_NAME = r.ABBREV_CUST_NAME,
                           SALES_ORD_NO = r.SALES_ORD_NO,
                           SALES_ORD_REL_NO = r.SALES_ORD_REL_NO,
                           CAT_NO = r.CAT_NO,
                           LUB_FLG = r.LUB_FLG,
                           LUB_CD = r.LUB_CD,
                           HI_LEV_PROD = r.HI_LEV_PROD,
                           PCE_SOURCE_CD = r.PCE_SOURCE_CD,
                           PURCH_PCE_FORM_CD = r.PURCH_PCE_FORM_CD,
                           PCE_COMNT = r.PCE_COMNT,
                           PCE_LGT = r.PCE_LGT,
                           PCE_IMP_LGT = r.PCE_IMP_LGT,
                           HT_NO = r.HT_NO,
                           GRD_CD = r.GRD_CD,
                           COAT_CD = r.COAT_CD,
                           SURF_FIN_CD = r.SURF_FIN_CD,
                           ANN_CD = r.ANN_CD,
                           TRANS_SLAB_FLAG = r.TRANS_SLAB_FLAG,
                           PCE_IN_DIAM = r.PCE_IN_DIAM,
                           PCE_IMP_IN_DIAM = r.PCE_IMP_IN_DIAM,
                           PAINT_FLG = r.PAINT_FLG,
                           PAINT_DESC = r.PAINT_DESC,
                           PCE_CNT = r.PCE_CNT,
                           BILL_OF_LADING_NO = r.BILL_OF_LADING_NO,
                           MATL_STATE_CD = r.MATL_STATE_CD,
                           PROD_ORD_NO = r.PROD_ORD_NO,
                           MILL_ORD_NO = r.MILL_ORD_NO,
                           PROCESS_COIL_SEQ_NO = r.PROCESS_COIL_SEQ_NO,
                           MATL_PHYS_FORM_CD = r.MATL_PHYS_FORM_CD,
                           PROCESS_STEP_IDENT_NO = r.PROCESS_STEP_IDENT_NO,
                           PCE_DOUBLE_UP_FLG = r.PCE_DOUBLE_UP_FLG,
                           OVRRIDE_MET_HOLD_FLG = r.OVRRIDE_MET_HOLD_FLG,
                           ANN_LOT_NO = r.ANN_LOT_NO,
                           LOC_CD_UPD_EST = r.LOC_CD_UPD_EST,
                           PREV_OP_CD_EST = r.PREV_OP_CD_EST,
                           NEXT_NEXT_OP_CD = r.NEXT_NEXT_OP_CD,
                           ORIGNL_PCE_DISPLAY_NO = r.ORIGNL_PCE_DISPLAY_NO,
                           ORIGN_COUNTRY_CD = r.ORIGN_COUNTRY_CD,
                           STOR_FLG = r.STOR_FLG,
                           HMILL_ROLL_EST = r.HMILL_ROLL_EST,
                           CUSTOM_COUNTRY_CD = r.CUSTOM_COUNTRY_CD,
                           ORIGN_COUNTRY_CD_TEXT = r.ORIGN_COUNTRY_CD_TEXT,
                           CUSTOM_COUNTRY_CD_TEXT = r.CUSTOM_COUNTRY_CD_TEXT,
                           LABEL_COUNTRY_CD = r.LABEL_COUNTRY_CD,
                           LABEL_COUNTRY_CD_TEXT = r.LABEL_COUNTRY_CD_TEXT,
                           CORE_LINR_WT = r.CORE_LINR_WT,
                           CORE_LINR_WT_IMP = r.CORE_LINR_WT_IMP,
                           TUBE_ROUND_OUT_DIM = r.TUBE_ROUND_OUT_DIM,
                           TUBE_ROUND_OUT_DIM_IMP = r.TUBE_ROUND_OUT_DIM_IMP,
                           BASIC_PROD_CD = r.BASIC_PROD_CD,
                           LEGACY_CUST_NO = r.LEGACY_CUST_NO,
                           CUST_NO = r.CUST_NO,
                           DEVAL_SCRAP_FLG = r.DEVAL_SCRAP_FLG,
                           STL_FAM_CD = r.STL_FAM_CD,
                           TYP = r.TYP,
                           LUB_COAT_WT_CD = r.LUB_COAT_WT_CD,
                           SLIT_FLG = r.SLIT_FLG,
                           HSLA_GRD_FLG = r.HSLA_GRD_FLG,
                           DEVAL_FLG = r.DEVAL_FLG,
                           CHEM_TREAT_FLG = r.CHEM_TREAT_FLG,
                           CAMPGNID = r.CAMPGNID,
                           CAMPID = r.CAMPID,
                           LAST_PROD_ORD_NO = r.LAST_PROD_ORD_NO,
                           DEVAL_SCRAP_EST = r.DEVAL_SCRAP_EST,
                           GEWBRUTTO = r.GEWBRUTTO,
                           GEWBRUTTO_IMP = r.GEWBRUTTO_IMP,
                           WEIGHTDEFAULT = r.WEIGHTDEFAULT,
                           WEIGHTDEFAULT_IMP = r.WEIGHTDEFAULT_IMP,
                           VCId = r.VCId
                       };
            }
        }

        public IQueryable<HSCode> HSCodes
        {
            get
            {
                return from h in db.Hscode
                       select new HSCode
                       {
                           MES_PCE_IDENT_NO = h.MES_PCE_IDENT_NO,
                           HSCODE = h.HSCODE1,
                           ALLOY = h.ALLOY,
                       };
            }
        }
        public string GetHSCode(int CoilNumber)
        {
            var data = (from h in db.PieceInfo
                        join s in db.Hscode on h.MES_PCE_IDENT_NO equals s.MES_PCE_IDENT_NO
                        where s.MES_PCE_IDENT_NO==CoilNumber
                        select s).FirstOrDefault();
            return data?.HSCODE1;
        }
        public void UpdateVCId(int CoilNumber,string VCId)
        {
            var data = db.PieceInfo.Where(p=>p.MES_PCE_IDENT_NO == CoilNumber).Single();
            if (data != null)
            {
                data.VCId = VCId;
                db.SaveChanges();
            }
        }
    }
}
