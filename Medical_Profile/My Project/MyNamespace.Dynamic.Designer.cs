using global::System;
using global::System.ComponentModel;
using global::System.Diagnostics;

namespace Medical_Profile.My
{
 internal static partial class MyProject
 {
  internal partial class MyForms
  {
   [EditorBrowsable(EditorBrowsableState.Never)]
   public Form1 m_Form1;

   public Form1 Form1
   {
    [DebuggerHidden]
    get
    {
     m_Form1 = MyForms.Create__Instance__(m_Form1);
     return m_Form1;
    }

    [DebuggerHidden]
    set
    {
     if (value == m_Form1)
      return;
     if (value is object)
      throw new ArgumentException("Property can only be set to Nothing");
     Dispose__Instance__(ref m_Form1);
    }
   }

   [EditorBrowsable(EditorBrowsableState.Never)]
   public Form3 m_Form3;

   public Form3 Form3
   {
    [DebuggerHidden]
    get
    {
     m_Form3 = MyForms.Create__Instance__(m_Form3);
     return m_Form3;
    }

    [DebuggerHidden]
    set
    {
     if (value == m_Form3)
      return;
     if (value is object)
      throw new ArgumentException("Property can only be set to Nothing");
     Dispose__Instance__(ref m_Form3);
    }
   }
  }
 }
}