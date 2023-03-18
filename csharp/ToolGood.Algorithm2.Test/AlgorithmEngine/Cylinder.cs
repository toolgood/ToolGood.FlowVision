﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm2;

namespace ToolGood.Algorithm
{
    //定义圆柱信息
    public class Cylinder : AlgorithmEngine
    {
        private int _radius;
        private int _height;
        public Cylinder(int radius, int height)
        {
            _radius = radius;
            _height = height;
        }

        protected override Operand GetParameter(string parameter)
        {
            if (parameter == "半径")
            {
                return Operand.Create(_radius);
            }
            if (parameter == "直径")
            {
                return Operand.Create(_radius * 2);
            }
            if (parameter == "高")
            {
                return Operand.Create(_height);
            }
            return base.GetParameter(parameter);
        }


    }
}
