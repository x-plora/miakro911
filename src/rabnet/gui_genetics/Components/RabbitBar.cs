﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace rabnet
{
	/// <summary>
	/// Rabbit class for miaGenetics
	/// </summary>
	public partial class RabbitBar : CustomGraphCmp
	{

		private Boolean _Duplicated = false;

		private int _ForcedGender = 0;
		public int ForcedGender 
		{
			get { return _ForcedGender; }
			set { _ForcedGender = value; }
		}

		private RabbitGen _Rabbit;
		public void SetRabbit(RabbitGen rab){
			_Rabbit=rab;
			UpdateTooltip();
			_Exists = true;
			TabStop = true;
			label1.Text = rab.rid.ToString();
			label2.Text = rab.r_father.ToString();
			label3.Text = rab.r_mother.ToString();
			RedrawMe();
		}
		public RabbitGen GetRabbit()
		{
			return _Rabbit; 
		}

		private Boolean _Exists = false;

		private Color _FgColor = SystemColors.Control;
		public Color FgColor
		{
			get { return _FgColor; }
			set { _FgColor = value; }
		}

		private RabbitPair _ParentPair;
		public void SetParentPair(RabbitPair p)
		{
			_ParentPair = p;
		}


		private string _ToolTip = "";

		private void UpdateTooltip()
		{
			string tt = "";
			string ttl = "";
			Boolean dead = false;
			if (_Rabbit != null)
			{
				string sex = "-";
				if (_Rabbit.sex == RabbitGen.RabbitSex.MALE)
				{
					sex = "мужской";
				}
				if (_Rabbit.sex == RabbitGen.RabbitSex.FEMALE)
				{
					sex = "женский";
				}
				tt = string.Format(@"Номер: {0:d}
Пол: {1}
Порода: {2}
Приплод: {3:f2}
Родительские качества: {4:f2}", _Rabbit.rid, sex, _Rabbit.breed_name, _Rabbit.PriplodK, _Rabbit.RodK);
				ttl = _Rabbit.fullname;
				
				dead = _Rabbit.IsDead;

				if (dead)
				{
					if (_Rabbit.sex == RabbitGen.RabbitSex.FEMALE)
					{
						ttl += " (умерла)";
					}
					else
					{
						ttl += " (умер)";
					}
				}

			}
			RabToolTip.ToolTipTitle = ttl;
//			RabToolTip.SetToolTip(this, tt);
			_ToolTip = tt;
			if (dead)
			{
				RabToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
			}
			else
			{
				RabToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.None;
			}
		}

		private Boolean _OrderedGenom = false;
		/// <summary>
		/// Tells class if genom must be ordered.
		/// </summary>
		public Boolean OrderedGenom
		{
			get { return _OrderedGenom; }
			set
			{
				_OrderedGenom = value;
				this.RedrawMe();
			}
		}
	
		Dictionary<int, Color> _GenomColors = new Dictionary<int, Color>();

		public void ReplaceGenomeColors(Dictionary<int, Color> gcs)
		{
			_GenomColors.Clear();
			_GenomColors = new Dictionary<int, Color>(gcs);

			RedrawMe();
		}

		public Dictionary<int, Color> GetGenomColors()
		{
			return _GenomColors;
		}

		private string _Genom = "";
		private string[] _GenomArr = { };
		private string[] _GenomOrderedArr = { };
		public string Genom
		{
			get
			{
				if (_Genom == "")
				{
					return "0";
				}
				else
				{
					return _Genom;
				}
			}
			set
			{
				if (CheckGenom(value))
				{
					_Genom = value;
					_GenomArr = _Genom.Split(new Char[] { ';' });
					_GenomOrderedArr = OrderGenom(_GenomArr);
				}
				this.RedrawMe();
			}
		}

		public RabbitBar()
		{
			InitializeComponent();
			TabStop = false;
			label1.Visible = false;
			label2.Visible = false;
			label3.Visible = false;
			RabToolTip.ShowAlways = true;
		}

		private Boolean CheckGenom(string g)
		{
			string[] gs = g.Split(new Char[] { ';' });
			Boolean valid = true;

			foreach (string gg in gs)
			{
				try
				{
					int test = Convert.ToInt32(gg);
				}
				catch (FormatException)
				{
					valid = false;
				}
			}

			return valid;
		}

		private void DrawColorBar(Graphics gr, Point point, Size size, float k)
		{
			Pen pen = new Pen(Color.Black);
			Pen penGr = new Pen(Color.DarkGray);
			SolidBrush brush = new SolidBrush(Color.White);

			gr.FillRectangle(brush,new Rectangle(point,size));

			string txt = Math.Round(k, 2).ToString();

			k = k * 100;

			float rk = k - 50;
			float gk = k;
			if (rk < 0)
			{
				rk = 0;
			}
			if (rk > 50)
			{
				rk = 50;
			}
			if (gk < 0)
			{
				gk = 0;
			}
			if (gk > 50)
			{
				gk = 50;
			}

			int r = (int)(255 * rk / 50);
			int g = (int)(255 * gk / 50);


			brush = new SolidBrush(Color.FromArgb(255,255-r,g,0));
			
			gr.FillRectangle(brush,new Rectangle(point,new Size((int)(size.Width * k / 100), size.Height)));



			gr.DrawLine(penGr, point, new Point(point.X + size.Width, point.Y));
			gr.DrawLine(penGr, point, new Point(point.X, point.Y + size.Height));
			gr.DrawLine(penGr, new Point(point.X,point.Y + size.Height), new Point(point.X + size.Width, point.Y + size.Height));
			gr.DrawLine(penGr, new Point(point.X + size.Width,point.Y), new Point(point.X + size.Width, point.Y + size.Height));


			SizeF txtsize = gr.MeasureString(txt, SystemFonts.DefaultFont);


			if (txtsize.Width > size.Width)
			{
				txtsize.Width = size.Width;
			}


			gr.DrawString(txt, SystemFonts.DefaultFont, Brushes.Black, new RectangleF((size.Width - txtsize.Width) / 2 + point.X, (size.Height - txtsize.Height) / 2 + point.Y, txtsize.Width, txtsize.Height));


		}

		private Boolean IsPowerOf2(int x)
		{
			if (x == 0)
			{
				return false;
			}
			if (x == 1)
			{
				return true;
			}
			double y = (double)x / 2;

			if (y == Math.Round(y))
			{
				if (y == 1)
				{
					return true;
				}
				else
				{
					return IsPowerOf2((int)y);
				}
			}
			else
			{
				return false;
			}
		}

		private int IsInArr(int[][] a,int offs,int needle)
		{
			for (int i = 0; i < a.GetLength(0);i++ )
			{
				if (a[i][offs] == needle)
				{
					return i;
				}
			}
			return -1;
		}

		private string[] OrderGenom(string[] genoms)
		{
			string[] res = { };

			if (genoms.Length > 0)
			{
				int[][] ids = { };
				//int i = 0;

				foreach (string g in genoms)
				{
					int inarr = IsInArr(ids, 0, Convert.ToInt32(g));
					if (inarr != -1)
					{
						ids[inarr][1]++;
					}
					else
					{
						Array.Resize<int[]>(ref ids, ids.GetLength(0) + 1);
						ids[ids.GetLength(0) - 1] = new int[] { 0, 0 };
						ids[ids.GetLength(0) - 1][0] = Convert.ToInt32(g);
						ids[ids.GetLength(0) - 1][1] = 1;

					}
				}

				Boolean swapped = false;
				int buff_g = 0;
				int buff_n = 0;

				do
				{
					swapped = false;
					for (int ind = 0; ind < ids.GetLength(0) - 1; ind++)
					{
						if (ids[ind][1] < ids[ind + 1][1])
						{
							buff_g = ids[ind][0];
							buff_n = ids[ind][1];
							ids[ind][0] = ids[ind + 1][0];
							ids[ind][1] = ids[ind + 1][1];
							ids[ind + 1][0] = buff_g;
							ids[ind + 1][1] = buff_n;
							swapped = true;
						}
					}
				} while (swapped);

				for (int ind = 0; ind < ids.GetLength(0); ind++)
				{
					for (int cnt = 0; cnt < ids[ind][1]; cnt++)
					{
						Array.Resize<string>(ref res, res.Length + 1);
						res[res.Length - 1] = ids[ind][0].ToString();
					}
				}
			}

			return res;
		}

		private void DrawGenom(Graphics gr, Point point, Size size,Boolean ordered)
		{
			string[] genoms = { };

			if (ordered)
			{
				genoms = _GenomOrderedArr;
			}
			else
			{
				genoms = _GenomArr;
			}

			Color cl= new Color();

			int gc = genoms.Length;

			if (IsPowerOf2(gc))
			{
				float gl = (float)size.Width / gc;
				float cgl = 0;
				foreach (string g in genoms)
				{

					if (_GenomColors.ContainsKey(Convert.ToInt32(g)))
					{
						cl = _GenomColors[Convert.ToInt32(g)];
					}
					else
					{
						cl = Color.White;
					}
					SolidBrush brush = new SolidBrush(cl);

					gr.FillRectangle(brush, new RectangleF(point.X+cgl,point.Y+0,gl,size.Height));
					
					cgl = cgl + gl;
					 
				}
			}

		}

		public void DrawBadge(Graphics g, Point p)
		{

			Pen pen = new Pen(Color.Black);
			Brush brush = new SolidBrush(Color.White);


			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.FillEllipse(brush, p.X, p.Y, 13, 13);
			g.DrawEllipse(pen, p.X, p.Y, 13, 13);
			g.SmoothingMode = SmoothingMode.None;

			g.DrawLine(pen, p.X + 7, p.Y + 3, p.X + 7, p.Y + 10);
			g.DrawLine(pen, p.X + 6, p.Y + 3, p.X + 6, p.Y + 10);

			g.DrawLine(pen, p.X + 3, p.Y + 7, p.X + 10, p.Y + 7);
			g.DrawLine(pen, p.X + 3, p.Y + 6, p.X + 10, p.Y + 6);

		}


		public override void DrawingProc(Graphics g)
		{
			Pen pen = new Pen(Color.Black);
			Pen penGr = new Pen(Color.DarkGray);

			Brush textbrush = SystemBrushes.ControlText;

			string pripl = "";
			if (_Rabbit != null)
			{
				pripl = Math.Round(_Rabbit.PriplodK, 2).ToString();
				if (_Rabbit.IsDead)
				{
					pen = new Pen(Color.Red);
				}
			}

			SizeF priplsize = g.MeasureString(pripl, SystemFonts.DefaultFont);


			if (priplsize.Width > this.Width / 3)
			{
				priplsize.Width = this.Width / 3;
			}


			GraphicsPath p = new GraphicsPath();

			p.StartFigure();

			int gender = 0;
			float rodk = 0;
			if (_ForcedGender > 0)
			{
				gender = _ForcedGender;
			}
			else
			{
				if (_Rabbit != null)
				{
					if (_Rabbit.sex == RabbitGen.RabbitSex.MALE)
					{
						gender = 1;
					}
					if (_Rabbit.sex == RabbitGen.RabbitSex.FEMALE)
					{
						gender = 2;
					}
				}
			}
			if (_Rabbit != null)
			{
				rodk = _Rabbit.RodK;
			}

			if (gender > 0)
			{
				if (gender == 1) // Male
				{
					p.AddLine(new Point(0, this.Height - 1), new Point(this.Width - 1, this.Height - 1));
					p.AddLine(new Point(this.Width - 1, this.Height - 1), new Point(this.Width - 1, (int)(this.Height / 2)));
					p.AddArc(0, 0, this.Height - 1, this.Height - 1, 180, 90);
					p.AddLine(new Point((int)(this.Height / 2), 0), new Point(this.Width - (int)(this.Height / 2) - 1, 0));
					p.AddArc(this.Width - this.Height, 0, this.Height - 1, this.Height - 1, 270, 90);
					p.AddLine(new Point(0, (int)(this.Height / 2)), new Point(0, this.Height - 1));
				}
				else if (gender == 2) //Female
				{
					p.AddLine(new Point((int)(this.Height / 2), this.Height - 1), new Point(this.Width - (int)(this.Height / 2) - 1, this.Height - 1));
					p.AddArc(this.Width - this.Height, 0, this.Height - 1, this.Height - 1, 90, -90);
					p.AddLine(new Point(this.Width - 1, (int)(this.Height / 2)), new Point(this.Width - 1, 0));
					p.AddLine(new Point(0, 0), new Point(this.Width - 1, 0));
					p.AddLine(new Point(0, 0), new Point(0, (int)(this.Height / 2)));
					p.AddArc(0, 0, this.Height - 1, this.Height - 1, 180, -90);
				}
			}
			else //Unknown gender;
			{
				p.AddRectangle(new Rectangle(0, 0, this.Width, this.Height));
			}

			p.CloseFigure();

			Color cl = Color.FromArgb(255, _FgColor);
			SolidBrush brush = new SolidBrush(cl);

			if (!_Exists)
			{
				cl = Color.FromArgb(200, _FgColor);
				Pen lp=new Pen(_FgColor);
				brush = new SolidBrush(cl);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.FillPath(brush, p);
				g.DrawPath(lp, p);
				g.SmoothingMode = SmoothingMode.None;
				return;
			}
			if (_Rabbit != null)
			{
				if (_Rabbit.IsDead)
				{
					cl = Color.FromArgb(50, Color.Red);
					brush = new SolidBrush(cl);
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.FillPath(brush, p);
					g.SmoothingMode = SmoothingMode.None;
				}
			}

			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.FillPath(brush, p);
			g.SmoothingMode = SmoothingMode.None;

			if (gender > 0)
			{
				if (gender == 1) // Male
				{
					DrawGenom(g, new Point(1, (int)(this.Height / 2)), new Size(this.Width - 2, (int)(this.Height / 2)), _OrderedGenom);

					g.DrawLine(pen, new Point(0, this.Height - 1), new Point(this.Width - 1, this.Height - 1));
					g.DrawLine(pen, new Point(this.Width - 1, (int)(this.Height / 2)), new Point(this.Width - 1, this.Height - 1));
					g.DrawLine(pen, new Point(0, (int)(this.Height / 2)), new Point(0, this.Height - 1));

					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.DrawArc(pen, 0, 0, this.Height - 1, this.Height - 1, 180, 90);
					g.DrawArc(pen, this.Width - this.Height, 0, this.Height - 1, this.Height - 1, 270, 90);
					g.SmoothingMode = SmoothingMode.None;

					g.DrawLine(pen, new Point((int)(this.Height / 2), 0), new Point(this.Width - (int)(this.Height / 2) - 1, 0));

					g.DrawLine(penGr, new Point((int)(this.Width / 3), 1), new Point((int)(this.Width / 3), (int)(this.Height / 2)));

					DrawColorBar(g, new Point((int)(this.Width / 3) + 2, 2), new Size((int)((this.Width / 3 * 2) - (this.Height / 2 * 0.65)) - 5, (int)(this.Height / 2) - 4), rodk);

					g.DrawString(pripl, SystemFonts.DefaultFont, textbrush, new RectangleF(((this.Width / 3) - priplsize.Width) / 2, ((this.Height / 2 - 1) - priplsize.Height) / 2, priplsize.Width, priplsize.Height));
				}
				else if (gender == 2) //Female
				{
					DrawGenom(g, new Point(1, 0), new Size(this.Width - 2, (int)(this.Height / 2)), _OrderedGenom);

					g.DrawLine(pen, new Point((int)(this.Height / 2), this.Height - 1), new Point(this.Width - (int)(this.Height / 2) - 1, this.Height - 1));
					g.DrawLine(pen, new Point(this.Width - 1, 0), new Point(this.Width - 1, (int)(this.Height / 2)));
					g.DrawLine(pen, new Point(0, 0), new Point(0, (int)(this.Height / 2)));

					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.DrawArc(pen, 0, 0, this.Height - 1, this.Height - 1, 90, 90);
					g.DrawArc(pen, this.Width - this.Height, 0, this.Height - 1, this.Height - 1, 0, 90);
					g.SmoothingMode = SmoothingMode.None;

					g.DrawLine(pen, new Point(0, 0), new Point(this.Width - 1, 0));

					g.DrawLine(penGr, new Point((int)(this.Width / 3), (int)(this.Height / 2)), new Point((int)(this.Width / 3), this.Height - 2));

					DrawColorBar(g, new Point((int)(this.Width / 3) + 2, (int)(this.Height / 2) + 2), new Size((int)((this.Width / 3 * 2) - (this.Height / 2 * 0.65)) - 5, (int)(this.Height / 2) - 5), rodk);

					g.DrawString(pripl, SystemFonts.DefaultFont, textbrush, new RectangleF(((this.Width / 3) - priplsize.Width) / 2, ((this.Height / 2 - 1) - priplsize.Height) / 2 + (this.Height / 2), priplsize.Width, priplsize.Height + (this.Height / 2)));

				}
			}
			else //Unknown gender;
			{
				DrawGenom(g, new Point(1, 0), new Size(this.Width - 2, (int)(this.Height / 2)), _OrderedGenom);

				g.DrawLine(pen, new Point(0, 0), new Point(0, this.Height - 1));
				g.DrawLine(pen, new Point(0, 0), new Point(this.Width - 1, 0));
				g.DrawLine(pen, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
				g.DrawLine(pen, new Point(0, this.Height - 1), new Point(this.Width - 1, this.Height - 1));

				g.DrawLine(penGr, new Point((int)(this.Width / 3), (int)(this.Height / 2)), new Point((int)(this.Width / 3), this.Height - 2));

				DrawColorBar(g, new Point((int)(this.Width / 3) + 2, (int)(this.Height / 2) + 2), new Size((int)((this.Width / 3 * 2) - (this.Height / 2 * 0.65)) - 5, (int)(this.Height / 2) - 5), rodk);

				g.DrawString(pripl, SystemFonts.DefaultFont, textbrush, new RectangleF(((this.Width / 3) - priplsize.Width) / 2, ((this.Height / 2 - 1) - priplsize.Height) / 2 + (this.Height / 2), priplsize.Width, priplsize.Height + (this.Height / 2)));


			}
			g.DrawLine(pen, new Point(0, (int)(this.Height / 2)), new Point(this.Width - 1, (int)(this.Height / 2)));


			if (_Active)
			{
				cl = Color.FromArgb(150,SystemColors.MenuHighlight);
				brush = new SolidBrush(cl);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.FillPath(brush, p);
				g.SmoothingMode = SmoothingMode.None;
				//				pen = new Pen(Color.Red,2);
			} else if (_Highlight)
			{
				cl = Color.FromArgb(150, SystemColors.MenuHighlight);
//				cl = Color.FromArgb(50, SystemColors.ControlText);
				brush = new SolidBrush(cl);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.FillPath(brush, p);
				g.SmoothingMode = SmoothingMode.None;
			}
			if (_Duplicated)
			{
				if (gender > 0)
				{
					if (gender == 1) // Male
					{
						DrawBadge(g, new Point(this.Width - 14, 0));
					}
					if (gender == 2) //Female
					{
						DrawBadge(g, new Point(this.Width - 14, this.Height - 14));

					}
				}
				else  //Unknown
				{
					DrawBadge(g, new Point(this.Width - 14, this.Height - 14));
				}
			}
		}

		private Boolean _Active = false;

		private void RabbitBar_Enter(object sender, EventArgs e)
		{
			_Active = true;
			RedrawMe();
			if (_Rabbit != null)
			{
				if (_ParentPair != null)
				{
					_ParentPair.SearchFromChild(_Rabbit.rid, 1);
				}
			}
		}

		private void RabbitBar_Leave(object sender, EventArgs e)
		{
			_Active = false;
			RedrawMe();
			if (_Rabbit != null)
			{
				if (_ParentPair != null)
				{
					_ParentPair.SearchFromChild(_Rabbit.rid, 2);
				}
			}
		}

		public void MeetNeighbors()
		{
			if (_Rabbit != null)
			{
				if (_ParentPair.SearchFromChild(_Rabbit.rid, 3))
				{
					_Duplicated = true;
					RedrawMe();
				}
			}
		}

		private Boolean _Highlight = false;
		public Boolean SearchFromParent(int rid, int cmd)
		{
			Boolean res = false;
			if (_Rabbit != null)
			{
				if (rid == _Rabbit.rid)
				{
					if (cmd == 1)
					{
						_Highlight = true;
						RedrawMe();
					}
					if (cmd == 2)
					{
						_Highlight = false;
						RedrawMe();
					}
					if (cmd == 3)
					{
						_Duplicated = true;
						res = true;
						RedrawMe();
					}
				}
			}
			return res;
		}

		private void RabbitBar_MouseLeave(object sender, EventArgs e)
		{
			RabToolTip.Hide(this);
		}

		private void RabbitBar_MouseEnter(object sender, EventArgs e)
		{
			RabToolTip.Show(_ToolTip, this);
		}

		private void RabbitBar_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ')
			{
				RabToolTip.Show(_ToolTip, this);
			}
			else
			{
				RabToolTip.Hide(this);
			}
		}

	}
}
