<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stcc.aspx.cs" Inherits="TXSoftware.DataObjectsNetEntityModel.Web.Site.stcc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE">
    <title>DataObjects.Net Entity Model Designer</title>
</head>
<body>
    <form id="form1" runat="server">

    <script>
        var urlToRedirect = '<%= GetUrlToRedirect() %>';

    </script>

    <div>
           <!-- Histats.com  START (hidden counter)-->
<script type="text/javascript">    document.write(unescape("%3Cscript src=%27http://s10.histats.com/js15.js%27 type=%27text/javascript%27%3E%3C/script%3E"));</script>
<a href="http://www.histats.com" target="_blank" title="free page hit counter" >
<script  type="text/javascript" >
    try {
        Histats.start(1, 1527096, 4, 0, 0, 0, "");
        Histats.track_hits();
        window.location = urlToRedirect;
    } catch (err) { };
</script></a>
<noscript><a href="http://www.histats.com" target="_blank"><img  src="http://sstatic1.histats.com/0.gif?1527096&101" alt="free page hit counter" border="0"></a></noscript>
<!-- Histats.com  END  -->

        <%= HttpContext.Current.Request.Url.Host %>
    </div>
    </form>
</body>
</html>
