using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cFormEntries: List<cFormEntry> {

        // this method creates & returns a collection of form entries for a special form
        internal bool loadFormEntries(cForm form) {
            SqlCommand cmd = new SqlCommand("SELECT ID, FormID FROM formEntries WHERE FormID = @feid ORDER BY ID", cMain.getConnection());
            cmd.Parameters.Add(new SqlParameter("feid", form.ID));
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) {
                while (reader.Read()) {
                    cFormEntry fe = new cFormEntry();
                    fe.ID = reader.GetString(0);
                    fe.FormID = reader.GetString(1);

                    this.Add(fe);
                }
                return true;
            }
            else
                return false;
        }
    }

}
