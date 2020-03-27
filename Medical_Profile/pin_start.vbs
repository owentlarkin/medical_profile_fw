Dim strFolder, strExecutable
Set objShell = CreateObject("Shell.Application")
Const CSIDL_PERSONAL = &H05 
strFolder = cisdl_programs
strExecutable = "Medical_Profile_Files"

Set objFolder = objShell.Namespace(CSIDL_PERSONAL) 

Set objFolderItem = objFolder.ParseName(strExecutable)

Set colVerbs = objFolderItem.Verbs

For Each objVerb In colVerbs
   If Replace(objVerb.name, "&", "") = "Pin to Quick access" Then
      objVerb.DoIt
   End If
Next