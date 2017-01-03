using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Knowledge_Management.Models;
using Knowledge_Management.Areas.User.ViewModels;

namespace Knowledge_Management.DAL
{
    public class KnowledgeMSDAL
    {
        KnowledgeMsDB db;


        public KnowledgeMSDAL()
        {
            db = new KnowledgeMsDB();
        }
        public bool login(string username, string password)
        {
            try
            {
                string pass = (new Encryption()).Encrypt(password);
                tbl_login login_user = (from l in db.tbl_login
                               where l.username.Equals(username) & l.pass.Equals(pass)
                               select l).FirstOrDefault();

                return (login_user == null ? false : true);
            }
            catch (Exception e1)
            {
                return false;
            }
        }

        public string[] get_user_roles(string pcode)
        {
            string role=db.tbl_login.Where(x => x.username == pcode).Select(x => x.role).First();

            if (role == "1")
                return new string[] { "Admin" };
            else if (role == "2")
                return new string[] { "DataEntry", "DataView" };
            else if (role == "3")
                return new string[] { "DataEntry" };
            else if (role == "4")
                return new string[] { "DataView" };
            else
                return new string[] { "Public" };

        }

        public bool check_userinRole(string pcode, string role)
        { 
            string role_code="";
            if(role=="Admin")
                role_code="1";
            else if(role=="DataEntry")
                role_code="3";
            else if(role=="DataView")
                role_code="4";
            else if(role=="Public")
                role_code = "5";

            tbl_login l= db.tbl_login.Where(x => x.username == pcode && x.role == role_code).FirstOrDefault();

            return (l == null ? false : true);
        }

        public void initialise_admin_user(string encrypt_pass)
        {
            tbl_login login_obj = new tbl_login { username = "admin", role = "1", pass = encrypt_pass };
            db.tbl_login.Add(login_obj);
            db.SaveChanges();
        }

        #region STRATEGY
        public List<tbl_strategy> get_all_strategies()
        {

            List<tbl_strategy> lst_strategies = (from s in db.tbl_strategy
                                                 select s).OrderBy(x => x.pkey).ToList();

            return lst_strategies;
        }

        public void InsertStrategy(int st_id, string st_name)
        {
            tbl_strategy s;
            if (st_id == 0 )
            {
                s = new tbl_strategy();
                s.strategy_name = st_name;
                db.tbl_strategy.Add(s);
            }
            else
            {
                s = db.tbl_strategy.Find(st_id);
                s.strategy_name = st_name;
            }
            db.SaveChanges();


        }

        public void DeleteStrategy(int id)
        {
            //tbl_strategy s = db.tbl_strategy.Find(id);
            tbl_strategy s = db.tbl_strategy.Where(x=>x.pkey==id).Include(x => x.tbl_questions).First();
            db.tbl_strategy.Remove(s);
            db.SaveChanges();
        }

        public string get_strategy_description(int id)
        {
            string strategy = "";
            tbl_strategy st=db.tbl_strategy.Find(id);
            if(st!=null)
                strategy = st.strategy_name;

            return strategy;
        }

        #endregion STRATEGY

        #region Department

        public List<tbl_department> get_all_Departments()
        {

            List<tbl_department> lst_departments = (from s in db.tbl_department
                                          select s).OrderBy(x => x.pkey).ToList();

            return lst_departments;
        }

        public string get_department_name(int dep_id)
        {
           string dep_name = db.tbl_department.Find(dep_id).department_name;
            return dep_name;
        }

        public void InsertDepartment(int dep_id, string dep_name)
        {
            tbl_department s;
            if (dep_id == 0)
            {
                s = new tbl_department();
                s.department_name = dep_name;
                db.tbl_department.Add(s);
            }
            else
            {
                s = db.tbl_department.Find(dep_id);
                s.department_name = dep_name;
            }
            db.SaveChanges();


        }

        public void DeleteDepartment(int id)
        {
            tbl_department s = db.tbl_department.Find(id);
            db.tbl_department.Remove(s);
            db.SaveChanges();
        }

       
        #endregion Department

        #region Objective

