Imports System.Windows.Forms

Public Class Form1
    Private Const WM_SETREDRAW As Integer = 11

    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hWnd As Integer, ByVal wMsg As Integer,
        ByVal wParam As Integer, ByRef lParam As Integer) As Integer

    Public Function Stop_redraw(ByVal target As Integer) As Integer
        Dim ret As Integer = SendMessage(target, WM_SETREDRAW, False, 0)
        Return ret
    End Function

    Public Function start_redraw(ByVal target As Integer) As Integer
        Dim ret As Integer = SendMessage(target, WM_SETREDRAW, True, 0)
        Return ret
    End Function
End Class

