using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cQuestionEntries: List<cQuestionEntry> {

        // this method creates & returns a collection of question entries for a special form entry
        internal bool loadFormEntryQuestionEntries(cFormEntry formEntry) {
            SqlCommand cmd = new SqlCommand("SELECT ID, QuestionID, FormEntryID, AnswerText FROM questionEntries WHERE FormEntryID = @feid ORDER BY ID", cMain.getConnection());
            cmd.Parameters.Add(new SqlParameter("feid", formEntry.ID));
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) {
                while (reader.Read()) {
                    cQuestionEntry qe = new cQuestionEntry();
                    qe.ID = reader.GetString(0);
                    qe.QuestionID = reader.GetString(1);
                    qe.FormEntryID = reader.GetString(2);
                    qe.Text = reader.GetString(3);

                    this.Add(qe);
                }
                return true;
            }
            else
                return false;
        }
    }

}
