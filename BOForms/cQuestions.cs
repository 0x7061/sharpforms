using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cQuestions: List<cQuestion> {

        // this method creates & returns a collection of questions for a special form
        internal bool loadFormQuestions(cForm form) {
            SqlCommand cmd = new SqlCommand("SELECT ID, FormID, Text, Info, Type FROM questions WHERE FormID = @fid ORDER BY ID", cMain.getConnection());
            cmd.Parameters.Add(new SqlParameter("fid", form.ID));
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) {
                while (reader.Read()) {
                    cQuestion q = new cQuestion();
                    q.ID = reader.GetString(0);
                    q.FormID = reader.GetString(1);
                    q.Text = reader.GetString(2);
                    q.Info = reader.GetString(3);
                    q.Type = reader.GetString(4);

                    this.Add(q);
                }
                return true;
            }
            else
                return false;
        }
    }

}