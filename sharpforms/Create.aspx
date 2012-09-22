<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="PLSharpforms.Create" EnableViewState="False" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sharpforms - Form Generator</title>
    <link href="assets/css/normalize.css" type="text/css" rel="Stylesheet" />
    <link href="assets/css/default.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <div class="outer">
        <div class="inner">
            <div class="ctrl-panel">
                <h1>Sharpforms v1.1 - Form Generator</h1>
                <div>
                    <form action="Form.aspx" method="get">
                    Access form by ID:&nbsp;<input type="hidden" name="type" value="view" />
                    <input id="view-form" name="form" type="text" value="" maxlength="36" />
                    <input id="btnViewForm" class="button" name="btnViewForm" type="submit" value="Search" />
                    </form>
                </div>
            </div>
            <div class="welcome-panel">
                <div>Welcome to Sharpforms!</div><br />
                <span>This is a free form generator created by Aleksandar Palic - 
                <a href="Form.aspx?type=manage">[Manage existing forms]</a></span>
            </div>
            <div class="form-panel">
                <form id="form1" runat="server">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                <div class="form-info">
                    <input id="txtFormName" name="txtFormName" type="text" value="Unnamed Form" />
                    <textarea id="txaFormDesc" name="txaFormDesc" rows="2" cols="70">Here you can enter information that is useful to others in completing the form. [optional]</textarea>
                </div>
                <div id="sortable">
                    <div id="q1" class="form-entry">
                        <div id="q1_tools" class="tools">
                            <img src="assets/img/icons/edit.png" alt="Edit" title="Edit" onclick="Sharpforms.editFormEntry('q1')" />
                            <img src="assets/img/icons/copy.png" alt="Copy" title="Copy" onclick="Sharpforms.copyFormEntry('q1')" />
                            <img src="assets/img/icons/delete.png" alt="Delete" title="Delete" onclick="Sharpforms.deleteFormEntry('q1')" />
                        </div>
                        <div id="q1_name" class="question">Unnamed Question<input type="hidden" id="q1_name_form" name="q1_name_form" value="Unnamed Question" /></div>
                        <div id="q1_info" class="infotext">Additional information to the question.<input type="hidden" id="q1_info_form" name="q1_info_form" value="Additional information to the question." /></div>
                        <input id="q1_a" name="q1_a" type="text" disabled="disabled" />
                        <input type="hidden" id="q1_a_form" name="q1_a_form" value="text" />
                    </div>
                </div>
                <br />
                <br />
                <asp:Button ID="btnCreateForm" CssClass="submit-form button" runat="server" 
Text="Create Form" OnClick="btnCreateForm_Click" 
    OnClientClick="return Sharpforms.checkFormEntry()" />
                <br />
                </form>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="assets/js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="assets/js/jquery-ui-1.8.23.custom.min.js"></script>
    <script type="text/javascript" src="assets/js/script.js"></script>
</body>
</html>
