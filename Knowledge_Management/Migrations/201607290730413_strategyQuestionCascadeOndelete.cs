namespace Knowledge_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class strategyQuestionCascadeOndelete : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.tbl_questions", "fk_depObj", "dbo.tbl_department_objectives");
            //DropForeignKey("dbo.tbl_questions", "fk_jobDesc", "dbo.tbl_job_description");
            //DropForeignKey("dbo.tbl_questions", "fk_strategy", "dbo.tbl_strategy");
            //AddForeignKey("dbo.tbl_questions", "fk_depObj", "dbo.tbl_department_objectives", "pkey", cascadeDelete: true);
            //AddForeignKey("dbo.tbl_questions", "fk_jobDesc", "dbo.tbl_job_description", "pkey", cascadeDelete: true);
            //AddForeignKey("dbo.tbl_questions", "fk_strategy", "dbo.tbl_strategy", "pkey", cascadeDelete: true);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.tbl_questions", "fk_strategy", "dbo.tbl_strategy");
            //DropForeignKey("dbo.tbl_questions", "fk_jobDesc", "dbo.tbl_job_description");
            //DropForeignKey("dbo.tbl_questions", "fk_depObj", "dbo.tbl_department_objectives");
            //AddForeignKey("dbo.tbl_questions", "fk_strategy", "dbo.tbl_strategy", "pkey");
            //AddForeignKey("dbo.tbl_questions", "fk_jobDesc", "dbo.tbl_job_description", "pkey");
            //AddForeignKey("dbo.tbl_questions", "fk_depObj", "dbo.tbl_department_objectives", "pkey");
        }
    }
}
