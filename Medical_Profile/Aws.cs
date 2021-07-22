using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using System.Windows.Forms;
using System;
using Enc256;

namespace Medical_Profile
{
 internal static class Aws
 {
 static Ienc256 Ede = EncFactory.GetEnc();
  public async static Task<Level1_Return> Get_Level1_aysnc(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   var l1 = new Level1_Return();
   if (File.Exists("l1ret.json"))
   {
    using (var sr = new StreamReader("l1ret.json"))
    {
     string l1d = sr.ReadToEnd();
     l1 = JsonConvert.DeserializeObject<Level1_Return>(l1d);
     return l1;
    }
   }

   Mret m1;
   var bpl = new Dictionary<string, object>(bp);
   bpl["Call_vector"] = 5000;
   string Token = Ede.Encode(Key, Salt, Claims);
   m1 = await Get_Data(Base, Token, bpl);
   if (m1.status == 200)
   {
    l1 = JsonConvert.DeserializeObject<Level1_Return>(m1.body);
   }
   else
   {
    l1.code = m1.status;
    l1.message = m1.message;
   }

   return l1;
  }


  public async static Task<Dsave_return> Save_exception(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   var Dr = new Dsave_return();
   Mret M1 = null;
   var Bpl = new Dictionary<string, object>(bp);
   Bpl["Call_vector"] = 6725;
   string Token = Ede.Encode(Key, Salt, Claims);
   try
   {
    M1 = await Get_Data(Base, Token, Bpl);
    //   M1 = await Cli.Request().PostJsonAsync(Bpl).ReceiveJson<Mret>();
    if (M1.status == 200)
    {
     Dr = JsonConvert.DeserializeObject<Dsave_return>(M1.body);
    }
    else
    {
     Dr.code = M1.status;
     Dr.message = M1.message;
    }
   }
#pragma warning disable CS0168 // Variable is declared but never used
   catch (FlurlHttpException ex)
#pragma warning restore CS0168 // Variable is declared but never used
   {
    MessageBox.Show(ex.Message);
   }

   return Dr;
  }

  public async static Task<Dsave_return> Pat_delete_aysnc(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   var Dr = new Dsave_return();
   Mret M1 = null;
   var Bpl = new Dictionary<string, object>(bp);
   Bpl["Call_vector"] = 5184;
   string Token = Ede.Encode(Key, Salt, Claims);
   try
   {
    M1 = await Get_Data(Base, Token, Bpl);
    if (M1.status == 200)
    {
     Dr = JsonConvert.DeserializeObject<Dsave_return>(M1.body);
    }
    else
    {
     Dr.code = M1.status;
     Dr.message = M1.message;
    }
   }
#pragma warning disable CS0168 // Variable is declared but never used
   catch (FlurlHttpException ex)
#pragma warning restore CS0168 // Variable is declared but never used
   {
    //    int i = 1;
   }
   //  }

   return Dr;
  }

  public async static Task<Dsave_return> Pat_save_aysnc(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   var Dr = new Dsave_return();
   Mret M1 = null;
   var Bpl = new Dictionary<string, object>(bp);
   Bpl["Call_vector"] = 5163;
   string Token = Ede.Encode(Key, Salt, Claims);
   try
   {
    M1 = await Get_Data(Base, Token, Bpl);
    if (M1.status == 200)
    {
     Dr = JsonConvert.DeserializeObject<Dsave_return>(M1.body);
    }
    else
    {
     Dr.code = M1.status;
     Dr.message = M1.message;
    }
   }
#pragma warning disable CS0168 // Variable is declared but never used
   catch (FlurlHttpException ex)
#pragma warning restore CS0168 // Variable is declared but never used
   {
    //   int i = 1;
   }

   return Dr;
  }

