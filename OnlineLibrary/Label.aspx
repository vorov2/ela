<%@ Page Language="C#" %>
<%@ Import Namespace="System.Drawing" %>
<%
    using (Bitmap bmp = new Bitmap(50, 130))
    using (Graphics g = Graphics.FromImage(bmp))
    using (Font f = new Font("Verdana", 14, FontStyle.Bold))
    {
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, 50, 130));
        string str = "Contents";
        
        SizeF sz = g.VisibleClipBounds.Size;
        //Offset the coordinate system so that point (0, 0) is at the center of the desired area.
        g.TranslateTransform(sz.Width / 2, sz.Height / 2);
        //Rotate the Graphics object.
        g.RotateTransform(270);
        sz = g.MeasureString(str, f);
        //Offset the Drawstring method so that the center of the string matches the center.
        g.DrawString(str, f, Brushes.White, -(sz.Width/2), -(sz.Height/2));
        //Reset the graphics object Transformations.
        g.ResetTransform();

        bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);        
    }
%>