        public List<tbl_department_objectives> get_Department_Objectives(int dep_id)
        {

            List<tbl_department_objectives> lst_objectives = (from o in db.tbl_department_objectives
                                                              where o.fk_department == dep_id
                                                              select o).OrderBy(x => x.pkey).ToList();

            
            return lst_objectives;
        }

        public void InsertObjective(int obj_id, string obj_name,int dep_id)
        {
            tbl_department_objectives s;
            if (obj_id == 0)
            {
                s = new tbl_department_objectives();
                s.objective = obj_name;
                s.fk_department = dep_id;
                db.tbl_department_objectives.Add(s);
            }
            else
            {
                s = db.tbl_department_objectives.Find(obj_id);
                s.objective = obj_name;
            }
            db.SaveChanges();


        }

        public void DeleteObjective(int id)
        {
           // tbl_department_objectives s = db.tbl_department_objectives.Find(id);
            tbl_department_objectives s = db.tbl_department_objectives.Where(x => x.pkey == id).Include(x => x.tbl_questions).First();
            
            db.tbl_department_objectives.Remove(s);
            db.SaveChanges();
        }

        public string get_department_objective(int id)
        {
            string obj = "";
            tbl_department_objectives st = db.tbl_department_objectives.Find(id);
            if (st != null)
                obj = st.objective;

            return obj;
        }

        #endregion Objective

        #region JOB

        public List<tbl_job> get_Jobs(int dep_id)
        {

            List<tbl_job> lst_jobs = (from o in db.tbl_job
                                      where o.fk_department == dep_id
                                     select o).OrderBy(x => x.pkey).ToList();
            return lst_jobs;
        }

        public void InsertJob(int job_id, string job_name, int dep_id)
        {
            tbl_job s;
            if (job_id == 0)
            {
                s = new tbl_job();
                s.job_name = job_name;
                s.fk_department = dep_id;
                db.tbl_job.Add(s);
            }
            else
            {
                s = db.tbl_job.Find(job_id);
                s.job_name = job_name;
            }
            db.SaveChanges();


        }

        public void DeleteJob(int id)
        {
            tbl_job s = db.tbl_job.Find(id);
            db.tbl_job.Remove(s);
            db.SaveChanges();
        }

        #endregion JOB

        #region EMPLOYEE

        public List<Employee> get_Employees()
        {

            List<Employee> lst_employee = (from e in db.tbl_employee
                                          join j in db.tbl_job on e.fk_job equals j.pkey
                                          join d in db.tbl_department on e.fk_department equals d.pkey
                                          select new Employee
                                              {
                                                  Emp_Id = e.pkey,
                                                  Emp_fname = e.fname,
                                                  Emp_lname = e.lname,
                                                  Emp_pcode = e.personel_code,
                                                  Dep_Id = d.pkey,
                                                  Dep_Name = d.department_name,
                                                  Job_Id=j.pkey,
                                                  Job_Name=j.job_name,
                                                  data_entry = e.data_entry,
                                                  data_view = e.data_view
                                              }).OrderBy(x => x.Emp_pcode).ToList();
           
            return lst_employee ;
        }

