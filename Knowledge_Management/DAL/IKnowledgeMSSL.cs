using Knowledge_Management.Areas.Admin.ViewModels;
using Knowledge_Management.Areas.User.ViewModels;
using Knowledge_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowledge_Management.DAL
{
    public interface IKnowledgeMSSL
    {
        IKnowledgeMSDAL DataLayer { get;set; }

        #region HomeController

        string Post_Login(string userName, string passWord, Controller ctrl);

        #endregion HomeController

        #region DepartmentController

        void Post_Add_Edit_Department(DepartmentViewModel vm);
        DepartmentViewModel Get_Delete_Department(int departmentId);
        void Post_Delete_Department(DepartmentViewModel vm);
        Tuple<List<tbl_department>,int> Get_DepartmentTableContent(string filter, string sortDirection, int displayStart, int displayLength);

        #endregion DepartmentController

        #region EmployeeController

        EmployeeViewModel Get_Employee_Index_Page();
        Tuple<int,string> Post_Add_Edit_Employee(EmployeeViewModel e);
        EmployeeViewModel Get_Delete_Employee(int employeeId);
        void Post_Delete_Employee(EmployeeViewModel vm);
        Tuple<List<Employee>, int> Get_EmployeeTableContent(string filter, int sortColumnIndex, string sortDirection, int displayStart, int displayLength);
        List<SelectListItem> GetJobList(int departmentId);

        #endregion EmployeeController

        #region JobController

        JobViewModel Get_Job_Index_Page();
        void Post_Add_Edit_Job(JobViewModel j);
        JobViewModel Get_Delete_Job(int jobId);
        void Post_Delete_Job(JobViewModel vm);
        Tuple<List<tbl_job>, int> Get_JobTableContent(int departmentId, string filter, string sortDirection, int displayStart, int displayLength);

        #endregion JobController

        #region JobDescriptionController

        JobDescriptionViewModel Get_JobDescription_Index_Page();
        void Post_Add_Edit_JobDescription(JobDescriptionViewModel j);
        JobDescriptionViewModel Get_Delete_JobDescription(int JobDescriptionId);
        void Post_Delete_JobDescription(JobDescriptionViewModel vm);
        Tuple<List<tbl_job_description>, int> Get_JobDescriptionTableContent(int jobId, string filter, string sortDirection, int displayStart, int displayLength);

        #endregion JobDescriptionController

        #region DepartmentObjectiveController

        DepartmentObjectiveViewModel Get_DepartmentObjective_Index_Page(int departmentId);
        void Post_Add_Edit_DepartmentObjective(DepartmentObjectiveViewModel j);
        DepartmentObjectiveViewModel Get_Delete_DepartmentObjective(int DepartmentObjectiveId);
        void Post_Delete_DepartmentObjective(DepartmentObjectiveViewModel vm);
        Tuple<List<tbl_department_objectives>, int> Get_DepartmentObjectiveTableContent(int departmentId, string filter, string sortDirection, int displayStart, int displayLength);

        #endregion DepartmentObjectiveController

        #region StrategyController

        void Post_Add_Edit_Strategy(StrategyViewModel s);
        StrategyViewModel Get_Delete_Strategy(int StrategyId);
        void Post_Delete_Strategy(StrategyViewModel vm);
        Tuple<List<tbl_strategy>, int> Get_StrategyTableContent( string filter, string sortDirection, int displayStart, int displayLength);

        #endregion StrategyController

         #region EntrybyDetailController

        DetailQuestionViewModel Get_EntrybyDetail_Index_Page();
        DetailQuestionViewModel Get_Delete_QuestionbyDetail(int questionId);
        void Post_Delete_QuestionbyDetail(DetailQuestionViewModel vm);
        Tuple<List<QuestionViewModel>, int> Get_QuestionbyDetailTableContent(int depOjectiveId,int jobDescriptionId,int strategyId, string filter, string sortDirection, int displayStart, int displayLength);
        List<SelectListItem> GetJobDescriptionList(int jobId);
        List<SelectListItem> GetDepartmentObjectioveList(int departmentId);

        #endregion EntrybyDetailController

        #region EntrybyJobController

        JobDepQuestionViewModel Get_EntrybyJob_Index_Page();
        JobDepQuestionViewModel Get_Delete_QuestionbyJob(int questionId);
        void Post_Delete_QuestionbyJob(JobDepQuestionViewModel vm);
        Tuple<List<QuestionViewModel>, int> Get_QuestionbyJobTableContent(int departmentId, int jobId, string filter, string sortDirection, int displayStart, int displayLength);

        #endregion EntrybyJobController

        #region EntrybyEmployeeController

        //Question
        EmployeeQuestionViewModel Get_Question_Index_Page(int employeeId);
        Tuple<List<QuestionViewModel>, int> Get_QuestionbyEmployeeTableContent(int employeeId, string filter, string sortDirection, int displayStart, int displayLength);
        EmployeeQuestionViewModel Get_Delete_QuestionbyEmployee(int questionId);
        void Post_Delete_QuestionbyEmployee(EmployeeQuestionViewModel vm);

        //Solution
        SolutionViewModel Get_QuestionSolutions_Index_Page(int questionId);
        EmployeeQuestionViewModel Get_EmployeeSolutions_Index_Page(int employeeId);
        Tuple<List<SolutionEmployeeViewModel>, int> Get_SolutionForQuestionTableContent(int questionId, string filter, string sortDirection, int displayStart, int displayLength);
        Tuple<List<SolutionEmployeeViewModel>, int> Get_SolutionForEmployeeTableContent(int employeeId, string filter, string sortDirection, int displayStart, int displayLength);
        SolutionEmployeeViewModel Get_Delete_Solution(int solutionId);
        void Post_Delete_Solution(SolutionEmployeeViewModel vm, Controller ctrl);
        FullSolutionViewModel Get_FullSolution(int solutionId);
        EmployeeQuestionViewModel Post_Confirm_Solution_for_Employee(int solutionId, int employeeId);
        SolutionViewModel Post_Confirm_Solution_for_Question(int solutionId, int questionId);

        //Upload
        UploadViewModel Get_Delete_Upload(int uploadId);
        void Post_Delete_Upload(UploadViewModel vm,Controller ctrl);
        Tuple<List<tbl_solution_uploads>, int> Get_UploadForSolutionTableContent(int solutionId, int displayStart, int displayLength);
        Tuple<byte[], string> GetFilePropertie(int uploadId, Controller ctrl);
        FullSolutionViewModel Post_Confirm_Upload(int uploadId,int solutionId);

        #endregion EntrybyEmployeeController

        #region InsertInfoController

        QuestionViewModel Get_NewQuestion_Page(int questionId,Controller ctrl);

        void Post_Add_Edit_Question(QuestionViewModel vm,Controller ctrl);

        QuestionViewModel Get_Delete_Question(int questionId);
        void Post_Delete_Question(QuestionViewModel vm);
        Tuple<List<QuestionViewModel>, int> Get_UserQuestionsTableContent(Controller ctrl,string filter, string sortDirection, int displayStart, int displayLength);

        #endregion InsertInfoController

        #region SearchInfoController

        Tuple<List<QuestionViewModel>, int> Get_AllQuestionsTableContent(int keywordId, string filter, string sortDirection, int displayStart, int displayLength);
        SearchKeywordViewModel Get_SearchKeyword_Page(Controller ctrl);
        Tuple<List<SearchKeywordViewModel>, int> Get_KeywordTableContent(int depOjectiveId, int jobDescriptionId, int strategyId, string filter, string sortDirection, int displayStart, int displayLength);
        KeywordDetailViewModel Get_KeywordDetails(KeywordDetailViewModel vm);
        
        #endregion SearchInfoController


    }
}