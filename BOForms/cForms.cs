using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public class cForms: List<cForm> {

        // this method returns a form collection from the database
        internal bool loadAll() {
            SqlCommand cmd = new SqlCommand("SELECT ID, Name, Info FROM forms ORDER BY ID", cMain.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) {
                while (reader.Read()) {
                    cForm form = new cForm();
                    form.ID = reader.GetString(0);
                    form.Name = reader.GetString(1);
                    form.Info = reader.GetString(2);

                    this.Add(form);
                }
                return true;
            }
            else
                return false;
        }
    }

}