        public List<string> get_Employee_byId(int emp_id)
        {
            List<string> props = new List<string>();
            props = (from e in db.tbl_employee
                            where e.pkey == emp_id
                     select new List<string>
                                {
                                    e.personel_code,
                                     e.fname+" "+e.lname
                                }).FirstOrDefault();

            return props;
        }
        public List<string> get_Employee_prop(string pcode)
        {
            List<string> lst_employee = new List<string>();
            lst_employee = (from e in db.tbl_employee
                               where e.personel_code == pcode
                                select new List<string>
                                {
                                    e.fk_department+"",
                                     e.fk_job+"",
                                    e.data_entry+"",
                                    e.data_view+"",
                                    e.fname+" "+e.lname
                                    ,e.pkey+""
                                }).FirstOrDefault();

            return lst_employee;
        }

       
        public int InsertEmployee(int emp_id, string first_name, string last_name,string personel_code
            , int dep_id, int job_id, string password, bool data_entry, bool data_view)
        {
            string emp_role = "5";
            if (data_entry && data_view)
                emp_role = "2";
            else if (data_entry && !data_view)
                emp_role = "3";
            else if (!data_entry && data_view)
                emp_role = "4";

            tbl_employee s;
            if (emp_id == 0)
            {
                int pre_emp= db.tbl_employee.Where(x => x.personel_code == personel_code)
                                            .Select(x=>x.pkey).FirstOrDefault();

                if (pre_emp==0)
                {
                    s = new tbl_employee();
                    s.fname = first_name;
                    s.lname = last_name;
                    s.fk_department = dep_id;
                    s.personel_code = personel_code;
                    s.fk_job = job_id;
                    s.data_entry = data_entry;
                    s.data_view = data_view;
                    db.tbl_employee.Add(s);
                    db.SaveChanges();

                    //insert login data for employee 
                    int newPK = s.pkey;

                    if (password != "000000")
                    {
                        string pass = (new Encryption()).Encrypt(password);

                        tbl_login login_obj = new tbl_login { username = personel_code, role = emp_role, pass = pass, fk_emp = newPK };
                        db.tbl_login.Add(login_obj);
                        db.SaveChanges();

                        return 1;
                    }
                    else
                        return -2;
                }
                else
                    return -1;
            }
            else
            {
                s = db.tbl_employee.Find(emp_id);
                s.fname = first_name;
                s.lname = last_name;
                s.fk_department = dep_id;
                s.personel_code = personel_code;
                s.fk_job = job_id;
                s.data_entry = data_entry;
                s.data_view = data_view;
                db.SaveChanges();


                tbl_login l = db.tbl_login.Where(x => x.fk_emp == emp_id).FirstOrDefault();
                if (l != null)
                {
                    if(password!="000000")
                        l.pass=(new Encryption()).Encrypt(password);
                    l.username = personel_code;
                    l.role = emp_role;
                    db.SaveChanges();
                }
                return 1;

            }
          

        }

        public void DeleteEmployee(int id)
        {
            tbl_employee s = db.tbl_employee.Find(id);
            if (s != null)
            {
                db.tbl_employee.Remove(s);
                db.SaveChanges();
            }
            tbl_login login_obj = db.tbl_login.Where(x => x.fk_emp == id).Select(x => x).FirstOrDefault();
            if (login_obj != null)
            {
                db.tbl_login.Remove(login_obj);
                db.SaveChanges();
            }
        }


        #endregion EMPLOYEE

        #region JobDescription

        public List<tbl_job_description> get_JobDescriptions(int job_id)
        {

            List<tbl_job_description> lst_jobdesc = (from s in db.tbl_job_description
                                                     where s.fk_job==job_id
                                                    select s).OrderBy(x => x.pkey).ToList();

            return lst_jobdesc;
        }

        public void InsertJobDescription(int job_desc_id, string job_desc,int job_id)
        {
            tbl_job_description s;
            if (job_desc_id == 0)
            {
                s = new tbl_job_description();
                s.job_desc = job_desc;
                s.fk_job = job_id;
                db.tbl_job_description.Add(s);
            }
            else
            {
                s = db.tbl_job_description.Find(job_desc_id);
                s.job_desc = job_desc;
            }
            db.SaveChanges();


        }

        public void DeleteJobDescription(int id)
        {
           // tbl_job_description s = db.tbl_job_description.Find(id);
            tbl_job_description s = db.tbl_job_description.Where(x => x.pkey == id).Include(x => x.tbl_questions).First();
            db.tbl_job_description.Remove(s);
            db.SaveChanges();
        }

        public string get_job_description(int id)
        {
            string jobDesc = "";
            tbl_job_description st = db.tbl_job_description.Find(id);
            if (st != null)
                jobDesc = st.job_desc;

            return jobDesc;
        }
        #endregion JobDescription

        #region Question

        public string get_Question_Writer(int question_id)
        {
            string emp_prop = (from q in db.tbl_questions
                               join e in db.tbl_employee on q.fk_employee equals e.pkey
                               select (e.fname + " " + e.lname + " - personel code: " + e.personel_code)).FirstOrDefault();

            return emp_prop;
        }

