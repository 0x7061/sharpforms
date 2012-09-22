using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cFormEntry {

        // attributes
        private string feID = "";
        private string feFormID;
        private cQuestionEntries feQEntries;

        // properties
        public string ID {
            get { return feID; }
            internal set { feID = value; }
        }
        internal string FormID {
            get { return feFormID; }
            set { feFormID = value; }
        }
        public cQuestionEntries QuestionEntries {
            get {
                if (feQEntries == null) {
                    feQEntries = new cQuestionEntries();
                    feQEntries.loadFormEntryQuestionEntries(this);
                }
                return feQEntries;
            }
        }

        // internal constructor
        internal cFormEntry() { 
        }

        // this method saves the form entry into the database
        public bool persist() {
            SqlCommand cmd = new SqlCommand("INSERT INTO formEntries (ID, FormID) values (@id, @fid)", cMain.getConnection());

            feID = Guid.NewGuid().ToString();

            cmd.Parameters.Add(new SqlParameter("id", feID));
            cmd.Parameters.Add(new SqlParameter("fid", feFormID));

            return (cmd.ExecuteNonQuery() > 0);
            //return true;
        }

        // this method removes the current form entry from the database
        public bool remove() {
            if (feID.Length != 0) {
                SqlCommand cmd = new SqlCommand("DELETE formEntries WHERE ID = @id", cMain.getConnection());

                cmd.Parameters.Add(new SqlParameter("id", feID));

                if (cmd.ExecuteNonQuery() > 0) {
                    feID = "";
                    return true;
                }
                else return false;
            }
            else return true;
        }
    }

}
