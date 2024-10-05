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
//                          ПРОЕКТ  "МКЭ"
//                         проектировщик:
//                           Потапов И.И.
//---------------------------------------------------------------------------
//                 кодировка : 25.12.2020 Потапов И.И.
//---------------------------------------------------------------------------
namespace CommonLib
{
    using System.ComponentModel;
    /// <summary>
    /// Тип КЭ сетки в 1D и 2D
    /// </summary>
    public enum TypeMesh
    {
        /// <summary>
        /// Одномерная КЭ сетка 1D
        /// </summary>
        [Description("Одномерная КЭ сетка")] Line = 0,
        /// <summary>
        /// Трехугольная КЭ сетка 2D
        /// </summary>
        [Description("Трехугольная КЭ сетка")] Triangle,
        /// <summary>
        /// Четырехугольная КЭ сетка 2D
        /// </summary>
        [Description("Четырехугольная КЭ сетка")] Rectangle,
        /// <summary>
        /// Смешанная КЭ сетка 2D
        /// </summary>        
        [Description("Смешанная КЭ сетка 2D")] MixMesh
    }
}
