using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPFSphere
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            drawSphere(new Point3D(0, 0, 0), 5.0, 60, Colors.Bisque, mainViewport);
        }
        private void drawSphere(Point3D center, double radius, int n, Color color, Viewport3D viewport)
        {
            Point3D[,] points = new Point3D[n, n];


            for (int i = 0; i < n; i++) //нахождение всех точек
            {
                for (int j = 0; j < n; j++)
                {
                    points[i, j] = getPositionSphere(center, radius, i * 360 / (n - 1), j * 360 / (n - 1));
                    points[i, j] += (Vector3D)center;
                }
            }
            Point3D[] p = new Point3D[4]; //4 точки для двух треугольников
            for (int i = 0; i < n - 1; i++) //отрисовка всех треугольников по точкам
            {
                for (int j = 0; j < n - 1; j++)
                {
                    p[0] = points[i, j]; 
                    p[1] = points[i + 1, j];
                    p[2] = points[i + 1, j + 1];
                    p[3] = points[i, j + 1];
                    drawTriangle(p[0], p[1], p[2], color, viewport); 
                    drawTriangle(p[2], p[3], p[0], color, viewport);

                }
            }
        }
        public void drawTriangle(Point3D p0, Point3D p1, Point3D p2, Color color, Viewport3D viewport) //создание треугольника
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(1);

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = color;
            Material material = new DiffuseMaterial(brush);

            GeometryModel3D geometry = new GeometryModel3D(mesh, material);
            ModelUIElement3D model = new ModelUIElement3D();
            model.Model = geometry;

            viewport.Children.Add(model);
        }
        public Point3D getPositionSphere(Point3D center, double r, double a, double b) //получение точки по параметрам сферы
        {
            double sinA = Math.Sin(a * Math.PI / 180);
            double sinB = Math.Sin(b * Math.PI / 180);
            double cosA = Math.Cos(a * Math.PI / 180);
            double cosB = Math.Cos(b * Math.PI / 180);

            Point3D point = new Point3D();
            point.X = center.X + r * sinA * cosB;
            //x = x0 + R · sin θ · cos φ
            point.Y = center.Y + r * sinA * sinB;
            //y = y0 + R · sin θ · sin φ
            point.Z = center.Z + r * cosA;
            //z = z0 + R · cos θ
            return point;
        }
    }
}
