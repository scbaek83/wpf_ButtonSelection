using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace FramePage
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page1 : Page
    {
        ToggleButton[] btns = new ToggleButton[9];
        static int margin = 10;
        static int btnW = 50;
        static int btnH = 30;
        static int btnS = 3;

        bool IsMDown = false;
        bool IsDrag = false; 
        Rectangle rect = new Rectangle();
        System.Windows.Point DownPoint = new System.Windows.Point(0, 0); 

        public Page1()
        {
            InitializeComponent();
            
            for (int i=0; i<9; i++)
            {
                btns[i] = new ToggleButton()
                {
                    Content = i + 1
                }; 

                int row = i / 3;
                int col = i % 3; 

                GridButtons.Children.Add(btns[i]);
                Grid.SetRow(btns[i], row);
                Grid.SetColumn(btns[i], col);

                btns[i].PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            }
        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            GridButtons.CaptureMouse();
            IsMDown = true;

            DownPoint = e.GetPosition(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsMDown)
            {
                if (Math.Abs(DownPoint.X - e.GetPosition(this).X) > 3 || Math.Abs(DownPoint.Y - e.GetPosition(this).Y) > 3)
                    IsDrag = true; 
            }

            if (IsDrag)
            {
                var pos = e.GetPosition(this); 
                foreach (ToggleButton btn in btns)
                {
                    bool isSelected = IsInsideSelection(pos, btn);
                    
                    if (isSelected)
                    {
                        Debug.WriteLine("Selected\n\n");
                        btn.IsChecked = true; 
                    } 
                }
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            GridButtons.ReleaseMouseCapture();

            // Drag 없이 클릭만 했을때 => 토글
            if (!IsDrag)
            {
                var pos = e.GetPosition(this); 
                foreach (ToggleButton btn in btns)
                {
                    if (IsInsideSelection(pos, btn))
                    {
                        btn.IsChecked = !btn.IsChecked;
                    }
                }

            }

            IsMDown = false;
            IsDrag = false; 
        }

        private bool IsInsideSelection(System.Windows.Point mousePoint, ToggleButton button)
        {
            var buttonPos = button.TransformToAncestor(this).Transform(new System.Windows.Point(0, 0));
            double left = buttonPos.X;
            double top = buttonPos.Y; 
            double right = buttonPos.X + button.ActualWidth;
            double bottom = buttonPos.Y + button.ActualHeight;

            if (mousePoint.X > left && mousePoint.X < right && mousePoint.Y > top && mousePoint.Y < bottom)
            {
                return true; 
            }

            return false; 
        }

    }
}