        public List<QuestionViewModel> get_all_Questions()
        {

            List<QuestionViewModel> lst_questions = (from q in db.tbl_questions
                                                     join jd in db.tbl_job_description on q.fk_jobDesc equals jd.pkey
                                                     into tbl_JobDesc
                                                     join obj in db.tbl_department_objectives on q.fk_depObj equals obj.pkey
                                                     into tbl_DepObj
                                                     join st in db.tbl_strategy on q.fk_strategy equals st.pkey
                                                     into tbl_St
                                                     from j in tbl_JobDesc.DefaultIfEmpty()
                                                     from s in tbl_St.DefaultIfEmpty()
                                                     from o in tbl_DepObj.DefaultIfEmpty()
                                                     select new QuestionViewModel
                                                     {
                                                         question_id = q.pkey,
                                                         question = q.subject,
                                                         job_desc = j.job_desc,
                                                         dep_objective = o.objective,
                                                         strategy_name = s.strategy_name
                                                     }).OrderByDescending(x => x.question_id).ToList();

            lst_questions.ForEach(x => x.lst_keywords = string.Join(",", (from k in db.tbl_question_keywords where k.fk_question == x.question_id select k.keyword).ToList()));

            return lst_questions;
        }

        public List<QuestionViewModel> get_all_Questionsby_key(long key_id)
        {
            string keyword = db.tbl_question_keywords.Where(x => x.pkey == key_id).Select(x => x.keyword).First();

            List<QuestionViewModel> lst_questions = (from k in db.tbl_question_keywords
                                                     join q in db.tbl_questions on k.fk_question equals q.pkey
                                                     join jd in db.tbl_job_description on q.fk_jobDesc equals jd.pkey
                                                     into tbl_JobDesc
                                                     join obj in db.tbl_department_objectives on q.fk_depObj equals obj.pkey
                                                     into tbl_DepObj
                                                     join st in db.tbl_strategy on q.fk_strategy equals st.pkey
                                                     into tbl_St
                                                     from j in tbl_JobDesc.DefaultIfEmpty()
                                                     from s in tbl_St.DefaultIfEmpty()
                                                     from o in tbl_DepObj.DefaultIfEmpty()
                                                     where k.keyword == keyword
                                                     select new QuestionViewModel
                                                     {
                                                         question_id = q.pkey,
                                                         question = q.subject,
                                                         job_desc = j.job_desc,
                                                         dep_objective = o.objective,
                                                         strategy_name = s.strategy_name
                                                        }).OrderByDescending(x => x.question_id).ToList();
            
            lst_questions.ForEach(x => x.lst_keywords = string.Join(",", (from k in db.tbl_question_keywords where k.fk_question == x.question_id select k.keyword).ToList()));

           
            return lst_questions;
        }


        public List<QuestionViewModel> get_all_Questions_by_employee(string pcode )
        {
             int fk_emp = db.tbl_employee.Where(x => x.personel_code == pcode).Select(x => x.pkey).First();

             List<QuestionViewModel> lst_questions = (from q in db.tbl_questions.Where(x=>x.fk_employee==fk_emp)
                                                      join jd in db.tbl_job_description on q.fk_jobDesc equals jd.pkey
                                                    into tbl_JobDesc
                                                      join obj in db.tbl_department_objectives on q.fk_depObj equals obj.pkey
                                                      into tbl_DepObj
                                                      join st in db.tbl_strategy on q.fk_strategy equals st.pkey
                                                      into tbl_St
                                                      from j in tbl_JobDesc.DefaultIfEmpty()
                                                      from s in tbl_St.DefaultIfEmpty()
                                                      from o in tbl_DepObj.DefaultIfEmpty()
                                                  select new QuestionViewModel { 
                                                        question_id=q.pkey,
                                                        question = q.subject,
                                                        job_desc = j.job_desc,
                                                        dep_objective = o.objective,
                                                        strategy_name = s.strategy_name
                                                  }).OrderByDescending(x => x.question_id).ToList();

       lst_questions.ForEach(x=> x.lst_keywords = string.Join(",",(from k in db.tbl_question_keywords where k.fk_question == x.question_id select k.keyword).ToList()));
            
            return lst_questions;
        }

