﻿#region License
/*
Copyright (c) 2019 - 2024  Potapov Igor Ivanovich, Khabarovsk

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion
//---------------------------------------------------------------------------
//                    ПРОЕКТ  "РУСЛОВЫЕ ПРОЦЕССЫ"
//                         проектировщик:
//                           Потапов И.И.
//---------------------------------------------------------------------------
//                    создание поля IField<T>
//                 кодировка : 14.12.2020 Потапов И.И.
//---------------------------------------------------------------------------

namespace GeometryLib
{
    /// <summary>
    /// Поля для отрисовки 
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Название поля
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Контекстный флаг используется при отображении поля
        /// </summary>
        //[NonSerialized]
        bool Check { get; set; }
        /// <summary>
        /// Контекстный флаг используется при отображении поля
        /// </summary>
        //[NonSerialized]
        uint idx { get; set; }
        /// <summary>
        /// Получить компоненты поля
        /// </summary>
        /// <param name="V"></param>
        void GetValue(ref object[] V);
        /// <summary>
        /// Размерность
        /// </summary>
        int Dimention { get; }
    }
}
