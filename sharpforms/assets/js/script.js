/*
 * > Sharpforms main frontend interface script <
 *
 * Copyright (c) 2012 Aleksandar Palic - mt111102 - FH St. Pölten
 * Released under MIT license
 * 
 * Twitter: _skripted
 * Github: skripted
 * Email: palic@gmx.at
 * 
 * Currently the script supports only text-fields and textareas.
 * Feel free to expand the script.
 *
 */


/***********************************************************************
 * Define and initiate important global variables for form entries
 */

var formEntryParent = "sortable";
var formEntryNumber = 1;
var formIconsFolder = "assets/img/icons/";
var formInstance    = document.getElementById("form1");

/***********************************************************************
 * A few functions to improve either user interface or functionality
 */

// makes the form entries sortable by extending the wrapper
$(function () {
    $("#" + formEntryParent).sortable();
    $("#" + formEntryParent).disableSelection();
});

// extend string object with a capitalize function
String.prototype.capitalize = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}

// a small trim function which deletes leading whitespaces
String.prototype.trim = function () {
    return this.replace(/^\s*([\S\s]*?)\s*$/, '$1');
}

/***********************************************************************
 * The main object which is responsible for the form manipulations
 */

var Sharpforms = Sharpforms || {};

Sharpforms = {

    // set form answer types - they have to be input types or tag names
    answerTypes: ["text", "textarea"],
    editMode: true,

    /**
    * This function is fired when the edit button is clicked and makes
    * the user leave global edit mode to make changes to the entry.
    */
    editFormEntry: function (formEntryId) {

        // disable edit mode for all other form entries
        this.disableGlobalEditMode(formEntryId);

        if (!this.editMode) {
            // get elements
            var formEntry = document.getElementById(formEntryId);
            var questionName = document.getElementById(formEntryId + "_name");
            var questionInfo = document.getElementById(formEntryId + "_info");
            var questionTools = document.getElementById(formEntryId + "_tools");
            var questionAnswer = document.getElementById(formEntryId + "_a");

            // delete placeholder and create editable form element
            this.createEditableFormElement(questionName, "input", "text", formEntryId + "_q");
            this.createEditableFormElement(questionInfo, "input", "text", formEntryId + "_i");

            // delete hidden field and create question type drop down element
            formEntry.removeChild(document.getElementById(formEntryId + "_a_form"));
            var questionType = this.createQuestionTypeField(formEntryId, questionAnswer);
            formEntry.appendChild(questionType);

            // add save form entry button
            var finish = this.createFormEditFinishButton(formEntryId, "Finish");
            formEntry.appendChild(finish);

            // remove old input field and append select box to form entry
            formEntry.removeChild(questionAnswer);

            // change a few styles / set hover styles as default
            formEntry.style.cursor = "default";
            formEntry.className = "form-entry form-entry-active";
            questionName.style.width = "100%";
            questionInfo.style.width = "100%";
            questionType.style.width = "125px";
        }

    },

    /**
    * Checks both form name and form information fields on the top of
    * the form for empty values and exceeded text lengths.
    */
    checkFormEntry: function () {
        var formName = document.getElementById("txtFormName");
        var formInfo = document.getElementById("txaFormDesc");

        if (formInfo.value.trim() == "")
            formInfo.value = " ";

        if (formName.value.trim() == "" || formName.value.trim() == "Unnamed Form") {
            alert("Please fill in a valid form name!");
            return false;
        }
        else
            return true;
    },

    /**
    * Checks both question name and info input fields for empty values
    * and exceeded text lengths.
    */
    checkFormEntryPart: function (formEntryId) {
        var questionName = document.getElementById(formEntryId + "_q");

        if (questionName.value.trim() == "") {
            alert("Please fill in a valid question name!");
            return false;
        }
        else
            return true;
    },

    /**
    * Finishes the editing by transforming all input elements back
    * to text based elements and re-enabling the global edit mode.
    */
    saveFormEntry: function (formEntryId) {
        // check for content and maxlenghts
        if (this.checkFormEntryPart(formEntryId)) {
            // get elements
            var formEntry = document.getElementById(formEntryId);

            // delete editable input fields and generate text instead
            this.createTextbasedFormElement(formEntryId + "_name");
            this.createTextbasedFormElement(formEntryId + "_info");
            // delete drop down field and create disabled input field
            this.createDisabledFormElement(formEntryId);

            // delete the finish button
            formEntry.removeChild(document.getElementById(formEntryId + "_b"));

            // change a few styles / set hover styles as default
            formEntry.style.cursor = "move";
            formEntry.className = "form-entry";

            // enable global edit mode
            this.enableGlobalEditMode(formEntryId);
        }
    },

    /**
    * This function is triggered when the copy button is clicked
    * and creates a complete copy of it / after the form entry.
    */
    copyFormEntry: function (formEntryId) {
        // increment form entry number
        formEntryNumber++;

        var nParent = document.getElementById(formEntryParent);
        var answerType = document.getElementById(formEntryId + "_a").tagName;

        var formEntry = document.createElement("div");
        formEntry.id = "q" + formEntryNumber;
        formEntry.className = "form-entry";

        // create toolbox and add to the form entry
        var formEntryTools = document.createElement("div");
        formEntryTools.id = "q" + formEntryNumber + "_tools";
        formEntryTools.className = "tools";
        // create and add the three icon elements to toolbox
        this.addToolboxItem(formEntryTools, "Edit", formEntryNumber);
        this.addToolboxItem(formEntryTools, "Copy", formEntryNumber);
        this.addToolboxItem(formEntryTools, "Delete", formEntryNumber);
        formEntry.appendChild(formEntryTools);

        // create form question name placeholder and add to form entry
        var formName = document.createElement("div");
        formName.id = "q" + formEntryNumber + "_name";
        formName.className = "question";
        var value = document.getElementById(formEntryId + "_name").childNodes[0].data;
        formName.appendChild(document.createTextNode(value));
        // also create the hidden input field
        var formNameHidden = document.createElement("input");
        formNameHidden.type = "hidden";
        formNameHidden.id = "q" + formEntryNumber + "_name_form";
        formNameHidden.name = "q" + formEntryNumber + "_name_form";
        formNameHidden.value = value;
        formName.appendChild(formNameHidden);
        formEntry.appendChild(formName);

        // create form question info placeholder and add to form entry
        var formInfo = document.createElement("div");
        formInfo.id = "q" + formEntryNumber + "_info";
        formInfo.className = "infotext";
        value = document.getElementById(formEntryId + "_info").childNodes[0].data;
        formInfo.appendChild(document.createTextNode(value));
        // also create the hidden input field
        var formInfoHidden = document.createElement("input");
        formInfoHidden.type = "hidden";
        formInfoHidden.id = "q" + formEntryNumber + "_info_form";
        formInfoHidden.name = "q" + formEntryNumber + "_info_form";
        formInfoHidden.value = value;
        formInfo.appendChild(formInfoHidden);
        formEntry.appendChild(formInfo);

        // add the hidden input field for the input type
        var formTypeHidden = document.createElement("input");
        formTypeHidden.type = "hidden";
        formTypeHidden.id = "q" + formEntryNumber + "_a_form";
        formTypeHidden.name = "q" + formEntryNumber + "_a_form";

        // create input type form based on previous answer type
        switch (answerType.toLowerCase()) {
            case "input":
                var input = document.createElement("input");
                input.type = "text";
                value = document.getElementById(formEntryId + "_a").value;
                formTypeHidden.value = "text";
                break;
            case "textarea":
                var input = document.createElement("textarea");
                input.rows = "2";
                input.cols = "70";
                value = document.getElementById(formEntryId + "_a").innerHTML;
                formTypeHidden.value = "textarea";
                break;
        }
        // add a few additional properties and then add to form entry
        input.id = "q" + formEntryNumber + "_a";
        input.name = "q" + formEntryNumber + "_a";
        input.disabled = true;
        formEntry.appendChild(input);
        formEntry.appendChild(formTypeHidden);

        // insert whole form entry into wrapper right after previous entry
        nParent.insertBefore(formEntry, document.getElementById(formEntryId).nextSibling);
    },

    /**
    * Is responsible to create and add items to the new toolbox
    */
    addToolboxItem: function (parentId, type, entryNumber) {
        // create image element and assign properties
        var iconEdit = document.createElement("img");
        iconEdit.src = formIconsFolder + type.toLowerCase() + ".png";
        iconEdit.alt = type;
        iconEdit.title = type;
        iconEdit.setAttribute("onclick", "Sharpforms." + type.toLowerCase() + "FormEntry('q" + entryNumber + "')");

        // append icon to toolbox
        parentId.appendChild(iconEdit);
    },

    /**
    * This function is triggered when the delete button is clicked
    * and deletes a whole form entry with all elements inside.
    */
    deleteFormEntry: function (formEntryId) {
        // if it's not the last element in the container
        if (document.getElementById(formEntryParent).children.length > 1) {
            // create prompt and confirm deletion
            if (window.confirm("Do you really want to delete this entry?")) {
                // get parent element and remove the given child
                var node = document.getElementById(formEntryParent);
                node.removeChild(document.getElementById(formEntryId));
            }
        }
        // if last element error message
        else
            alert("You can't remove your last form entry!");
    },

    /**
    * Creates the drop down input field based on the answer types
    * in the global variable and returns it.
    */
    createQuestionTypeField: function (formEntryId, questionAnswer) {
        // create drop down input field
        var questionType = document.createElement("select");

        // assign mandatory properties
        questionType.id = formEntryId + "_t";
        questionType.name = formEntryId + "_t";

        // iterate through form answer types and generate options
        for (var i = 0; i < this.answerTypes.length; i++) {
            var qType = document.createElement("option");
            qType.value = this.answerTypes[i].toLowerCase();
            qType.innerHTML = this.answerTypes[i].capitalize();

            // either check for a input type or the whole tag name
            if (questionAnswer.type == this.answerTypes[i].toLowerCase())
                qType.setAttribute("selected", "selected");
            else if ((questionAnswer.tagName).toLowerCase() == this.answerTypes[i].toLowerCase()) {
                qType.setAttribute("selected", "selected");
            }
            // append drop down element
            questionType.appendChild(qType);
        }

        // return the whole drop down element
        return questionType;
    },

    /**
    * Creates the input type button to complete the editing of a
    * form entry an returns it.
    */
    createFormEditFinishButton: function (formEntryId, value) {
        // create the button input field
        var finish = document.createElement("input");

        // assign mandatory properties
        finish.type = "button";
        finish.id = formEntryId + "_b";
        finish.value = value;
        finish.className = "button small";
        finish.setAttribute("onclick", "Sharpforms.saveFormEntry('" + formEntryId + "')");

        // return the button element
        return finish;
    },

    /**
    * Responsible to enable the edit mode again after changes have
    * been finished on a certain form entry.
    */
    enableGlobalEditMode: function (exceptId) {
        // set edit mode to true and enable new edit attempts
        this.editMode = true;

        // enable submit form button
        document.getElementById("btnCreateForm").disabled = false;
        document.getElementById("btnCreateForm").className = "submit-form button";

        // get form entry parent element
        var container = document.getElementById(formEntryParent);

        // set all other form entries to default
        for (var i = 0, l = container.children.length; i < l; i++) {
            if (container.children[i].id != exceptId) {
                container.children[i].className = "form-entry";
                container.children[i].style.cursor = "move";
            }
        }

        // enable draggable/sortable
        $("#" + formEntryParent).sortable("option", "disabled", false);
    },

    /**
    * Disables the global edit mode so that only one form entry can
    * be edited at a time. Removes the tools from other entries.
    */
    disableGlobalEditMode: function (exceptId) {
        // set edit mode to false and block other edit attempts
        this.editMode = false;

        // disable submit form button
        document.getElementById("btnCreateForm").disabled = true;
        document.getElementById("btnCreateForm").className = "submit-form button disabled";

        // get form entry parent element
        var container = document.getElementById(formEntryParent);

        // set all other form entries to inactive
        for (var i = 0, l = container.children.length; i < l; i++) {
            if (container.children[i].id != exceptId) {
                container.children[i].className = "form-entry form-entry-inactive";
                container.children[i].style.cursor = "default";
            }
        }

        // disable draggable/sortable
        $("#" + formEntryParent).sortable("option", "disabled", true);
    },

    /**
    * Basically transforms the drop down form element into a input
    * type text field or textarea depending on the selection.
    */
    createDisabledFormElement: function (elementId) {
        // get the element
        var node = document.getElementById(elementId + "_t");

        // also create hidden input field and add to entry
        var hidden = document.createElement("input");
        hidden.type = "hidden";
        hidden.id = elementId + "_a_form";
        hidden.name = elementId + "_a_form";

        // iterate options and create element based on selection
        switch (node.options[node.selectedIndex].value) {
            case "text":
                var input = document.createElement("input");
                input.type = "text";
                hidden.value = "text";
                break;
            case "textarea":
                var input = document.createElement("textarea");
                input.rows = "2";
                input.cols = "70";
                hidden.value = "textarea";
                break;
        }

        // add a few additional properties
        input.id = elementId + "_a";
        input.name = elementId + "_a";
        input.disabled = true;

        // add input element and delete old drop down list
        node.parentNode.appendChild(input);
        node.parentNode.appendChild(hidden);
        node.parentNode.removeChild(node);
    },

    /**
    * Gets the text-based element and transforms it into a real
    * input form element with the text as the value.
    */
    createEditableFormElement: function (nParent, type, subtype, id) {
        // create form name text input field
        var questionElement = document.createElement(type);

        // assign the input field some values
        questionElement.type = subtype;
        questionElement.id = id;
        questionElement.name = id;
        questionElement.style.width = "99.0%";
        questionElement.style.fontSize = "1.1em";

        questionElement.setAttribute('value', nParent.childNodes[0].data.trim());

        // remove text and hidden input field and add text input field
        while (nParent.firstChild) {
            nParent.removeChild(nParent.firstChild);
        }
        nParent.appendChild(questionElement);
    },

    /**
    * Gets the input form element and transforms it into a not
    * editable standard text element with the value as the text.
    */
    createTextbasedFormElement: function (parentId) {
        var nParent = document.getElementById(parentId);
        var node = document.createTextNode(nParent.children[0].value);

        // also add the hidden text field for the post array
        var hidden = document.createElement("input");
        hidden.type = "hidden";
        hidden.id = parentId + "_form";
        hidden.name = parentId + "_form";

        if (nParent.children[0].value.trim() == "")
            hidden.setAttribute('value', ' ');
        else
            hidden.value = nParent.children[0].value.trim();

        // delete input field and append text and hidden field
        nParent.removeChild(nParent.children[0]);
        nParent.appendChild(node);
        nParent.appendChild(hidden);

        // reset width to 90%
        nParent.style.width = "90%";
    }

};