        public List<QuestionViewModel> get_all_Questions_by_job(int job_id)
        {
            List<QuestionViewModel> lst_questions = (from q in db.tbl_questions
                                                     join jd in db.tbl_job_description.Where(x => x.fk_job == job_id)
                                                     on q.fk_jobDesc equals jd.pkey into tbl_JobDesc
                                                     join obj in db.tbl_department_objectives on q.fk_depObj equals obj.pkey
                                                     into tbl_DepObj
                                                     join st in db.tbl_strategy on q.fk_strategy equals st.pkey
                                                     into tbl_St
                                                     join e in db.tbl_employee on q.fk_employee equals e.pkey
                                                     from j in tbl_JobDesc.DefaultIfEmpty()
                                                     from s in tbl_St.DefaultIfEmpty()
                                                     from o in tbl_DepObj.DefaultIfEmpty()
                                                     select new QuestionViewModel
                                                     {
                                                         question_id = q.pkey,
                                                         question = q.subject,
                                                         job_desc = j.job_desc,
                                                         dep_objective = o.objective,
                                                         strategy_name = s.strategy_name,
                                                         emp_prop=e.fname+" "+e.lname+" -personel code: "+e.personel_code
                                                     }).OrderByDescending(x => x.question_id).ToList();
           
            lst_questions.ForEach(x => x.lst_keywords = string.Join(",", (from k in db.tbl_question_keywords where k.fk_question == x.question_id select k.keyword).ToList()));

   
            return lst_questions;
        }

        public List<QuestionViewModel> get_all_Questions_by_alljobs_department(int dep_id)
        {
            List<QuestionViewModel> lst_questions = (from q in db.tbl_questions                                                     
                                                     join jd in db.tbl_job_description on q.fk_jobDesc equals jd.pkey 
                                                     into tbl_JobDesc
                                                     join obj in db.tbl_department_objectives on q.fk_depObj equals obj.pkey
                                                     into tbl_DepObj
                                                     join st in db.tbl_strategy on q.fk_strategy equals st.pkey
                                                     into tbl_St
                                                     join e in db.tbl_employee on q.fk_employee equals e.pkey
                                                     from j in tbl_JobDesc.DefaultIfEmpty()
                                                     from s in tbl_St.DefaultIfEmpty()
                                                     from o in tbl_DepObj.DefaultIfEmpty()
                                                     join job in db.tbl_job.Where(x => x.fk_department == dep_id) on j.fk_job equals job.pkey
                                                     select new QuestionViewModel
                                                     {
                                                         question_id = q.pkey,
                                                         question = q.subject,
                                                         job_desc = j.job_desc,
                                                         dep_objective = o.objective,
                                                         strategy_name = s.strategy_name,
                                                         emp_prop = e.fname + " " + e.lname + " -personel code: " + e.personel_code
                                                     }).OrderByDescending(x => x.question_id).ToList();

            lst_questions.ForEach(x => x.lst_keywords = string.Join(",", (from k in db.tbl_question_keywords where k.fk_question == x.question_id select k.keyword).ToList()));
            
            return lst_questions;
        }


