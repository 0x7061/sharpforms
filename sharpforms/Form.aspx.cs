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

    public partial class Form : System.Web.UI.Page {

        private cForm form;
        private cForms allForms;

        private bool saveset;
        private bool typeset;
        private bool formset;

        private string rType;

        protected void Page_Load(object sender, EventArgs e) {
            // check for actions and init form object if necessary
            initSaveSet();
            initTypeSet();
            initFormSet();
            // reset message box value
            Session["message"] = "";
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

        // get the GET parameter value for actions and check if set
        public void initTypeSet() {
            try {
                rType = Request.QueryString["type"];
                if (rType.Length != 0) typeset = true;
                else typeset = false;
            }
            catch { typeset = false; }
        }

        // get the POST parameter value for saving and check if set
        public void initSaveSet() {
            try {
                string post = HttpContext.Current.Request["insert"];
                if (post.Length != 0 && post == "make") saveset = true;
                else saveset = false;
            }
            catch { saveset = false; }
        }

        // checks if the form parameter is set and prints the form id
        public void insertFormId() {
            if (formset) Response.Write(form.ID);
            else Response.Write("Not set");
        }

        // very small controller which checks for the requested action
        public void checkRequest() {
            if (saveset) makeFormEntry();
            else if(typeset) {
                switch (rType) {
                    case "view": requestForm(); break;
                    case "manage": requestForms(); break;
                    case "delete": deleteForm(); break;
                }
            }
            else
                renderInfoMessage("Oops! It seems the requested Page or Form doesn't exist!<br />My deepest apologies! <a href='Create.aspx'>Back</a>.");
        }

        // get's the list of question answers and creates a form entry to that form
        public void makeFormEntry() {
            if (formset) {
                List<string> list = new List<string>();

                foreach (string i in Request.Form) {
                    list.Add(Request.Form[i]);
                }

                // remove first element and last two input types so only question answers remain
                list.RemoveAt(0);
                list.RemoveAt(list.Count - 1);
                list.RemoveAt(list.Count - 1);

                if (form.addFormEntry(list))
                    renderInfoMessage("Thank you! Your entry has been saved successfully!<br />See all entries on this form <a href='Entry.aspx?form=" + form.ID + "'>here</a> or go <a href='Create.aspx'>back</a>.");
                else
                    renderInfoMessage("Oh no! Something went terribly wrong and should get fixed soon!<br />Hurry, go back to where stuff works! Click <a href='Create.aspx'>here</a>.");
            }
            else
                renderInfoMessage("Oops! It seems the requested Page or Form doesn't exist!<br />My deepest apologies! <a href='Create.aspx'>Back</a>.");
        }

        // this function deletes a certain form by id
        public void deleteForm() {
            if (formset) {
                // first remove all question entries associated to form entries and of course all form entries
                foreach (var i in form.FormEntries) {
                    foreach (var j in i.QuestionEntries) {
                        j.remove();
                    }
                    i.remove();
                }
                // now remove all associated questions to the form structure
                foreach (var i in form.Questions) {
                    i.remove();
                }
                // then remove the form
                form.remove();
                // redirect back to the managing page
                Response.Redirect("Form.aspx?type=manage");
            }
            else
                renderInfoMessage("Oops! It seems the requested Page or Form doesn't exist!<br />My deepest apologies! <a href='Create.aspx'>Back</a>.");
        }

        // get's the form id from the get parameter and checks if it's valid
        public void requestForm() {
            if (formset) {
                renderForm(form);
            }
            else
                renderInfoMessage("Oops! It seems the requested Page or Form doesn't exist!<br />My deepest apologies! <a href='Create.aspx'>Back</a>.");
        }

        // get's the list of forms and checks if the list isn't empty
        public void requestForms() {
            allForms = cMain.getForms();

            if (allForms.Count >= 1)
                renderFormElements(allForms);
            else
                renderInfoMessage("Ohoh! You either deleted the last one or there are no saved forms yet!<br />Please try to create a form first! <a href='Create.aspx'>Back</a>.");
        }

        // this function renders the form and it's questions
        public void renderForm(cForm form) {
            // get questions from form
            cQuestions questions = form.Questions;

            // render form name and info
            Response.Write("<h2>" + form.Name + "</h2><h5>" + form.Info + "</h5>");
            
            // render questions
            int j = 0;
            foreach(var i in questions) {
                Response.Write("<div class='form-view-question'>");
                Response.Write("<div class='question'>" + i.Text + "</div><div class='infotext'>" + i.Info + "</div>");
                switch (i.Type) {
                    case "text": Response.Write("<input type='text' id='q" + j + "' name='q" + j + "' maxlength='150' />");
                        break;
                    case "textarea": Response.Write("<textarea id='q" + j + "' name='q" + j + "' rows='2' cols='70' maxlength='255'></textarea>");
                        break;
                }
                Response.Write("</div>");
                j++;
            }
            Response.Write("<input type='hidden' name='insert' value='make' />");
            // render save entry submit button and back button
            Response.Write("<input type='submit' id='submitentry' name='submitentry' value='Save' class='button form-view-button' />&nbsp;<input type='button' class='button form-view-button' value='Cancel' onclick='window.location.href=&#39Create.aspx&#39' />");
        }

        // renders the list of forms
        public void renderFormElements(cForms allForms) {
            Response.Write("<table><tr><td class='head' width='280'>ID</td><td width='20'></td><td class='head' width='250'>Name</td><td width='20'></td><td class='head' width='250'>Information</td><td width='20'></td><td width='120'></td></tr>");
            foreach (var i in allForms) {
                // cut the name and info for readability reasons
                if (i.Name.Length > 30) i.Name = i.Name.Substring(0, 30) + " ...";
                if (i.Info.Length > 30) i.Info = i.Info.Substring(0, 30) + " ...";

                Response.Write("<tr><td>" + i.ID + "</td><td width='20'></td><td><a href='Form.aspx?type=view&form=" + i.ID + "'>" + i.Name + "</a></td><td width='20'></td><td>" + i.Info + "</td><td width='20'></td><td><a href='Entry.aspx?form=" + i.ID + "'>Entries</a> | <a href='Form.aspx?type=delete&form=" + i.ID + "'>Delete</a></td></tr>");
            }
            Response.Write("</table>");
            Response.Write("<input type='button' id='back_button' class='button' value='Back' onclick='window.location.href=&#39Create.aspx&#39' />");
        }

        // small function to render an information regarding requests
        public void renderInfoMessage(string text) {
            Response.Write("<div id='info_box'>" + text + "</div>");
        }
    }

}