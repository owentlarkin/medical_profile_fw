Dim strFolder, strExecutable
Dim objFolderItems
Dim Path, subpath
Dim notfound
Const CSIDL_PERSONAL = &H05 
const CSIDL_APPDATA = &H1A
subpath = "\ClassicShell\Pinned"
Set objShell = CreateObject("Shell.Application") 
strFolder = cisdl_programs
strExecutable = "Medical_Profile_Files"
notfound = true

set adr  = objShell.Namespace(CSIDL_APPDATA)
set fself = adr.Self
Path = fself.Path
Set objFolder = objShell.Namespace(path&subpath)

set objFolderItems = objFolder.Items

For Each objItem in objFolderItems 
  if strcomp(objItem.Name,strExecutable,1) = 0 then
    notfound = false
  end if  
Next 
if notfound then
  Set objFolder = objShell.Namespace(CSIDL_PERSONAL)
  Set objFolderItem = objFolder.ParseName(strExecutable)

  Set colVerbs = objFolderItem.Verbs

  For Each objVerb In colVerbs
    If Replace(objVerb.name, "&", "") = "Pin to Start menu (Classic Shell)" Then
      objVerb.DoIt
    End If
  Next
end if