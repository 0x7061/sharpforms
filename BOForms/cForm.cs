using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cForm {

        // attributes
        private string fID = "";
        private string fName;
        private string fInfo;
        private cQuestions fQuestions;
        private cFormEntries fEntries;

        // properties
        public string ID {
            get { return fID; }
            internal set { fID = value; }
        }
        public string Name {
            get { return fName; }
            set { fName = value; }
        }
        public string Info {
            get { return fInfo; }
            set { fInfo = value; }
        }
        public cQuestions Questions {
            get {
                if (fQuestions == null) {
                    fQuestions = new cQuestions();
                    fQuestions.loadFormQuestions(this); 
                }
                return fQuestions;
            }
        }
        public cFormEntries FormEntries {
            get {
                if (fEntries == null) {
                    fEntries = new cFormEntries();
                    fEntries.loadFormEntries(this);
                }
                return fEntries;
            }
        }

        // internal constructor
        internal cForm() {
        }

        // this method adds the questions and the form to the database
        public bool persist() {
            SqlCommand cmd;

            if (fID == "") {
                cmd = new SqlCommand("INSERT INTO forms (ID, Name, Info) VALUES (@id, @nam, @inf)", cMain.getConnection());
                fID = Guid.NewGuid().ToString();
            }
            else 
                cmd = new SqlCommand("UPDATE forms set Name = @nam, Info = @inf WHERE ID = @id", cMain.getConnection());

            cmd.Parameters.Add(new SqlParameter("id", fID));
            cmd.Parameters.Add(new SqlParameter("nam", fName));
            cmd.Parameters.Add(new SqlParameter("inf", fInfo));

            return (cmd.ExecuteNonQuery() > 0);
            //return true;
        }

        // this method removes the current form and all associated questions
        public bool remove() {
            if (fID != "") {
                foreach (cQuestion q in Questions) { q.remove(); }

                SqlCommand cmd = new SqlCommand("DELETE forms WHERE ID = @id", cMain.getConnection());
                cmd.Parameters.Add(new SqlParameter("id", fID));

                if (cmd.ExecuteNonQuery() > 0) {
                    fID = "";
                    return true;
                }
                else return false;
            }
            else return true;
        }

        // this method adds an association to the form and persists to db
        public bool addQuestion(string qText, string qInfo, string qType) {
            if (fID == "") return false;
            else {
                cQuestion q = new cQuestion();
                q.FormID = fID;
                q.Text = qText;
                q.Info = qInfo;
                q.Type = qType;

                if (q.persist()) return true;
                else return false;
            }
        }

        // this method get's the question answers and creates the full form entry
        public bool addFormEntry(List<string> list) {
            System.Diagnostics.Debug.WriteLine(list[0]);

            if (fID.Length == 0) return false;
            else {
                // create the form entry and assign form id (FK) then persist
                cFormEntry fe = new cFormEntry();
                fe.FormID = fID;
                if (!fe.persist()) return false;
                var j = 0;

                // now iterate the question answers
                foreach (var i in list) {
                    // create the question entry and assign form entry id and question id
                    cQuestionEntry qe = new cQuestionEntry();
                    qe.QuestionID = Questions[j].ID;
                    qe.FormEntryID = fe.ID;
                    qe.Text = i;
                    if (!qe.persist()) return false;
                    j++;
                }
                return true;
            }
        }

        // this method loads the data for a specific object from the db
        internal bool load(string formID) {
            SqlCommand cmd = new SqlCommand("SELECT ID, Name, Info FROM forms WHERE ID = @id", cMain.getConnection());
            cmd.Parameters.Add(new SqlParameter("id", formID));
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) {
                reader.Read();
                
                this.ID = reader.GetString(0);
                this.Name = reader.GetString(1);
                this.Info = reader.GetString(2);
                
                return true;
            }
            else
                return false;
        }
    }

}
