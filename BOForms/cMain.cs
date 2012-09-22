using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BOForms {

    public static class cMain {

        // this method creates the connection to the sql server and returns it
        static internal SqlConnection getConnection() {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Substring(0, path.Length - 2);
            path = path.Substring(0, path.LastIndexOf("\\"));
            path += "\\Database\\SFDB.mdf";

            string connectionString = "Data Source=localhost\\SQLEXPRESS;AttachDbFilename=" + path + ";Integrated Security=True;Connect Timeout=30;User Instance=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        // only the PL is meant to not use new, the factory however is the only
        // class with static methods and uses new as is usual, this way every
        // entity-based class maintains it's original object oriented behaviour

        public static cForm newForm() {
            return new cForm();
        }

        public static cForms getForms() {
            cForms forms = new cForms();
            forms.loadAll();
            return forms;
        }

        public static cForm getForm(string formID) {
            cForm form = new cForm();
            form.load(formID);
            return form;
        }
    }

}