  public async static Task<Dsave_return> Dsave_get_aysnc(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   var Dvr = new Dsave_return();
   Mret M1 = null;
   var Bpl = new Dictionary<string, object>(bp);
   if (File.Exists("dsave_get_return.json"))
   {
    using (var sr = new StreamReader("dsave_get_return.json"))
    {
     string l1d = sr.ReadToEnd();
     Dvr = JsonConvert.DeserializeObject<Dsave_return>(l1d);
     return Dvr;
    }
   }

   Bpl["Call_vector"] = 7943;
   string Token = Ede.Encode(Key, Salt, Claims);
   try
   {
    M1 = await Get_Data(Base, Token, Bpl);
    if (M1.status == 200)
    {
     Dvr = JsonConvert.DeserializeObject<Dsave_return>(M1.body);
    }
    else
    {
     Dvr.code = M1.status;
     Dvr.message = M1.message;
    }
   }
#pragma warning disable CS0168 // Variable is declared but never used
   catch (FlurlHttpException ex)
#pragma warning restore CS0168 // Variable is declared but never used
   {
    //   int i = 1;
   }

   return Dvr;
  }

  public async static Task<Level2_Return> Get_Level2_aysnc(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   var l2 = new Level2_Return();
   if (File.Exists("l2ret.json"))
   {
    using (var sr = new StreamReader("l2ret.json"))
    {
     string l2d = sr.ReadToEnd();
     l2 = JsonConvert.DeserializeObject<Level2_Return>(l2d);
     return l2;
    }
   }

   Mret m1 = null;
   var bpl = new Dictionary<string, object>(bp);
   bpl["Call_vector"] = 6000;
   string Token = Ede.Encode(Key, Salt, Claims);
   try
   {
    m1 = await Get_Data(Base, Token, bpl);
    if (m1.status == 200)
    {
     l2 = JsonConvert.DeserializeObject<Level2_Return>(m1.body);
    }
    else
    {
     l2.code = m1.status;
     l2.message = m1.message;
    }
   }
#pragma warning disable CS0168 // Variable is declared but never used
   catch (FlurlHttpException ex)
#pragma warning restore CS0168 // Variable is declared but never used
   {
    //   int i = 1;
   }

   return l2;
  }

  public async static Task<Register_Return> Register_aysnc(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   Register_Return Rv = null;
   Mret m1 = null;
   var bpl = new Dictionary<string, object>(bp);
   bpl["Call_vector"] = 4152;
   string Token = Ede.Encode(Key, Salt, Claims);
   m1 = await Get_Data(Base, Token, bpl);

   if (m1.status == 200)
   {
    Rv = JsonConvert.DeserializeObject<Register_Return>(m1.body);
   }
   else
   {
    Rv.code = m1.status;
    Rv.message = m1.message;
   }

   return Rv;
  }

  internal static async Task<Mret> Get_Data(string Base, string Token, Dictionary<string, object> Bpl)
  {
   Mret m1 = null;
   using (var cli = new FlurlClient(Base).WithHeader("X-Auth", Token))
   {
    cli.Configure(settings => settings.Timeout = TimeSpan.FromSeconds(60));
    var task = Task.Run(() => cli.Request().PostJsonAsync(Bpl));
    task.Wait();
    var rsp = task.Result;
    var cnt = rsp.Content;

    string s = await cnt.ReadAsStringAsync();

    m1 = JsonConvert.DeserializeObject<Mret>(s);
   }
   return m1;
  }
  public async static Task<Ckup_Return> Update_aysnc(string Base, string Key, string Salt, Dictionary<string, object> Claims, Dictionary<string, object> bp)
  {
   Ckup_Return Uv = new Ckup_Return();
   Mret m1 = null;
   var bpl = new Dictionary<string, object>(bp);
   bpl["Call_vector"] = 4162;
   string Token = Ede.Encode(Key, Salt, Claims);

   m1 = await Get_Data(Base, Token, bpl);

   if (m1.status == 200)
   {
    Uv = JsonConvert.DeserializeObject<Ckup_Return>(m1.body);
   }
   else
   {
    Uv.code = m1.status;
    Uv.message = m1.message;
   }

   return Uv;
  }
 }
}