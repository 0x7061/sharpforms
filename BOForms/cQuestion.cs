using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cQuestion {

        // attributes
        private string qID = "";
        private string qFormID; // FK
        private string qText;
        private string qInfo;
        private string qType;

        // properties
        internal string ID { 
            get { return qID; }
            set { qID = value; }
        }
        internal string FormID {
            get { return qFormID; }
            set { qFormID = value; }
        }
        public string Text {
            get { return qText; }
            internal set { qText = value; }
        }
        public string Info {
            get { return qInfo; }
            internal set { qInfo = value; }
        }
        public string Type {
            get { return qType; }
            internal set { qType = value; }
        }

        // internal constructor
        internal cQuestion() { 
        }

        // this method saves the question by inserting a new entry into the database
        public bool persist() {

            SqlCommand cmd = new SqlCommand("INSERT INTO questions (ID, FormID, Text, Info, Type) values (@id, @fid, @tex, @inf, @typ)", cMain.getConnection());

            qID = Guid.NewGuid().ToString();

            cmd.Parameters.Add(new SqlParameter("id", qID));
            cmd.Parameters.Add(new SqlParameter("fid", qFormID));
            cmd.Parameters.Add(new SqlParameter("tex", qText));
            cmd.Parameters.Add(new SqlParameter("inf", qInfo));
            cmd.Parameters.Add(new SqlParameter("typ", qType));

            return (cmd.ExecuteNonQuery() > 0);
            //return true;
        }

        // this method removes the current question (if set) from the form & database
        public bool remove() {
            if (qID != "") {
                SqlCommand cmd = new SqlCommand("DELETE questions WHERE ID = @id", cMain.getConnection());
                
                cmd.Parameters.Add(new SqlParameter("id", qID));
                
                if (cmd.ExecuteNonQuery() > 0) {
                    qID = "";
                    return true;
                }
                else return false;
            }
            else return true;
        }
    }

}
