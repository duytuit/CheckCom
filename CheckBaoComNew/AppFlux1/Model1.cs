namespace AppFlux1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<DATA0006> DATA0006 { get; set; }
        public virtual DbSet<DATA0050> DATA0050 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DATA0006>()
                .Property(e => e.WORK_ORDER_NUMBER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.PRIORITY_CODE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.ENGG_RTE_MOD_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.PROD_RTE_MOD_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.BOM_MOD_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_SCH)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_REJ)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_PROD)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.CUST_PART_REV_NO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.BOM_REV_NO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.SCRAP_RATE)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_BAL)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.BASE_WO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.JOB_COST_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.DIRECT_COST)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.PARTS_PER_PANEL)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.PRODUCTION_COST)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.MATERIAL_COST)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.OVERHEAD_COST)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.PLANNED_QTY)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.HARD_LINK_TO_PARENT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.ANALYSIS_CODE_1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.ANALYSIS_CODE_2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.ANALYSIS_CODE_3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.ANALYSIS_CODE_4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.ANALYSIS_CODE_5)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.DATE_CODE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_SCH_PANELS)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_REJ_PANELS)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_PROD_PANELS)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_BAL_PANELS)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.XOUT_UNIT_PTR)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.PARTS_PER_XOUT)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_SCH_XOUT)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_REJ_XOUT)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_PROD_XOUT)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0006>()
                .Property(e => e.QUAN_BAL_XOUT)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.CUSTOMER_PART_NUMBER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.CUSTOMER_PART_DESC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.CP_REV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.QTY_SCH)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.QTY_REJ)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.QTY_ON_HAND)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.QTY_ALLOC)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.EST_SCRAP)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ENGG_ROUTE_MOD_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.PROD_ROUTE_MOD_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.FIXED_SCRAP_RATE)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.CATALOG_NUMBER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.STANDARD_COST)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.STANDARD_MATERIAL)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.STANDARD_OVERHEAD)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.CURRENT_ACT_ACTIVITY)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.CURRENT_ACT_MATERIAL)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ALLOW_EDIT_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.REPORT_UNIT_VALUE1)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.REPORT_UNIT_VALUE2)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.REPORT_UNIT_VALUE3)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.SALES_ORDER_UNIT_VAL)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.SET_UP_COST)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.PROCESS_COST)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.INDIRECT_MATERIAL1)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.INDIRECT_MATERIAL2)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.DIRECT_MATERIAL)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.OVER_HEAD)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.CUR_MONTH_OPEN_BAL)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ANALYSIS_CODE_1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ANALYSIS_CODE_2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ANALYSIS_CODE_3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ACTIVE_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ANALYSIS_CODE_4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.ANALYSIS_CODE_5)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.PLAN_INNER_LAYER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.LAST_SO_PRICE)
                .HasPrecision(21, 8);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.MIN_STOCK)
                .HasPrecision(20, 7);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.LONG_CUSTOMER_PART_NUMBER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.LONG_CUSTOMER_PART_DESC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DATA0050>()
                .Property(e => e.DATE_CODE_FLAG)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
