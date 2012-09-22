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

    public partial class Entry : System.Web.UI.Page {

        private cForm form;

        private bool formset;

        protected void Page_Load(object sender, EventArgs e) {
            // check for actions and init form object if necessary
            initFormSet();
        }

        // grab the GET parameter with form id and validate form id
        public void initFormSet() {
            try {
                if (Request.QueryString["form"].Length != 0) {
                    form = cMain.getForm(Request.QueryString["form"]);
                    if (form.ID.Length != 0) formset = true;
                    else formset = false;
                }
                else formset = false;
            }
            catch { formset = false; }
        }

        // checks if the form is set and if true it prints the form id
        public void insertFormId() {
            if (formset) Response.Write(form.ID);
            else Response.Write("Not set");
        }

        // renders all entries for the specific form in the session
        public void renderFormEntries() {
            if (formset) {
                int k = 0, l = 0;
                foreach(var i in form.Questions) {
                    Response.Write(i.Text + " - <span class='entry-infotext'>" + i.Info + "</span><div class='answer-box'>");
                    foreach (var j in form.FormEntries) {
                        Response.Write(l + ". " + j.QuestionEntries[k].Text + "<br />");
                        l++;
                    }
                    k++; l = 0;
                    Response.Write("</div>");
                }
                Response.Write("<input type='button' class='button' value='Back to list' onclick='window.location.href=&#39Form.aspx?type=manage&#39' />");
            }
            else renderInfoMessage("Oh snap! It seems the requested Form entries don't exist!<br />My deepest apologies! <a href='Create.aspx'>Back</a>.");
        }

        // small function to render an information regarding requests
        public void renderInfoMessage(string text) {
            Response.Write("<div id='info_box'>" + text + "</div>");
        }
    }

}