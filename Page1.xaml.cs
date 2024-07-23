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
        Rectangle rect = new Rectangle();

        System.Windows.Point mouseDownPos;
        

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
            }

            //MouseLeftButtonDown += OnMouseLeftButtonDown; 
            //GridButtons.MouseLeftButtonDown += OnMouseLeftButtonDown;

            Debug.WriteLine("print\n\n\n");

        }

        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //base.OnMouseLeftButtonDown(e);
            GridButtons.CaptureMouse();
            IsMDown = true;
            //rect.Location = (System.Drawing.Point)e.GetPosition(this); 
            rect.X = (int)e.GetPosition(this).X;
            rect.Y = (int)e.GetPosition(this).Y;
            rect.Width = 0;
            rect.Height = 0;

            mouseDownPos = e.GetPosition(this);

            Debug.WriteLine("print mouse down\n\n");
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsMDown)
            {
                //System.Drawing.Point pos = e.GetPosition(this);
                System.Windows.Point pos = e.GetPosition(this);
                rect.Width = (int)pos.X - rect.X;
                rect.Height = (int)pos.Y - rect.Y;

                Debug.WriteLine("mouse move", rect.Width, rect.Height); 
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //base.OnMouseLeftButtonUp(e);
            GridButtons.ReleaseMouseCapture();
            IsMDown = false;
            //Rectangle box = GetBox
            Rectangle box = GetBox(rect);

            System.Windows.Point mouseUpPos = e.GetPosition(this);

            foreach (ToggleButton ctrl in btns)
            {
                var isInSelection = IsInsideSelection(mouseDownPos, mouseUpPos, ctrl);
                ctrl.IsChecked = isInSelection;
            }
            Debug.WriteLine("print mouse up\n", mouseUpPos);
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
    }
}
