using System;
namespace Knowledge_Management.DAL
{
    public interface IKnowledgeMSDAL
    {
        void change_confirm_status_solution(long soution_id);
        void change_confirm_status_upload(long upload_id);
        bool check_userinRole(string pcode, string role);
        void Delete_Solution(long id_sol);
        void DeleteDepartment(int id);
        void DeleteEmployee(int id);
        void DeleteJob(int id);
        void DeleteJobDescription(int id);
        void DeleteObjective(int id);
        void DeleteQuestion(long q_id);
        void DeleteStrategy(int id);
        void DeleteUpload(long upload_id);
        void Edit_upload_description(long upload_id, string upload_desc);
        System.Collections.Generic.List<Knowledge_Management.Models.tbl_department> get_all_Departments();
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.QuestionViewModel> get_all_Questions();
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.QuestionViewModel> get_all_Questions_by_alljobs_department(int dep_id);
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.QuestionViewModel> get_all_Questions_by_details(int jobDesc_id, int depObj_id, int strategy_id);
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.QuestionViewModel> get_all_Questions_by_employee(string pcode);
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.QuestionViewModel> get_all_Questions_by_job(int job_id);
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.QuestionViewModel> get_all_Questionsby_key(long key_id);
        System.Collections.Generic.List<Knowledge_Management.Models.tbl_strategy> get_all_strategies();
        int get_count_solution_uploads(long new_sol_id);
        string get_department_name(int dep_id);
        string get_department_objective(int id);
        System.Collections.Generic.List<Knowledge_Management.Models.tbl_department_objectives> get_Department_Objectives(int dep_id);
        System.Collections.Generic.List<string> get_Employee_byId(int emp_id);
        System.Collections.Generic.List<string> get_Employee_prop(string pcode);
        System.Collections.Generic.List<Knowledge_Management.Models.Employee> get_Employees();
        string get_file_path(long upload_id);
        string get_job_description(int id);
        System.Collections.Generic.List<Knowledge_Management.Models.tbl_job_description> get_JobDescriptions(int job_id);
        System.Collections.Generic.List<Knowledge_Management.Models.tbl_job> get_Jobs(int dep_id);
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.SearchKeywordViewModel> get_Keywords(int jobDesc_id, int depObj_id, int st_id);
        Knowledge_Management.Areas.User.ViewModels.QuestionViewModel get_question_byId(int q_id);
        string get_question_name(int q_id);
        string get_Question_Writer(int question_id);
        Knowledge_Management.Areas.User.ViewModels.FullSolutionViewModel get_Solution_by_id(long solution_id);
        string[][] get_Solutions(long question_id);
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.SolutionEmployeeViewModel> get_Solutions_by_employee(int emp_id);
        System.Collections.Generic.List<Knowledge_Management.Areas.User.ViewModels.SolutionEmployeeViewModel> get_Solutions_by_Question(long question_id, int confirm);
        string get_strategy_description(int id);
        System.Collections.Generic.List<Knowledge_Management.Models.tbl_solution_uploads> get_uploads_by_solution(long solution_id, int confirm);
        string[] get_user_roles(string pcode);
        void initialise_admin_user(string encrypt_pass);
        void InsertDepartment(int dep_id, string dep_name);
        int InsertEmployee(int emp_id, string first_name, string last_name, string personel_code, int dep_id, int job_id, string password, bool data_entry, bool data_view);
        void InsertJob(int job_id, string job_name, int dep_id);
        void InsertJobDescription(int job_desc_id, string job_desc, int job_id);
        long InsertNewSolution(long soution_id, long q_id, string new_solution, string pcode);
        void InsertNewUpload(long solution_id, string filepath, string upload_desc);
        void InsertObjective(int obj_id, string obj_name, int dep_id);
        void InsertQuestion(long q_id, string q_subject, int? depObjId, long? jobDescId, int? strategyId, System.Collections.Generic.List<string> keywords, string pcode);
        void InsertStrategy(int st_id, string st_name);
        bool login(string username, string password);
    }
}
