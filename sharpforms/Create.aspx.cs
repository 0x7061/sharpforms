using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;

using BOForms;

namespace PLSharpforms {

    public partial class Create : System.Web.UI.Page {

        private cForm form;

        protected void Page_Load(object sender, EventArgs e) {
            // disable view state
            this.EnableViewState = false;

            // set messagebox
            try {
                if (((string)Session["message"]).Length != 0) {
                    lblMessage.Text = (string)Session["message"];
                    lblMessage.CssClass = "green";
                }
                else
                    lblMessage.CssClass = "";
            }
            catch { 
                Session["message"] = "";
            }
        }

        protected void btnCreateForm_Click(object sender, EventArgs e) {
            // get the list of post data
            List<string> list = new List<string>();

            foreach (string i in Request.Form) {
                list.Add(Request.Form[i]);
            }

            // get rid of viewstate and hidden input field values
            for (int i = 0; i < 2; i ++) list.RemoveAt(0);

            // create form object and set name and info field values
            form = cMain.newForm();
            form.Name = list[0].Trim();
            form.Info = list[1].Trim();

            // persist form
            if (form.persist()) {
                // get question names and question info field values
                for (int i = 2, until = list.Count - 1; i < until; i += 3) {
                    if (!form.addQuestion(list[i], list[i + 1], list[i + 2])) {
                        setErrorMessage("Error while saving Form!");
                        break;
                    }
                }
                // redirect
                Session["message"] = convertSuccessMessage();
                Response.Redirect("Create.aspx");
            }
            else
                setErrorMessage("Error while saving Form!");
        }

        // simply returns a success message and link to the new form
        public string convertSuccessMessage() {
            return "<strong>Form saved!</strong> Find it here: <a href='Form.aspx?type=view&form=" +
                form.ID + "'>Form.aspx?form=" + form.ID + "</a> - ID: <strong>" + form.ID + "</strong>";
        }

        // small function which debugs errors and displays error messages
        public void setErrorMessage(string text) {
            System.Diagnostics.Debug.WriteLine(text);
            lblMessage.Text = text;
            lblMessage.CssClass = "red";
        }
    }

}