        public List<QuestionViewModel> get_all_Questions_by_details(int jobDesc_id, int depObj_id,int strategy_id)
        {

              var query= (from q in db.tbl_questions
                        join jd in db.tbl_job_description
                        on q.fk_jobDesc equals jd.pkey
                        into tbl_JobDesc
                        join obj in db.tbl_department_objectives on q.fk_depObj equals obj.pkey
                        into tbl_DepObj
                        join st in db.tbl_strategy on q.fk_strategy equals st.pkey
                        into tbl_St
                        join e in db.tbl_employee on q.fk_employee equals e.pkey
                        from j in tbl_JobDesc.DefaultIfEmpty()
                        from s in tbl_St.DefaultIfEmpty()
                        from o in tbl_DepObj.DefaultIfEmpty()
                    select new QuestionViewModel{
                        question = q.subject,
                        question_id = q.pkey,
                        job_desc_id=j.pkey,
                        dep_obj_id=o.pkey,
                        strategy_id = s.pkey,
                        job_desc = j.job_desc,
                        dep_objective = o.objective,
                        strategy_name = s.strategy_name,
                        emp_prop = e.fname + " " + e.lname + " -personel code: " + e.personel_code
                    }).OrderByDescending(x => x.question_id);


              List<QuestionViewModel> lst_questions = null;

            if(jobDesc_id!=0 && strategy_id!=0 && depObj_id!=0)
                lst_questions=query.Where(x => x.job_desc_id == jobDesc_id
                                       && x.dep_obj_id==depObj_id && x.strategy_id==strategy_id)
                                       .ToList<QuestionViewModel>();

            else if (jobDesc_id != 0 && strategy_id != 0 && depObj_id == 0)
                lst_questions = query.Where(x => x.job_desc_id == jobDesc_id
                                         && x.strategy_id == strategy_id)
                                       .ToList<QuestionViewModel>();

            else if (jobDesc_id != 0 && strategy_id == 0 && depObj_id != 0)
                lst_questions = query.Where(x => x.job_desc_id == jobDesc_id
                                         && x.dep_obj_id == depObj_id)
                                       .ToList<QuestionViewModel>();

            else if (jobDesc_id != 0 && strategy_id == 0 && depObj_id == 0)
                lst_questions = query.Where(x => x.job_desc_id == jobDesc_id)
                                       .ToList<QuestionViewModel>();

            else if (jobDesc_id == 0 && strategy_id != 0 && depObj_id != 0)
                lst_questions = query.Where(x=>x.dep_obj_id == depObj_id
                                            && x.strategy_id == strategy_id)
                                       .ToList<QuestionViewModel>();

            else if (jobDesc_id == 0 && strategy_id != 0 && depObj_id == 0)
                lst_questions = query.Where(x=> x.strategy_id == strategy_id)
                                       .ToList<QuestionViewModel>();
            
            else if (jobDesc_id == 0 && strategy_id == 0 && depObj_id != 0)
                lst_questions = query.Where(x => x.dep_obj_id == depObj_id)
                                       .ToList<QuestionViewModel>();
            else
                lst_questions = query.ToList<QuestionViewModel>();

            lst_questions.ForEach(x => x.lst_keywords = string.Join(",", (from k in db.tbl_question_keywords where k.fk_question == x.question_id select k.keyword).ToList()));
            
           
            return lst_questions;
        }

        public void InsertQuestion(long q_id, string q_subject,int? depObjId,long? jobDescId,int? strategyId
            ,List<string> keywords,string pcode )
        {
            int fk_emp = db.tbl_employee.Where(x => x.personel_code == pcode).Select(x => x.pkey).First();

            tbl_questions q;
            if (q_id == 0)
            {
                q = new tbl_questions();
                q.subject = q_subject;
                q.fk_employee = fk_emp;
                q.fk_jobDesc = jobDescId;
                q.fk_depObj = depObjId;
                q.fk_strategy = strategyId;
                db.tbl_questions.Add(q);
                db.SaveChanges();

                long new_pk = q.pkey;

                tbl_question_keywords key;
                foreach (string k in keywords)
                {
                    key = new tbl_question_keywords();
                    key.fk_question = new_pk;
                    key.keyword = k;
                    db.tbl_question_keywords.Add(key);
                    db.SaveChanges();
                }
            }
            else
            {
                q = db.tbl_questions.Find(q_id);
                q.subject = q_subject;
                q.fk_jobDesc = jobDescId;
                q.fk_depObj= depObjId;
                q.fk_strategy = strategyId;

                db.SaveChanges();
                

                db.tbl_question_keywords.RemoveRange(db.tbl_question_keywords.Where(x => x.fk_question == q_id));
                tbl_question_keywords key;
                foreach (string k in keywords)
                {
                    key = new tbl_question_keywords();
                    key.fk_question = q_id;
                    key.keyword = k;
                    db.tbl_question_keywords.Add(key);
                    db.SaveChanges();
                }

                
            }


        }


        public void DeleteQuestion(long q_id)
        {
            tbl_questions s = db.tbl_questions.Find(q_id);
            db.tbl_questions.Remove(s);
            db.SaveChanges();
        }

