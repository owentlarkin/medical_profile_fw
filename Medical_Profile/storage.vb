Partial Class form1
 'Microsoft Sans Serif
 ' Public scr_font As System.Drawing.Font = New Font(10.25,
 Public scr_font As System.Drawing.Font = New Font("Calibri", 10.25, FontStyle.Regular)

 Public scb_font As System.Drawing.Font = New Font("Calibri", 10.25, FontStyle.Bold)

 Public pidep As ErrorProvider = New ErrorProvider

 Public patep As ErrorProvider = New ErrorProvider

 Public inpid As Boolean = False

 Public inpat As Boolean = False

 Public firstname As String = ""

 Public lastname As String = ""

 Public installation_key As String = ""

 Public installation_url As String = ""

 Public valid_patientid As Boolean = True

 Public valid_patient As Boolean = True

 Public log As Boolean = False

 Public pname As String

 Public pdob As String

 Public rdoc As String

 Public logfile As String

 Public valfld As String

 Public fullname As String = String.Empty

 Public read_name As String = String.Empty

 Public read_path As String = String.Empty

 Public selected_name As String = String.Empty

 Public lines_setting As String

 Public label_length As Single = 72.0 * 2.7

 Public total_lines As Integer

 Public labels_number As Integer

 Public lines_number As Integer

 Public blocks_number As Integer

 Public minimum_blocks As Integer

 Public Screen_width As Integer

 Public Screen_height As Integer

 Public redo_bn As Integer

 Public eval_enc As String = Nothing

 Public Patient_limit As Integer

 Private _Dalt As Boolean = False

 Public USB_Fname As String = Nothing

 Public USB_Ftime As DateTimeOffset = Nothing

 Public aws_body As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

 Public saved_patients As SortedDictionary(Of String, Dsave) = New SortedDictionary(Of String, Dsave)

 Public dict_dsaves As Dictionary(Of String, String) = New Dictionary(Of String, String)

 Public read_endpoint_file As Boolean = False

 Public loading As Boolean = False

 Public rethit As Boolean = False

 Public tabhit As Boolean = False

 Public Window_Text As String = Nothing

 Public file_clear As Boolean = False

 Public allow_paint As Boolean = True

 Public max_lines_flag As Boolean = True

 Public Pnglablist As List(Of Byte()) = New List(Of Byte())

 Delegate Sub Setfl(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)

 Public blocks As Dictionary(Of Integer, Blk_info) = New Dictionary(Of Integer, Blk_info)

 Public bfont As Dictionary(Of String, Boolean) = New Dictionary(Of String, Boolean)

 Public mp_backcolor As System.Drawing.Color = System.Drawing.Color.FromArgb(126, 255, 255)

 Public drive_label_encoded As String = Nothing

 Public eval_encoded As String = Nothing

 Public file_access As Boolean = False

 Public testmode As Boolean = False

 Public save_ver As Integer = 1

 Public run_timer As Boolean = False

 Private bl_loaded As SortedDictionary(Of String, Blk_entry) = New SortedDictionary(Of String, Blk_entry)

 Public bl_available As SortedDictionary(Of Integer, String) = New SortedDictionary(Of Integer, String)

 Public bl_used As SortedDictionary(Of Integer, String) = New SortedDictionary(Of Integer, String)

 Public maxlines As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)

 Public patientid_val As String = Nothing

 Public first_run As Boolean = True

 Public originaly As Integer

 Public currenty As Integer

 Public SW As Stopwatch = New Stopwatch()

 Public title As String = "This program is named medical profile"

 Public fieldsgb As List(Of String) = New List(Of String)

 Public fieldsmr As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)

 Public lab1 As Dictionary(Of String, String) = New Dictionary(Of String, String)

 Public labgb As Dictionary(Of String, String) = New Dictionary(Of String, String)

 Public max_rec As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)

 Public f3s As Dictionary(Of String, Form3) = New Dictionary(Of String, Form3)

 Public saved_type As Boolean = False

 Public Dsi As KeyValuePair(Of String, Dsave) = New KeyValuePair(Of String, Dsave)

 Public Ds As Dsave = Nothing

 Public dsn As String

 Public usbd As Dictionary(Of String, String) = New Dictionary(Of String, String)()

 Public ath_blist As List(Of Ath_block) = New List(Of Ath_block)

 Public gpn As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)
 Public gph As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)
 Public gpb As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

 Public athp As Dictionary(Of String, String) = New Dictionary(Of String, String)
 Public atdp As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)

 Public departments As Dictionary(Of Integer, Dept_Return) = New Dictionary(Of Integer, Dept_Return)

 Public atpv As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

 Public providers As Dictionary(Of Integer, Provider_return) = New Dictionary(Of Integer, Provider_return)

 Dim L1_ret As Level1_Return = Nothing

 Dim L2_ret As Level2_Return = Nothing

 Public Shared installed_version As String = Nothing

 Private emppn As Panel = Nothing

 Dim Person_loaded As Boolean = False

 Public cr As String = Chr(13)
 Public lftab As String = Chr(10) & Chr(9)
 Public lf As String = Chr(10)
 Public crlf As String = Chr(13) & Chr(10)
 Public tab As String = Chr(9)

 Public cwd As Single() = {0.0, 0.0, 0.226, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.0, 0.226,
      0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.579, 0.763, 0.763, 0.544, 0.533,
      0.533, 0.533, 0.226, 0.326, 0.401, 0.498, 0.507, 0.715, 0.682, 0.221, 0.303, 0.303, 0.498, 0.498, 0.25,
      0.306, 0.252, 0.386, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.268, 0.268,
      0.498, 0.498, 0.498, 0.463, 0.894, 0.579, 0.544, 0.533, 0.615, 0.488, 0.459, 0.631, 0.623, 0.252, 0.319,
      0.52, 0.42, 0.855, 0.646, 0.662, 0.517, 0.673, 0.543, 0.459, 0.487, 0.642, 0.567, 0.89, 0.519, 0.487,
      0.468, 0.307, 0.386, 0.307, 0.498, 0.498, 0.291, 0.479, 0.525, 0.423, 0.525, 0.498, 0.305, 0.471, 0.525,
      0.229, 0.239, 0.455, 0.229, 0.799, 0.525, 0.527, 0.525, 0.525, 0.349, 0.391, 0.335, 0.525, 0.452, 0.715,
      0.433, 0.453, 0.395, 0.314, 0.46, 0.314, 0.498, 0.226, 0.326, 0.498, 0.507, 0.498, 0.507, 0.498, 0.498,
      0.393, 0.834, 0.402, 0.512, 0.498, 0.306, 0.507, 0.394, 0.339, 0.498, 0.336, 0.334, 0.292, 0.55, 0.586,
      0.252, 0.307, 0.246, 0.422, 0.512, 0.636, 0.671, 0.675, 0.463, 0.579, 0.226, 0.326, 0.498, 0.507, 0.498,
      0.507, 0.498, 0.498, 0.393, 0.834, 0.402, 0.512, 0.498, 0.306, 0.507, 0.394, 0.339, 0.498, 0.336, 0.334,
      0.292, 0.55, 0.586, 0.252, 0.307, 0.246, 0.422, 0.512, 0.636, 0.671, 0.675, 0.463, 0.579, 0.579, 0.579,
      0.579, 0.579, 0.579, 0.763, 0.533, 0.488, 0.488, 0.488, 0.488, 0.252, 0.252, 0.252, 0.252, 0.625, 0.646,
      0.662, 0.662, 0.662, 0.662, 0.662, 0.498, 0.664, 0.642, 0.642, 0.642, 0.642, 0.487, 0.517, 0.527, 0.479,
      0.479, 0.479, 0.479, 0.479, 0.479, 0.773, 0.423, 0.498, 0.498, 0.498, 0.498, 0.229, 0.229, 0.229, 0.229,
      0.525, 0.525, 0.527, 0.527, 0.527, 0.527, 0.527, 0.498, 0.529, 0.525, 0.525, 0.525, 0.525, 0.453, 0.525,
      0.453}

 Public cwdb As Single() = {0.0, 0.0, 0.226, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.0, 0.226,
      0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.606, 0.775, 0.775, 0.561, 0.529,
      0.529, 0.529, 0.226, 0.326, 0.438, 0.498, 0.507, 0.729, 0.705, 0.233, 0.312, 0.312, 0.498, 0.498, 0.258,
      0.306, 0.267, 0.43, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.507, 0.276, 0.276,
      0.498, 0.498, 0.498, 0.463, 0.898, 0.606, 0.561, 0.529, 0.63, 0.488, 0.459, 0.637, 0.631, 0.267, 0.331,
      0.547, 0.423, 0.874, 0.659, 0.676, 0.532, 0.686, 0.563, 0.473, 0.495, 0.653, 0.591, 0.906, 0.551, 0.52,
      0.478, 0.325, 0.43, 0.325, 0.498, 0.498, 0.3, 0.494, 0.537, 0.418, 0.537, 0.503, 0.316, 0.474, 0.537,
      0.246, 0.255, 0.48, 0.246, 0.813, 0.537, 0.538, 0.537, 0.537, 0.355, 0.399, 0.347, 0.537, 0.473, 0.745,
      0.459, 0.474, 0.397, 0.344, 0.475, 0.344, 0.498, 0.226, 0.326, 0.498, 0.507, 0.498, 0.507, 0.498, 0.498,
      0.415, 0.834, 0.416, 0.539, 0.498, 0.306, 0.507, 0.39, 0.342, 0.498, 0.338, 0.336, 0.301, 0.563, 0.598,
      0.268, 0.303, 0.252, 0.435, 0.539, 0.658, 0.691, 0.702, 0.463, 0.606, 0.226, 0.326, 0.498, 0.507, 0.498,
      0.507, 0.498, 0.498, 0.415, 0.834, 0.416, 0.539, 0.498, 0.306, 0.507, 0.39, 0.342, 0.498, 0.338, 0.336,
      0.301, 0.563, 0.598, 0.268, 0.303, 0.252, 0.435, 0.539, 0.658, 0.691, 0.702, 0.463, 0.606, 0.606, 0.606,
      0.606, 0.606, 0.606, 0.775, 0.529, 0.488, 0.488, 0.488, 0.488, 0.267, 0.267, 0.267, 0.267, 0.639, 0.659,
      0.676, 0.676, 0.676, 0.676, 0.676, 0.498, 0.681, 0.653, 0.653, 0.653, 0.653, 0.52, 0.532, 0.555, 0.494,
      0.494, 0.494, 0.494, 0.494, 0.494, 0.775, 0.418, 0.503, 0.503, 0.503, 0.503, 0.246, 0.246, 0.246, 0.246,
      0.537, 0.537, 0.538, 0.538, 0.538, 0.538, 0.538, 0.498, 0.544, 0.537, 0.537, 0.537, 0.537, 0.474, 0.537,
      0.474
     }

 Public Property Emppn1 As Panel
  Get
   Return emppn
  End Get
  Set(value As Panel)
   emppn = value
  End Set
 End Property

 Public Property Data_altered As Boolean
  Get
   Return _Dalt
  End Get
  Set(value As Boolean)
   _Dalt = value
  End Set
 End Property

End Class