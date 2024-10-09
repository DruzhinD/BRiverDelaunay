using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TriangleNet;
using TriangleNet.Geometry;

namespace MeshExplorer
{
    /// <summary>
    /// Предназначен для привязки объектов (данных) к элементам формы <br/>
    /// </summary>
    internal class FormDataContext : INotifyPropertyChanged
    {
        #region Реализация интерфейса
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
        IPolygon input;
        public IPolygon Input {  get => input; set
            {
                input = value;
                if (input != null)
                    AmountPoints = input.Points.Count; //изменяем актуальное количество точек
                OnPropertyChanged();
            }}

        int amountPoints;
        public int AmountPoints { get => amountPoints; set
            {
                amountPoints = value;
                OnPropertyChanged();
            } }

        Mesh mesh;
        public Mesh Mesh
        {
            get => mesh; set
            {
                mesh = value;
                if (mesh != null)
                    AmountPoints = mesh.Vertices.Count; //изменяем актуальное количество точек
                OnPropertyChanged();
            }
        }
    }
}
