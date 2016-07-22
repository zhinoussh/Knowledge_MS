namespace Knowledge_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterQuestion : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.tbl_questions", new[] { "tbl_job_description_pkey" });
            DropColumn("dbo.tbl_questions", "fk_depObj");
            DropColumn("dbo.tbl_questions", "fk_jobDesc");
            DropColumn("dbo.tbl_questions", "fk_strategy");
            RenameColumn(table: "dbo.tbl_questions", name: "tbl_department_objectives_pkey", newName: "fk_depObj");
            RenameColumn(table: "dbo.tbl_questions", name: "tbl_job_description_pkey", newName: "fk_jobDesc");
            RenameColumn(table: "dbo.tbl_questions", name: "tbl_strategy_pkey", newName: "fk_strategy");
            RenameIndex(table: "dbo.tbl_questions", name: "IX_tbl_department_objectives_pkey", newName: "IX_fk_depObj");
            RenameIndex(table: "dbo.tbl_questions", name: "IX_tbl_strategy_pkey", newName: "IX_fk_strategy");
            AlterColumn("dbo.tbl_questions", "fk_jobDesc", c => c.Long());
            CreateIndex("dbo.tbl_questions", "fk_jobDesc");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tbl_questions", new[] { "fk_jobDesc" });
            AlterColumn("dbo.tbl_questions", "fk_jobDesc", c => c.Int());
            RenameIndex(table: "dbo.tbl_questions", name: "IX_fk_strategy", newName: "IX_tbl_strategy_pkey");
            RenameIndex(table: "dbo.tbl_questions", name: "IX_fk_depObj", newName: "IX_tbl_department_objectives_pkey");
            RenameColumn(table: "dbo.tbl_questions", name: "fk_strategy", newName: "tbl_strategy_pkey");
            RenameColumn(table: "dbo.tbl_questions", name: "fk_jobDesc", newName: "tbl_job_description_pkey");
            RenameColumn(table: "dbo.tbl_questions", name: "fk_depObj", newName: "tbl_department_objectives_pkey");
            AddColumn("dbo.tbl_questions", "fk_strategy", c => c.Int());
            AddColumn("dbo.tbl_questions", "fk_jobDesc", c => c.Int());
            AddColumn("dbo.tbl_questions", "fk_depObj", c => c.Int());
            CreateIndex("dbo.tbl_questions", "tbl_job_description_pkey");
        }
    }
}
