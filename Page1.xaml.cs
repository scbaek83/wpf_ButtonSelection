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

            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(1, GridUnitType.Star);

            GridButtons.RowDefinitions.Add(gridRow1); 
            

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

                btns[i].Click += new RoutedEventHandler(ToggleButton_Clicked);
            }

            Debug.WriteLine("print\n\n\n");

        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            GridButtons.CaptureMouse();
            IsMDown = true;

            DownPoint = e.GetPosition(this);

            Debug.WriteLine("print mouse down\n\n");
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
            //Rectangle box = GetBox
            //Rectangle box = GetBox(rect);

            //System.Windows.Point mouseUpPos = e.GetPosition(this);

            //foreach (ToggleButton ctrl in btns)
            //{
            //    var isInSelection = IsInsideSelection(mouseDownPos, mouseUpPos, ctrl);
            //    ctrl.IsChecked = isInSelection;
            //}
            //Debug.WriteLine("print mouse up\n", mouseUpPos);
        }

        private Rectangle GetBox(Rectangle rect)
        {
            Rectangle box = rect;
            return box;
        }

        private bool IsInsideSelection(System.Windows.Point mouseDown, System.Windows.Point mouseUp, ToggleButton button)
        {
            var buttonPos = button.TransformToAncestor(this).Transform(new System.Windows.Point(0, 0));
            var btnBottomRight = new System.Windows.Point(buttonPos.X + button.Width, buttonPos.Y + button.Height);
            if (buttonPos.X < mouseDown.X || buttonPos.Y < mouseDown.Y)
                return false;
            if (btnBottomRight.X > mouseUp.X || btnBottomRight.Y > mouseUp.Y)
                return false;

            return true;
        }

        private bool IsInsideSelection(System.Windows.Point mousePoint, ToggleButton button)
        {
            var buttonPos = button.TransformToAncestor(this).Transform(new System.Windows.Point(0, 0));
            double left = buttonPos.X;
            double top = buttonPos.Y; 
            double right = buttonPos.X + button.ActualWidth;
            double bottom = buttonPos.Y + button.ActualHeight;
            //var size = new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity);
            //button.Measure(size); 

            Debug.WriteLine("left: {0:f2}, right: {1:f2}, top: {2:f2}, bottom: {3:f2}, point.X: {4:f2}, point.Y: {5:f2}",
                buttonPos.X, buttonPos.Y, right, bottom, mousePoint.X, mousePoint.Y);

            if (mousePoint.X > left && mousePoint.X < right && mousePoint.Y > top && mousePoint.Y < bottom)
            {
                return true; 
            }

            return false; 
        }

        private void ToggleButton_Clicked(object sender, RoutedEventArgs e)
        {
            ((ToggleButton)sender).IsChecked = !((ToggleButton)sender).IsChecked; 
        }
    }
}
