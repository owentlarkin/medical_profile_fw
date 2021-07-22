using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MPClabel;
using Enc256;
namespace Medical_Profile
{
 public partial class Form1
 {
  public MPClabel.ILabint Dlab = LabFactory.GetLab();
  public Ienc256 Ede = EncFactory.GetEnc();
  public Font scr_font = new Font("Calibri", 10.25F, FontStyle.Regular);
  public Font scb_font = new Font("Calibri", 10.25F, FontStyle.Bold);
  public ErrorProvider pidep = new ErrorProvider();
  public ErrorProvider patep = new ErrorProvider();
  public bool inpid = false;
  public bool inpat = false;
  public string firstname = "";
  public string lastname = "";
  public string installation_key = "";
  public string installation_url = "";
  public bool valid_patientid = true;
  public bool valid_patient = true;
  public bool log = false;
  public string pname;
  public string pdob;
  public string rdoc;
  public string logfile;
  public string valfld;
  public string fullname = string.Empty;
  public string read_name = string.Empty;
  public string read_path = string.Empty;
  public string selected_name = string.Empty;
  public string lines_setting;
  public float label_length = (float)(72.0 * 2.7);
  public int total_lines;
  public int labels_number;
  public int lines_number;
  public int blocks_number;
  public int minimum_blocks;
  public int Screen_width;
  public int Screen_height;
  public System.Drawing.Size Form_size;
  public System.Drawing.Size Form_area;

  public Panel Pn;
  public Panel Pe;
  public int Ystart = 0;
  public int Original_Margin = -1;
  public int W_delta;
  public int _H_delta;
  public Rectangle Form_Bounds;
  public Rectangle Screen_Area;

  public int redo_bn;
  public string eval_enc = null;
  public int Patient_limit;
  private bool _Dalt = false;
  public string USB_Fname = null;
  public DateTimeOffset USB_Ftime = default;
  public Dictionary<string, object> aws_body = new Dictionary<string, object>();
  public SortedDictionary<string, Dsave> saved_patients = new SortedDictionary<string, Dsave>();
  public Dictionary<string, string> dict_dsaves = new Dictionary<string, string>();
  public bool read_endpoint_file = false;
  public bool loading = false;
  public bool rethit = false;
  public bool tabhit = false;
  public string Window_Text = null;
  public bool file_clear = false;
  public bool allow_paint = true;
  public bool max_lines_flag = true;
  public List<byte[]> Pnglablist = new List<byte[]>();

  public bool Vsm = false;
  public VScrollBar Sb = new VScrollBar();
  public int Vertical_Scroll_Position = 0;
  public int Form_Height = -1;

  public delegate void Setfl(object source, System.IO.FileSystemEventArgs e);

