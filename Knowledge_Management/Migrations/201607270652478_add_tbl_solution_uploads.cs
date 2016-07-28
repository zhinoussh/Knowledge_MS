namespace Knowledge_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tbl_solution_uploads : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_solution_uploads",
                c => new
                    {
                        pkey = c.Long(nullable: false, identity: true),
                        fk_solution = c.Long(),
                        file_path = c.String(),
                    })
                .PrimaryKey(t => t.pkey)
                .ForeignKey("dbo.tbl_question_solutions", t => t.fk_solution, cascadeDelete: true)
                .Index(t => t.fk_solution);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_solution_uploads", "fk_solution", "dbo.tbl_question_solutions");
            DropIndex("dbo.tbl_solution_uploads", new[] { "fk_solution" });
            DropTable("dbo.tbl_solution_uploads");
        }
    }
}
