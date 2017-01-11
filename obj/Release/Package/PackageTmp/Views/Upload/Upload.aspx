protected void Page_Load(object sender, EventArgs e)
{
    FileUpload fu =  PreviousPage.FindControl("fuTest") as FileUpload;
    if (fu != null)
    {
        int length = fu.PostedFile.ContentLength;
    }
}