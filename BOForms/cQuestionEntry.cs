using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cQuestionEntry {

        // attributes
        private string qeID = "";
        private string qeQuestionID;
        private string qeFormEntryID;
        private string answerText;

        // properties
        internal string ID {
            get { return qeID; }
            set { qeID = value; }
        }
        internal string QuestionID {
            get { return qeQuestionID; }
            set { qeQuestionID = value; }
        }
        internal string FormEntryID {
            get { return qeFormEntryID; }
            set { qeFormEntryID = value; }
        }
        public string Text {
            get { return answerText; }
            internal set { answerText = value; }
        }

        // internal constructor
        internal cQuestionEntry() { 
        }

        // this method saves the question entry into the database
        public bool persist() {
            SqlCommand cmd = new SqlCommand("INSERT INTO questionEntries (ID, QuestionID, FormEntryID, AnswerText) values (@id, @qid, @feid, @atx)", cMain.getConnection());

            qeID = Guid.NewGuid().ToString();

            cmd.Parameters.Add(new SqlParameter("id", qeID));
            cmd.Parameters.Add(new SqlParameter("qid", qeQuestionID));
            cmd.Parameters.Add(new SqlParameter("feid", qeFormEntryID));
            cmd.Parameters.Add(new SqlParameter("atx", answerText));

            return (cmd.ExecuteNonQuery() > 0);
            //return true;
        }

        // this method removes the current question entry from the database
        public bool remove() {
            if (qeID.Length != 0) {
                SqlCommand cmd = new SqlCommand("DELETE questionEntries WHERE ID = @id", cMain.getConnection());

                cmd.Parameters.Add(new SqlParameter("id", qeID));

                if (cmd.ExecuteNonQuery() > 0) {
                    qeID = "";
                    return true;
                }
                else return false;
            }
            else return true;
        }
    }

}
