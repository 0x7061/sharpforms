<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Entry.aspx.cs" Inherits="PLSharpforms.Entry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sharpforms - Form Entry</title>
    <link href="assets/css/normalize.css" type="text/css" rel="Stylesheet" />
    <link href="assets/css/default.css" type="text/css" rel="Stylesheet" />
</head>
<body>
   <div class="outer">
        <div class="inner">
            <div class="ctrl-panel">
                <h1>Sharpforms v1.1 - Form ID: <% insertFormId(); %></h1>
                <div>
                    <form action="Form.aspx" method="get">
                    Access form by ID:&nbsp;<input type="hidden" name="type" value="view" />
                    <input id="view-form" name="form" type="text" value="" maxlength="36" />
                    <input id="btnViewForm" class="button" name="btnViewForm" type="submit" value="Search" />
                    </form>
                </div>
            </div>
            <div class="form-panel">
                <form id="form1" runat="server">
                    <div id="form_entryBox">
                        <% renderFormEntries(); %>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="assets/js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="assets/js/jquery-ui-1.8.23.custom.min.js"></script>
    <script type="text/javascript" src="assets/js/script.js"></script>
</body>
</html>
