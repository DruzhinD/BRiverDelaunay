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
//              Реализация библиотеки для моделирования 
//               гидродинамических и русловых процессов
//                      - (C) Copyright 2024 -
//---------------------------------------------------------------------------
//                   разработка: Потапов И.И.
//                           29.01.24
//---------------------------------------------------------------------------
namespace CommonLib.Function
{
    /// <summary>
    /// ОО: Одномерная функция
    /// </summary>
    public interface IFunction1D
    {
        /// <summary>
        /// Имя функции/данных
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Получение интерполированного (линейно) не сглаженного значения данных 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double FunctionValue(double x);
        /// <summary>
        /// Получение сглаженных данных 
        /// </summary>
        /// <param name="x">аргумент</param>
        /// <param name="y">функция</param>
        /// <param name="Count">количество точек</param>
        void GetFunctionData(ref double[] x, ref double[] y, int Count = 0);
    }
}
