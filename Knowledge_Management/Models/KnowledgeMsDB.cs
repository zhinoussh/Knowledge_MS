namespace Knowledge_Management.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class KnowledgeMsDB : DbContext
    {
        public KnowledgeMsDB()
            : base("name=KnowledgeMsDB")
        {
        }

        public virtual DbSet<tbl_strategy> tbl_strategy { get; set; }
        public virtual DbSet<tbl_department> tbl_department { get; set; }
        public virtual DbSet<tbl_department_objectives> tbl_department_objectives { get; set; }
        public virtual DbSet<tbl_employee> tbl_employee { get; set; }
        public virtual DbSet<tbl_employee_job> tbl_employee_job { get; set; }
        public virtual DbSet<tbl_job> tbl_job { get; set; }
        public virtual DbSet<tbl_job_description> tbl_job_description { get; set; }
        public virtual DbSet<tbl_login> tbl_login { get; set; }
        public virtual DbSet<tbl_question_keywords> tbl_question_keywords { get; set; }
        public virtual DbSet<tbl_question_solutions> tbl_question_solutions { get; set; }
        public virtual DbSet<tbl_solution_uploads> tbl_solution_uploads { get; set; }
        public virtual DbSet<tbl_questions> tbl_questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<tbl_department>()
                .HasMany(e => e.tbl_department_objectives)
                .WithOptional(e => e.tbl_department)
                .HasForeignKey(e => e.fk_department)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_department>()
                .HasMany(e => e.tbl_employee)
                .WithOptional(e => e.tbl_department)
                .HasForeignKey(e => e.fk_department);

            modelBuilder.Entity<tbl_department>()
                .HasMany(e => e.tbl_job)
                .WithOptional(e => e.tbl_department)
                .HasForeignKey(e => e.fk_department)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_employee>()
                .Property(e => e.personel_code)
                .IsUnicode(false);

            //modelBuilder.Entity<tbl_employee>()
            //    .HasMany(e => e.tbl_employee_job)
            //    .WithOptional(e => e.tbl_employee)
            //    .HasForeignKey(e => e.fk_employee)
            //    .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_employee>()
                .HasMany(e => e.tbl_login)
                .WithOptional(e => e.tbl_employee)
                .HasForeignKey(e => e.fk_emp)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_employee>()
                .HasMany(e => e.tbl_questions)
                .WithOptional(e => e.tbl_employee)
                .HasForeignKey(e => e.fk_employee)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_job>()
                .HasMany(e => e.tbl_employee)
                .WithOptional(e => e.tbl_job)
                .HasForeignKey(e => e.fk_job);
               // .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_job>()
                .HasMany(e => e.tbl_job_description)
                .WithOptional(e => e.tbl_job)
                .HasForeignKey(e => e.fk_job)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_login>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_login>()
                .Property(e => e.pass)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_login>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_questions>()
                .HasMany(e => e.tbl_question_keywords)
                .WithOptional(e => e.tbl_questions)
                .HasForeignKey(e => e.fk_question)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_questions>()
                .HasMany(e => e.tbl_question_solutions)
                .WithOptional(e => e.tbl_questions)
                .HasForeignKey(e => e.fk_question)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tbl_question_solutions>()
               .HasMany(e => e.tbl_solution_uploads)
               .WithOptional(e => e.tbl_question_solutions)
               .HasForeignKey(e => e.fk_solution)
               .WillCascadeOnDelete();
        }
    }
}
