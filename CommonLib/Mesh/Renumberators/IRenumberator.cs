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
//                 кодировка : 27.11.2021 Потапов И.И.
//---------------------------------------------------------------------------
namespace CommonLib.Mesh
{
    using System;
    /// <summary>
    /// Направления фронта перенумерации
    /// </summary>
    [Serializable]
    public enum Direction
    {
        toRight,
        toLeft,
        toUp,
        toDown,
    }
    public interface IRenumberator
    {
        /// <summary>
        /// Метод фронтальной перенумерации 2D сетки
        /// </summary>
        /// <param name="mesh">сетка</param>
        /// <param name="direction">направление фронта</param>
        /// <returns></returns>
        void FrontRenumberation(ref IMesh mesh, Direction direction = Direction.toRight);
    }
}
