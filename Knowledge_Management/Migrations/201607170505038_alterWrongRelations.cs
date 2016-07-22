namespace Knowledge_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterWrongRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbl_employee_job", "fk_job", "dbo.tbl_job");
            DropIndex("dbo.tbl_employee_job", new[] { "fk_job" });
            DropColumn("dbo.tbl_question_solutions", "fk_employee");
            RenameColumn(table: "dbo.tbl_question_solutions", name: "tbl_employee_pkey", newName: "fk_employee");
            RenameIndex(table: "dbo.tbl_question_solutions", name: "IX_tbl_employee_pkey", newName: "IX_fk_employee");
            AddColumn("dbo.tbl_employee_job", "tbl_job_pkey", c => c.Int());
            CreateIndex("dbo.tbl_employee", "fk_job");
            CreateIndex("dbo.tbl_employee_job", "tbl_job_pkey");
            AddForeignKey("dbo.tbl_employee", "fk_job", "dbo.tbl_job", "pkey");
            AddForeignKey("dbo.tbl_employee_job", "tbl_job_pkey", "dbo.tbl_job", "pkey");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_employee_job", "tbl_job_pkey", "dbo.tbl_job");
            DropForeignKey("dbo.tbl_employee", "fk_job", "dbo.tbl_job");
            DropIndex("dbo.tbl_employee_job", new[] { "tbl_job_pkey" });
            DropIndex("dbo.tbl_employee", new[] { "fk_job" });
            DropColumn("dbo.tbl_employee_job", "tbl_job_pkey");
            RenameIndex(table: "dbo.tbl_question_solutions", name: "IX_fk_employee", newName: "IX_tbl_employee_pkey");
            RenameColumn(table: "dbo.tbl_question_solutions", name: "fk_employee", newName: "tbl_employee_pkey");
            AddColumn("dbo.tbl_question_solutions", "fk_employee", c => c.Int());
            CreateIndex("dbo.tbl_employee_job", "fk_job");
            AddForeignKey("dbo.tbl_employee_job", "fk_job", "dbo.tbl_job", "pkey", cascadeDelete: true);
        }
    }
}
