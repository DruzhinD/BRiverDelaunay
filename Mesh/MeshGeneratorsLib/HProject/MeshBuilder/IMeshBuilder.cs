﻿//---------------------------------------------------------------------------
//                         проектировщик:
//                           Потапов И.И.
//                  - (C) Copyright 2000-2003
//                      ALL RIGHT RESERVED
//---------------------------------------------------------------------------
//                 кодировка : 1.02.2003 Потапов И.И.
//               ПРОЕКТ  "MixTasker' на базе "DISER"
//---------------------------------------------------------------------------
//        Перенос на C#, вариант от : 05.02.2022  Потапов И.И.
//    реализация генерации базисной КЭ сетки без поддержки полей свойств
//                         убран 4 порядок сетки 
//---------------------------------------------------------------------------
namespace MeshGeneratorsLib
{
    using CommonLib;
    /// <summary>
    /// ОО: Интерфейс строителя КЭ сетки по заданным подобластям, 
    /// реализована концепция паттерна строитель
    /// </summary>
    public interface IMeshBuilder
    {
        /// <summary>
        /// Название строителя КЭ сетки
        /// </summary>
        string Name { get; }
        /// <summary>
        /// настройка билдера
        /// </summary>
        /// <param name="mp">парамеры сетки</param>
        /// <param name="mesh">сетка</param>
        void Set(MeshBuilderArgs mp, IFEMesh mesh);
        /// <summary>
        /// вычисление массива конечных элементов в подобласти
        /// независимый вызов
        /// </summary>
        void BuilderAElement();
        /// <summary>
        /// вычисление массива граничных конечных элементов в подобласти
        /// вызывать после метода BuilderBNods();
        /// </summary>
        void BuilderBElement();
        /// <summary>
        /// вычисление массива граничных  узлов
        /// независимый вызов
        /// </summary>
        void BuilderBNods();
        /// <summary>
        /// вычисление координат узлов
        /// независимый вызов
        /// </summary>
        void BuilderCoords();
        /// <summary>
        /// Определение массива параметров сетки
        /// вызывать после метода BuilderCoords()
        /// </summary>
        void BuilderParams();
        /// <summary>
        /// Возвращает ссылку на сетку подобласти
        /// </summary>
        /// <returns></returns>
        IFEMesh GetFEMesh();
        /// <summary>
        /// Клонируем билдер
        /// </summary>
        /// <returns></returns>
        IMeshBuilder Clone();
    }
}