        public string get_question_name(int q_id)
        {
            string question_name = db.tbl_questions.Find(q_id).subject;
            return question_name;
        }

        public QuestionViewModel get_question_byId(int q_id)
        {
            tbl_questions question= db.tbl_questions.Find(q_id);
            QuestionViewModel vm = new QuestionViewModel();

            if (question != null)
            {
                vm.question = question.subject;
                vm.dep_obj_id = question.fk_depObj;
                vm.job_desc_id = question.fk_jobDesc;
                vm.strategy_id = question.fk_strategy;
                vm.question = question.subject;
                vm.lst_keywords = string.Join(",", (from k in db.tbl_question_keywords where k.fk_question == q_id select k.keyword).ToList());
            }
                
            return vm;
        }

        #endregion Question

        #region Solution

        public string[][] get_Solutions(long question_id)
        {
            string[][] lst_solutions = null;
            var query = (from s in db.tbl_question_solutions
                         where s.fk_question == question_id
                         select s).OrderBy(x => x.pkey)
                             .Select(x => new { x.pkey, x.solution });


            lst_solutions = new string[query.Count()][];

            if (lst_solutions != null)
            {
                int i = 0;
                foreach (var item in query)
                {
                    lst_solutions[i] = new string[2];
                    lst_solutions[i][0] = item.pkey + "";
                    lst_solutions[i][1] = item.solution.Length <= 200 ? item.solution : (item.solution.Substring(0, 200)+"...");
                    i++;
                }
            }
            return lst_solutions;
        }


         public List<SolutionEmployeeViewModel> get_Solutions_by_Question(long question_id,int confirm)
        {
            List<SolutionEmployeeViewModel> lst_solutions = (from s in db.tbl_question_solutions
                                                             where s.fk_question == question_id
                                                             select new SolutionEmployeeViewModel { 
                                                                solution_id=s.pkey
                                                                ,solution=s.solution
                                                                ,confirm=s.confirm
                                                                ,count_upload=db.tbl_solution_uploads.Count(x=>x.fk_solution==s.pkey)
                                                             }).OrderBy(x => x.solution)
                                                             .ToList<SolutionEmployeeViewModel>();

            if (confirm == 1)
                lst_solutions = lst_solutions.Where(x => x.confirm == true).ToList();

            return lst_solutions;
         }

         public List<SolutionEmployeeViewModel> get_Solutions_by_employee(int emp_id)
        {
            
            List<SolutionEmployeeViewModel> lst_solutions = (from s in db.tbl_question_solutions.Where(s => s.fk_employee == emp_id)
                                                             join q in db.tbl_questions on s.fk_question equals q.pkey
                                                             orderby s.fk_question descending, s.pkey
                                                             select new SolutionEmployeeViewModel
                                                             {
                                                                 question_id = q.pkey,
                                                                 question = q.subject,
                                                                 solution = s.solution,
                                                                 solution_id = s.pkey,
                                                                 count_upload = db.tbl_solution_uploads.Count(x => x.fk_solution == s.pkey)
                                                                 ,confirm=s.confirm
                                                             }).ToList();

             
            return lst_solutions;
        }

        public void Delete_Solution(long id_sol)
        {
           tbl_question_solutions s =db.tbl_question_solutions.Find(id_sol);
            db.tbl_question_solutions.Remove(s);
            db.SaveChanges();
        }

        public FullSolutionViewModel get_Solution_by_id(long solution_id)
        {
            FullSolutionViewModel temp = (from s in db.tbl_question_solutions.Where(s=>s.pkey==solution_id)
                                          join q in db.tbl_questions on s.fk_question equals q.pkey
                                          join e1 in db.tbl_employee on s.fk_employee equals e1.pkey
                                          join e2 in db.tbl_employee on q.fk_employee equals e2.pkey
                                          select new FullSolutionViewModel
                                          {
                                              solution_id=s.pkey
                                              ,question_id=q.pkey,
                                              full_solution = s.solution,
                                              question = q.subject,
                                              solution_writer=e1.fname+" "+e1.lname+" - personel code: "+e1.personel_code,
                                              question_writer=e2.fname+" "+e2.lname+" - personel code: "+e2.personel_code
                                              
                                          }).First();

            return temp;
        }