  public Dictionary<int, Blk_info> blocks = new Dictionary<int, Blk_info>();
  public Dictionary<string, bool> bfont = new Dictionary<string, bool>();
  public System.Drawing.Color mp_backcolor = System.Drawing.Color.FromArgb(126, 255, 255);
  public string drive_label_encoded = null;
  public string eval_encoded = null;
  public bool file_access = false;
  public bool testmode = false;
  public int save_ver = 1;
  public bool run_timer = false;
  private SortedDictionary<string, Blk_entry> bl_loaded = new SortedDictionary<string, Blk_entry>();
  public SortedDictionary<int, string> bl_available = new SortedDictionary<int, string>();
  public SortedDictionary<int, string> bl_used = new SortedDictionary<int, string>();
  public Dictionary<string, int> maxlines = new Dictionary<string, int>();
  public string patientid_val = null;
  public bool first_run = true;
  public int Originaly;
  public int currenty;
  public Stopwatch SW = new Stopwatch();
  public string title = "This program is named medical profile";
  public List<string> fieldsgb = new List<string>();
  public Dictionary<string, int> fieldsmr = new Dictionary<string, int>();
  public Dictionary<string, string> lab1 = new Dictionary<string, string>();
  public Dictionary<string, string> labgb = new Dictionary<string, string>();
  public Dictionary<int, int> Max_rec = new Dictionary<int, int>();
  public Dictionary<string, Form3> f3s = new Dictionary<string, Form3>();
  public bool saved_type = false;
  public KeyValuePair<string, Dsave> Dsi = new KeyValuePair<string, Dsave>();
  public Dsave Ds = null;
  public string dsn;
  public Dictionary<string, string> usbd = new Dictionary<string, string>();
  public List<Ath_block> ath_blist = new List<Ath_block>();
  public Dictionary<int, string> gpn = new Dictionary<int, string>();
  public Dictionary<int, string> gph = new Dictionary<int, string>();
  public Dictionary<int, string> gpb = new Dictionary<int, string>();
  public Dictionary<int, GroupBox> Grpblocks = new Dictionary<int, GroupBox>();
  public Dictionary<string, string> athp = new Dictionary<string, string>();
  public Dictionary<string, int> atdp = new Dictionary<string, int>();
  public Dictionary<int, Dept_Return> departments = new Dictionary<int, Dept_Return>();
  public Dictionary<int, string> atpv = new Dictionary<int, string>();
  public Dictionary<int, Provider_return> providers = new Dictionary<int, Provider_return>();
  private Level1_Return L1_ret = null;
  private Level2_Return L2_ret = null;
  public static string installed_version = null;
  private Panel emppn = null;
//  private bool Person_loaded = false;
  public string cr =Convert.ToString((char)13);
  public string lftab = Convert.ToString((char)10) + Convert.ToString((char)9);
  public string lf = Convert.ToString((char)10);
  public string crlf = Convert.ToString((char)13) + Convert.ToString((char)10);
  private string tab = Convert.ToString((char)9);
  private float[] cwd = new[] { 0.0F, 0.0F, 0.226F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.0F, 0.226F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.763F, 0.763F, 0.544F, 0.533F, 0.533F, 0.533F, 0.226F, 0.326F, 0.401F, 0.498F, 0.507F, 0.715F, 0.682F, 0.221F, 0.303F, 0.303F, 0.498F, 0.498F, 0.25F, 0.306F, 0.252F, 0.386F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.268F, 0.268F, 0.498F, 0.498F, 0.498F, 0.463F, 0.894F, 0.579F, 0.544F, 0.533F, 0.615F, 0.488F, 0.459F, 0.631F, 0.623F, 0.252F, 0.319F, 0.52F, 0.42F, 0.855F, 0.646F, 0.662F, 0.517F, 0.673F, 0.543F, 0.459F, 0.487F, 0.642F, 0.567F, 0.89F, 0.519F, 0.487F, 0.468F, 0.307F, 0.386F, 0.307F, 0.498F, 0.498F, 0.291F, 0.479F, 0.525F, 0.423F, 0.525F, 0.498F, 0.305F, 0.471F, 0.525F, 0.229F, 0.239F, 0.455F, 0.229F, 0.799F, 0.525F, 0.527F, 0.525F, 0.525F, 0.349F, 0.391F, 0.335F, 0.525F, 0.452F, 0.715F, 0.433F, 0.453F, 0.395F, 0.314F, 0.46F, 0.314F, 0.498F, 0.226F, 0.326F, 0.498F, 0.507F, 0.498F, 0.507F, 0.498F, 0.498F, 0.393F, 0.834F, 0.402F, 0.512F, 0.498F, 0.306F, 0.507F, 0.394F, 0.339F, 0.498F, 0.336F, 0.334F, 0.292F, 0.55F, 0.586F, 0.252F, 0.307F, 0.246F, 0.422F, 0.512F, 0.636F, 0.671F, 0.675F, 0.463F, 0.579F, 0.226F, 0.326F, 0.498F, 0.507F, 0.498F, 0.507F, 0.498F, 0.498F, 0.393F, 0.834F, 0.402F, 0.512F, 0.498F, 0.306F, 0.507F, 0.394F, 0.339F, 0.498F, 0.336F, 0.334F, 0.292F, 0.55F, 0.586F, 0.252F, 0.307F, 0.246F, 0.422F, 0.512F, 0.636F, 0.671F, 0.675F, 0.463F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.579F, 0.763F, 0.533F, 0.488F, 0.488F, 0.488F, 0.488F, 0.252F, 0.252F, 0.252F, 0.252F, 0.625F, 0.646F, 0.662F, 0.662F, 0.662F, 0.662F, 0.662F, 0.498F, 0.664F, 0.642F, 0.642F, 0.642F, 0.642F, 0.487F, 0.517F, 0.527F, 0.479F, 0.479F, 0.479F, 0.479F, 0.479F, 0.479F, 0.773F, 0.423F, 0.498F, 0.498F, 0.498F, 0.498F, 0.229F, 0.229F, 0.229F, 0.229F, 0.525F, 0.525F, 0.527F, 0.527F, 0.527F, 0.527F, 0.527F, 0.498F, 0.529F, 0.525F, 0.525F, 0.525F, 0.525F, 0.453F, 0.525F, 0.453F };
  private float[] cwdb = new[] { 0.0F, 0.0F, 0.226F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.0F, 0.226F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.775F, 0.775F, 0.561F, 0.529F, 0.529F, 0.529F, 0.226F, 0.326F, 0.438F, 0.498F, 0.507F, 0.729F, 0.705F, 0.233F, 0.312F, 0.312F, 0.498F, 0.498F, 0.258F, 0.306F, 0.267F, 0.43F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.507F, 0.276F, 0.276F, 0.498F, 0.498F, 0.498F, 0.463F, 0.898F, 0.606F, 0.561F, 0.529F, 0.63F, 0.488F, 0.459F, 0.637F, 0.631F, 0.267F, 0.331F, 0.547F, 0.423F, 0.874F, 0.659F, 0.676F, 0.532F, 0.686F, 0.563F, 0.473F, 0.495F, 0.653F, 0.591F, 0.906F, 0.551F, 0.52F, 0.478F, 0.325F, 0.43F, 0.325F, 0.498F, 0.498F, 0.3F, 0.494F, 0.537F, 0.418F, 0.537F, 0.503F, 0.316F, 0.474F, 0.537F, 0.246F, 0.255F, 0.48F, 0.246F, 0.813F, 0.537F, 0.538F, 0.537F, 0.537F, 0.355F, 0.399F, 0.347F, 0.537F, 0.473F, 0.745F, 0.459F, 0.474F, 0.397F, 0.344F, 0.475F, 0.344F, 0.498F, 0.226F, 0.326F, 0.498F, 0.507F, 0.498F, 0.507F, 0.498F, 0.498F, 0.415F, 0.834F, 0.416F, 0.539F, 0.498F, 0.306F, 0.507F, 0.39F, 0.342F, 0.498F, 0.338F, 0.336F, 0.301F, 0.563F, 0.598F, 0.268F, 0.303F, 0.252F, 0.435F, 0.539F, 0.658F, 0.691F, 0.702F, 0.463F, 0.606F, 0.226F, 0.326F, 0.498F, 0.507F, 0.498F, 0.507F, 0.498F, 0.498F, 0.415F, 0.834F, 0.416F, 0.539F, 0.498F, 0.306F, 0.507F, 0.39F, 0.342F, 0.498F, 0.338F, 0.336F, 0.301F, 0.563F, 0.598F, 0.268F, 0.303F, 0.252F, 0.435F, 0.539F, 0.658F, 0.691F, 0.702F, 0.463F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.606F, 0.775F, 0.529F, 0.488F, 0.488F, 0.488F, 0.488F, 0.267F, 0.267F, 0.267F, 0.267F, 0.639F, 0.659F, 0.676F, 0.676F, 0.676F, 0.676F, 0.676F, 0.498F, 0.681F, 0.653F, 0.653F, 0.653F, 0.653F, 0.52F, 0.532F, 0.555F, 0.494F, 0.494F, 0.494F, 0.494F, 0.494F, 0.494F, 0.775F, 0.418F, 0.503F, 0.503F, 0.503F, 0.503F, 0.246F, 0.246F, 0.246F, 0.246F, 0.537F, 0.537F, 0.538F, 0.538F, 0.538F, 0.538F, 0.538F, 0.498F, 0.544F, 0.537F, 0.537F, 0.537F, 0.537F, 0.474F, 0.537F, 0.474F };

  private Panel Emppn1
  {
   get
   {
    return emppn;
   }

   set
   {
    emppn = value;
   }
  }

  public bool Data_altered
  {
   get
   {
    return _Dalt;
   }

   set
   {
    _Dalt = value;
   }
  }
  public int H_delta
  {
   get
   {
    return _H_delta;
   }

   set
   {
    _H_delta = value;
   }
  }
 }
}