        public long InsertNewSolution(long soution_id,long q_id, string new_solution, string pcode)
        {
            int fk_emp = db.tbl_employee.Where(x => x.personel_code == pcode).Select(x => x.pkey).First();

            tbl_question_solutions s;
            if (soution_id == 0)
            {
                s = new tbl_question_solutions { fk_employee = fk_emp, fk_question = q_id, solution = new_solution };
                db.tbl_question_solutions.Add(s);
                db.SaveChanges();
                long newPK = s.pkey;
                return newPK;
            }
            else
            {
                s = db.tbl_question_solutions.Find(soution_id);
                s.solution = new_solution;
                db.SaveChanges();
                return soution_id;
            }
        }

        public void change_confirm_status_solution(long soution_id)
        {
          tbl_question_solutions s=  db.tbl_question_solutions.Find(soution_id);
            if(s!=null)
            {
                bool pre_confirm=s.confirm;
                s.confirm = !pre_confirm;
                db.SaveChanges();
            }
        }



       
        #endregion Solution

        #region Upload
        public int get_count_solution_uploads(long new_sol_id)
        {
            int count_uploads = db.tbl_solution_uploads.Count(u => u.fk_solution == new_sol_id);
            return count_uploads;
        }

        public List<tbl_solution_uploads> get_uploads_by_solution(long solution_id,int confirm)
        {
            List<tbl_solution_uploads> lst_uploads = (from u in db.tbl_solution_uploads
                                                      where u.fk_solution == solution_id 
                                                      select u).ToList();

            if (confirm == 1)
                lst_uploads = lst_uploads.Where(x => x.confirm == true).ToList();

            return lst_uploads;
          
        }

        public void InsertNewUpload(long solution_id, string filepath,string upload_desc)
        {
            tbl_solution_uploads u = new tbl_solution_uploads { fk_solution = solution_id, file_path = filepath, file_desc = upload_desc };
            db.tbl_solution_uploads.Add(u);
            db.SaveChanges();
        }

        public void Edit_upload_description(long upload_id, string upload_desc)
        {
            tbl_solution_uploads u = db.tbl_solution_uploads.Find(upload_id);
            if (u != null)
            {
                u.file_desc = upload_desc;
                db.SaveChanges();
            }
        }
        public string get_file_path(long upload_id)
        {
            string file_path = db.tbl_solution_uploads.Find(upload_id).file_path;
            return file_path;
        }

        public void DeleteUpload(long upload_id)
        {
            tbl_solution_uploads s = db.tbl_solution_uploads.Find(upload_id);
            db.tbl_solution_uploads.Remove(s);
            db.SaveChanges();
        }


        public void change_confirm_status_upload(long upload_id)
        {
            tbl_solution_uploads s = db.tbl_solution_uploads.Find(upload_id);
            if (s != null)
            {
                bool pre_confirm = s.confirm;
                s.confirm = !pre_confirm;
                db.SaveChanges();
            }
        }
        #endregion Upload

        #region Keyword

        public List<SearchKeywordViewModel> get_Keywords(int jobDesc_id,int depObj_id,int st_id)
        {

            var query = (from k in db.tbl_question_keywords
                         join q in db.tbl_questions on k.fk_question equals q.pkey
                         select new SearchKeywordViewModel
                     {
                         key_id = k.pkey,
                         keyword = k.keyword,
                         job_desc_id = q.fk_jobDesc,
                         dep_obj_id = q.fk_depObj,
                         strategy_id = q.fk_strategy
                     });


            if (jobDesc_id != 0)
                query = query.Where(x => x.job_desc_id == jobDesc_id);
            if (depObj_id != 0)
                query = query.Where(x => x.dep_obj_id == depObj_id);
            if (st_id != 0)
                query = query.Where(x => x.strategy_id == st_id);

            return query.GroupBy(x => x.keyword).Select(g => g.FirstOrDefault()).OrderBy(x => x.keyword).ToList();
        }

         #endregion Keyword

      


       
    }
}